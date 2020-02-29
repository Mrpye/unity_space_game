using UnityEngine;

public class FilterInventoryItem : MonoBehaviour {

    public void FilterItems(bool intern, bool Corner, bool middle, bool top) {
        Transform obj = gameObject.transform.Find("DropPanel");
        foreach (Transform go in obj) {
            InventoryItem inv_item = go.GetComponent<InventoryItem>();
            if (intern == true && inv_item.sub_zone_internal == true) {
                inv_item.Disable();
            } else if (Corner == true && inv_item.sub_zone_corner == true) {
                inv_item.Disable();
            } else if (middle == true && inv_item.sub_zone_middle == true) {
                inv_item.Disable();
            } else if (top == true && inv_item.sub_zone_top == true) {
                inv_item.Disable();
            } else {
                inv_item.Enabled();
            }
        }
    }
}