using UnityEngine;
using UnityEngine.UI;

public class StorageReader : MonoBehaviour {
    [SerializeField] public Enums.enum_item item_type;
    private InventoryManager storage;
    private Text txt;

    // Start is called before the first frame update
    private void Start() {
        storage = GetComponentInParent<InventoryManager>();
        txt = GetComponentInChildren<Text>();
        InvokeRepeating("UpdateDisplayData", 0, 1.0f);
    }

    private void UpdateDisplayData() {
        if(storage !=null && txt != null) {
            txt.text = storage.Item_Count(item_type).ToString();
        }
       
    }
}