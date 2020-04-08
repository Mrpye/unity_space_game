using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public GameObject menu; // Assign in inspector
    private bool isShowing;
    [SerializeField] ScrollViewAddModules scrollview;
    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            isShowing = !isShowing;
            if (isShowing) {
                scrollview.Populate();
            }
            menu.SetActive(isShowing);
        }
    }

}
