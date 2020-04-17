using UnityEngine;
using UnityEngine.UI;

public class ManagmentInterface : MonoBehaviour {
    public GameObject mangment_interface; // Assign in inspector
    [SerializeField] public GameObject panel_modules;
    [SerializeField] public GameObject panel_replicator;
    private Replicator replicator = null;
    private bool isShowing;
    [SerializeField]  private Button replicator_button;
    [SerializeField] private ScrollViewAddModules scrollview;

    private void Start() {
        replicator = UnityFunctions.modules.GetComponentInChildren<Replicator>();
        if (replicator == null) { replicator_button.interactable = false; }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            isShowing = !isShowing;
            if (isShowing) {
                UnityFunctions.controls_locked = true;
                if (replicator == null) {
                    Show_Modules();
                } else {
                    Show_Replicator();
                }

            } else {
                UnityFunctions.controls_locked = false;
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
        if (replicator != null) {
            panel_modules.SetActive(false);
            panel_replicator.SetActive(true);
        }
    }
}