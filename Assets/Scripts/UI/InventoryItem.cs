using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryItem : MonoBehaviour
{
    /*
    module_storage,
        module_single_blaster,
        module_double_blaster,
        module_mining_laser,
        module_power_reactor,
        module_command_module_type1,
        module_main_engine,
        module_thruster,
        module_refiner,
        module_tracktor_beam
        */
    [SerializeField] public Enums.enum_item item_type;
    [SerializeField] public GameObject item;
    [SerializeField] private Sprite single_blaster;
    [SerializeField] private Sprite double_blaster;
    [SerializeField] private Sprite mining_laser;
    [SerializeField] private Sprite main_engine;
    [SerializeField] private Sprite thruster;
    [SerializeField] private Sprite refiner;
    [SerializeField] private Sprite tracktor_beam;
    [SerializeField] private Sprite power;
    [SerializeField] private Sprite unknown;
    private Image sr;
    private LineRenderer line_renderer;
    public static void WriteLine(object obj) {
        Debug.Log(JsonUtility.ToJson(obj));
    }
    private void Start() {
        line_renderer = GetComponent<LineRenderer>();

        UnHighlight();
    }

    public void Highlight() {
        line_renderer.startColor = Color.red;
        line_renderer.endColor = Color.red;
    }
    public void UnHighlight() {
        line_renderer.startColor = Color.grey;
        line_renderer.endColor = Color.grey;
    }

    public void SetItem(GameObject  item) {
        Debug.Log("Adding item");
        //WriteLine(item);
        ItemResorce ir = item.GetComponent<ItemResorce>();
        sr = GetComponent<Image>();
        this.item_type = ir.Item_type;
        this.item = item;
        //*************
        //Set the image
        //*************
        switch (this.item_type) {
            case Enums.enum_item.module_single_blaster:
                sr.sprite = single_blaster;
                break;
            case Enums.enum_item.module_double_blaster:
                sr.sprite = double_blaster;
                break;
            case Enums.enum_item.module_mining_laser:
                sr.sprite = mining_laser;
                break;
            case Enums.enum_item.module_main_engine:
                sr.sprite = main_engine;
                break;
            case Enums.enum_item.module_thruster:
                sr.sprite = thruster;
                break;
            case Enums.enum_item.module_refiner:
                sr.sprite = refiner;
                break;
            case Enums.enum_item.module_tracktor_beam:
                sr.sprite = tracktor_beam;
                break;
            case Enums.enum_item.module_power_reactor:
                sr.sprite = power;
                break;
            default:
                sr.sprite = unknown;
                break;
        }



    }

}
