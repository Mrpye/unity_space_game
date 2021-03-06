﻿using System;
using System.IO;
using UnityEngine;

public class LoadSave : MonoBehaviour {
    [SerializeField] private bool is_config = false;
    [SerializeField] private float scale = 1f;
    private Vector3 stored_pos;
    private Quaternion stored_rotation;

    private void Awake() {
        LoadPlayer();
        UnityFunctions.PopulateCommonVariables();
    }

    private void Update() {
        if (is_config == true) {
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    public void SavePlayer() {
        PlayerSaveModel player_model = new PlayerSaveModel();
        player_model.ReadData(gameObject);
        if (is_config) {
            player_model.position = stored_pos;
            player_model.roation = stored_rotation;
        }

        //************
        //Used Modules
        //************
        GameObject go = UnityFunctions.modules;
        ModuleSystemInfo[] modules = go.GetComponentsInChildren<ModuleSystemInfo>();
        foreach (ModuleSystemInfo e in modules) {
            ModuleSaveModel m = new ModuleSaveModel();
            m.ReadData(e.gameObject);
            player_model.modules.Add(m);
        }

       

        //**************
        //Stored Modules
        //**************
        //go = GameObject.Find("Stored_Modules");
        modules = UnityFunctions.stored_modules.GetComponentsInChildren<ModuleSystemInfo>();
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
        UnityFunctions.PopulateItemResorces();

        PlayerSaveModel player_model = PlayerSaveModel.LoadPlayer();
        ShipManagment ship_mamanmger = gameObject.GetComponent<ShipManagment>();

        ship_mamanmger.fuel = player_model.fuel;
        ship_mamanmger.battery = player_model.battery;

        if (is_config == false) {
            transform.position = player_model.position;
            transform.rotation = player_model.roation;
        } else {
            stored_pos = player_model.position;
            stored_rotation = player_model.roation;
        }

        //*************
        //Load invetory
        //*************
        InventoryManager inv = gameObject.GetComponent<InventoryManager>();
        inv.inventory.Clear();
        if(inv!=null && player_model.inventory != null) {
            foreach (Enums.enum_item e in player_model.inventory) {
                //if (UnityFunctions.GetItemTypeResorceType(e) == Enums.enum_resorce_type.blueprint) {
                    //inv.blueprints.Add(new InventoryManager.Item(e));
                //} else {
                   inv.inventory.Add(new InventoryManager.Item(e));
               // }
               
            }
        }

        //****************
        //Load blue prints
        //****************
        //inv.blueprints.Clear();
        if (inv != null && player_model.blueprints != null) {
            foreach (Enums.enum_item e in player_model.blueprints) {
               
                inv.blueprints.Add(new InventoryManager.Item(e));
            }
        }


        //*****************
        //Load the upgrades
        //*****************
        ship_mamanmger.stored_upgrades.Clear();
        foreach (string e in player_model.stored_upgrades) {
            Upgrade_Settings refab = Resources.Load(e) as Upgrade_Settings;
            ship_mamanmger.stored_upgrades.Add(refab);
         }

        //********************************************
        //Loop through the modules in our player model
        //********************************************
        foreach (ModuleSaveModel module in player_model.modules) {
            //****************
            //Create our modue
            //****************
            GameObject new_module = Create_Module(module);
            if (new_module != null) {
                //************
                //Add upgrades
                //************
                ship_mamanmger.Equip(new_module);
            }
        }

        //********************************************
        //Loop through the modules in our player model
        //********************************************
       
        foreach (ModuleSaveModel module in player_model.stored_modules) {
            //****************
            //Create our modue
            //****************
            GameObject new_module=null;
            try {
                 new_module = Create_Module(module);
                
            } catch (Exception ex) {
                Debug.Log(ex);
            }
            if (new_module != null) {
                ship_mamanmger.Store_Module(new_module);
            }

        }
    }

    public GameObject Create_Module(ModuleSaveModel model) {
        //GameObject refab = Resources.Load(model.module_name.ToString()) as GameObject;
        Recipe r=  UnityFunctions.GetItemTypeItem(model.item_type);
        if (r.preFab != null) {
            GameObject modules = GameObject.Find("Modules");
            GameObject stored_modules = GameObject.Find("Stored_Modules");
            ShipModule sm = modules.GetComponentInChildren<ShipModule>();

            GameObject obj_module = null;

            if (sm == null) {
                //Debug.Log("Loading Model");
                obj_module = Instantiate(r.preFab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
                sm = obj_module.GetComponent<ShipModule>();
                if (sm != null) {
                    sm.LoadMountPoints();
                }
            } else {
                if (model.is_internal_module == false) {
                    MountPoint mp = null;
                    if (model.is_in_storage == false) {
                        mp = sm.mount_points[0];
                    } else {
                        mp = sm.mount_points[model.mount_point - 1];
                    }
                    
                    if (mp != null) {
                        obj_module = Instantiate(r.preFab, mp.transform.position, mp.transform.rotation) as GameObject;
                    }
                } else {
                    obj_module = Instantiate(r.preFab, stored_modules.transform) as GameObject;
                }
            }

            //Need to add the keybindings
            if (obj_module == null) { return null; }
            ModuleSystemInfo mod_sys = obj_module.GetComponent<ModuleSystemInfo>();
            ItemResorce ir = obj_module.GetComponent<ItemResorce>();
            //mod_sys.key_mappings = model.key_mappings;
            //mod_sys.id = model.id;
            mod_sys.current_health = model.health;
            mod_sys.mount_point = model.mount_point;
            mod_sys.order_layer = model.order_layer;
            mod_sys.is_internal_module = model.is_internal_module;
            mod_sys.active = model.enabled;
            mod_sys.upgrades.Clear();
            //Load upgrades
            foreach (string s in model.upgrades) {
                Upgrade_Settings upgrade = Resources.Load(s) as Upgrade_Settings;
                mod_sys.upgrades.Add(upgrade);
            }
            return obj_module;
        } else {
            return null;
        }
    }
}