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

    [System.NonSerialized]
    private GameObject PrefabObj;

    [System.NonSerialized]
    private SystemInfo sys;

    [System.NonSerialized]
    private ModuleSystemInfo mod_sys;




    public List<KeyMappingModel> key_mappings = new List<KeyMappingModel>();
    public string id;//used to hep idetify the module if there are multiple
    public string module_name;//Store the location of prefab and name
    public Vector3 position;
    public Vector3 scale;
    public Quaternion roation;
    public int order_layer;
    //public string key;
    // public string value_mapping;
    //public string value;

    public List<PlayerSaveModel> modules = new List<PlayerSaveModel>();
    public bool is_player = false;
    

   // public void Set_Object(GameObject PrefabObj) {
   //     this.PrefabObj = PrefabObj;
   // }

    public void Write_To_Object(GameObject PrefabObj) {

        PrefabObj.transform.localScale = this.scale;

    }

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

        Debug.Log(Application.persistentDataPath);
        string data = File.ReadAllText(Application.persistentDataPath + "/player_config.save");
        PlayerSaveModel gameSaving = JsonUtility.FromJson<PlayerSaveModel>(data);
        return gameSaving;
    }
}