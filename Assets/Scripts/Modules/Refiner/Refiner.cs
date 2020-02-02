using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Possible upgrades
/// Faster processing time
/// </summary>
public class Refiner : ModuleSystemInfo {
    private List<Storage.enum_item> processing_bin = new List<Storage.enum_item>();
    private float processing_time = 20;

    private Storage storage;
    private bool processing = false;
    private ItemResorce mr;

    /// <summary>
    /// This gets Moduleinfo settigs and apply to the specific module
    /// </summary>
    public void UpdateModuleStats() {
        processing_time = this.Get_ActionSpeed();
    }

    private void Start() {
        storage = GetComponentInParent<Storage>();
        UpdateModuleStats();
        StartMonitor();
    }

    public int Bin_Item_Count() {
        return processing_bin.Count;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "material") {
            mr = collision.gameObject.GetComponent<ItemResorce>();
            if (mr != null) {
                processing_bin.Add(mr.material_type);
            }
            Destroy(collision.gameObject);
        }
    }

    private void Update() {
        if (processing == false && processing_bin.Count > 0) {
            StartCoroutine(ProcessBin());
        }
    }

    private IEnumerator ProcessBin() {
        processing = true;
        do {
            yield return new WaitForSeconds(processing_time);
            Storage.enum_item item = processing_bin[0];
            if (storage != null) {
                storage.Store_Material(item);
            }
            processing_bin.RemoveAt(0);
        } while (processing_bin.Count > 0);
        processing = false;
    }
}