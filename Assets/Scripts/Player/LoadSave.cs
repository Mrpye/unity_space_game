using UnityEngine;

public class LoadSave : MonoBehaviour {
    //[SerializeField] private GameObject player;

    private void Start() {
        LoadPlayer();
    }

    /* public void SavePlayer() {
         PlayerSaveModel sm = new PlayerSaveModel();
         sm.Set_Object(gameObject);
         string a = sm.SavePlayer();
     }*/

    public void LoadPlayer() {
        PlayerSaveModel player_model = PlayerSaveModel.LoadPlayer();
        SystemInfo sys = gameObject.GetComponent<SystemInfo>();

        //********************************************
        //Loop through the modules in our player model
        //********************************************
        foreach (PlayerSaveModel module in player_model.modules) {
            GameObject refab = Resources.Load(module.module_name.ToString()) as GameObject;
            if (refab != null) {
                //****************
                //Create our modue
                //****************
                GameObject new_module = Create_Module(module);
                if (new_module != null) {
                    sys.AddModule(new_module);
                }
            } else {
                Debug.Log("Missing ref");
            }
        }
    }

    public GameObject Create_Module(PlayerSaveModel model) {
        GameObject refab = Resources.Load(model.module_name.ToString()) as GameObject;
        if (refab != null) {
            GameObject obj_module = Instantiate(refab, model.position, model.roation) as GameObject;
            obj_module.transform.localScale = model.scale;
            //Need to add the keybindings
            ModuleSystemInfo mod_sys = obj_module.GetComponent<ModuleSystemInfo>();
            mod_sys.key_mappings = model.key_mappings;
            mod_sys.ModuleName = model.module_name;
            mod_sys.id = model.id;
            SpriteRenderer sr=obj_module.GetComponent<SpriteRenderer>();
            Debug.Log(model.order_layer);
            if (sr != null) { sr.sortingOrder = model.order_layer; }
            LineRenderer lr = obj_module.GetComponent<LineRenderer>();
            if (lr != null) { lr.sortingOrder = model.order_layer; }

            return obj_module;
        } else {
            return null;
        }
    }
}