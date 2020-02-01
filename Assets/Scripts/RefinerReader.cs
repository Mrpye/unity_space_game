using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RefinerReader : MonoBehaviour
{
    [SerializeField] private Refiner refiner;
    private Slider bar;

    // Start is called before the first frame update
    private void Start() {
        bar = GetComponent<Slider>();
        InvokeRepeating("UpdateDisplayData", 0, 1.0f);
    }

    private void UpdateDisplayData() {
        if (refiner != null && bar != null) {
            bar.maxValue = refiner.max_items;
            bar.value = refiner.Item_Count();
        }

    }
}
