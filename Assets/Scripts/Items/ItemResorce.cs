using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using static Enums;

public class ItemResorce : MonoBehaviour {
   [ReadOnly] [SerializeField] public Enums.enum_item Item_type;
    [SerializeField] Recipe Item_Data;
    private void OnValidate() {
        if (Item_Data != null) { Item_type = Item_Data.item_type; }
       
    }

    public struct ItemResorceData {
        public string resorce;
        public string description;
        public bool need_refining;
        public Enums.enum_item item_type;
        public Enums.enum_resorce_type resorce_type;

        public ItemResorceData(string resorce, string description, Enums.enum_item item_type, Enums.enum_resorce_type resorce_type,bool need_refining) {
            this.resorce = resorce;
            this.description = description;
            this.item_type = item_type;
            this.resorce_type = resorce_type;
            this.need_refining = need_refining;
        }
    }

  
    public ItemResorceData Spawn_Any_Module_Upgrade_Material() {
        ItemResorceData item;
        do {
            int index = Random.Range(0,UnityFunctions.resource_data.Count);
            item=(ItemResorceData)UnityFunctions.resource_data[index];
        } while (item.resorce_type == enum_resorce_type.asset|| item.item_type == enum_item.module_command_module_type1 || item.item_type == enum_item.pickup);

        return item;

    }
    public string GetDescription() {
        return UnityFunctions.GetItemTypeDescription(Item_type);
    }

    public string GetResorceLocation() {
        return UnityFunctions.GetItemTypeResorceLocation(Item_type);
    }
    public Enums.enum_item GetItemType() {
        return UnityFunctions.GetItemType(Item_type);
    }
    public Enums.enum_resorce_type GetResorceType() {
        return UnityFunctions.GetItemTypeResorceType(Item_type);
    }

    public bool GetNeedsRefining() {
        return UnityFunctions.GetItemTypeNeedsRefining(Item_type);
    }
    public ItemResorceData GetItem() {
        return UnityFunctions.GetItemTypeItem(Item_type);
    }
}