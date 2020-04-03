using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;
using static ItemResorce;

public class ModuleSystemInfo : MonoBehaviour {

    #region Public Fields

    //[SerializeField] public int storage_usage = 1;

    [Header("Module Info")]
    [SerializeField] public string id = "";

    [SerializeField] public int max_storage_items = 10; //Max items that can be stored in our inventory
    [SerializeField] public bool is_in_storage;
    [SerializeField] public bool is_internal_module;
    [SerializeField] public bool is_command_module;

    [Header("Config Info")]
    [SerializeField] public Module_Settings settings;

    [SerializeField] public List<Upgrade_Settings> upgrades;

    [Header("Mounting Position")]
    [SerializeField] public int mount_point;

    [SerializeField] public bool mount_type_util_top = true;
    [SerializeField] public bool mount_type_util_side = true;
    [SerializeField] public bool mount_type_thruster = true;
    [SerializeField] public bool mount_type_engine = true;

    [Header("Key Mapping")]
    public List<KeyMappingModel> key_mappings = new List<KeyMappingModel>();

    [Header("Module Usage Infomation")]
    public float current_heat = 0;

    public float current_health = 0;
    public float current_power = 0;
    public float current_fuel = 0;
    public float current_thrust = 0;

    [Header("Render Info")]
    [SerializeField] public int order_layer = 100;

    #endregion Public Fields

    #region private Fields

    private bool use_continuous_usage = true;
    private float calced_current_heat = 0;
    [SerializeField] private bool is_malfunctioning = false;
    private bool in_use = false;

    private float offline_malfunction_time;
    private float online_malfunction_time;
    private ItemResorce ir;
    private Alert alerts = null;
    private enum_staus_type status = enum_staus_type.Online;
    private enum_staus_type last_mafuntion_message_sent = enum_staus_type.Online;
    private Refiner refiner;

    #endregion private Fields

    #region methods

    #region public

    /// <summary>
    /// This sorts the sprite and line render
    /// </summary>
    /// <param name="sort_order"></param>
    public void SetRenderOrder(int sort_order) {
        //********************
        //Sort the order layer
        //********************
        order_layer = sort_order;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) { sr.sortingOrder = order_layer; }
        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) { sr.sortingOrder = order_layer; }
        LineRenderer lr = GetComponent<LineRenderer>();
        if (lr != null) { lr.sortingOrder = order_layer; }
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null) {
            ps.GetComponent<Renderer>().sortingOrder = order_layer;
        }
    }

    private float total_upgrade_power_usage;
    private float total_upgrade_fuel_usage;
    private float total_upgrade_heat_usage;
    private float total_upgrade_action_speed;
    private float total_upgrade_action_speed2;
    private float total_upgrade_speed;
    private float total_upgrade_range;
    private float total_upgrade_thrust;
    private float total_upgrade_ammount;
    private float total_upgrade_damage;
    private float total_upgrade_damage_resistance;
    private float total_mass;

    public void SendAlert(enum_status status, string message) {
        if (this.alerts != null) {
            message = message.Replace("(Clone)", "");
            this.alerts.RaiseAlert(status, message);
        }
    }

    public void Run_Start() {
        GameObject go = GameObject.Find("Alerts");
        if (go != null) {
            this.alerts = go.GetComponent<Alert>();
        }
        total_upgrade_damage_resistance = 1;
        total_upgrade_power_usage = 1;
        total_upgrade_fuel_usage = 1;
        total_upgrade_heat_usage = 1;
        total_upgrade_action_speed = 1;
        total_upgrade_action_speed2 = 1;
        total_upgrade_damage = 1;
        total_upgrade_speed = 1;
        total_upgrade_range = 1;
        total_upgrade_thrust = 1;
        total_upgrade_ammount = 1;
        total_mass = 1;
        foreach (Upgrade_Settings u in this.upgrades) {
            total_upgrade_power_usage += u.Power_usage_p;
            total_upgrade_fuel_usage += u.Fuel_usage_p;
            total_upgrade_heat_usage += u.Heat_usage_p;
            total_upgrade_action_speed += u.Action_speed_p;
            total_upgrade_action_speed2 += u.Action_speed2_p;
            total_upgrade_speed += u.Speed_p;
            total_upgrade_range += u.Range_p;
            total_upgrade_thrust += u.Thrust_p;
            total_upgrade_ammount += u.Ammount_p;
            total_upgrade_damage += u.Damage_P;
            total_upgrade_damage_resistance += u.Damage_resistance_P;
            total_mass += u.Mass_P;
        }
    }

    #region Damage

    private void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer) {
            ProcessHit(damageDealer);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
       ItemResorce ir = other.gameObject.GetComponent<ItemResorce>();
        if (ir != null&& (ir.GetResorceType()==  enum_resorce_type.material || ir.GetResorceType() == enum_resorce_type.pickup)) {
            //**********************************
            //We need to see if this is a pickup
            //**********************************
            if (ir != null && ir.Item_type == enum_item.pickup) {
                ItemResorceData item = ir.Spawn_Any_Module_Upgrade_Material();
                InventoryManager storage = GetComponentInParent<InventoryManager>();
                if (item.resorce_type == enum_resorce_type.material) {
                    UnityFunctions.SendAlert(  enum_status.Info,"Collected Item: " + item.item_type.ToString());
                    storage.Store_Material(item.item_type);
                    Destroy(other.gameObject);
                } else if (item.resorce_type == enum_resorce_type.module) {
                    GameObject refab = Resources.Load(item.resorce) as GameObject;
                    if(refab == null) {
                        UnityFunctions.SendAlert(enum_status.Info, "Null object" );
                    }
                    GameObject obj_module = Instantiate(refab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
                    UnityFunctions.SendAlert(enum_status.Info, "Collected Module: " + obj_module.name.ToString());
                    storage.Store_Module(obj_module);
                    Destroy(other.gameObject);
                }
            } else {
                //************************************************
                //Else must be a material this goes to the refiner
                //************************************************
                refiner = gameObject.transform.parent.GetComponentInChildren<Refiner>();
                if (refiner != null) {
                    UnityFunctions.SendAlert(enum_status.Info, "Refining Material: " + other.gameObject.name.ToString());
                    refiner.AddItemToRefiner(other.gameObject);
                }
            }
        } else{
            DamageShip();
        }
       
    }

    private void ProcessHit(DamageDealer damageDealer) {
        current_health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (current_health <= 0) {
            current_health = 0;
        }
        this.SendAlert(enum_status.Danger, this.name + " is taking Damage!");
    }

    public void DamageShip(float damage_f = 0) {
        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
        if (damage_f > 0) {
            this.current_health -= damage_f * total_upgrade_damage_resistance;
        } else {
            float kernetic_energy1 = UnityFunctions.Calc_Kinetic_Energy(rb);
            this.current_health -= (kernetic_energy1 * 0.05f) * total_upgrade_damage_resistance;
        }
        this.SendAlert(enum_status.Danger, this.name + " is taking Damage!");
    }

    #endregion Damage

    public bool Generates_Heat() {
        //This returns if the item Generates Heat
        if (this.settings.Heat_usage != 0) {
            return true;
        } else {
            return false;
        }
    }

    public void IteminStorage() {
        this.is_in_storage = true;
        SpriteRenderer sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) {
            sr.enabled = false;
        }
    }

    public void IteminUse(bool is_internal = false) {
        this.is_in_storage = false;
        SpriteRenderer sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) {
            sr.enabled = !is_internal;
        }
    }

    public void StartMonitor() {
        online_malfunction_time = Random.Range(settings.Malfunction_min_online_time, settings.Malfunction_max_online_time);
        offline_malfunction_time = Random.Range(settings.Malfunction_min_offline_time, settings.Malfunction_max_offline_time);
        ir = gameObject.GetComponent<ItemResorce>();
        StartCoroutine(Malfunction());
    }

    public void ResetUsage() {
        current_power = settings.Power_idle;
        current_fuel = settings.Fuel_idle;
    }

    public void UpdateUsage() {
        if (use_continuous_usage) {
            if (in_use == true) {
                calced_current_heat = this.Get_Calculated_Heat() * Time.deltaTime;
                if (current_heat > settings.Heat_damage_at) {
                    current_health -= settings.Heat_damage_factor * Time.deltaTime;
                }
                current_power = this.Get_Calculated_Power() * Time.deltaTime;
                current_fuel = this.Get_Calculated_Fuel() * Time.deltaTime;
            } else {
                current_fuel = this.Get_Calculated_Fuel_Idle() * Time.deltaTime;
                current_power = this.Get_Calculated_Power_idle() * Time.deltaTime;
                calced_current_heat = this.Get_Calculated_Heat_Idle() * Time.deltaTime;
            }
        } else {
            calced_current_heat = this.Get_Calculated_Heat_Idle() * Time.deltaTime;
        }

        current_heat += calced_current_heat;

        if (current_heat < 0) {
            current_heat = 0;
        }
    }

    public void SingleUpdateUsage() {
        //***********************************************
        //Used for things like wepons when they are fired
        //***********************************************
        in_use = true;
        current_heat += this.Get_Calculated_Heat();
        current_power = this.Get_Calculated_Power();
        current_fuel = this.Get_Calculated_Fuel();
        in_use = false;
    }

    public virtual void Set_Values(float heat, float max_heat, float power, float max_power, float fuel, float max_fuel) {
    }

    public bool Is_Malfunctioning() {
        return !is_malfunctioning;
    }

    public bool Is_OffLine() {
        return current_health <= settings.Health_offline_at;
    }

    public bool Is_Destroyed() {
        return current_health <= settings.Health_offline_at;
    }

    public bool IsInUse() {
        return in_use;
    }

    public int IsInUseInt() {
        if (in_use) { return 1; } else { return 0; };
    }

    public void StartUsage() {
        in_use = true;
    }

    public void StopUsage() {
        in_use = false;
    }

    #region Calculated Values for upgrades

    public float Get_Calculated_CPU_V() {
        float value = this.settings.Cpu;
        foreach (Upgrade_Settings u in this.upgrades) {
            value += u.Cpu_V;
        }
        return value;
    }

    public float Get_Calculated_Extra_Fuel_Capacity_P() {
        float value = 0;
        foreach (Upgrade_Settings u in this.upgrades) {
            value += u.Fuel_capacity_P;
        }
        return value;
    }

    public float Get_Calculated_Extra_Battery_Capacity_P() {
        float value = 0;
        foreach (Upgrade_Settings u in this.upgrades) {
            value += u.Battery_capacity_P;
        }
        return value;
    }

    #endregion Calculated Values for upgrades

    #endregion public

    #region private

    private float Get_Trust_Useage_Factor() {
        if (current_thrust > 0 && settings.Thrust_start > 0) {
            return current_thrust / settings.Thrust_start;
        } else {
            return 1;
        }
    }

    private float Get_Posotive_Eff_Factor() {
        if (current_health >= settings.Health_efficiency_fall_off_at) {
            return 1;
        } else {
            return (100 / current_health);
        }
    }

    private float Get_negative_EffFactor() {
        if (current_health >= settings.Health_efficiency_fall_off_at) {
            return 1;
        } else {
            return (current_health / 100);
        }
    }

    private IEnumerator Malfunction() {
        //************************************************
        //This is used to see if a module will malfunction
        //************************************************
        while (true) {
            yield return new WaitForSeconds(online_malfunction_time);
            if (current_health <= 0) {
                is_malfunctioning = true;
                status = enum_staus_type.Destroyed;
            } else if (current_health < settings.Health_malfunction_at) {
                float chance = 100 - current_health;
                if (Random.Range(0, 100) < chance) {
                    is_malfunctioning = true;
                    status = enum_staus_type.Malfunction;
                } else {
                    status = enum_staus_type.Online;
                    is_malfunctioning = false;
                }
                yield return new WaitForSeconds(offline_malfunction_time);
            } else {
                is_malfunctioning = false;
                status = enum_staus_type.Online;
            }

            if (status != last_mafuntion_message_sent) {
                this.SendAlert(enum_status.Warning, this.name + " is " + status.ToString());
                last_mafuntion_message_sent = status;
            }
        }
    }

    #region Calulated Values

    public float Get_Calculated_Speed() {
        return settings.Speed * Get_negative_EffFactor() * total_upgrade_speed;
    }

    public float Get_Calculated_Action_Speed() {
        return settings.Action_speed * Get_negative_EffFactor() * total_upgrade_action_speed;
    }

    public float Get_Calculated_Action_Speed2() {
        return settings.Action_speed2 * Get_negative_EffFactor() * total_upgrade_action_speed2;
    }

    public float Get_Calculated_Range() {
        return settings.Range * Get_negative_EffFactor() * total_upgrade_range;
    }

    public float Get_Calculated_Ammount() {
        return settings.Ammount * Get_negative_EffFactor() * total_upgrade_ammount;
    }

    public float Get_Calculated_Thrust() {
        return settings.Thrust_start * Get_negative_EffFactor() * total_upgrade_thrust;
    }

    public int Get_Calculated_Damage() {
        return (int)(settings.Damage * Get_negative_EffFactor() * total_upgrade_damage);
    }

    public float Get_Calculated_Thrust(float thrust) {
        float percentage = thrust / 100;
        return settings.Thrust_start * percentage * Get_negative_EffFactor();
    }

    public float Get_Calculated_Mass() {
        float stored_item_mass = 0;
        if (ir != null) {
            if (ir.Item_type == Enums.enum_item.module_storage) {
                InventoryManager storage = gameObject.GetComponent<InventoryManager>();
                if (storage != null) {
                    stored_item_mass = storage.Get_Total_Stored_Item_Mass();
                }
            }
        }
        return (settings.Mass * total_mass) + stored_item_mass;
    }

    private float Get_Calculated_Heat() {
        return this.IsInUseInt() * this.settings.Heat_usage * Get_Posotive_Eff_Factor();
    }

    private float Get_Calculated_Heat_Idle() {
        return this.settings.Heat_idle * Get_Posotive_Eff_Factor();
    }

    private float Get_Calculated_Power() {
        return this.IsInUseInt() * this.settings.Power_usage * Get_Posotive_Eff_Factor() * Get_Trust_Useage_Factor() * total_upgrade_power_usage;
    }

    private float Get_Calculated_Power_idle() {
        return this.settings.Power_idle * Get_Posotive_Eff_Factor() * Get_Trust_Useage_Factor(); ;
    }

    private float Get_Calculated_Fuel() {
        return this.IsInUseInt() * this.settings.Fuel_usage * Get_Posotive_Eff_Factor() * Get_Trust_Useage_Factor() * total_upgrade_fuel_usage;
    }

    private float Get_Calculated_Fuel_Idle() {
        return this.settings.Fuel_idle * Get_Posotive_Eff_Factor() * Get_Trust_Useage_Factor();
    }

    #endregion Calulated Values

    #endregion private

    #endregion methods
}