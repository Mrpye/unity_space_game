using UnityEngine;

public class OptionSelect : MonoBehaviour {
    [SerializeField] public GameObject panel_command;
    [SerializeField] public GameObject panel_internal;
    [SerializeField] public GameObject panel_front;
    [SerializeField] public GameObject panel_bottom;
    [SerializeField] public GameObject panel_left;
    [SerializeField] public GameObject panel_right;
    [SerializeField] public GameObject panel_top;
    [SerializeField] public Enums.emun_zone side;
    [SerializeField] public GameObject invetory_panel;
    private FilterInventoryItem invetory_filter;

    private void FilterItems(bool intern, bool truster, bool engine, bool util_top,bool util_side,bool command) {
        if (!invetory_panel) { return; }
        FilterInventoryItem invetory_filter = invetory_panel.GetComponent<FilterInventoryItem>();
        if (invetory_filter != null) {
            invetory_filter.FilterItems(intern, truster,  engine, util_top,util_side,command);
        }
    }

    public void Show() {
        panel_internal.SetActive(false);
        panel_front.SetActive(false);
        panel_bottom.SetActive(false);
        panel_left.SetActive(false);
        panel_right.SetActive(false);
        panel_top.SetActive(false);
        panel_command.SetActive(false);

        switch (side) {
            case Enums.emun_zone.front:
                panel_front.SetActive(true);
                FilterItems(false, true,true, false, true, false);
                break;

            case Enums.emun_zone.left:
                panel_left.SetActive(true);
                FilterItems(false, true, true, false, true, false);
                break;

            case Enums.emun_zone.right:
                panel_right.SetActive(true);
                FilterItems(false, true, true, false, true, false);
                break;

            case Enums.emun_zone.bottom:
                panel_bottom.SetActive(true);
                FilterItems(false, true, true, false, true, false);
                break;

            case Enums.emun_zone.Top:
                panel_top.SetActive(true);
                FilterItems(false, false, false, true, false, false);
                break;

            case Enums.emun_zone.intern:
                panel_internal.SetActive(true);
                FilterItems(true, false, false, false, false, false);
                break;
            case Enums.emun_zone.Command:
                panel_command.SetActive(true);
                FilterItems(false, false, false, false, false, true);
                break;
        }
    }
}