using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagmentInterface : MonoBehaviour
{
    public GameObject mangment_interface; // Assign in inspector
    [SerializeField] public GameObject panel_modules;
    [SerializeField] public GameObject panel_replicator;



    private bool isShowing;
    [SerializeField] ScrollViewAddModules scrollview;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            isShowing = !isShowing;
            if (isShowing) {
                Show_Replicator();
            }
            mangment_interface.SetActive(isShowing);
        }
    }


    public void Show_Modules() {
        panel_modules.SetActive(true);
        panel_replicator.SetActive(false);
        scrollview.Populate();
    }


    public void Show_Replicator() {
        panel_modules.SetActive(false);
        panel_replicator.SetActive(true);
    }






}
