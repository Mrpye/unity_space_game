using UnityEngine;

[CreateAssetMenu(fileName = "Module_Settings", menuName = "Module_Settings", order = 1)]
public class Module_Settings : ScriptableObject {

    #region inspector Fields

    [SerializeField] private Enums.enum_class basic_info_system_class = Enums.enum_class.Class_D;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string basic_info_name = "";
    [SerializeField] private float heat_idle = -1;
    [SerializeField] private float heat_usage = 1f;
    [SerializeField] private float heat_damage_at = 90;
    [SerializeField] private float heat_damage_factor = 0.1f;
    [SerializeField] private float power_idle = 0f;
    [SerializeField] private float power_usage = -1f;
    [SerializeField] private float fuel_idle = 0;
    [SerializeField] private float fuel_usage = -1f;
    [SerializeField] private float cpu = 1;
    [SerializeField] private float health_start = 100;
    [SerializeField] private float health_malfunction_at = 30;
    [SerializeField] private float health_offline_at = 10;
    [SerializeField] private float health_efficiency_fall_off_at = 80;
    [SerializeField] private float mass = 10;
    [SerializeField] private float thrust_start=100;
    [SerializeField] private float damage = 1;//How much damage is afflicted
    [SerializeField] private float action_speed = 0.5f;//can be used for rate of fire
    [SerializeField] private float action_speed2 = 0.5f;//can be used for rate of fire
    [SerializeField] private float speed = 30;//Can be used for speed of bullets
    [SerializeField] private int items_max = 20; //Max number items allowed
    [SerializeField] private float range = 15;//Range of wepon or tractor beam
    [SerializeField] private float ammount = 20;//Can be used for how much the tractor bbeam pulls
    [SerializeField] private float malfunction_min_offline_time = 1;
    [SerializeField] private float malfunction_max_offline_time = 2;
    [SerializeField] private float malfunction_min_online_time = 2;
    [SerializeField] private float malfunction_max_online_time = 5;



    #endregion inspector Fields

    #region Properties
    public Enums.enum_class Basic_info_system_class { get => basic_info_system_class; set => basic_info_system_class = value; }
    public string Basic_info_name { get => basic_info_name; set => basic_info_name = value; }
    public float Heat_idle { get => heat_idle; set => heat_idle = value; }
    public float Heat_usage { get => heat_usage; set => heat_usage = value; }
    public float Heat_damage_at { get => heat_damage_at; set => heat_damage_at = value; }
    public float Heat_damage_factor { get => heat_damage_factor; set => heat_damage_factor = value; }
    public float Power_idle { get => power_idle; set => power_idle = value; }
    public float Power_usage { get => power_usage; set => power_usage = value; }
    public float Fuel_idle { get => fuel_idle; set => fuel_idle = value; }
    public float Fuel_usage { get => fuel_usage; set => fuel_usage = value; }
    public float Cpu { get => cpu; set => cpu = value; }
    public float Health_start { get => health_start; set => health_start = value; }
    public float Health_malfunction_at { get => health_malfunction_at; set => health_malfunction_at = value; }
    public float Health_offline_at { get => health_offline_at; set => health_offline_at = value; }
    public float Health_efficiency_fall_off_at { get => health_efficiency_fall_off_at; set => health_efficiency_fall_off_at = value; }
    public float Mass { get => mass; set => mass = value; }
    public float Thrust_start { get => thrust_start; set => thrust_start = value; }
    public float Damage { get => damage; set => damage = value; }
    public float Action_speed { get => action_speed; set => action_speed = value; }
    public float Action_speed2 { get => action_speed2; set => action_speed = value; }
    public float Speed { get => speed; set => speed = value; }
    public int Items_max { get => items_max; set => items_max = value; }
    public float Range { get => range; set => range = value; }
    public float Ammount { get => ammount; set => ammount = value; }
    public float Malfunction_min_offline_time { get => malfunction_min_offline_time; set => malfunction_min_offline_time = value; }
    public float Malfunction_max_offline_time { get => malfunction_max_offline_time; set => malfunction_max_offline_time = value; }
    public float Malfunction_min_online_time { get => malfunction_min_online_time; set => malfunction_min_online_time = value; }
    public float Malfunction_max_online_time { get => malfunction_max_online_time; set => malfunction_max_online_time = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    #endregion Properties
}