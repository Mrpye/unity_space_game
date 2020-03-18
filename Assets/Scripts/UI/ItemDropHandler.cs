using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler {
    public int max_items = 0;
    [SerializeField] public bool enforce_max;
    private InventoryManager storage;
    private GameObject player;
    public Enums.emun_inventory inventory_box;
    public bool is_disabled = false;

    private void Start() {
        player = GameObject.Find("Player_Config");
        if (player != null) {
            storage = player.GetComponent<InventoryManager>();
        }
    }

    public void Enable() {
        is_disabled = false;
    }

    public void Disable() {
        is_disabled = true;
    }

    public void OnDrop(PointerEventData eventData) {
        //Debug.Log("Item Dropped");
        ItemDragHandler d = eventData.pointerDrag.GetComponent<ItemDragHandler>();
        InventoryItem inv_item = eventData.pointerDrag.gameObject.GetComponent<InventoryItem>();
        EnabledDisabled enable_disable = gameObject.GetComponentInParent<EnabledDisabled>();
        if (enable_disable != null) {
            if (enable_disable.Is_Enabled == false) {
                return;
            }
        }

        if (inv_item != null) {
            if (inv_item.is_disabled) { d.parentToReturnTo = this.transform; return; }
        }

        if (d != null) {
            if (enforce_max == true) {
                InventoryItem[] i = GetComponentsInChildren<InventoryItem>();
                if (i != null) {
                    if (i.Length + 1 > max_items) { return; }
                }
            }

            if (inventory_box == Enums.emun_inventory.Selected) {
                MountPoint mp = GetComponentInParent<MountPoint>();
                if (mp != null) {
                    MountPoint amp = mp.associated_mountpoint.GetComponent<MountPoint>();
                    inv_item.item.GetComponent<ModuleSystemInfo>().mount_point = amp.index;

                    storage.Equip(inv_item.item);
                }
            } else {
                storage.Store_Module(inv_item.item);
            }

            d.parentToReturnTo = this.transform;
        }
    }
}