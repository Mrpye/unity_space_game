using System.Collections;
using UnityEngine;

public class ModuleSystemInfo : MonoBehaviour {

    public enum enum_system_info {
        Class_A,
        Class_B,
        Class_C,
        Class_D,
    }

    [Header("Class")]
    [SerializeField] private enum_system_info system_class;

    [Header("Heat")]
    [SerializeField] private float idle_heat = 1;

    [SerializeField] private float min_idle_heat = 1;
    [SerializeField] private float usage_factor_heat = 0.1f;
    [SerializeField] private float min_usage_factor_heat = 0.1f;

    [Header("Power")]
    [SerializeField] private float idle_power = 0.1f;

    [SerializeField] private float min_idle_power = 0.1f;
    [SerializeField] private float power_usage_factor = 1f;
    [SerializeField] private float min_power_usage_factor = 1f;

    [Header("Fuel")]
    [SerializeField] private float idle_fuel = 0;

    [SerializeField] private float min_idle_fuel = 0;
    [SerializeField] private float fuel_usage_factor = 1f;
    [SerializeField] private float min_fuel_usage_factor = 1f;

    [Header("CPU")]
    [SerializeField] private float cpu = 1;

    [SerializeField] private float min_cpu = 1;

    [Header("Health")]
    [SerializeField] private float health = 100;

    [SerializeField] private float health_malfunction = 20;
    [SerializeField] private float health_offline = 20;

    [Header("Mass")]
    [SerializeField] private float mass = 10;

    [SerializeField] private float min_mass = 10;

    [Header("Thrust")]
    [SerializeField] private float max_thrust = 10;

    [SerializeField] private float thrust = 10;

    [Header("Damage")]
    [SerializeField] private float max_Damage = 10;

    [SerializeField] private float damage = 10;

    [Header("Action Speed")]
    [SerializeField] private float max_action_speed = 10;

    [SerializeField] private float min_action_speed = 10;
    [SerializeField] private float action_speed = 10;

    [Header("Speed")]
    [SerializeField] private float max_speed = 10;

    [SerializeField] private float min_speed = 10;
    [SerializeField] private float speed = 10;

    [Header("Items")]
    [SerializeField] private float max_items = 10;

    [SerializeField] private  int items = 10;
 


    [Header("Range")]
    [SerializeField] private float max_range = 10;

    [SerializeField] private float range = 10;


    [Header("Ammount")]
    [SerializeField] private float max_ammount = 10;

    [SerializeField] private float ammount = 10;

    [Header("Malfunction")]
    [SerializeField] private float min_offline_malfunction_time = 1;
    [SerializeField] private float max_offline_malfunction_time = 2;
    [SerializeField] private float min_online_malfunction_time = 1;
    [SerializeField] private float max_online_malfunction_time = 2;

    private float offline_malfunction_time;
    private float online_malfunction_time;
    private bool in_use = false;
    private float current_heat = 0;
    private float current_power = 0;
    private float current_fuel = 0;
    private bool is_malfunctioning = false;

    public void StartMonitor() {
        online_malfunction_time = Random.Range(min_online_malfunction_time, max_online_malfunction_time);
        offline_malfunction_time = Random.Range(min_offline_malfunction_time, max_offline_malfunction_time);
        StartCoroutine(Malfunction());
    }

    /// <summary>
    /// This will say if the system has malfunctioned
    /// </summary>
    /// <returns></returns>
    public bool Is_Online() {
        return !is_malfunctioning;
    }

    private IEnumerator Malfunction() {
        while (true) {
            if (health < health_malfunction) {
                yield return new WaitForSeconds(online_malfunction_time);
                float chance = 100 - health;
                if (Random.Range(0, 100) < chance) {
                    is_malfunctioning = true;
                } else {
                    is_malfunctioning = false;
                }
                yield return new WaitForSeconds(offline_malfunction_time);
            } else {
                is_malfunctioning = true;
            }
        }
    }

    public bool Is_OffLine() {
        return health <= health_offline;
    }

    private void Update() {
        if (in_use == true) {
            current_heat += usage_factor_heat * Time.deltaTime;
            current_power += power_usage_factor * Time.deltaTime;
            current_fuel += fuel_usage_factor * Time.deltaTime;
        } else {
            current_heat -= usage_factor_heat * Time.deltaTime; if (current_heat <= idle_heat) { current_heat = idle_heat; }
            current_power = idle_power;
            current_fuel = idle_fuel;
        }
    }
    public float Get_Ammount() {
        return ammount;
    }
    public enum_system_info Get_Class() {
        return system_class;
    }
    public float Get_MaxItems() {
        return items;
    }
    public float Get_Items() {
        return items;
    }
    public float Get_Range() {
        return range;
    }

    public float Get_ActionSpeed() {
        return action_speed;
    }

    public float Get_Speed() {
        return speed;
    }

    public float Get_Thrust() {
        return thrust;
    }

    public float Get_Damage() {
        return damage;
    }

    public float Get_Mass() {
        return mass;
    }

    public float Get_Heat() {
        return current_heat;
    }

    public float Get_CPU() {
        return cpu;
    }

    public float Get_Power() {
        float val = current_power;
        current_power = 0;
        return val;
    }

    public float Get_Fuel() {
        float val = current_fuel;
        current_fuel = 0;
        return val;
    }

    public void StartUsage() {
        in_use = true;
    }

    public void StopUsage() {
        in_use = false;
    }
}