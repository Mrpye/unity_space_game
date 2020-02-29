using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private Vector3 start_click_pos;
    public Transform parentToReturnTo;
    public bool IsDragable;

    public void OnBeginDrag(PointerEventData eventData) {
        InventoryItem inv_item = eventData.pointerDrag.gameObject.GetComponent<InventoryItem>();
        if (inv_item != null) {
            IsDragable = !inv_item.is_disabled;
        }
        parentToReturnTo = transform.parent;
        this.transform.SetParent(parentToReturnTo.parent);
        // start_click_pos = Camera.main.ScreenToWorldPoint(eventData.position) - transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        if (!IsDragable) { return; }
        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);// - start_click_pos;
        transform.position = new Vector3(pos.x, pos.y, 100);
        transform.parent = GameObject.Find("Panels").gameObject.transform;
        // transform.position = Camera.main.ScreenToViewportPoint(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!IsDragable) { return; }
        //Debug.Log("OnEndDrag");
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}