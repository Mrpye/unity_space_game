using System.IO;
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
        Storage storage = gameObject.GetComponent<Storage>();
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
            GameObject command_module = GameObject.Find("CommandModule (Clone)");

            GameObject obj_module = null;

            if (command_module == null) {
                //Debug.Log("Loading Model");
                obj_module = Instantiate(refab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            } else {
                ShipModule sm = command_module.GetComponent<ShipModule>();
                MountPoint mp = sm.mount_points[model.mount_point];
                if (mp != null) {
                    obj_module = Instantiate(refab, mp.gameObject.transform.position + gameObject.transform.position, mp.gameObject.transform.rotation) as GameObject;
                    // obj_module.transform.localScale = mp.gameObject.transform.localScale;

                    //Need to add the keybindings
                    ModuleSystemInfo mod_sys = obj_module.GetComponent<ModuleSystemInfo>();
                    mod_sys.key_mappings = model.key_mappings;
                    mod_sys.ModuleName = model.module_name;
                    mod_sys.id = model.id;
                    mod_sys.is_internal_module = model.is_internal_module;
                    SpriteRenderer sr = obj_module.GetComponent<SpriteRenderer>();
                    //Debug.Log(model.order_layer);
                    if (sr != null) { sr.sortingOrder = model.order_layer; }
                    LineRenderer lr = obj_module.GetComponent<LineRenderer>();
                    if (lr != null) { lr.sortingOrder = model.order_layer; }
                }
            }

            return obj_module;
        } else {
            return null;
        }
    }
}