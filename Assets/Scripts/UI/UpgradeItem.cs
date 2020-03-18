using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour {
    public bool is_disabled = false;
    public bool is_selection = false;
    private Image sr;
    private LineRenderer line_renderer;
    public Upgrade_Settings item;

    //public string description;
    private void Start() {
        line_renderer = GetComponent<LineRenderer>();

        UnHighlight();
    }

    public void SetItem(Upgrade_Settings item) {
        this.item = item;
        sr = GetComponent<Image>();
        if (sr != null && item != null) {
            sr.sprite = item.Sprite;
        }
    }

    public void Disable() {
        is_disabled = true;
        sr.color = Color.gray;
        UnHighlight();
    }

    public void Enabled() {
        sr.color = Color.white;
        is_disabled = false;
    }

    public void Highlight() {
        if (is_disabled == false) {
            line_renderer.startColor = Color.red;
            line_renderer.endColor = Color.red;
        }
    }

    public void UnHighlight() {
        line_renderer.startColor = Color.grey;
        line_renderer.endColor = Color.grey;
    }
}