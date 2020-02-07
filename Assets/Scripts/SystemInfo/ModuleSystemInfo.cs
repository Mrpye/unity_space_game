using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSystemInfo : MonoBehaviour {

    #region enum

    public enum enum_system_info {
        Class_A,
        Class_B,
        Class_C,
        Class_D,
    }

    #endregion enum

    #region inspector Fields

    [Header("Class")]
    [SerializeField] public enum_system_info system_class = enum_system_info.Class_D;
    [SerializeField] public string ModuleName = "";
    [SerializeField] public string id = "";
    public List<KeyMappingModel> key_mappings = new List<KeyMappingModel>();

    [Header("Heat")]
    [SerializeField] public float idle_heat = 0;

    [SerializeField] public float min_idle_heat = 0;
    [SerializeField] public float usage_factor_heat = 1f;
    [SerializeField] public float min_usage_factor_heat = 0.1f;

    [Header("Power")]
    [SerializeField] public float idle_power = 0f;

    [SerializeField] public float min_idle_power = 0f;
    [SerializeField] public float power_usage_factor = -5f;
    [SerializeField] public float min_power_usage_factor = -1f;

    [Header("Fuel")]
    [SerializeField] public float idle_fuel = 0;

    [SerializeField] public float min_idle_fuel = 0;
    [SerializeField] public float fuel_usage_factor = -5f;
    [SerializeField] public float min_fuel_usage_factor = -1f;

    [Header("CPU")]
    [SerializeField] public float cpu = 1;

    [SerializeField] public float min_cpu = 1;

    [Header("Health")]
    [SerializeField] public float health = 100;

    [SerializeField] public float health_malfunction = 30;
    [SerializeField] public float health_offline = 10;

    [Header("Mass")]
    [SerializeField] public float mass = 10;

    [SerializeField] public float min_mass = 10;

    [Header("Thrust")]
    [SerializeField] public float max_thrust;

    [SerializeField] public float thrust;

    [Header("Damage")]
    [SerializeField] public float max_Damage = 100;

    [SerializeField] public float damage = 10;

    [Header("Action Speed")]//can be used for rate of fire
    [SerializeField] public float max_action_speed = 2;

    [SerializeField] public float min_action_speed = 0.1f;
    [SerializeField] public float action_speed = 0.5f;

    [Header("Speed")]//Can be used for speed of bullets
    [SerializeField] public float max_speed = 100;

    [SerializeField] public float min_speed = 10;
    [SerializeField] public float speed = 6;

    [Header("Items")]//storage items
    [SerializeField] public float max_items = 20;

    [SerializeField] public int items = 10;

    [Header("Range")]//Range of wepon or tractor beam
    [SerializeField] public float max_range = 30;

    [SerializeField] public float range = 10;

    [Header("Ammount")]//Can be used for how much the tractor bbeam pulls
    [SerializeField] public float max_ammount = 10;

    [SerializeField] public float ammount = 10;

    [Header("Malfunction")]//This is used for handling malfunctions
    [SerializeField] public float min_offline_malfunction_time = 1;

    [SerializeField] public float max_offline_malfunction_time = 2;
    [SerializeField] public float min_online_malfunction_time = 2;
    [SerializeField] public float max_online_malfunction_time = 5;

    #endregion inspector Fields

    #region private fileds

    private float offline_malfunction_time;
    private float online_malfunction_time;
    [SerializeField] private bool in_use = false;
    private float current_heat = 0;
    private float current_power = 0;
    private float current_fuel = 0;
    private bool is_malfunctioning = false;

    #endregion private fileds

    #region methods

    public void StartMonitor() {
        online_malfunction_time = Random.Range(min_online_malfunction_time, max_online_malfunction_time);
        offline_malfunction_time = Random.Range(min_offline_malfunction_time, max_offline_malfunction_time);
        StartCoroutine(Malfunction());
    }

    /// <summary>
    /// Calculate mafunctions
    /// </summary>
    /// <returns></returns>
    private IEnumerator Malfunction() {
        while (true) {
            yield return new WaitForSeconds(online_malfunction_time);
            if (health < health_malfunction) {
                float chance = 100 - health;
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
        current_power = idle_power;
        current_fuel = idle_fuel;
    }
    public void UpdateUsage() {
        if (in_use == true) {
            current_heat += usage_factor_heat * Time.deltaTime;
            current_power += power_usage_factor * Time.deltaTime;
            current_fuel += fuel_usage_factor * Time.deltaTime;
        } else {
            current_heat -= usage_factor_heat * Time.deltaTime; if (current_heat <= idle_heat) { current_heat = idle_heat; }
        }
    }

    public void SingleUpdateUsage() {
        current_heat += usage_factor_heat;
        current_power += power_usage_factor;
        current_fuel += fuel_usage_factor;
    }

    #endregion methods

    #region functions

    /// <summary>
    /// This will say if the system has malfunctioned
    /// </summary>
    /// <returns></returns>
    public bool Is_Online() {
        return !is_malfunctioning;
    }

     public virtual void Set_Values(float heat, float max_heat, float power, float max_power, float fuel, float max_fuel) {

    }

    public bool Is_OffLine() {
        return health <= health_offline;
    }

    public float Get_Ammount() {
        return ammount;
    }

    public enum_system_info Get_Class() {
        return system_class;
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

    #endregion functions
}