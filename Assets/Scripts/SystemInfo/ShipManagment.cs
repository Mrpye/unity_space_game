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

    [Header("Ship Stats ")]
    [SerializeField] private float heat_max;

    [SerializeField] private float heat;

    [SerializeField] private float battery_max;
    [SerializeField] private float upgraded_battery_max;
    [SerializeField] private float battery;
    [SerializeField] private float battery_drain;

    [SerializeField] private float fuel_max;
    [SerializeField] private float upgraded_fuel_max;
    [SerializeField] private float fuel;
    [SerializeField] private float fuel_drain;

    [SerializeField] private float cpu_max = 0;
    [SerializeField] private float cpu_usage = 0;

    [SerializeField] private float health_max;
    [SerializeField] private float health;

    [SerializeField] private float mass;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] public float kernetic_energy;

    //Private
    private float total_upgrade_cpu;

    private float total_upgrade_battery_max;
    private float total_upgrade_fuel_max;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        this.child_Start();
        CalcUpgrades();
    }

    private void Update() {
        UpdateValues();
    }

    public void DamageShip(float damage_f = 0) {
        Rigidbody2D rb1 = GetComponent<Rigidbody2D>();
        if (damage_f > 0) {
            this.health -= damage_f;
        } else {
            float kernetic_energy1 = UnityFunctions.Calc_Kinetic_Energy(rb);
            this.health -= (kernetic_energy1 * 0.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer) {
            ProcessHit(damageDealer);
        }
    }

    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0) {
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        DamageShip();
    }

    private void CalcUpgrades() {
        //************************
        //Run once to calc modules
        //************************
        total_upgrade_battery_max = 1;
        total_upgrade_fuel_max = 1;
        total_upgrade_cpu = 0;
        cpu_usage = 0;
        cpu_max = 0;
        //This will cal the upgrades values
        foreach (ModuleSystemInfo ms in GeEquipedItems()) {
            if (ms != null) {
                if (ms.is_in_storage == false) {
                    //*********************************
                    //Calc the module internal upgrades
                    //*********************************
                    ms.CalcUpgrades();

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
                    //Mass
                    //*******
                }
            }
        }
    }

    private void UpdateValues() {
        heat = 0;
        battery_drain = 0;
        fuel_drain = 0;
        mass = 0;
       
        int heat_generating_module = 0;
        //****************************************
        //Loop through each module and gather data
        //****************************************
        foreach (ModuleSystemInfo ms in GeEquipedItems()) {
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
                    battery_drain += ms.current_power;

                    //******
                    //Fuel
                    //******
                    fuel_drain += ms.current_fuel;

                    //*******
                    //Mass
                    //*******
                    mass += ms.Get_Calculated_Mass();

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

        //******************************************
        //This feeds back information to each module
        //******************************************
        foreach (ModuleSystemInfo ms in GeEquipedItems()) {
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
                return new value_data(battery_max, battery);

            case Enums.enum_system_info.cpu:
                return new value_data(cpu_max, cpu_usage);

            case Enums.enum_system_info.fuel:
                return new value_data(fuel_max, fuel);

            case Enums.enum_system_info.health:
                return new value_data(health_max, health);

            default:
                return new value_data(health_max, health);
        }
    }
}