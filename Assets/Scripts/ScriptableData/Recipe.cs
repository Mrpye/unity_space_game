using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe", order = 1)]
public class Recipe : ScriptableObject
{

    [SerializeField] public string recipe_name = "";
    [SerializeField] public Enums.enum_item item_type;
    [SerializeField] public int make_time = 10;
    [SerializeField] public int ammount;
    [SerializeField] public List<Ingreadient> ingredients = new List<Ingreadient>();



    [Serializable()]
    public class Ingreadient {
        [SerializeField] public Enums.enum_item item_type;
        [SerializeField] public int qty;
      
        public Ingreadient(Enums.enum_item item_type,int qty) {
            this.item_type = item_type;
            this.qty = qty;
        }
    }
   
}
