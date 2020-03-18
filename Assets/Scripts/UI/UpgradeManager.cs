using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour {
    [SerializeField] private GameObject invetory_upgrade_item_prrefab;
    [SerializeField] private UpgradeItem selected_item;

    
    public void LoadData(UpgradeItem selected_item) {
        gameObject.SetActive(true);

        this.selected_item = selected_item;
        //**********************
        //Get the ship Managment
        //**********************
        GameObject player = GameObject.Find("Player_Config");
        if (player == null) { return; }
        ShipManagment ship_managment = player.GetComponent<ShipManagment>();
        if (ship_managment == null) { return; }

        //***********************************
        //Lets get the drop panel of the item
        //***********************************
        Transform panel = gameObject.transform.Find("DropPanel");
        if (panel != null) {
            //*************************
            //Lest remove all old items
            //*************************
            foreach (Transform child in panel.transform) {
                GameObject.Destroy(child.gameObject);
            }
            if (invetory_upgrade_item_prrefab == null) { return; }
            foreach (Upgrade_Settings e in ship_managment.stored_upgrades) {
                GameObject button = Instantiate(invetory_upgrade_item_prrefab, panel);
                UpgradeItem item = button.GetComponent<UpgradeItem>();
                if (item != null) { item.SetItem(e); }
                EventTrigger trigger = item.GetComponent<EventTrigger>();
                UnityFunctions.AddListener(ref trigger, EventTriggerType.PointerClick, onClickListener);
            }
        }
    }
    void onClickListener(PointerEventData eventData) {

        GameObject  selected = eventData.selectedObject;
        UpgradeItem item = selected.GetComponent<UpgradeItem>();
        if (item != null) {
            selected_item.SetItem(item.item);
        }

        //put your logic here...
        gameObject.SetActive(false);

    }
  
}