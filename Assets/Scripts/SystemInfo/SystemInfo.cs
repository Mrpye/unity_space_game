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

    [SerializeField] public List<GameObject> modules;

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

    private void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        GameObject mods = GameObject.Find("Modules");
        modules = new List<GameObject>();
        ModuleSystemInfo[] childmods = mods.GetComponentsInChildren<ModuleSystemInfo>();
        foreach (ModuleSystemInfo c in childmods) {
            AddModule(c.gameObject);
        }
    }

    public GameObject GetCommandModule() {
        foreach (GameObject g in modules) {
            ModuleSystemInfo m = g.GetComponent<ModuleSystemInfo>();
            if (m.is_command_module == true) {
                return g;
            }
        }
        return null;
    }
    private void Update() {
       
        UpdateValues();
    }
    public void AddModule(GameObject module,bool is_in_storage = false) {
        Debug.Log("Adding Module" + module.name);
        SpaceShipMovment controls = gameObject.GetComponent<SpaceShipMovment>();
        ModuleSystemInfo m = module.GetComponent<ModuleSystemInfo>();
        m.is_in_storage = is_in_storage;
        if (is_in_storage == false) {
            GameObject parent_mods = GameObject.Find("Modules");
            foreach (KeyMappingModel e in m.key_mappings) {
                controls.AddKeyBinding(e, module);
            }
            modules.Add(module);
            module.transform.parent = parent_mods.transform;
        } else {
            GameObject parent_storage_mods = GameObject.Find("Stored_Modules");
            module.transform.parent = parent_storage_mods.transform;
        }
       
    }
    private void UpdateValues() {
        heat = 0;
        cpu_usage = 0;
        power_drain = 0;
        fuel_drain = 0;
        mass = 0;

        foreach (GameObject e in modules) {
            ModuleSystemInfo ms = e.GetComponent<ModuleSystemInfo>();
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
        foreach (GameObject e in modules) {
            ModuleSystemInfo ms = e.GetComponent<ModuleSystemInfo>();
            if (ms != null) {
                ms.Set_Values(heat,heat_max, power, power_max,fuel, fuel_max);
            }

        }

       


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