using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour ,IBeginDragHandler, IDragHandler,IEndDragHandler
{
    Vector3 start_click_pos;
  public  Transform parentToReturnTo;
    public void OnBeginDrag(PointerEventData eventData) {
        parentToReturnTo = transform.parent;
        this.transform.SetParent(parentToReturnTo.parent);
        // start_click_pos = Camera.main.ScreenToWorldPoint(eventData.position) - transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;


    }

    public void OnDrag(PointerEventData eventData) {

        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);// - start_click_pos;
        transform.position = new Vector3(pos.x, pos.y, 100);
        transform.parent = GameObject.Find("Panels").gameObject.transform;
       // transform.position = Camera.main.ScreenToViewportPoint(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData) {
        //Debug.Log("OnEndDrag");
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

  
}
