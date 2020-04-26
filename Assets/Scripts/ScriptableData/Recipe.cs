using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Recipe", menuName = "Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    [Header("Basic Info")]
    [ReadOnly] public string recipe_name = "";
    [SerializeField] public Enums.enum_item item_type;
    [SerializeField] public Enums.enum_resorce_type resorce_type;
    [SerializeField] public Sprite sprite;
    [MultilineAttribute] [SerializeField] public string description;
    [SerializeField] public GameObject preFab;


    [Header("Blueprint Info")]
    [SerializeField] public bool blueprint;
    [SerializeField] public Sprite blueprint_sprite;
    [SerializeField] public Enums.enum_item required_blueprint;

    [Header("Recipe Info")]
    [SerializeField] public int make_time = 10;
    [SerializeField] public bool need_refining;
    [SerializeField] public int ammount;
    [SerializeField] public List<Ingreadient> ingredients = new List<Ingreadient>();




    private void OnValidate() {
        recipe_name = item_type.ToString();
    }

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
