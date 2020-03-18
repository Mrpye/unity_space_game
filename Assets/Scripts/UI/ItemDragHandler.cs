using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private Vector3 start_click_pos;
    public Transform parentToReturnTo;

    public void OnBeginDrag(PointerEventData eventData) {
        InventoryItem inv_item = eventData.pointerDrag.gameObject.GetComponent<InventoryItem>();
        if (inv_item != null) {
            if (inv_item.is_disabled) {
                return; 
            }
        }
        parentToReturnTo = transform.parent;
        this.transform.SetParent(parentToReturnTo.parent);
        // start_click_pos = Camera.main.ScreenToWorldPoint(eventData.position) - transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        //lets filter the panels
        GameObject panel = GameObject.Find("Panels");
        if (panel != null) {
            PanelFilter panel_filter = panel.GetComponent<PanelFilter>();
            if (panel_filter != null) {
                panel_filter.FilterItems(inv_item.mount_type_internal, inv_item.mount_type_thruster, inv_item.mount_type_engine,inv_item.mount_type_util_top, inv_item.mount_type_util_side, inv_item.is_command_module);
            }
        }
    }

    public void OnDrag(PointerEventData eventData) {
        InventoryItem inv_item = eventData.pointerDrag.gameObject.GetComponent<InventoryItem>();
        if (inv_item != null) {
            if (inv_item.is_disabled) {
                return;
            }
        }
        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);// - start_click_pos;
        transform.position = new Vector3(pos.x, pos.y, 100);
        transform.parent = GameObject.Find("Panels").gameObject.transform;
        // transform.position = Camera.main.ScreenToViewportPoint(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData) {
        InventoryItem inv_item = eventData.pointerDrag.gameObject.GetComponent<InventoryItem>();
        if (inv_item != null) {
            if (inv_item.is_disabled) {
                return;
            }
        }
        //Debug.Log("OnEndDrag");
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}