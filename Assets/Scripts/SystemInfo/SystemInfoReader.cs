using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This will read system info and display on slider bar
/// </summary>
public class SystemInfoReader : MonoBehaviour {
    [SerializeField] private Enums.enum_system_info data_type;
    [SerializeField] private GameObject player_prefab;

    private ShipManagment sys;
    private SpaceShipMovment ship_movment;
    private Refiner refiner;
    private Slider bar;
    private Image sr;

    // Start is called before the first frame update
    private void Start() {
        if (player_prefab == null) { return; }

        //*******************************************************
        //Lets look at the what type of data we are going to read
        //*******************************************************
        if (data_type == Enums.enum_system_info.flight_assist || data_type == Enums.enum_system_info.landing_zone) {
            //*************************
            //This is icon data boolean
            //*************************
            ship_movment = player_prefab.GetComponent<SpaceShipMovment>();
            sr = GetComponent<Image>();
        } else if (data_type== Enums.enum_system_info.refiner ) {
            GameObject r = GameObject.Find("Refiner(Clone)");

        } else {
            //**************************************
            //This is slider data from ShipManagment
            //**************************************
            sys = player_prefab.GetComponent<ShipManagment>();
            bar = GetComponent<Slider>();
        }
        //*******************************************
        //Lets start te reading
        //*******************************************
        InvokeRepeating("UpdateDisplayData", 0, 0.1f);
    }


    private void UpdateDisplayData() {
        if (sys != null && bar != null) {
            ShipManagment.value_data res = sys.Get_Data(data_type);
            bar.maxValue = res.max_value;
            bar.value = res.value;
        } else if (data_type == Enums.enum_system_info.flight_assist) {
            if (ship_movment.flight_assist == true) {
                sr.enabled = true;
            } else {
                sr.enabled = false;
            }
        } else if (data_type == Enums.enum_system_info.landing_zone) {
            if (ship_movment.is_in_docking_zone == true) {
                sr.enabled = true;
            } else {
                sr.enabled = false;
            }
        }else if (data_type == Enums.enum_system_info.refiner) {
            if (refiner == null) {
                GameObject r = GameObject.Find("Refiner(Clone)");
                if (r != null) { refiner = r.GetComponent<Refiner>(); }
            }
            if (refiner != null && bar != null) {
                bar.maxValue = refiner.Get_Items();
                bar.value = refiner.Bin_Item_Count();
            }
        }
    }
}
