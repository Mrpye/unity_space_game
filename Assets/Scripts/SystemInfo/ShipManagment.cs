using System.Collections.Generic;
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
    public static float KineticEnergy(Rigidbody2D rb) {
        // mass in kg, velocity in meters per second, result is joules
        return 0.5f * rb.mass * rb.velocity.sqrMagnitude;
    }

    public void DamageShip(float damage_f = 1) {
        Rigidbody2D rb1 = GetComponent<Rigidbody2D>();
        if (damage_f > 0) {
            float kernetic_energy1 = KineticEnergy(rb);
            this.health -= damage_f;
        } else {
            float kernetic_energy1 = KineticEnergy(rb);
            this.health -= (kernetic_energy1 * 0.1f);
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

        foreach (ModuleSystemInfo ms in GeEquipedItems()) {
            if (ms != null) {
                if (ms.is_in_storage == false) {
                    heat += ms.Get_Heat();
                    cpu_usage += ms.Get_CPU();
                    power_drain += ms.Get_Power();
                    fuel_drain += ms.Get_Fuel();
                    mass += ms.Get_Mass();
                    ms.ResetUsage();
                }
            }

        }
        power += power_drain;
        if (power > power_max) {
            power = power_max;
        } else if (power == 0) {
            power = 0;
        }
        fuel += fuel_drain;
        if (rb != null) {
            rb.mass = mass;
        }
        foreach (ModuleSystemInfo ms in GeEquipedItems()) {
            if (ms != null) {
                ms.Set_Values(heat,heat_max, power, power_max,fuel, fuel_max);
            }

        }

       


    }
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