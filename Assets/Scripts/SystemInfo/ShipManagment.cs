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

    [SerializeField] private float power_max;
    [SerializeField] private float power;
    [SerializeField] private float power_drain;

    [SerializeField] private float fuel_max;
    [SerializeField] private float fuel;
    [SerializeField] private float fuel_drain;

    [SerializeField] private float cpu_max;
    [SerializeField] private float cpu_usage;

    [SerializeField] private float health_max;
    [SerializeField] private float health;

    [SerializeField] private float mass;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] public float kernetic_energy;

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        this.child_Start();
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

    private void UpdateValues() {
        heat = 0;
        cpu_usage = 0;
        power_drain = 0;
        fuel_drain = 0;
        mass = 0;
        //****************************************
        //Loop through each module and gather data
        //****************************************
        foreach (ModuleSystemInfo ms in GeEquipedItems()) {
            if (ms != null) {
                if (ms.is_in_storage == false) {
                    heat += ms.current_heat;  
                   cpu_usage += ms.settings.Cpu;
                    power_drain += ms.current_power;
                    fuel_drain += ms.current_fuel;
                    mass += ms.Get_Calculated_Mass();
                    ms.ResetUsage();
                }
            }
        }
        if (heat < 0) { heat = 0; }
        power += power_drain;
        power = Mathf.Clamp(power, 0, power_max);
        fuel += fuel_drain;
        fuel = Mathf.Clamp(fuel, 0, fuel);
        if (rb != null) {rb.mass = mass;}
        //******************************************
        //This feeds back information to each module
        //******************************************
        foreach (ModuleSystemInfo ms in GeEquipedItems()) {
            if (ms != null) {
                ms.Set_Values(heat, heat_max, power, power_max, fuel, fuel_max);
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
                return new value_data(power_max, power);
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