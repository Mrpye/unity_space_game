using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Class is to handle managing of items.
/// This will be used to store and Equip
/// </summary>

public class InventoryManager : MonoBehaviour {

    #region Class

    [Serializable]
    public class Item {

        #region Public Fields

        public GameObject item;

        /// <summary>
        /// Class to store module or material
        /// </summary>
        public Enums.enum_item item_type;

        #endregion Public Fields

        //Enum of the item type

        //Ref to the item

        #region Public Constructors

        public Item(Enums.enum_item item_type) {
            this.item_type = item_type;
            this.item = null;
        }

        public Item(Enums.enum_item item_type, GameObject item) {
            this.item_type = item_type;
            this.item = item;
        }

        #endregion Public Constructors
    }

    #endregion Class

    #region Fields

    [SerializeField] public List<GameObject> modules = new List<GameObject>();
    [SerializeField] private ShipModule command_module;

    [Header("Storage ")]
    [SerializeField] public List<InventoryManager.Item> inventory = new List<InventoryManager.Item>();// Inventory items

    [SerializeField] private GameObject inventory_item;

    [SerializeField] public List<Upgrade_Settings> stored_upgrades;
    //public List<string> stored_upgrades = new List<string>();

    [Header("Config Inventory")]
    [SerializeField] private GameObject inventory_panel;

    [SerializeField] private int max_storage_items = 10; //Max items that can be stored in our inventory
    private GameObject modules_game_object;

    [Header("Config Mount Point")]
    [SerializeField] private GameObject mount_point_drop_zone;

    //
    [SerializeField] private GameObject mount_point_drop_zone_grid;

    private GameObject[] mount_point_drop_zone_list;

    // This is used in the config screen for object
    [SerializeField] private List<GameObject> mount_point_panels;

    private GameObject Stored_Modules_game_object;
    private GameObject Stored_Upgrades_game_object;
    private GameObject tmp_drop_panel;
    private bool drop_panels_loaded = false;
    //Panels the lists are on
    //Panels the lists are on

    #endregion Fields

    #region Public Methods
    

    public void ClearInvetoryPanel() {
        int childs = inventory_panel.transform.childCount;
        for (int i = childs - 1; i >= 0; i--) {
            Destroy(inventory_panel.transform.GetChild(i).gameObject);
        }
    }
    public void Build_Inventory_List_Items() {
        //******************************
        //Clear the invetory panel first
        //******************************
        ClearInvetoryPanel();
        //*************************
        //Load stored Items in list
        //*************************
        ModuleSystemInfo[] modules = GetStoredItems();
        foreach (ModuleSystemInfo module in modules) {
            GameObject item = Instantiate(inventory_item.gameObject, inventory_panel.transform);
            //Lets config it
            InventoryItem inv = item.GetComponent<InventoryItem>();
            inv.SetItem(module.gameObject);
            item.transform.parent = inventory_panel.transform;
        }
    }

    public void child_Start() {
        // Load_Modules();
        Stored_Modules_game_object = GameObject.Find("Stored_Modules");
        modules_game_object = GameObject.Find("Modules");

        //Lets load out config
        if (inventory_panel != null && inventory_item != null) {
            //Build_Inventory_List_Items();
            Build_Mount_Point_Drop_Panels();
        }
    }

    public void Equip(GameObject obj) {
        if (modules_game_object == null) { modules_game_object = GameObject.Find("Modules"); }
        ModuleSystemInfo module_info = obj.GetComponent<ModuleSystemInfo>();
        ItemResorce ir = obj.GetComponent<ItemResorce>();
        SpaceShipMovment controls = gameObject.GetComponent<SpaceShipMovment>();

        obj.transform.parent = modules_game_object.transform;

        if (ir.Item_type == Enums.enum_item.module_command_module_type1) {
            command_module = obj.GetComponent<ShipModule>(); //First we need to store the item
            command_module.LoadMountPoints();
            MountPoint mp = command_module.mount_points[0];//Set the max storage
            command_module.ShowMountPoints();
            max_storage_items = module_info.max_storage_items;
            mp.max_mounting = max_storage_items;
        } else {
            MountPoint mp = command_module.mount_points[module_info.mount_point];
            obj.transform.position = mp.transform.position;
            obj.transform.rotation = mp.transform.rotation;
            module_info.mount_point = mp.index;
            module_info.key_mappings = mp.key_mappings;
            module_info.SetRenderOrder(mp.render_order);
        }

        foreach (KeyMappingModel e in module_info.key_mappings) {
            controls.AddKeyBinding(e, obj);
        }

        module_info.IteminUse(module_info.is_internal_module);
    }

    public ModuleSystemInfo[] GeEquipedItems() {
        modules_game_object = GameObject.Find("Modules");
        return modules_game_object.GetComponentsInChildren<ModuleSystemInfo>();
    }

    public float Get_Total_Stored_Item_Mass() {
        return 1;
    }

    public ModuleSystemInfo[] GetStoredItems() {
        Stored_Modules_game_object = GameObject.Find("Stored_Modules");
        return Stored_Modules_game_object.GetComponentsInChildren<ModuleSystemInfo>();
    }

    public Upgrade_Settings[] GetStoredUpgrades() {
        Stored_Upgrades_game_object = GameObject.Find("Stored_Upgrades");
        return Stored_Upgrades_game_object.GetComponentsInChildren<Upgrade_Settings>();
    }

    public int Item_Count(Enums.enum_item material) {
        var res = (from n in inventory where n.item_type == material select n).Count();
        return res;
    }

    public void Set_Max_Storage(int max) {
        max_storage_items = max;
    }

    public void Store_Material(Enums.enum_item material) {
        inventory.Add(new InventoryManager.Item(material));
    }

    /// <summary>
    /// This function stores the modules
    /// </summary>
    /// <param name="module"></param>
    public void Store_Module(GameObject module) {
        if (Stored_Modules_game_object == null) { Stored_Modules_game_object = GameObject.Find("Stored_Modules"); }
        ModuleSystemInfo module_info = module.GetComponent<ModuleSystemInfo>();
        if (module_info.is_command_module) {
            if (command_module != null) {
                command_module.HideMountPoints();
            }
            this.command_module = null;
        }
        ItemResorce ir = module.GetComponent<ItemResorce>();
        module_info.IteminStorage();
        module.transform.parent = Stored_Modules_game_object.transform;
        if (!module_info.is_command_module) {
            inventory.Add(new InventoryManager.Item(ir.Item_type, module));
        }
            
    }

    #endregion Public Methods

    //public void

    #region Private Methods

    private void DisableEnableButtons(bool action) {
        GameObject.Find("cmdLaunch").GetComponent<Button>().interactable = action;
        GameObject.Find("cmdInternal").GetComponent<Button>().interactable = action;
        GameObject.Find("cmdFront").GetComponent<Button>().interactable = action;
        GameObject.Find("cmdBottom").GetComponent<Button>().interactable = action;
        GameObject.Find("cmdRight").GetComponent<Button>().interactable = action;
        GameObject.Find("cmdLeft").GetComponent<Button>().interactable = action;
        GameObject.Find("cmdTop").GetComponent<Button>().interactable = action;
        GameObject.Find("cmdCommand").GetComponent<Button>().onClick.Invoke();
    }

    public void SetScreenNoCommandModule() {
        //***********************************
        //First we need to remove all modules
        //***********************************
        ModuleSystemInfo[] modules = GeEquipedItems();
        foreach (ModuleSystemInfo module in modules) {
            this.Store_Module(module.gameObject);
        }

        Build_Inventory_List_Items();

        //********************************
        //We need to remove all drop zones
        //********************************
        foreach (GameObject p in this.mount_point_panels) {
            p.SetActive(false);
            int childs = p.transform.childCount;
            for (int i = childs - 1; i >= 0; i--) {
                GameObject.DestroyImmediate(p.transform.GetChild(i).gameObject);
            }
        }
        //**********************************************
        //Create a panel to handle adding command module
        //**********************************************
        if (!this.tmp_drop_panel) {
            this.mount_point_panels[0].SetActive(true);
            this.tmp_drop_panel = Instantiate(mount_point_drop_zone, this.mount_point_panels[0].transform);
            Populate_Mount_Point_Drop_Panels();
        }
        this.drop_panels_loaded = false;
        //*******************
        //Disable all buttons
        //*******************
        DisableEnableButtons(false);//Disble all the button

       
    }

    public void Build_Mount_Point_Drop_Panels() {
        //****************************
        //Get the list of mount points
        //****************************
        GameObject g = null;

        if (this.command_module == null) {
            //We need to hide all buttons and only show a command
            SetScreenNoCommandModule();
        } else {
            if (this.tmp_drop_panel) { Destroy(this.tmp_drop_panel); }
            if (drop_panels_loaded) { return; }
            GameObject mount_points = this.command_module.transform.Find("MountPoints").gameObject;
            ShipModule sys = mount_points.GetComponentInParent<ShipModule>();
            mount_point_drop_zone_list = new GameObject[sys.mount_points.Count];

            foreach (MountPoint m in sys.mount_points) {
                if (m.zone == Enums.emun_zone.intern) {
                    g = Instantiate(mount_point_drop_zone_grid, mount_point_panels[(int)m.zone].gameObject.transform);
                } else {
                    g = Instantiate(mount_point_drop_zone, mount_point_panels[(int)m.zone].gameObject.transform);
                }

                mount_point_drop_zone_list[m.index] = g.transform.Find("DropZone").gameObject;
                MountPoint omp = g.GetComponent<MountPoint>();
                omp.SetValues(m);
                omp.SetSize(new Vector2(100 + (m.max_mounting * 50), 50));
                omp.mount_type_util_top = m.mount_type_util_top;
                omp.mount_type_util_side = m.mount_type_util_side;
                omp.mount_type_thruster = m.mount_type_thruster;
                omp.mount_type_engine = m.mount_type_engine;

                omp.associated_mountpoint = m.gameObject;
                ItemDropHandler dh = g.GetComponentInChildren<ItemDropHandler>();
                dh.enforce_max = true;
                dh.max_items = m.max_mounting;
            }
            DisableEnableButtons(true);
            Populate_Mount_Point_Drop_Panels();
            Build_Inventory_List_Items();
            drop_panels_loaded = true;
        }
    }

    private void Populate_Mount_Point_Drop_Panels() {
        ModuleSystemInfo[] modules = GeEquipedItems();
        foreach (ModuleSystemInfo module in modules) {
            GameObject item = Instantiate(inventory_item, inventory_panel.transform);
            item.transform.parent = mount_point_drop_zone_list[module.mount_point].transform;
            InventoryItem inv = item.GetComponent<InventoryItem>();
            inv.SetItem(module.gameObject);
        }
    }

    #endregion Private Methods
}