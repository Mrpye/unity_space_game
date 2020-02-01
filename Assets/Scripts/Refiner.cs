using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Possible upgrades
/// Faster processing time
/// </summary>
public class Refiner : SystemRequirments {
    [SerializeField] public List<PlayerStorage.enum_item> processing_bin = new List<PlayerStorage.enum_item>();
    [SerializeField] public int processing_time = 20;
    [SerializeField] public int max_items = 10;
    private PlayerStorage storage ;

    private bool processing = false;
    private MaterialResorce mr;

    private void Start() {
        storage=GetComponentInParent<PlayerStorage>();
    }

    public int Item_Count() {
        return processing_bin.Count;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "material") {
            mr = collision.gameObject.GetComponent<MaterialResorce>();
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
            PlayerStorage.enum_item item = processing_bin[0];
            if(storage != null) {
                storage.Store_Material(item);
            }
            processing_bin.RemoveAt(0);
        } while (processing_bin.Count > 0);
        processing = false;
    }
}