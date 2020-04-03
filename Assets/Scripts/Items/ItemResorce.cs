using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using static Enums;

public class ItemResorce : MonoBehaviour {
    [SerializeField] public Enums.enum_item Item_type;

    public struct ItemResorceData {
        public string resorce;
        public string description;
        public Enums.enum_item item_type;
        public Enums.enum_resorce_type resorce_type;

        public ItemResorceData(string resorce, string description, Enums.enum_item item_type, Enums.enum_resorce_type resorce_type) {
            this.resorce = resorce;
            this.description = description;
            this.item_type = item_type;
            this.resorce_type = resorce_type;
        }
    }

    private OrderedDictionary resource_data = new OrderedDictionary();

    private void Start() {
        resource_data.Add(Enums.enum_item.material_gold.ToString(), new ItemResorceData("Material\\Mat_Gold", "Gold material used for manufacturing and making money", Enums.enum_item.material_gold, Enums.enum_resorce_type.material));
        resource_data.Add(Enums.enum_item.material_iron.ToString(), new ItemResorceData("Material\\Mat_Iron", "Iron material used for manufacturing and making money", Enums.enum_item.material_iron, Enums.enum_resorce_type.material));
        resource_data.Add(Enums.enum_item.material_ice.ToString(), new ItemResorceData("Material\\Mat_Ice", "Gold material used for manufacturing and making money", Enums.enum_item.material_ice, Enums.enum_resorce_type.material));
        resource_data.Add(Enums.enum_item.material_ilium.ToString(), new ItemResorceData("Material\\Mat_Ilium", "ilium Used for making fuel", Enums.enum_item.material_ilium, Enums.enum_resorce_type.material));
        resource_data.Add(Enums.enum_item.material_neutronium.ToString(), new ItemResorceData("Material\\Mat_Neutonium", "neutonium material used for manufacturing and making money", Enums.enum_item.material_neutronium, Enums.enum_resorce_type.material));
        resource_data.Add(Enums.enum_item.material_plastica.ToString(), new ItemResorceData("Material\\Mat_Plastica", "plastica material used for manufacturing and making money", Enums.enum_item.material_plastica, Enums.enum_resorce_type.material));
        //resource_data.Add(Enums.enum_item.module_storage.ToString(), new ItemResorceData("storage", "This is a ship storage", Enums.enum_item.module_storage, Enums.enum_resorce_type.module));
        resource_data.Add(Enums.enum_item.module_double_blaster.ToString(), new ItemResorceData("Modules\\WeponSystems\\WeponSystem-BlasterDouble", "Wepon Fires a Double shot", Enums.enum_item.module_double_blaster, Enums.enum_resorce_type.module));
        resource_data.Add(Enums.enum_item.module_single_blaster.ToString(), new ItemResorceData("Modules\\WeponSystems\\WeponSystem-BlasterSingle", "Wepon Fires a Single shot", Enums.enum_item.module_single_blaster, Enums.enum_resorce_type.module));
        resource_data.Add(Enums.enum_item.module_turret.ToString(), new ItemResorceData("Modules\\WeponSystems\\WeponSystem-Turret", "Rotation", Enums.enum_item.module_turret, Enums.enum_resorce_type.module));
        resource_data.Add(Enums.enum_item.pickup.ToString(), new ItemResorceData("Material\\Pickup", "Spawn Modules and Blue Prints or Material", Enums.enum_item.pickup, Enums.enum_resorce_type.pickup));
        resource_data.Add(Enums.enum_item.module_mining_laser.ToString(), new ItemResorceData("Modules\\WeponSystems\\WeponSystem-Laser", "Used for mining asteroids", Enums.enum_item.module_mining_laser, Enums.enum_resorce_type.module));
        resource_data.Add(Enums.enum_item.module_power_reactor.ToString(), new ItemResorceData("Modules\\Power\\PowerReactor", "Used to power your ship", Enums.enum_item.module_power_reactor, Enums.enum_resorce_type.module));
        resource_data.Add(Enums.enum_item.module_command_module_type1.ToString(), new ItemResorceData("Modules\\CommandModule\\CommandModuleType1", "Type1 command module", Enums.enum_item.module_command_module_type1, Enums.enum_resorce_type.module));
        resource_data.Add(Enums.enum_item.module_main_engine.ToString(), new ItemResorceData("Modules\\Engines\\MainEngine", "Main enging use to make your ship go", Enums.enum_item.module_main_engine, Enums.enum_resorce_type.module));
        resource_data.Add(Enums.enum_item.module_thruster.ToString(), new ItemResorceData("Modules\\Engines\\Thruster", "Thrusters used to maneuver your ship", Enums.enum_item.module_thruster, Enums.enum_resorce_type.module));
        resource_data.Add(Enums.enum_item.module_refiner.ToString(), new ItemResorceData("Modules\\Refiner\\Refiner", "Used to refine materials", Enums.enum_item.module_refiner, Enums.enum_resorce_type.module));
        resource_data.Add(Enums.enum_item.module_tracktor_beam.ToString(), new ItemResorceData("Modules\\TracktorBeam\\TracktorBeam", "This is a Tracktor Beam", Enums.enum_item.module_tracktor_beam, Enums.enum_resorce_type.module));
        resource_data.Add(Enums.enum_item.asset_asteroid_large.ToString(), new ItemResorceData("GameAssets\\Asteroid\\Asteroid_large", "This is a Tracktor Beam", Enums.enum_item.asset_asteroid_large, Enums.enum_resorce_type.asset));
        resource_data.Add(Enums.enum_item.asset_asteroid_med.ToString(), new ItemResorceData("GameAssets\\Asteroid\\Asteroid_med", "This is a Tracktor Beam", Enums.enum_item.asset_asteroid_med, Enums.enum_resorce_type.asset));
        resource_data.Add(Enums.enum_item.asset_small_enemy.ToString(), new ItemResorceData("GameAssets\\Enemy\\SmallEnemy", "Small Enemy", Enums.enum_item.asset_small_enemy, Enums.enum_resorce_type.asset));
        resource_data.Add(Enums.enum_item.asset_large_enemy.ToString(), new ItemResorceData("GameAssets\\Enemy\\LargeEnemy", "Large Enemy", Enums.enum_item.asset_large_enemy, Enums.enum_resorce_type.asset));
        resource_data.Add(Enums.enum_item.asset_player.ToString(), new ItemResorceData("GameAssets\\LargeEnemy", "Player", Enums.enum_item.asset_player, Enums.enum_resorce_type.asset));
    }


    public ItemResorceData Spawn_Any_Module_Upgrade_Material() {
        ItemResorceData item;
        do {
            int index = Random.Range(0, resource_data.Count);
            item=(ItemResorceData)resource_data[index];
        } while (item.resorce_type == enum_resorce_type.asset|| item.item_type == enum_item.module_command_module_type1 || item.item_type == enum_item.pickup);

        return item;

    }
    public string GetDescription() {

        if ( resource_data.Contains(Item_type.ToString())){
            ItemResorceData item = (ItemResorceData)resource_data[Item_type.ToString()];
            return item.description;
        } else {
            return "";
        }

       
    }

    public string GetResorceLocation() {
        ItemResorceData item = (ItemResorceData)resource_data[Item_type.ToString()];
        return item.resorce;
    }
    public Enums.enum_item GetItemType() {
        ItemResorceData item = (ItemResorceData)resource_data[Item_type.ToString()];
        return item.item_type;
    }
    public Enums.enum_resorce_type GetResorceType() {
        ItemResorceData item = (ItemResorceData)resource_data[Item_type.ToString()];
        return item.resorce_type;
    }

    public ItemResorceData GetItem() {
        ItemResorceData item = (ItemResorceData)resource_data[Item_type.ToString()];
        return item;
    }
}