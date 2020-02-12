using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler {
    public int max_items = 0;
    [SerializeField] public bool enforce_max;
    private InventoryManager storage;
    private GameObject player;
    public Enums.emun_inventory inventory_box;

    private void Start() {
        player = GameObject.Find("Player_Config");
        if (player != null) {
            storage = player.GetComponent<InventoryManager>();
        }
    }

    public void OnDrop(PointerEventData eventData) {
        //Debug.Log("Item Dropped");
        ItemDragHandler d = eventData.pointerDrag.GetComponent<ItemDragHandler>();

        if (d != null) {
            if (enforce_max == true) {
                InventoryItem[] i = GetComponentsInChildren<InventoryItem>();
                if (i != null) {
                    if (i.Length + 1 > max_items) { return; }
                }
            }
            InventoryItem inv_item = eventData.pointerDrag.gameObject.GetComponent<InventoryItem>();
            if (inventory_box == Enums.emun_inventory.Selected) {
                MountPoint mp = GetComponentInParent<MountPoint>();
                if (mp != null) {
                    MountPoint amp = mp.associated_mountpoint.GetComponent<MountPoint>();
                    storage.Equip(amp, inv_item.item);
                } 
            } else {
                storage.Store_Module(inv_item.item);
            }

            d.parentToReturnTo = this.transform;
        }
    }
}