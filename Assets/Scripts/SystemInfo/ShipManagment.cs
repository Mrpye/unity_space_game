using UnityEngine;

public class ShipManagment : InventoryManager {

    public struct value_data {
        public float max_value;
        public float value;

        public value_data(float max, float val) {
            this.max_value = max;
            this.value = val;
        }
    }

    [SerializeField] private bool is_config = false;

    [Header("Ship Stats ")]
    [SerializeField] private float heat_max;

    [SerializeField] private float heat;

    [SerializeField] private float battery_max;
    [SerializeField] private float upgraded_battery_max;
    [SerializeField] public float battery;
    [SerializeField] private float battery_drain;

    [SerializeField] private float fuel_max;
    [SerializeField] private float upgraded_fuel_max;
    [SerializeField] public float fuel = 0;
    [SerializeField] private float fuel_drain;

    [SerializeField] private float cpu_max = 0;
    [SerializeField] private float cpu_usage = 0;

    [SerializeField] private float health_max;
    [SerializeField] private float health;

    [SerializeField] private float shield_max;
    [SerializeField] private float shield;

    [SerializeField] private float mass;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] public float kernetic_energy;

    //private Shield shield_obj;

    //Private
    private float total_upgrade_cpu;

    private float total_upgrade_battery_max;
    private float total_upgrade_fuel_max;
    private float total_upgrade_shield_max;
    private ModuleSystemInfo command_mod_system_info;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (command_module != null) {
            this.command_mod_system_info = command_module.GetComponent<ModuleSystemInfo>();
        }
       

        
        this.child_Start();
        CalcUpgrades();
    }

   /* public bool ShieldOnline() {
        if(this.shield_obj != null) {
            return this.shield_obj.shield_online;
        } else {
            return false;
        }
    }*/
    private void Update() {
        if (is_config == false) {
            UpdateValues();
        }
    }

   

    private void CalcUpgrades() {
        //************************
        //Run once to calc modules
        //************************
        total_upgrade_battery_max = 1;
        total_upgrade_fuel_max = 1;
        total_upgrade_shield_max = 1;
        total_upgrade_cpu = 0;
        cpu_usage = 0;
        cpu_max = 0;

        //This will cal the upgrades values
        foreach (ModuleSystemInfo ms in UnityFunctions.GetModules()) {
            if (ms != null) {
                if (ms.is_in_storage == false) {
                    //*********************************
                    //Calc the module internal upgrades
                    //*********************************
                    //ms.CalcUpgrades();
                   /* if (shield_obj == null) {
                        if(ms.GetComponent<ItemResorce>().Item_type== Enums.enum_item.module_shield) {
                            this.shield_obj = (Shield)ms;
                        }
                    }*/
                    //**************
                    //heat
                    //**************
                    //************
                    //CPU
                    //************
                    float cpu = ms.Get_Calculated_CPU_V();
                    if (cpu > 0) {
                        cpu_max += cpu;
                    } else {
                        cpu_usage += Mathf.Abs(cpu);
                    }

                    //*******************
                    //Power
                    //*******************
                    total_upgrade_battery_max += ms.Get_Calculated_Extra_Battery_Capacity_P();

                    //******
                    //Fuel
                    //******
                    total_upgrade_fuel_max += ms.Get_Calculated_Extra_Fuel_Capacity_P();

                    fuel_drain += ms.current_fuel;

                    //*******
                    //shield
                    //*******
                    shield_max += ms.Get_Calculated_Max_Shield_Capacity();

                }
            }
        }
    }

    private void UpdateValues() {
        heat = 0;
        battery_drain = 0;
        fuel_drain = 0;
        mass = 0;
        this.shield = 0;
        int heat_generating_module = 0;
        //****************************************
        //Loop through each module and gather data
        //****************************************
        foreach (ModuleSystemInfo ms in UnityFunctions.GetModules()) {
            if (ms != null) {
                if (ms.is_in_storage == false) {
                    //**************
                    //heat
                    //**************
                    heat += ms.current_heat;
                    if (ms.Generates_Heat()) { heat_generating_module += 1; }

                    //*******************
                    //Power
                    //*******************
                    if (ms.current_power > 0) {

                    }
                    battery_drain += ms.current_power;

                    //******
                    //Fuel
                    //******
                    fuel_drain += ms.current_fuel;

                    //*******
                    //Mass
                    //*******
                    mass += ms.Get_Calculated_Mass();

                    //Shield
                    shield += ms.current_shield;

                    ms.ResetUsage();
                }
            }
        }

        //******
        //Heat
        //******
        if (heat < 0) { heat = 0; }
        heat_max = (heat_generating_module * 100);

        //*******
        //Battery
        //*******
        battery += battery_drain;
        upgraded_battery_max = total_upgrade_battery_max * battery_max;
        battery = Mathf.Clamp(battery, 0, upgraded_battery_max);

        //*******
        //Fuel
        //*******
        fuel += fuel_drain;
        upgraded_fuel_max = total_upgrade_fuel_max * fuel_max;
        fuel = Mathf.Clamp(fuel, 0, upgraded_fuel_max);

        //*******
        //Mass
        //*******
        if (rb != null) { rb.mass = mass; }

        //******
        //health
        //******
        this.health_max = command_mod_system_info.settings.Health_start;
        this.health = command_mod_system_info.current_health;

      


        //******************************************
        //This feeds back information to each module
        //******************************************
        foreach (ModuleSystemInfo ms in UnityFunctions.GetModules()) {
            if (ms != null) {
                ms.Set_Values(heat, heat_max, battery, upgraded_battery_max, fuel, fuel_max);
            }
        }
    }

    /// <summary>
    /// Get the data and max data
    /// </summary>
    /// <param name="info_type"></param>
    /// <returns></returns>
    public value_data Get_Data(Enums.enum_system_info info_type) {
        switch (info_type) {
            case Enums.enum_system_info.heat:
                return new value_data(heat_max, heat);
            case Enums.enum_system_info.power:
                return new value_data(upgraded_battery_max, battery);
            case Enums.enum_system_info.cpu:
                return new value_data(cpu_max, cpu_usage);
            case Enums.enum_system_info.fuel:
                return new value_data(upgraded_fuel_max, fuel);
            case Enums.enum_system_info.health:
                return new value_data(health_max, health);
            case Enums.enum_system_info.shield:
                return new value_data(shield_max, shield);
            default:
                return new value_data(health_max, health);
        }
    }
}