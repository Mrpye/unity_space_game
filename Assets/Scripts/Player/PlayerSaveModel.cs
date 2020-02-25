using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class KeyMappingModel {
    public string Key;
    public Enums.enum_movment_type movement_type = Enums.enum_movment_type.none;
    public float value;
}

[Serializable]
public class PlayerSaveModel {
    public string module_name;//Store the location of prefab and name
    public Vector3 position;
    public Vector3 scale;
    public Quaternion roation;
    public int order_layer;
    public List<ModuleSaveModel> modules = new List<ModuleSaveModel>();
    public List<ModuleSaveModel> stored_modules = new List<ModuleSaveModel>();
    public bool is_player = true;

    public void ReadData(GameObject player_prefab) {
        ShipManagment ship_mamanmger = player_prefab.GetComponent<ShipManagment>();
        position = player_prefab.transform.position;
        roation = player_prefab.transform.rotation;
        scale = player_prefab.transform.localScale;
        module_name = ship_mamanmger.name;
    }

    public static PlayerSaveModel LoadPlayer() {
        //Debug.Log(Application.persistentDataPath);
        string data = File.ReadAllText(Application.persistentDataPath + "/player_config.save");
        PlayerSaveModel gameSaving = JsonUtility.FromJson<PlayerSaveModel>(data);
        return gameSaving;
    }
}

[Serializable]
public class ModuleSaveModel {
    public List<KeyMappingModel> key_mappings = new List<KeyMappingModel>();
    public string id;//used to hep idetify the module if there are multiple
    public string module_name;//Store the location of prefab and name
    public int order_layer;
    public int mount_point;
    public bool is_in_storage;
    public bool is_internal_module;
    public float health;

    public void ReadData(GameObject module) {
        ModuleSystemInfo sys = module.GetComponent<ModuleSystemInfo>();
        ItemResorce ir = module.GetComponent<ItemResorce>();

        module_name = ir.GetResorceLocation();
        id = module.name;
        mount_point = sys.mount_point;
        health = sys.current_health;
        order_layer = sys.order_layer;
        is_in_storage = sys.is_in_storage;
        is_internal_module = sys.is_internal_module;
    }
}