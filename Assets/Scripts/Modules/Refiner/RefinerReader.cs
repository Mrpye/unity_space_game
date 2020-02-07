using UnityEngine;
using UnityEngine.UI;

public class RefinerReader : MonoBehaviour {
    private Refiner refiner;
    private Slider bar;

    // Start is called before the first frame update
    private void Start() {
        bar = GetComponent<Slider>();
        InvokeRepeating("UpdateDisplayData", 0, 1.0f);
    }

    private void UpdateDisplayData() {
        if(refiner == null) {
            GameObject r = GameObject.Find("Refiner(Clone)");
            if (r != null) { refiner = r.GetComponent<Refiner>(); }
        }
        if (refiner != null && bar != null) {
            bar.maxValue = refiner.Get_Items();
            bar.value = refiner.Bin_Item_Count();
        }
    }
}