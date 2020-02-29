using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour {
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

    [Header("Mounting Position")]
    [SerializeField] public bool sub_zone_middle = true;
    [SerializeField] public bool sub_zone_corner = true;
    [SerializeField] public bool sub_zone_top = true;
    [SerializeField] public bool sub_zone_internal = true;

    public bool is_disabled = false;
    private Image sr;
    private LineRenderer line_renderer;
    public string description;

    public static void WriteLine(object obj) {
        Debug.Log(JsonUtility.ToJson(obj));
    }

    private void Start() {
        line_renderer = GetComponent<LineRenderer>();

        UnHighlight();
    }

    public void Disable() {
        is_disabled = true;
        sr.color = Color.gray;
        UnHighlight();
    }

    public void Enabled() {
        sr.color = Color.white;
        is_disabled = false;
    }

    public void Highlight() {
        if (is_disabled == false) {
            line_renderer.startColor = Color.red;
            line_renderer.endColor = Color.red;
        }
    }

    public void UnHighlight() {
        line_renderer.startColor = Color.grey;
        line_renderer.endColor = Color.grey;
    }

    public void SetInfo() {
        GameObject go = GameObject.Find("InfoPanel");
        if (go != null) {
            InfoPanel info_pnel = go.GetComponent<InfoPanel>();
            info_pnel.SetInfo(this);
        }
    }

    public void SetItem(GameObject item) {
        Debug.Log("Adding item");
        //WriteLine(item);
        ItemResorce ir = item.GetComponent<ItemResorce>();
        sr = GetComponent<Image>();
        this.item_type = ir.Item_type;

        ModuleSystemInfo module_sys = item.GetComponent<ModuleSystemInfo>();
        this.sub_zone_corner = module_sys.sub_zone_corner;
        this.sub_zone_internal = module_sys.is_internal_module;
        this.sub_zone_middle = module_sys.sub_zone_middle;
        this.sub_zone_top = module_sys.sub_zone_top;
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