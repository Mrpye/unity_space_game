using UnityEngine;
using UnityEngine.UI;
public class InfoPanel : MonoBehaviour {
    [SerializeField] private GameObject img;

    private InventoryItem inv;
    private bool visible;

    // Start is called before the first frame update
    private void Start() {
    }

    public void SetInfo(InventoryItem inv) {
        //This will display the info
        this.inv = inv;
        ItemResorce ir = inv.item.GetComponent<ItemResorce>();
        ModuleSystemInfo module_info = inv.item.GetComponent<ModuleSystemInfo>();
        SetText("txtTitle", inv.item_type.ToString().Replace("_"," "));
        SetText("txtDescription", ir.GetDescription());
        SetImg("Image", inv.GetComponent<Image>().sprite);
        SetInfo("txtInfo(0)", "Mass: "+ module_info.mass.ToString());
        SetInfo("txtInfo(1)", "Power: " + module_info.power_usage_factor.ToString());
        SetInfo("txtInfo(2)", "Cpu: " + module_info.cpu.ToString());
        SetInfo("txtInfo(3)", "Fuel: " + module_info.idle_fuel.ToString());
        SetInfo("txtInfo(4)", "Heat: " + module_info.usage_factor_heat.ToString());


    }
    private void SetImg(string item, Sprite sprite) {
        
        if (img != null) {
            img.GetComponent<Image>().sprite = sprite;
        }
    }
    private void SetInfo(string item, string text) {
        GameObject go = GetChildWithName(GetChildWithName(GetChildWithName(gameObject, "SubPanel"), "Info"), item);

        if (go != null) { go.GetComponent<Text>().text = text; }
    }
    private void SetText(string item,string text) {
        GameObject go = GetChildWithName(GetChildWithName(gameObject, "SubPanel"), item);
        if (go != null) { go.GetComponent<Text>().text = text; }
    }
    private GameObject GetChildWithName(GameObject obj, string name) {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null) {
            return childTrans.gameObject;
        } else {
            return null;
        }
    }
}