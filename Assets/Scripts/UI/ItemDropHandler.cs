using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ItemDropHandler : MonoBehaviour, IDropHandler
{
 
    
    public void OnDrop(PointerEventData eventData) {
        //Debug.Log("Item Dropped");
        ItemDragHandler d = eventData.pointerDrag.GetComponent<ItemDragHandler>();
        if(d != null) {
            d.parentToReturnTo = this.transform;
        }
       
    }
}
