using UnityEngine;

public class ItemResorce : MonoBehaviour {
    /*
      material_gold,
        material_iron,
        material_ice,
        material_ilium,
        material_neutonium,
        material_plastica,
        module_storage,
        module_single_blaster,
        module_double_blaster,
        module_mining_laser,
        module_power_reactor,
        module_command_module_type1,
        module_main_engine,
        module_thruster,
        module_refiner,
        module_tracktor_beam
     * */
    [SerializeField] public Enums.enum_item Item_type;

    private string[] description_list = new string[] {
        "Gold material used for manufacturing and making money",
        "Iron material used for manufacturing and making money",
        "Ice Used for making fuel",
        "ilium Used for making fuel",
        "neutonium material used for manufacturing and making money",
        "plastica material used for manufacturing and making money",
        "This is a ship storage",
        "Wepon Fires a sinle shot",
        "Wepon Fires a double shots",
       "Used for mining asteroids",
        "Used to power your ship",
        "Type1 command module",
        "Main enging use to make your ship go",
        "Thrusters used to maneuver your ship",
        "Used to refine materials",
       "Used to pull ktems to your ship"
    };
    public string GetDescription() {
        return description_list[(int)Item_type];
    }
}