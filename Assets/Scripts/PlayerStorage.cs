using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is used for storing items such as materials or plans
/// </summary>
public class PlayerStorage : MonoBehaviour {

    public enum enum_item {
        gold,
        iron,
        ice,
        ilium,
        neutonium,
        plastica
    }

    [SerializeField] int max_storage_items = 10;
    private List<enum_item> storage = new List<enum_item> ();



    public void Set_Max_Storage(int max) {
        max_storage_items = max;
    }
    public void Store_Material(enum_item material) {
        storage.Add(material);
    }

    public int Item_Count(enum_item material) {
        var res = (from n in storage where n == material select n).Count();
        return res;
    }

}

