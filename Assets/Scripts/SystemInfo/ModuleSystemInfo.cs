using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSystemInfo : MonoBehaviour {

    #region Public Fields

    [Header("Storage Info")]
    [SerializeField] public string id = "";

    [SerializeField] public int storage_usage = 1;
    [SerializeField] public bool is_in_storage;
    [SerializeField] public bool is_internal_module;
    [SerializeField] public int mount_point;
    [SerializeField] public int order_layer = 100;
    [SerializeField] public bool is_command_module;
    [SerializeField] public int max_storage_items = 10; //Max items that can be stored in our inventory
    [SerializeField] public bool use_continuous_usage = true;
    [SerializeField] public Module_Settings settings;

    public List<KeyMappingModel> key_mappings = new List<KeyMappingModel>();

    #endregion Public Fields

    #region private Fields

    private float offline_malfunction_time;
    private float online_malfunction_time;
    private bool is_malfunctioning = false;
    private bool in_use = false;
    public float current_heat = 0;
    public float current_health = 0;
    public float current_power = 0;
    public float current_fuel = 0;
    public float current_thrust = 0;

    private ItemResorce ir;

    #endregion private Fields

    #region methods

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

    public float Get_Trust_Useage_Factor() {
        if (current_thrust > 0 && settings.Thrust_start > 0) {
            return current_thrust / settings.Thrust_start;
        } else {
            return 1;
        }
    }

    public float Get_Posotive_Eff_Factor() {
        if (current_health <= settings.Health_efficiency_fall_off_at) {
            return 1;
        } else {
            return (100 / current_health);
        }
    }

    public float Get_negative_EffFactor() {
        if (current_health <= settings.Health_efficiency_fall_off_at) {
            return 1;
        } else {
            return (current_health / 100);
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

    /// <summary>
    /// Calculate mafunctions
    /// </summary>
    /// <returns></returns>
    private IEnumerator Malfunction() {
        while (true) {
            yield return new WaitForSeconds(online_malfunction_time);
            if (current_health <= 0) {
                is_malfunctioning = true;
            } else if (current_health < settings.Health_malfunction_at) {
                float chance = 100 - current_health;
                if (Random.Range(0, 100) < chance) {
                    is_malfunctioning = true;
                } else {
                    is_malfunctioning = false;
                }
                yield return new WaitForSeconds(offline_malfunction_time);
            } else {
                is_malfunctioning = false;
            }
        }
    }

    public void ResetUsage() {
        current_power = settings.Power_idle;
        current_fuel = settings.Fuel_idle;
    }

    public void UpdateUsage() {
        if (use_continuous_usage) {
            if (in_use == true) {
                current_heat = this.Get_Calculated_Heat() * Time.deltaTime;
                if (current_heat > settings.Heat_damage_at) {
                    current_health -= settings.Heat_damage_factor * Time.deltaTime;
                }
                current_power = this.Get_Calculated_Power() * Time.deltaTime;
                current_fuel = this.Get_Calculated_Fuel() * Time.deltaTime;
            } else {
                current_fuel = this.Get_Calculated_Fuel_Idle() * Time.deltaTime;
                current_power = this.Get_Calculated_Power_idle() * Time.deltaTime;
                current_heat = this.Get_Calculated_Heat_Idle() * Time.deltaTime;
            }
        } else {
            current_heat += this.Get_Calculated_Heat_Idle() * Time.deltaTime;
        }
    }

    /// <summary>
    /// Used for things like wepons when they are fired
    /// </summary>
    public void SingleUpdateUsage() {
        in_use = true;
        current_heat += this.Get_Calculated_Heat();
        current_power = this.Get_Calculated_Power();
        current_fuel = this.Get_Calculated_Fuel();
        in_use = false;
    }

    #endregion methods

    #region functions

    public virtual void Set_Values(float heat, float max_heat, float power, float max_power, float fuel, float max_fuel) {
    }

    public bool Is_Online() {
        return !is_malfunctioning;
    }

    public bool Is_OffLine() {
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

    #region Calulated Values

    public float Get_Calculated_Speed() {
        return settings.Speed * Get_negative_EffFactor();
    }

    public float Get_Calculated_Thrust() {
        return settings.Thrust_start * Get_negative_EffFactor();
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
        return settings.Mass + stored_item_mass;
    }

    public float Get_Calculated_Heat() {
        return this.IsInUseInt() * this.settings.Heat_usage * Get_Posotive_Eff_Factor();
    }

    public float Get_Calculated_Heat_Idle() {
        return this.settings.Heat_idle * Get_Posotive_Eff_Factor();
    }

    public float Get_Calculated_Power() {
        return this.IsInUseInt() * this.settings.Power_usage * Get_Posotive_Eff_Factor() * Get_Trust_Useage_Factor(); ;
    }

    public float Get_Calculated_Power_idle() {
        return this.settings.Power_idle * Get_Posotive_Eff_Factor() * Get_Trust_Useage_Factor(); ;
    }

    public float Get_Calculated_Fuel() {
        return this.IsInUseInt() * this.settings.Fuel_usage * Get_Posotive_Eff_Factor() * Get_Trust_Useage_Factor();
    }

    public float Get_Calculated_Fuel_Idle() {
        return this.settings.Fuel_idle * Get_Posotive_Eff_Factor() * Get_Trust_Useage_Factor();
    }

    #endregion Calulated Values

    #endregion functions
}