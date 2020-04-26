using System;
using UnityEngine;
using UnityEngine.UI;

public class StorageReader : MonoBehaviour, Reader {
    [ReadOnly] public Enums.enum_item item_type;
    [SerializeField] private Recipe recipe;
    [SerializeField] public Enums.enum_read_type read_type;

    private ShipManagment storage;
    private Replicator replicator;


    private Text txt;
    private Button but;
    private Image img;

    private bool is_Blueprint = false;
    private float replicator_time = 0;
    private Enums.enum_item replicator_current_item;

    private void OnValidate() {
        //***********************************
        //this deals with just setting values
        //Annd configuring the control
        //***********************************
        if (recipe != null) {
            Setup();
            item_type = recipe.item_type;
            img.sprite = recipe.sprite;
            txt.enabled = true;
            if (recipe != null && read_type == Enums.enum_read_type.replicator_make) {
                is_Blueprint = recipe.blueprint;
                txt.enabled = !is_Blueprint;
            }
            if (but != null) {
                but.targetGraphic = img;
            }
            
        }
    }

    private void Setup() {
        but = GetComponent<Button>();
        txt = GetComponentInChildren<Text>();
        Transform go = gameObject.transform.Find("Image");
        if (go != null) {
            img =go.transform.GetComponent<Image>();
        }
        
    }
    // Start is called before the first frame update
    private void Start() {
        storage = UnityFunctions.ship_manager;

        Setup();
        OnValidate();

        txt.enabled = true;
        if (recipe != null && read_type== Enums.enum_read_type.replicator_make) {
            txt.enabled = !recipe.blueprint;
        }
        

        //*************************
        //Get the replicator object
        //*************************
        if (read_type == Enums.enum_read_type.replicator_que) {
            if (replicator == null) { replicator = UnityFunctions.modules.GetComponentInChildren<Replicator>(); }
        } else if (read_type == Enums.enum_read_type.replicator_time) {
            if (replicator == null) { replicator = UnityFunctions.modules.GetComponentInChildren<Replicator>(); }
        }

        if (but != null) {
            //Lets hook up event 
            if (read_type == Enums.enum_read_type.replicator_make) {
                but.onClick.AddListener(MakeItem);
            } else if (read_type == Enums.enum_read_type.use) {
                but.onClick.AddListener(Use);
            } else {
                but.onClick.RemoveAllListeners();
            }
        }

        InvokeRepeating("Reader.UpdateDisplayData", 0, 1.0f);
    }

    void Reader.UpdateDisplayData() {
        if (storage != null && txt != null) {

            //**************
            //Set the colour
            //**************
            if (is_Blueprint && read_type== Enums.enum_read_type.replicator_make) {
                ColorBlock cb = but.colors;
                if (storage.Blueprint_Item_Count(recipe.required_blueprint) > 0) {
                    cb.disabledColor = Color.blue;
                    but.interactable = HasIngredients();
                } else {
                    but.interactable = false;
                    cb.disabledColor = Color.red;
                }
                but.colors = cb;
            }


            if (read_type == Enums.enum_read_type.replicator_que) {
                //**********************************
                //Counts items in the replicator que
                //**********************************
                if (replicator != null) { txt.text = replicator.processbin_Item_Count(item_type).ToString(); }
            } else if (read_type == Enums.enum_read_type.replicator_time) {
                //************************
                //Show the replicator time
                //************************
                if (replicator != null) {
                    if (replicator_time <= 0) {
                        if (replicator.processing) {
                            replicator_current_item = replicator.current_item;
                            replicator_time = replicator.est_finish_time.Subtract(new DateTimeOffset(DateTime.Now)).Seconds;
                            txt.text = "Making " + replicator_current_item.ToString() + " Completion time " + replicator_time;
                        } else {
                            txt.text = "";
                        }
                    } else {
                        if (replicator.processing) {
                            replicator_time = replicator.est_finish_time.Subtract(new DateTimeOffset(DateTime.Now)).Seconds;
                        } else {
                            replicator_time = 0;
                            txt.text = "";
                        }
                        if (replicator_time < 0) {
                            replicator_time = 0;
                            txt.text = "";
                        } else {
                            txt.text = "Making " + replicator_current_item.ToString() + " Completion time " + replicator_time;
                        }
                    }
                }
            } else {
                //*************
                //Show the time
                //*************
                txt.text = storage.Inventory_Item_Count(item_type).ToString();
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
            case Enums.enum_item.material_fuel:
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