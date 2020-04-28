using System.Collections.Specialized;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;


public class UnityFunctions {
    public static GameObject modules;
    public static GameObject stored_modules;
    public static GameObject game_elements;
    public static GameObject stored_upgrades;
    public static GameObject player;
    public static ShipManagment ship_manager;
    public static OrderedDictionary resource_data = new OrderedDictionary();
    public static bool data_loaded = false;
    public static bool controls_locked = false;



    public static void PopulateCommonVariables() {
        UnityFunctions.modules = GameObject.Find("Modules");
        UnityFunctions.stored_modules = GameObject.Find("Stored_Modules");
        UnityFunctions.game_elements = GameObject.Find("GameElements");
        UnityFunctions.stored_upgrades = GameObject.Find("Stored_Upgrades");
        UnityFunctions.player = GameObject.Find("Player");
        if (UnityFunctions.player == null) {
            UnityFunctions.player = GameObject.Find("Player_Config");
        } else {
            UnityFunctions.ship_manager = (ShipManagment)UnityFunctions.player.GetComponent("ShipManagment");
        }
        
        PopulateItemResorces();
    }

    public static void PopulateItemResorces() {
        if (UnityFunctions.data_loaded == true) { return; }
        UnityFunctions.data_loaded = true;

        AssetDatabase.FindAssets("t:Recipe");
        Recipe[] d = Resources.FindObjectsOfTypeAll(typeof(Recipe)) as Recipe[];
        foreach(Recipe item in d) {
            if (!UnityFunctions.resource_data.Contains(item.item_type.ToString())) {
                UnityFunctions.resource_data.Add(item.item_type.ToString(), item);
            } else {
                Debug.Log("Duplicate");
            }
           
            if (item.blueprint) {
                if (!UnityFunctions.resource_data.Contains(item.required_blueprint.ToString())) {
                    UnityFunctions.resource_data.Add(item.required_blueprint.ToString(), item);
                } else {
                    Debug.Log("Duplicate");
                }
            }
        }

        /*
        UnityFunctions.resource_data.Add(Enums.enum_item.pickup.ToString(), new ItemResorce.ItemResorceData("Material\\Pickup", "Spawn Modules and Blue Prints or Material", Enums.enum_item.pickup, Enums.enum_resorce_type.pickup, false));

        UnityFunctions.resource_data.Add(Enums.enum_item.material_gold.ToString(), new ItemResorce.ItemResorceData("Material\\Mat_Gold", "Gold material used for manufacturing and making money", Enums.enum_item.material_gold, Enums.enum_resorce_type.material, true));
        UnityFunctions.resource_data.Add(Enums.enum_item.material_iron.ToString(), new ItemResorce.ItemResorceData("Material\\Mat_Iron", "Iron material used for manufacturing and making money", Enums.enum_item.material_iron, Enums.enum_resorce_type.material, true));
        UnityFunctions.resource_data.Add(Enums.enum_item.material_ice.ToString(), new ItemResorce.ItemResorceData("Material\\Mat_Ice", "Gold material used for manufacturing and making money", Enums.enum_item.material_ice, Enums.enum_resorce_type.material, true));
        UnityFunctions.resource_data.Add(Enums.enum_item.material_ilium.ToString(), new ItemResorce.ItemResorceData("Material\\Mat_Ilium", "ilium Used for making fuel", Enums.enum_item.material_ilium, Enums.enum_resorce_type.material, true));
        UnityFunctions.resource_data.Add(Enums.enum_item.material_neutronium.ToString(), new ItemResorce.ItemResorceData("Material\\Mat_Neutonium", "neutonium material used for manufacturing and making money", Enums.enum_item.material_neutronium, Enums.enum_resorce_type.material, true));
        UnityFunctions.resource_data.Add(Enums.enum_item.material_plastica.ToString(), new ItemResorce.ItemResorceData("Material\\Mat_Plastica", "plastica material used for manufacturing and making money", Enums.enum_item.material_plastica, Enums.enum_resorce_type.material, true));

        UnityFunctions.resource_data.Add(Enums.enum_item.module_double_blaster.ToString(), new ItemResorce.ItemResorceData("Modules\\WeponSystems\\WeponSystem-BlasterDouble", "Wepon Fires a Double shot", Enums.enum_item.module_double_blaster, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_single_blaster.ToString(), new ItemResorce.ItemResorceData("Modules\\WeponSystems\\WeponSystem-BlasterSingle", "Wepon Fires a Single shot", Enums.enum_item.module_single_blaster, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_turret.ToString(), new ItemResorce.ItemResorceData("Modules\\WeponSystems\\WeponSystem-Turret", "Rotation", Enums.enum_item.module_turret, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_mining_laser.ToString(), new ItemResorce.ItemResorceData("Modules\\WeponSystems\\WeponSystem-Laser", "Used for mining asteroids", Enums.enum_item.module_mining_laser, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_power_reactor.ToString(), new ItemResorce.ItemResorceData("Modules\\Power\\PowerReactor", "Used to power your ship", Enums.enum_item.module_power_reactor, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_command_module_type1.ToString(), new ItemResorce.ItemResorceData("Modules\\CommandModule\\CommandModuleType1", "Type1 command module", Enums.enum_item.module_command_module_type1, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_main_engine.ToString(), new ItemResorce.ItemResorceData("Modules\\Engines\\MainEngine", "Main enging use to make your ship go", Enums.enum_item.module_main_engine, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_thruster.ToString(), new ItemResorce.ItemResorceData("Modules\\Engines\\Thruster", "Thrusters used to maneuver your ship", Enums.enum_item.module_thruster, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_refiner.ToString(), new ItemResorce.ItemResorceData("Modules\\Refiner\\Refiner", "Used to refine materials", Enums.enum_item.module_refiner, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_tracktor_beam.ToString(), new ItemResorce.ItemResorceData("Modules\\TracktorBeam\\TracktorBeam", "This is a Tracktor Beam", Enums.enum_item.module_tracktor_beam, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_repair.ToString(), new ItemResorce.ItemResorceData("Modules\\Repair\\AutoRepair", "Auto Repair Module", Enums.enum_item.module_repair, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_cooling.ToString(), new ItemResorce.ItemResorceData("Modules\\Cooling\\Cooling", "Heat Pump Cooling Module", Enums.enum_item.module_cooling, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_shield.ToString(), new ItemResorce.ItemResorceData("Modules\\Shield\\Shield", "Shield Module", Enums.enum_item.module_shield, Enums.enum_resorce_type.module, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.module_replicator.ToString(), new ItemResorce.ItemResorceData("Modules\\Replicator\\Replicator", "Replictaor tobuild items", Enums.enum_item.module_replicator, Enums.enum_resorce_type.module, false));

        UnityFunctions.resource_data.Add(Enums.enum_item.asset_asteroid_large.ToString(), new ItemResorce.ItemResorceData("GameAssets\\Asteroid\\Asteroid_large", "This is a Tracktor Beam", Enums.enum_item.asset_asteroid_large, Enums.enum_resorce_type.asset, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.asset_asteroid_med.ToString(), new ItemResorce.ItemResorceData("GameAssets\\Asteroid\\Asteroid_med", "This is a Tracktor Beam", Enums.enum_item.asset_asteroid_med, Enums.enum_resorce_type.asset, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.asset_small_enemy.ToString(), new ItemResorce.ItemResorceData("GameAssets\\Enemy\\SmallEnemy", "Small Enemy", Enums.enum_item.asset_small_enemy, Enums.enum_resorce_type.asset, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.asset_large_enemy.ToString(), new ItemResorce.ItemResorceData("GameAssets\\Enemy\\LargeEnemy", "Large Enemy", Enums.enum_item.asset_large_enemy, Enums.enum_resorce_type.asset, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.material_fuel.ToString(), new ItemResorce.ItemResorceData("Material\\fuel", "Used to fuel the ship", Enums.enum_item.material_fuel, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_fuel.ToString(), new ItemResorce.ItemResorceData("Blueprint\\blueprint_fuel", "Blueprint to make fuel", Enums.enum_item.material_fuel, Enums.enum_resorce_type.blueprint, false));

        UnityFunctions.resource_data.Add(Enums.enum_item.component_wire.ToString(), new ItemResorce.ItemResorceData("", "Component Wire", Enums.enum_item.component_wire, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.component_circuit.ToString(), new ItemResorce.ItemResorceData("", "Circuits", Enums.enum_item.component_circuit, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.component_coil.ToString(), new ItemResorce.ItemResorceData("", "Power Coil", Enums.enum_item.component_coil, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.component_matrix.ToString(), new ItemResorce.ItemResorceData("", "Circuit Matrix", Enums.enum_item.component_matrix, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.component_capacitor.ToString(), new ItemResorce.ItemResorceData("", "Power Capacitor", Enums.enum_item.component_capacitor, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.component_insulation.ToString(), new ItemResorce.ItemResorceData("", "Insulation", Enums.enum_item.component_insulation, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.component_focus_crystal.ToString(), new ItemResorce.ItemResorceData("", "Focus Crystal", Enums.enum_item.component_focus_crystal, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.component_glass.ToString(), new ItemResorce.ItemResorceData("", "Glass", Enums.enum_item.component_glass, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.component_vents.ToString(), new ItemResorce.ItemResorceData("", "Cooling Vents", Enums.enum_item.component_vents, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.component_cooling_pump.ToString(), new ItemResorce.ItemResorceData("", "Colling pump", Enums.enum_item.component_cooling_pump, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.component_money.ToString(), new ItemResorce.ItemResorceData("", "Money", Enums.enum_item.component_money, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.component_plastic.ToString(), new ItemResorce.ItemResorceData("", "Plastic Material used for construction", Enums.enum_item.component_plastic, Enums.enum_resorce_type.material, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.component_metal.ToString(), new ItemResorce.ItemResorceData("", "Metal use for constrution", Enums.enum_item.component_metal, Enums.enum_resorce_type.material, false));

        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_money.ToString(), new ItemResorce.ItemResorceData("", "Print Money", Enums.enum_item.component_money, Enums.enum_resorce_type.blueprint, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_wire.ToString(), new ItemResorce.ItemResorceData("", "Component Wire", Enums.enum_item.blueprint_wire, Enums.enum_resorce_type.blueprint, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_circuit.ToString(), new ItemResorce.ItemResorceData("", "Circuits", Enums.enum_item.blueprint_circuit, Enums.enum_resorce_type.blueprint, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_coil.ToString(), new ItemResorce.ItemResorceData("", "Power Coil", Enums.enum_item.blueprint_coil, Enums.enum_resorce_type.blueprint, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_matrix.ToString(), new ItemResorce.ItemResorceData("", "Circuit Matrix", Enums.enum_item.blueprint_matrix, Enums.enum_resorce_type.blueprint, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_capacitor.ToString(), new ItemResorce.ItemResorceData("", "Power Capacitor", Enums.enum_item.blueprint_capacitor, Enums.enum_resorce_type.blueprint, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_insulation.ToString(), new ItemResorce.ItemResorceData("", "Insulation", Enums.enum_item.blueprint_insulation, Enums.enum_resorce_type.blueprint, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_focus_crystal.ToString(), new ItemResorce.ItemResorceData("", "Focus Crystal", Enums.enum_item.blueprint_focus_crystal, Enums.enum_resorce_type.blueprint, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_glass.ToString(), new ItemResorce.ItemResorceData("", "Glass", Enums.enum_item.blueprint_glass, Enums.enum_resorce_type.blueprint, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_vents.ToString(), new ItemResorce.ItemResorceData("", "Cooling Vents", Enums.enum_item.blueprint_vents, Enums.enum_resorce_type.blueprint, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_cooling_pump.ToString(), new ItemResorce.ItemResorceData("", "Colling pump", Enums.enum_item.blueprint_cooling_pump, Enums.enum_resorce_type.blueprint, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_plastic.ToString(), new ItemResorce.ItemResorceData("", "Plastic Material used for construction", Enums.enum_item.blueprint_plastic, Enums.enum_resorce_type.blueprint, false));
        UnityFunctions.resource_data.Add(Enums.enum_item.blueprint_metal.ToString(), new ItemResorce.ItemResorceData("", "Metal use for constrution", Enums.enum_item.blueprint_metal, Enums.enum_resorce_type.blueprint, false));
        */
    }

    public static string GetItemTypeDescription(Enums.enum_item Item_type) {
        UnityFunctions.PopulateItemResorces();
        if (UnityFunctions.resource_data.Contains(Item_type.ToString())) {
            Recipe item = (Recipe)UnityFunctions.resource_data[Item_type.ToString()];
            return item.description;
        } else {
            return "";
        }
    }

    public static string GetItemTypeResorceLocation(Enums.enum_item Item_type) {
        UnityFunctions.PopulateItemResorces();
        Recipe item = (Recipe)UnityFunctions.resource_data[Item_type.ToString()];
        return item.prefab_path;
    }
    public static Enums.enum_item GetItemType(Enums.enum_item Item_type) {
        Recipe item = (Recipe)UnityFunctions.resource_data[Item_type.ToString()];
        return item.item_type;
    }
    public static Enums.enum_resorce_type GetItemTypeResorceType(Enums.enum_item Item_type) {
        UnityFunctions.PopulateItemResorces();
        Recipe item = (Recipe)UnityFunctions.resource_data[Item_type.ToString()];
        return item.resorce_type;
    }

    public static bool GetItemTypeNeedsRefining(Enums.enum_item Item_type) {
        UnityFunctions.PopulateItemResorces();
        Recipe item = (Recipe)UnityFunctions.resource_data[Item_type.ToString()];
        return item.need_refining;
    }
    public static Recipe GetItemTypeItem(Enums.enum_item Item_type) {
        UnityFunctions.PopulateItemResorces();
        Recipe item = (Recipe)UnityFunctions.resource_data[Item_type.ToString()];
        return item;
    }

    public static void LookAt2D(Transform obj, Transform theTarget, float speed, Enums.enum_facing_direction facing) {
        Vector3 vectorToTarget = theTarget.position - obj.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        angle -= (float)facing;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, q, Time.deltaTime * speed);
    }

    public static Color RGBA(float R,float G,float B,float A) {
        return new Color(R/255f,G/255, B/255,A/255f);
    }
    public static void CameraShake(float duration = 0.1f, float magnitude = 0.3f) {
        magnitude = Mathf.Clamp(magnitude, 0.05f, 0.5f);
        GameObject go = GameObject.Find("Main Camera");
        CameraShake shake = go.GetComponent<CameraShake>();
        if (shake != null) { shake.ShakeCamera(duration, magnitude); ; }
    }

    public static void SendAlert(Enums.enum_status status, string msg) {
        GameObject go = GameObject.Find("Alerts");
        if (go != null) { go.GetComponent<Alert>().RaiseAlert(status, msg); }
        
    }

    public static float normValue(float x, float min, float max) {
        return (x - min) / (max - min);
    }

    public static void Move_RB_Random(Rigidbody2D rb, float speed_min, float speed_max) {
        float move_speed_x = Random.Range(speed_min, speed_max);
        float move_speed_y = Random.Range(speed_min, speed_max);
        rb.velocity = new Vector3(Random.Range(-move_speed_x, move_speed_x), Random.Range(-move_speed_y, move_speed_y), 0);
    }

    public static GameObject FireProjectile(GameObject projectile_prefab, GameObject fire_point, int sort_order, float speed = 0, int damage = 2) {
        GameObject laser = Object.Instantiate(projectile_prefab, fire_point.transform.position, fire_point.transform.rotation, UnityFunctions.game_elements.transform) as GameObject;
        laser.GetComponent<DamageDealer>().SetDamage(damage);
        laser.GetComponent<SpriteRenderer>().sortingOrder = 100;
        laser.GetComponent<Rigidbody2D>().velocity = fire_point.transform.TransformDirection(Vector3.up * speed);
        return laser;
    }

    public static void Trust_At_Point(Rigidbody2D rb, Transform thrust_point, float thrust) {
        Vector2 force = -1 * thrust_point.up * Time.deltaTime * thrust;
        rb.AddForceAtPosition(force, thrust_point.position, ForceMode2D.Impulse);
    }

    public static float Calc_Kinetic_Energy(Rigidbody2D rb) {
        return 0.5f * rb.mass * rb.velocity.sqrMagnitude;
    }

    public static void AddListener(ref EventTrigger trigger, EventTriggerType eventType, System.Action<PointerEventData> listener) {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(data => listener.Invoke((PointerEventData)data));
        trigger.triggers.Add(entry);
    }

    public static ModuleSystemInfo[] GetModules() {
        return UnityFunctions.modules.GetComponentsInChildren<ModuleSystemInfo>();
    }

    public static ModuleSystemInfo[] GetStoredModules() {
        return UnityFunctions.stored_modules.GetComponentsInChildren<ModuleSystemInfo>();
    }

    public static bool ModuleInstalled(Enums.enum_item item) {
        ItemResorce[] items = UnityFunctions.modules.GetComponentsInChildren<ItemResorce>();
        var r = (from ItemResorce n in items where n.Item_type == item select n).Count();
        if (r > 0) {
            return true;
        } else {
            return false;
        }
    }

   
    public Upgrade_Settings[] GetStoredUpgrades() {
        return UnityFunctions.stored_modules.GetComponentsInChildren<Upgrade_Settings>();
    }

    public static Color Color_Green() {
        return UnityFunctions.RGBA(45, 195, 52, 255);
    }
    public static Color Color_Red() {
        return Color.red;
    }
    public static Color Color_Orange() {
        return UnityFunctions.RGBA(255, 150, 0, 255);
    }
}