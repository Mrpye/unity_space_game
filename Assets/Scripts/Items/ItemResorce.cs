using System.Collections;
using UnityEngine;

public class ItemResorce : MonoBehaviour {
    [SerializeField] public Enums.enum_item Item_type;

    public struct ItemResorceData {
        public string resorce;
        public string description;
        public Enums.enum_item item_type;

        public ItemResorceData(string resorce, string description, Enums.enum_item item_type) {
            this.resorce = resorce;
            this.description = description;
            this.item_type = item_type;
        }
    }

    private Hashtable resource_data = new Hashtable();

    private void Start() {
        resource_data.Add(Enums.enum_item.material_gold.ToString(), new ItemResorceData("Material\\Mat_Gold", "Gold material used for manufacturing and making money", Enums.enum_item.material_gold));
        resource_data.Add(Enums.enum_item.material_iron.ToString(), new ItemResorceData("Material\\Mat_Iron", "Iron material used for manufacturing and making money", Enums.enum_item.material_iron));
        resource_data.Add(Enums.enum_item.material_ice.ToString(), new ItemResorceData("Material\\Mat_Ice", "Gold material used for manufacturing and making money", Enums.enum_item.material_ice));
        resource_data.Add(Enums.enum_item.material_ilium.ToString(), new ItemResorceData("Material\\Mat_Ilium", "ilium Used for making fuel", Enums.enum_item.material_ilium));
        resource_data.Add(Enums.enum_item.material_neutronium.ToString(), new ItemResorceData("Material\\Mat_Neutonium", "neutonium material used for manufacturing and making money", Enums.enum_item.material_neutronium));
        resource_data.Add(Enums.enum_item.material_plastica.ToString(), new ItemResorceData("Material\\Mat_Plastica", "plastica material used for manufacturing and making money", Enums.enum_item.material_plastica));
        resource_data.Add(Enums.enum_item.module_storage.ToString(), new ItemResorceData("storage", "This is a ship storage", Enums.enum_item.module_storage));
        resource_data.Add(Enums.enum_item.module_double_blaster.ToString(), new ItemResorceData("Modules\\WeponSystems\\WeponSystem-BlasterDouble", "Wepon Fires a Double shot", Enums.enum_item.module_double_blaster));
        resource_data.Add(Enums.enum_item.module_single_blaster.ToString(), new ItemResorceData("Modules\\WeponSystems\\WeponSystem-BlasterSingle", "Wepon Fires a Single shot", Enums.enum_item.module_single_blaster));
        resource_data.Add(Enums.enum_item.module_mining_laser.ToString(), new ItemResorceData("Modules\\WeponSystems\\WeponSystem-Laser", "Used for mining asteroids", Enums.enum_item.module_mining_laser));
        resource_data.Add(Enums.enum_item.module_power_reactor.ToString(), new ItemResorceData("Modules\\Power\\PowerReactor", "Used to power your ship", Enums.enum_item.module_power_reactor));
        resource_data.Add(Enums.enum_item.module_command_module_type1.ToString(), new ItemResorceData("Modules\\CommandModule\\CommandModuleType1", "Type1 command module", Enums.enum_item.module_command_module_type1));
        resource_data.Add(Enums.enum_item.module_main_engine.ToString(), new ItemResorceData("Modules\\Engines\\MainEngine", "Main enging use to make your ship go", Enums.enum_item.module_main_engine));
        resource_data.Add(Enums.enum_item.module_thruster.ToString(), new ItemResorceData("Modules\\Engines\\Thruster", "Thrusters used to maneuver your ship", Enums.enum_item.module_thruster));
        resource_data.Add(Enums.enum_item.module_refiner.ToString(), new ItemResorceData("Modules\\Refiner\\Refiner", "Used to refine materials", Enums.enum_item.module_refiner));
        resource_data.Add(Enums.enum_item.module_tracktor_beam.ToString(), new ItemResorceData("Modules\\TracktorBeam\\TracktorBeam", "This is a Tracktor Beam", Enums.enum_item.module_tracktor_beam));
        resource_data.Add(Enums.enum_item.asset_asteroid_large.ToString(), new ItemResorceData("GameAssets\\Asteroid_large", "This is a Tracktor Beam", Enums.enum_item.asset_asteroid_large));
        resource_data.Add(Enums.enum_item.asset_asteroid_med.ToString(), new ItemResorceData("GameAssets\\Asteroid_med", "This is a Tracktor Beam", Enums.enum_item.asset_asteroid_med));
        resource_data.Add(Enums.enum_item.asset_small_enemy.ToString(), new ItemResorceData("GameAssets\\SmallEnemy", "Small Enemy", Enums.enum_item.asset_small_enemy));
        resource_data.Add(Enums.enum_item.asset_large_enemy.ToString(), new ItemResorceData("GameAssets\\LargeEnemy", "Large Enemy", Enums.enum_item.asset_large_enemy));
        resource_data.Add(Enums.enum_item.asset_player.ToString(), new ItemResorceData("GameAssets\\LargeEnemy", "Player", Enums.enum_item.asset_player));
    }

    public string GetDescription() {
        ItemResorceData item = (ItemResorceData)resource_data[Item_type.ToString()];
        return item.description;
    }

    public string GetResorceLocation() {
        ItemResorceData item = (ItemResorceData)resource_data[Item_type.ToString()];
        return item.resorce;
    }

    public Enums.enum_item GetItemType() {
        ItemResorceData item = (ItemResorceData)resource_data[Item_type.ToString()];
        return item.item_type;
    }

    public ItemResorceData GetItem() {
        ItemResorceData item = (ItemResorceData)resource_data[Item_type.ToString()];
        return item;
    }
}