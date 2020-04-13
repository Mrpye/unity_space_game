using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dock : MonoBehaviour
{
    [SerializeField] bool plyer_in_landing_zone;
    private void OnTriggerEnter2D(Collider2D collision) {

        SpaceShipMovment sm = GameObject.Find("Player").GetComponent<SpaceShipMovment>();
        if (sm != null) {
            sm.is_in_docking_zone = true;
        }
        plyer_in_landing_zone = true;
       
    }
    private void OnTriggerExit2D(Collider2D collision) {

       
        SpaceShipMovment sm = GameObject.Find("Player").GetComponent<SpaceShipMovment>();
        if (sm != null) {
            sm.is_in_docking_zone = false;
        }
        plyer_in_landing_zone = false;
    }
}
