using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    private bool isShowing;
    [SerializeField] public GameObject menu_panel;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isShowing = !isShowing;
            if (isShowing) {
                UnityFunctions.controls_locked = true;
            } else {
                UnityFunctions.controls_locked = false;
            }
            menu_panel.SetActive(isShowing);
        }
    }
}
