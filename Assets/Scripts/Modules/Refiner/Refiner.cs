﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Possible upgrades
/// Faster processing time
/// </summary>
public class Refiner : ModuleSystemInfo {
    private List<Enums.enum_item> processing_bin = new List<Enums.enum_item>();
    private float processing_time = 20;
    private bool processing = false;
    //private ItemResorce mr;

    /// <summary>
    /// This gets Moduleinfo settigs and apply to the specific module
    /// </summary>
    public void UpdateModuleStats() {
        processing_time = this.Get_Calculated_Action_Speed();
        if (processing_time == 0) { processing_time = 1; }
    }

    private void Start() {
        if (!this.is_in_storage) {
            this.Run_Start();
            UpdateModuleStats();
            StartMonitor();
        }
    }

    public int Bin_Item_Count() {
        return processing_bin.Count;
    }

    public void AddItemToRefiner(GameObject go) {
        float maxbin = this.settings.Items_max;
        if (go.tag == "material" && processing_bin.Count < maxbin) {
            ItemResorce mr = go.GetComponent<ItemResorce>();
            ModuleSystemInfo msi = go.GetComponent<ModuleSystemInfo>();
            if (mr != null && msi == null) {
                if (mr.GetNeedsRefining() == true) {
                    //Storing material
                    processing_bin.Add(mr.Item_type);
                    Destroy(go);
                } else {
                    UnityFunctions.ship_manager.Store_Material(mr.Item_type);
                    Destroy(go);
                }
            } else if (mr != null && msi != null) {
                //Storing a module
                UnityFunctions.ship_manager.Store_Module(go);
            }
        } else if (go.tag == "blueprint") {
            ItemResorce mr = go.GetComponent<ItemResorce>();
            UnityFunctions.ship_manager.Store_Material(mr.Item_type);
            Destroy(go);
        }
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
            yield return new WaitForSeconds(processing_time);
            if (this.is_online && this.active) {
                Enums.enum_item item = processing_bin[0];
                UnityFunctions.ship_manager.Store_Material(item);
                processing_bin.RemoveAt(0);
            }
        } while (processing_bin.Count > 0);
        processing = false;
        StopUsage();
    }
}