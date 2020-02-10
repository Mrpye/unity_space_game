using UnityEngine;

public class InventoryManager : MonoBehaviour {
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject item_prefab;

    private void Start() {
       Populate_Items();
    }

    public void Populate_Items() {
        Storage strorage = player.GetComponent<Storage>();
        ModuleSystemInfo[] modules = strorage.GetSToredItems();
        foreach (ModuleSystemInfo module in modules) {
            GameObject item = Instantiate(item_prefab, gameObject.transform);
            //Lets config it
            InventoryItem inv = item.GetComponent<InventoryItem>();
            inv.SetItem(module.gameObject);
        }
    }
}