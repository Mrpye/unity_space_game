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
    #endregion

    #region    Fields
    [Header("Storage ")]
    [SerializeField] int max_storage_items = 10; //Max items that can be stored in our inventory
    private List<InventoryManager.Item> inventory = new List<InventoryManager.Item>();// Inventory items

    [Header("Config List")]
    [SerializeField] private GameObject available_list; // This is used in the config screen for object 
    [SerializeField] private InventoryItem inventory_item; // 
    #endregion


    private void Start() {
        //Lets load out config
        if (available_list != null && inventory_item != null) {
            Populate_Items();
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
        GameObject Stored_Modules = GameObject.Find("Stored_Modules");
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
        if(sr != null){
            sr.sortingOrder = mp.render_order;
        }
        ModuleSystemInfo ms = obj.GetComponent<ModuleSystemInfo>();
        ms.mount_point = mp.index;
        ms.is_in_storage = false;
        ms.UseItem();
    }

    //public void 

    public void Populate_Items() {
        //*************************
        //Load stored Items in list
        //*************************
        ModuleSystemInfo[] modules = GetStoredItems();
        foreach (ModuleSystemInfo module in modules) {
            GameObject item = Instantiate(inventory_item.gameObject, available_list.transform);
            //Lets config it
            InventoryItem inv = item.GetComponent<InventoryItem>();
            inv.SetItem(module.gameObject);
        }



    }

}

