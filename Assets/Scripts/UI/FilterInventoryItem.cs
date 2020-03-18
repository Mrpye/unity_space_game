using UnityEngine;

public class FilterInventoryItem : MonoBehaviour {

    public void FilterItems(bool intern, bool thruster, bool engine, bool utility_top,bool utility_side, bool command) {
        Transform obj = gameObject.transform.Find("DropPanel");
   
        foreach (Transform go in obj) {

            InventoryItem inv_item = go.GetComponent<InventoryItem>();
            inv_item.Disable();
            if (intern == true && inv_item.mount_type_internal == true) {
                inv_item.Enabled();
            } else if (thruster == true && inv_item.mount_type_thruster == true) {
                inv_item.Enabled();
            } else if (engine == true && inv_item.mount_type_engine == true) {
                inv_item.Enabled();
            } else if (utility_top == true && inv_item.mount_type_util_top == true) {
                inv_item.Enabled();
            } else if (utility_side == true && inv_item.mount_type_util_side == true) {
                inv_item.Enabled();
            } else if (command == true && inv_item.is_command_module == true) {
                inv_item.Enabled();
            } 
        }
    }
}

