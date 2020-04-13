using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Possible upgrades
/// Faster processing time
/// </summary>
public class Replicator : ModuleSystemInfo {
    private List<Recipe> processing_bin = new List<Recipe>();
    private float processing_time = 20;

    private ShipManagment storage;
    public bool processing = false;
    public int process_time =0;
    public Enums.enum_item current_item;
    //private ItemResorce mr;

    /// <summary>
    /// This gets Moduleinfo settigs and apply to the specific module
    /// </summary>
    public void UpdateModuleStats() {
        processing_time = this.Get_Calculated_Action_Speed()  ;
        if (processing_time == 0) { processing_time = 1; }
    }

    private void Start() {
        if (!this.is_in_storage) {
            storage = UnityFunctions.ship_manager;
            this.Run_Start();
            UpdateModuleStats();
            StartMonitor();
        }
    }

    public int processbin_Item_Count(Enums.enum_item material) {
        var res = (from n in processing_bin where n.item_type == material select n).Count();
        return res;
    }


    public void AddItemToReplicator(Recipe recipe) {
        float maxbin = this.settings.Items_max;
        if (processing_bin.Count < maxbin) {
            processing_bin.Add(recipe);
        }
        //ItemResorce.ItemResorceData id=   UnityFunctions.GetItemTypeItem(item);
       // if(id.resorce_type== Enums.enum_resorce_type.material) {
            //Need to remove 
           
      //  }
        /*
        if (go.tag == "material" && processing_bin.Count < maxbin) {
            ItemResorce mr = go.GetComponent<ItemResorce>();
            ModuleSystemInfo msi = go.GetComponent<ModuleSystemInfo>();
            if (mr != null && msi==null) {
                //Storing material
                processing_bin.Add(mr.Item_type);
                Destroy(go);
            } else if (mr != null && msi != null) {
                //Storing a module
                storage.Store_Module(go);
            }   
        }*/
    }

    private void Update() {
        if (!this.is_in_storage) {
            UpdateUsage();
            if (processing == false && processing_bin.Count > 0) {
                StartUsage();
                StartCoroutine(ProcessBin());
            }
        }
    }

    private IEnumerator ProcessBin() {
        processing = true;
        do {
            if (this.is_online && this.active) {
                Recipe item = processing_bin[0];
                process_time = item.make_time;
                current_item = item.item_type;
                yield return new WaitForSeconds(process_time);   
                ItemResorce.ItemResorceData id = UnityFunctions.GetItemTypeItem(item.item_type);
                if (storage != null) {
                    if( id.resorce_type== Enums.enum_resorce_type.material) {
                        //************************************
                        //We need to remove x items from stock
                        //************************************
                        foreach(Recipe.Ingreadient i in item.ingredients) {
                            storage.remove_x_material(i.item_type, i.qty);
                        }
                        storage.Store_Material(item.item_type);
                    }
                }
                processing_bin.RemoveAt(0);
            }
        } while (processing_bin.Count > 0);
        processing = false;
        StopUsage();
    }
}