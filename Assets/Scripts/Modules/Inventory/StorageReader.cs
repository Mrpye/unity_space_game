using UnityEngine;
using UnityEngine.UI;

public class StorageReader : MonoBehaviour, Reader {
    [SerializeField] public Enums.enum_item item_type;
    private ShipManagment storage;
    private Text txt;
    private Button but;
    private bool is_Blueprint = false;
    [SerializeField] private Recipe recipe;
    private Replicator replicator;
    [SerializeField] public Enums.enum_read_type read_type;
    private float  replicator_time=0;
    private Enums.enum_item replicator_current_item;
    // Start is called before the first frame update
    private void Start() {
        storage = UnityFunctions.ship_manager;
        is_Blueprint = UnityFunctions.GetItemTypeResorceType(item_type) == Enums.enum_resorce_type.blueprint;
        but = GetComponent<Button>();
        txt = GetComponentInChildren<Text>();
        if (read_type == Enums.enum_read_type.replicator_que) {
            if (replicator == null) { replicator = UnityFunctions.modules.GetComponentInChildren<Replicator>(); }
        }else if ( read_type == Enums.enum_read_type.replicator_time) {
            if (replicator == null) { replicator = UnityFunctions.modules.GetComponentInChildren<Replicator>(); }
           
        }

        InvokeRepeating("Reader.UpdateDisplayData", 0, 1.0f);
    }

    void Reader.UpdateDisplayData() {
        if (storage != null && txt != null) {
            if (read_type == Enums.enum_read_type.replicator_que) {
                txt.text = replicator.processbin_Item_Count(item_type).ToString();
            } else if (read_type == Enums.enum_read_type.replicator_time) {
                if (replicator_time <= 0) {
                    if (replicator.processing) {
                        replicator_current_item = replicator.current_item;
                        replicator_time = replicator.process_time;
                        txt.text = "Making " + replicator_current_item.ToString() + " Completion time " + replicator_time;
                    }
                } else {
                    replicator_time -= 1.0f;
                    if (replicator_time < 0) { 
                        replicator_time = 0;
                        txt.text ="";
                    } else {
                        txt.text = "Making " + replicator_current_item.ToString() + " Completion time " + replicator_time;
                    }
                }
               
            } else {
                txt.text = storage.Inventory_Item_Count(item_type).ToString();
            }
        } else {
            if (is_Blueprint) {
                ColorBlock cb = but.colors;
                if (storage.Inventory_Item_Count(item_type) > 0) {
                    cb.disabledColor = Color.blue;
                    but.interactable = HasIngredients();
                } else {
                    but.interactable = false;
                    cb.disabledColor = Color.red;
                }
                but.colors = cb;
            }
        }
    }

    private bool HasIngredients() {
        if (recipe != null) {
            foreach (Recipe.Ingreadient i in recipe.ingredients) {
                if (storage.Inventory_Item_Count(i.item_type) < i.qty) {
                    return false;
                }
            }
            return true;
        } else {
            return false;
        }
    }
    public void Use() {
        //We need to find the Replicator

        switch (item_type) {
            case  Enums.enum_item.material_fuel:
                if (storage.AddFuel(recipe.ammount)) {
                    storage.remove_material(item_type);
                }
                break;
        }
    }
    public void MakeItem() {
        //We need to find the Replicator
        if (replicator == null) { replicator = UnityFunctions.modules.GetComponentInChildren<Replicator>(); }
        if (replicator != null) {
            replicator.AddItemToReplicator(recipe);
        }
    }
}