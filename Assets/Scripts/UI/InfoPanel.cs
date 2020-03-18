using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {
    [SerializeField] private GameObject img;
    [SerializeField] private Dropdown upgrade_item1;
    [SerializeField] private Dropdown upgrade_item2;
    [SerializeField] private Dropdown upgrade_item3;
    private ModuleSystemInfo module_info;
    private ShipManagment ship_managment;
    private InventoryItem inv;
    private bool visible;
    private bool loading = false;

    // Start is called before the first frame update
    private void Start() {
    }

    private void LoadUpgradeList() {
        //**********************
        //Get the ship Managment
        //**********************
        loading = true;
        upgrade_item1.ClearOptions();
        upgrade_item2.ClearOptions();
        upgrade_item3.ClearOptions();
        upgrade_item2.gameObject.SetActive(false);
        upgrade_item3.gameObject.SetActive(false);

        GameObject player = GameObject.Find("Player_Config");
        if (player == null) { return; }
        ship_managment = player.GetComponent<ShipManagment>();
        if (ship_managment == null) { return; }
        //********************
        //Populate the options
        //********************

        //*********************
        //Set the default value
        //*********************
        for (int i = 0; i < module_info.upgrades.Count; i++) {
            switch (i) {
                case 0:
                    upgrade_item2.gameObject.SetActive(true);
                    upgrade_item1.options.Add(new Dropdown.OptionData("None"));
                    foreach (Upgrade_Settings e in ship_managment.stored_upgrades) {
                        upgrade_item1.options.Add(new Dropdown.OptionData(e.name, e.Sprite));
                    }
                    upgrade_item1.options.Add(new Dropdown.OptionData(module_info.upgrades[i].name, module_info.upgrades[i].Sprite));
                    
                    //Populate 
                    upgrade_item2.options.Add(new Dropdown.OptionData("None"));
                    foreach (Upgrade_Settings e in ship_managment.stored_upgrades) {
                        upgrade_item2.options.Add(new Dropdown.OptionData(e.name, e.Sprite));
                    }
                    upgrade_item1.value = GetIndexByName(upgrade_item1, module_info.upgrades[i].name) ;
                    upgrade_item1.RefreshShownValue(); // this is the key
                    break;

                case 1:
                    upgrade_item3.gameObject.SetActive(true);
                    upgrade_item2.options.Add(new Dropdown.OptionData(module_info.upgrades[i].name, module_info.upgrades[i].Sprite));
                    
                    //Populate 3
                    upgrade_item3.options.Add(new Dropdown.OptionData("None"));
                    foreach (Upgrade_Settings e in ship_managment.stored_upgrades) {
                        upgrade_item3.options.Add(new Dropdown.OptionData(e.name, e.Sprite));
                    }
                    upgrade_item2.value = GetIndexByName(upgrade_item2, module_info.upgrades[i].name);
                    upgrade_item2.RefreshShownValue(); // this is the key
                    break;

                case 2:
                    upgrade_item3.options.Add(new Dropdown.OptionData(module_info.upgrades[i].name, module_info.upgrades[i].Sprite));
                    upgrade_item3.value = GetIndexByName(upgrade_item3, module_info.upgrades[i].name);
                    upgrade_item3.RefreshShownValue(); // this is the key
                    break;
            }
        }

 
        if (module_info.upgrades.Count <= 0) {
            //Load the first item with data
            upgrade_item1.options.Add(new Dropdown.OptionData("None"));
            foreach (Upgrade_Settings e in ship_managment.stored_upgrades) {
                upgrade_item1.options.Add(new Dropdown.OptionData(e.name, e.Sprite));
            }
            upgrade_item1.value = GetIndexByName(upgrade_item1, "None");
        }
        this.SetInvetoryInfo();
        loading = false;
    }

    public void Item1Changed(int index) {
        if (loading == true) { return; }
        loading = true;
        Dropdown.OptionData data = upgrade_item1.options[index];
        
        if (data != null) {
            if (data.text == "None") {
                ship_managment.stored_upgrades.Add(module_info.upgrades[0]);
                module_info.upgrades.RemoveAt(0);
            } else {
                int i = GetIndexByName(upgrade_item1, data.text);
                module_info.upgrades.Add(ship_managment.stored_upgrades[i-1]);
                ship_managment.stored_upgrades.RemoveAt(i-1);

            }
            LoadUpgradeList();
        }

        this.SetInvetoryInfo();
        loading = false;
    }

    public void Item2Changed(int index) {
        if (loading == true) { return; }
        loading = true;
        Dropdown.OptionData data = upgrade_item2.options[index];
        
        if (data != null) {
            if (data.text == "None") {
                ship_managment.stored_upgrades.Add(module_info.upgrades[0]);
                module_info.upgrades.RemoveAt(0);
            } else {
                int i = GetIndexByName(upgrade_item2, data.text);
                module_info.upgrades.Add(ship_managment.stored_upgrades[i-1]);
                ship_managment.stored_upgrades.RemoveAt(i-1);
            }
            
            LoadUpgradeList();
        }
        loading = false;
    }

    public void Item3Changed(int index) {
        if (loading == true) { return; }
        loading = true;
        Dropdown.OptionData data = upgrade_item3.options[index];
        if (data != null) {
            if (data.text == "None") {
                ship_managment.stored_upgrades.Add(module_info.upgrades[0]);
                module_info.upgrades.RemoveAt(0);
            } else {
                int i = GetIndexByName(upgrade_item3, data.text);

                module_info.upgrades.Add(ship_managment.stored_upgrades[i-1]);
                ship_managment.stored_upgrades.RemoveAt(i-1);

            }
            LoadUpgradeList();
        }
        loading = false;
    }

    public int GetIndexByName(Dropdown dropDown, string name) {
        if (dropDown == null) { return -1; } // or exception
        if (string.IsNullOrEmpty(name) == true) { return -1; }
        //List<Dropdown.OptionData> list = dropDown.options;
        for (int i = 0; i < dropDown.options.Count; i++) {
            if (dropDown.options[i].text.Equals(name)) { 
                return i;
            }
        }
        return 0;
    }


    public void SetInvetoryInfo(InventoryItem inv) {
        //This will display the info
        this.inv = inv;
       
        this.module_info = this.inv.item.GetComponent<ModuleSystemInfo>();
        //*********************
        //Load the upgrade info
        //*********************
        this.LoadUpgradeList();
       

    }

    public void SetInvetoryInfo() {
        //This will display the info
        ItemResorce ir = this.inv.item.GetComponent<ItemResorce>();
        InfoPaneSetText("txtTitle", this.inv.item_type.ToString().Replace("_", " "));
        InfoPaneSetText("txtDescription", ir.GetDescription());
        InfoPaneSetImg("Image", this.inv.GetComponent<Image>().sprite);
        InfoPaneSetInfo("txtInfo(0)", "Mass: " + module_info.settings.Mass.ToString());
        InfoPaneSetInfo("txtInfo(1)", "Power: " + module_info.settings.Power_usage.ToString());
        InfoPaneSetInfo("txtInfo(2)", "Cpu: " + Mathf.Abs(module_info.Get_Calculated_CPU()).ToString());
        InfoPaneSetInfo("txtInfo(3)", "Fuel: " + module_info.settings.Fuel_usage.ToString());
        InfoPaneSetInfo("txtInfo(4)", "Heat: " + module_info.settings.Heat_usage.ToString());
    }

    private void UpgradePaneSetText(string panel_name, string text) {
        GameObject go = GetChildWithName(GetChildWithName(gameObject, "UpgradeSubPanel"), panel_name);
        if (go != null) {
            go.GetComponent<Text>().text = text;
        }
    }

    private void UpgradeItem(string panel_name, Upgrade_Settings upgrade) {
        GameObject go = GetChildWithName(GetChildWithName(gameObject, "UpgradeSubPanel"), panel_name);
        if (go != null) {
            go.GetComponent<UpgradeItem>().SetItem(upgrade);
        }
    }

    private void InfoPaneSetImg(string item, Sprite sprite) {
        if (img != null) {
            img.GetComponent<Image>().sprite = sprite;
        }
    }

    private void InfoPaneSetInfo(string item, string text) {
        GameObject go = GetChildWithName(GetChildWithName(GetChildWithName(gameObject, "InfoSubPanel"), "Info"), item);
        if (go != null) { go.GetComponent<Text>().text = text; }
    }

    private void InfoPaneSetText(string item, string text) {
        GameObject go = GetChildWithName(GetChildWithName(gameObject, "InfoSubPanel"), item);
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