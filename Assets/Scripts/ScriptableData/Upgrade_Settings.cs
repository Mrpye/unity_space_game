using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade_Settings", menuName = "Upgrade_Settings", order = 1)]
public class Upgrade_Settings : ScriptableObject {
    #region Fields
    [SerializeField] private List< Enums.enum_item> avalible_for;
    [SerializeField] private Sprite sprite;
    [SerializeField] private float cpu_v = 1;
    [SerializeField] private float fuel_capacity_p = 0;
    [SerializeField] private float battery_capacity_p = 0;
    [SerializeField] private float power_usage_p = 0;
    [SerializeField] private float fuel_usage_p = 0;
    [SerializeField] private float heat_usage_p = 0;

    [SerializeField] private float speed_p = 0;
    [SerializeField] private float action_speed_p = 0;
    [SerializeField] private float action_speed2_p = 0;
    [SerializeField] private float range_p = 0;
    [SerializeField] private float thrust_p = 0;
    [SerializeField] private float ammount_p = 0;
    [SerializeField] private float damage_P = 0;
    [SerializeField] private float damage_resistance_P = 0;
    [SerializeField] private float mass_p = 0;
    [SerializeField] private float shield_p = 0;
    #endregion




    public bool IsAvalible(Enums.enum_item  item) {
        //***************************************
        //Check to see if can be used on a module
        //***************************************
        foreach (Enums.enum_item  e in avalible_for) {
            if(e== item) {
                return true;
            }
        }
        return false;
    }


    #region Properties

    public List<Enums.enum_item> Basic_info_system_class { get => avalible_for; set => avalible_for = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    
    public string Full_name {
        get { return "Upgrades\\" + this.name; }
        
    }
    public float Cpu_V { get => cpu_v; set => cpu_v = value; }
    public float Fuel_capacity_P { get => fuel_capacity_p; set => fuel_capacity_p = value; }
    public float Battery_capacity_P { get => battery_capacity_p; set => battery_capacity_p = value; }
    public float Power_usage_p { get => power_usage_p; set => power_usage_p = value; }
    public float Fuel_usage_p { get => fuel_usage_p; set => fuel_usage_p = value; }
    public float Heat_usage_p { get => heat_usage_p; set => heat_usage_p = value; }
    public float Range_p { get => range_p; set => range_p = value; }
    public float Speed_p { get => speed_p; set => speed_p = value; }
    public float Action_speed_p { get => action_speed_p; set => action_speed_p = value; }
    public float Action_speed2_p { get => action_speed2_p; set => action_speed2_p = value; }
    public float Thrust_p { get => thrust_p; set => thrust_p = value; }
    public float Ammount_p { get => ammount_p; set => ammount_p = value; }
    public float Damage_P { get => damage_P; set => damage_P = value; }
    public float Damage_resistance_P { get => damage_resistance_P; set => damage_resistance_P = value; }
    public float Mass_P { get => mass_p; set => mass_p = value; }
    public float Shield_P { get => shield_p; set => shield_p = value; }
    #endregion
}