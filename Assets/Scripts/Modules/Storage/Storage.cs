using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is used for storing items such as materials or plans
/// </summary>
public class Storage : MonoBehaviour {

    public class Item {
        public Enums.enum_item item_type;
        public GameObject item;
        public Item(Enums.enum_item item_type) {
            this.item_type = item_type;
            this.item = null;
        }
        public Item(Enums.enum_item item_type,GameObject item) {
            this.item_type = item_type;
             this.item = item;
        }
    }

    [SerializeField] int max_storage_items = 10;
    private List<Storage.Item> storage = new List<Storage.Item> ();


    public float Get_Total_Stored_Item_Mass() {
        return 1;
    }
    public void Set_Max_Storage(int max) {
        max_storage_items = max;
    }

    public void Store_Material(Enums.enum_item material) {
        storage.Add(new Storage.Item(material));
    }

    public void Store_Module(GameObject module) {
        //Lets 
        ModuleSystemInfo ms = module.GetComponent<ModuleSystemInfo>();
        ItemResorce ir = module.GetComponent<ItemResorce>();
        ms.StoreItem();
        GameObject Stored_Modules = GameObject.Find("Stored_Modules");
        module.transform.parent = Stored_Modules.transform;
        storage.Add(new Storage.Item(ir.Item_type, module));
    }
    public ModuleSystemInfo[] GetSToredItems() {
        GameObject Stored_Modules = GameObject.Find("Stored_Modules");
        return Stored_Modules.GetComponentsInChildren<ModuleSystemInfo>();
    }
    public int Item_Count(Enums.enum_item material) {
        var res = (from n in storage where n.item_type == material select n).Count();
        return res;
    }

}

