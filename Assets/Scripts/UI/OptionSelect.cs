using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSelect : MonoBehaviour
{
    [SerializeField] public GameObject panel_front;
    [SerializeField] public GameObject panel_bottom;
    [SerializeField] public GameObject panel_left;
    [SerializeField] public GameObject panel_right;
    [SerializeField] public GameObject panel_top;
    [SerializeField] public Enums.emun_zone side;
    public void Show() {
        panel_front.SetActive(false);
        panel_bottom.SetActive(false);
        panel_left.SetActive(false);
        panel_right.SetActive(false);
        panel_top.SetActive(false);
        switch (side) {
            case  Enums.emun_zone.front:
                panel_front.SetActive(true);
                break;
            case Enums.emun_zone.left:
                panel_left.SetActive(true);
                break;
            case Enums.emun_zone.right:
                panel_right.SetActive(true);
                break;
            case Enums.emun_zone.bottom:
                panel_bottom.SetActive(true);
                break;
            case Enums.emun_zone.Top:
                panel_top.SetActive(true);
                break;
        }

    }
}
