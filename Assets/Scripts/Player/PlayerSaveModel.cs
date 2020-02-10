using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class KeyMappingModel {
    public string Key;
    public string mapping_value;
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
        SystemInfo sys = player_prefab.GetComponent<SystemInfo>();
        position = player_prefab.transform.position;
        roation = player_prefab.transform.rotation;
        scale = player_prefab.transform.localScale;
        module_name = sys.name;
    }

    // public void Write_To_Object(GameObject PrefabObj) {
    //    PrefabObj.transform.localScale = this.scale;

    //}

    /*
    public void ReadData(bool is_player) {
        this.is_player = is_player;

        sys = this.PrefabObj.GetComponent<SystemInfo>();
        mod_sys = this.PrefabObj.GetComponent<ModuleSystemInfo>();

        //position = Player.transform.position;
        //roation = Player.transform.rotation;
        //scale = Player.transform.localScale;

        if (sys != null) {
            //This is the player
            this.module_name = sys.name;
        } else if (mod_sys != null) {
            //this is a module
            this.module_name = mod_sys.ModuleName;
            this.key_mappings = mod_sys.key_mappings;
        }
    }*/
    /*
    public string SavePlayer() {
        ReadData(true);

        foreach (GameObject e in sys.modules) {
            PlayerSaveModel m = new PlayerSaveModel();

            m.Set_Object(e);
            m.ReadData(false);
            this.modules.Add(m);
        }

        string json = JsonUtility.ToJson(this);
        File.WriteAllText(Application.persistentDataPath + "/player_save.save", json);

        return json;
    }*/

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
    public void ReadData(GameObject module) {
        ModuleSystemInfo sys = module.GetComponent<ModuleSystemInfo>();
        module_name = sys.ModuleName;
        id = module.name;
        mount_point = sys.mount_point;
        order_layer = sys.order_layer;
        is_in_storage = sys.is_in_storage;
        is_internal_module = sys.is_internal_module;
    }



}