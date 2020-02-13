using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This Class is to handle managing of items.
/// This will be used to store and Equip
/// </summary>
public class InventoryManager : MonoBehaviour {

    #region Class

    public class Item {

        /// <summary>
        /// Class to store module or material
        /// </summary>
        public Enums.enum_item item_type; //Enum of the item type

        public GameObject item; //Ref to the item

        public Item(Enums.enum_item item_type) {
            this.item_type = item_type;
            this.item = null;
        }

        public Item(Enums.enum_item item_type, GameObject item) {
            this.item_type = item_type;
            this.item = item;
        }
    }

    #endregion Class

    #region Fields

    [Header("Storage ")]
    [SerializeField] private int max_storage_items = 10; //Max items that can be stored in our inventory

    private List<InventoryManager.Item> inventory = new List<InventoryManager.Item>();// Inventory items

    [Header("Inventory")]
    [SerializeField] private GameObject inventory_panel; // This is used in the config screen for object

    [SerializeField] private InventoryItem inventory_item; //

    [Header("Mount Point")]
    [SerializeField] private GameObject mount_point_drop_zone;

    [SerializeField] private List<GameObject> mount_point_panels;//Panels the lists are on
    private GameObject[] mount_point_drop_zone_list;//Panels the lists are on

    #endregion Fields

    private void Start() {
        //Lets load out config
        if (inventory_panel != null && inventory_item != null) {
            Build_Inventory_List_Items();
            Build_Mount_Point_Drop_Panels();
            Populate_Mount_Point_Drop_Panels();
        }
    }

    public float Get_Total_Stored_Item_Mass() {
        return 1;
    }

    public void Set_Max_Storage(int max) {
        max_storage_items = max;
    }

    public void Store_Material(Enums.enum_item material) {
        inventory.Add(new InventoryManager.Item(material));
    }

    public ModuleSystemInfo[] GetStoredItems() {
        GameObject Stored_Modules = GameObject.Find("Stored_Modules");
        return Stored_Modules.GetComponentsInChildren<ModuleSystemInfo>();
    }

    public ModuleSystemInfo[] GeEquipedItems() {
        GameObject Stored_Modules = GameObject.Find("Modules");
        return Stored_Modules.GetComponentsInChildren<ModuleSystemInfo>();
    }

    public int Item_Count(Enums.enum_item material) {
        var res = (from n in inventory where n.item_type == material select n).Count();
        return res;
    }

    //This adds item to the store
    public void Store_Module(GameObject module) {
        ModuleSystemInfo ms = module.GetComponent<ModuleSystemInfo>();
        ItemResorce ir = module.GetComponent<ItemResorce>();
        ms.StoreItem();
        GameObject Stored_Modules = GameObject.Find("Stored_Modules");
        module.transform.parent = Stored_Modules.transform;
        inventory.Add(new InventoryManager.Item(ir.Item_type, module));
    }

    //Equips the item

    public void Equip(MountPoint mp, GameObject obj) {
        GameObject Modules = GameObject.Find("Modules");
        obj.transform.parent = Modules.transform;
        obj.transform.position = mp.transform.position;
        obj.transform.rotation = mp.transform.rotation;
        // obj.transform.localScale = mp.transform.localScale;
        SpriteRenderer sr = obj.GetComponentInChildren<SpriteRenderer>();
        if (sr != null) {
            sr.sortingOrder = mp.render_order;
        }
        ModuleSystemInfo ms = obj.GetComponent<ModuleSystemInfo>();
        ms.mount_point = mp.index;
        ms.order_layer = mp.render_order;
        ms.is_in_storage = false;
        ms.UseItem();
    }

    //public void

    private void Populate_Mount_Point_Drop_Panels() {
        ModuleSystemInfo[] modules = GeEquipedItems();
        foreach (ModuleSystemInfo module in modules) {
            if (module.is_internal_module == false) {
                if (module.mount_point > 0) {
                    GameObject item = Instantiate(inventory_item.gameObject, inventory_panel.transform);

                   
                    item.transform.parent = mount_point_drop_zone_list[module.mount_point].transform;
                    //GameObject item = Instantiate(inventory_item.gameObject, inventory_panel.transform);
                    //Lets config it
                    InventoryItem inv = item.GetComponent<InventoryItem>();
                    inv.SetItem(module.gameObject);
            
                    //this.Equip(,inv.item);

                }
            }
        }
    }

    public void Build_Inventory_List_Items() {
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

    private void Build_Mount_Point_Drop_Panels() {
        //****************************
        //Get the list of mount points
        //****************************

        GameObject mount_points = GameObject.Find("MountPoints");//This find the moun points on our command module
        ShipModule sys = mount_points.GetComponentInParent<ShipModule>();
        mount_point_drop_zone_list = new GameObject[sys.mount_points.Count];
        foreach (MountPoint m in sys.mount_points) {
            GameObject g = Instantiate(mount_point_drop_zone, mount_point_panels[(int)m.zone].gameObject.transform);
            mount_point_drop_zone_list[m.index] = g.transform.Find("DropZone").gameObject;
            MountPoint omp = g.GetComponent<MountPoint>();
            omp.SetValues(m);
            omp.SetSize(new Vector2(100 + (m.max_mounting * 50), 50));
            omp.associated_mountpoint = m.gameObject;
            ItemDropHandler dh = g.GetComponentInChildren<ItemDropHandler>();
            dh.enforce_max = true;
            dh.max_items = m.max_mounting;
        }
    }
}