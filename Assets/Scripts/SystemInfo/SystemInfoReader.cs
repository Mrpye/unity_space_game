using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This will read system info and display on slider bar
/// </summary>
public class SystemInfoReader : MonoBehaviour {
    [SerializeField] private Enums.enum_system_info data_type;

    [SerializeField] private GameObject player_prefab;
    private ShipManagment sys;
    private Slider bar;

    // Start is called before the first frame update
    private void Start() {
        if (player_prefab == null) { return; }
        sys = player_prefab.GetComponent<ShipManagment>();
        bar = GetComponent<Slider>();
        InvokeRepeating("UpdateDisplayData", 0, 0.1f);
    }

    // Update is called once per frame
    private void Update() {
    }

    private void UpdateDisplayData() {
        if (sys != null && bar != null) {
            ShipManagment.value_data res = sys.Get_Data(data_type);
            bar.maxValue = res.max_value;
            bar.value = res.value;
        }
    }
}