﻿using System.IO;
using UnityEngine;

public class LoadSave : MonoBehaviour {
    [SerializeField] private bool is_config = false;
    [SerializeField] private float scale = 1f;

    private void Awake() {
        LoadPlayer();
    }

    private void Update() {
        if (is_config == true) {
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    /* public void SavePlayer() {
         PlayerSaveModel sm = new PlayerSaveModel();
         sm.Set_Object(gameObject);
         string a = sm.SavePlayer();
     }*/

    public void SavePlayer() {
        PlayerSaveModel player_model = new PlayerSaveModel();
        player_model.ReadData(gameObject);

        //Save Modules
        GameObject go = GameObject.Find("Modules");
        ModuleSystemInfo[] modules = go.GetComponentsInChildren<ModuleSystemInfo>();
        foreach (ModuleSystemInfo e in modules) {
            ModuleSaveModel m = new ModuleSaveModel();
            m.ReadData(e.gameObject);
            player_model.modules.Add(m);
        }

        //Stored Modules
        go = GameObject.Find("Stored_Modules");
        modules = go.GetComponentsInChildren<ModuleSystemInfo>();
        foreach (ModuleSystemInfo e in modules) {
            ModuleSaveModel m = new ModuleSaveModel();
            m.ReadData(e.gameObject);
            player_model.stored_modules.Add(m);
        }
        string json = JsonUtility.ToJson(player_model);
        File.WriteAllText(Application.persistentDataPath + "/player_config.save", json);
    }

    public static void WriteLine(object obj) {
        Debug.Log(JsonUtility.ToJson(obj));
    }

    public void LoadPlayer() {
        PlayerSaveModel player_model = PlayerSaveModel.LoadPlayer();
        SystemInfo sys = gameObject.GetComponent<SystemInfo>();
        WriteLine(player_model);
        //********************************************
        //Loop through the modules in our player model
        //********************************************
        foreach (ModuleSaveModel module in player_model.modules) {
            WriteLine(module);
            //****************
            //Create our modue
            //****************
            GameObject new_module = Create_Module(module);
            if (new_module != null) {
                sys.AddModule(new_module, false);
            }
        }

        //********************************************
        //Loop through the modules in our player model
        //********************************************
        InventoryManager storage = gameObject.GetComponent<InventoryManager>();
        foreach (ModuleSaveModel module in player_model.stored_modules) {
            //****************
            //Create our modue
            //****************
            GameObject new_module = Create_Module(module);
            if (new_module != null) {
                storage.Store_Module(new_module);
            }
        }
    }

    public GameObject Create_Module(ModuleSaveModel model) {
        GameObject refab = Resources.Load(model.module_name.ToString()) as GameObject;
        if (refab != null) {
            GameObject modules = GameObject.Find("Modules");
            ShipModule sm = modules.GetComponentInChildren<ShipModule>();

            GameObject obj_module = null;

            if (sm == null) {
                //Debug.Log("Loading Model");
                obj_module = Instantiate(refab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
                sm = obj_module.GetComponent<ShipModule>();
                if (sm != null) {
                    sm.LoadMountPoints();
                }
            } else {
                if (model.is_internal_module == false) {
                    MountPoint mp = sm.mount_points[model.mount_point - 1];
                    if (mp != null) {
                        obj_module = Instantiate(refab, mp.transform.position, mp.transform.rotation) as GameObject;
                        //obj_module.transform.localScale = new Vector3(1, 1, 1);
                        //obj_module.transform.parent = Modules.transform;
                        //obj_module.transform.position = mp.transform.position;
                        // obj_module.transform.rotation = mp.transform.rotation;
                        // obj_module.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
            }

            //Need to add the keybindings
            if (obj_module == null) { return null; }

            ModuleSystemInfo mod_sys = obj_module.GetComponent<ModuleSystemInfo>();
            mod_sys.key_mappings = model.key_mappings;
            mod_sys.ModuleName = model.module_name;
            mod_sys.id = model.id;
            mod_sys.mount_point = model.mount_point;
            mod_sys.order_layer = model.order_layer;
            mod_sys.is_internal_module = model.is_internal_module;
            mod_sys.UseItem();

            //********************
            //Sort the order layer
            //********************
            SpriteRenderer sr = obj_module.GetComponent<SpriteRenderer>();
            if (sr != null) { sr.sortingOrder = model.order_layer; }
            sr = obj_module.GetComponentInChildren<SpriteRenderer>();
            if (sr != null) { sr.sortingOrder = model.order_layer; }
            LineRenderer lr = obj_module.GetComponent<LineRenderer>();
            if (lr != null) { lr.sortingOrder = model.order_layer; }

            return obj_module;
        } else {
            return null;
        }
    }
}