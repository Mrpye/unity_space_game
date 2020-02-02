using System.Collections.Generic;
using UnityEngine;

public class SystemInfo : MonoBehaviour {

    public enum enum_system_info {
        heat,
        power,
        cpu,
        fuel,
        health
    }

    public struct value_data {
        public float max_value;
        public float value;

        public value_data(float max, float val) {
            this.max_value = max;
            this.value = val;
        }
    }

    [SerializeField] private List<GameObject> modules;

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

    private void Update() {
        UpdateValues();
    }

    private void UpdateValues() {
        heat = 0;
        cpu_usage = 0;
        power_drain = 0;
        fuel_drain = 0;
        foreach (GameObject e in modules) {
            ModuleSystemInfo ms = e.GetComponent<ModuleSystemInfo>();
            if (ms != null) {
                heat += ms.Get_Heat();
                cpu_usage += ms.Get_CPU();
                power_drain += ms.Get_Power();
                fuel_drain += ms.Get_Fuel();
            }

        }
        power -= power_drain;
        fuel -= fuel_drain;
    }
    public value_data Get_Data(enum_system_info info_type) {
        switch (info_type) {
            case enum_system_info.heat:
                return new value_data(heat_max, heat);

            case enum_system_info.power:
                return new value_data(power_max, power);

            case enum_system_info.cpu:
                return new value_data(cpu_max, cpu_usage);

            case enum_system_info.fuel:
                return new value_data(fuel_max, fuel);

            case enum_system_info.health:
                return new value_data(health_max, health);

            default:
                return new value_data(health_max, health);
        }
    }
}