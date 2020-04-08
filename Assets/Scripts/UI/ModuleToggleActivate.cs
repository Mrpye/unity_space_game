using UnityEngine;
using UnityEngine.UI;

public class ModuleToggleActivate : MonoBehaviour {
    [SerializeField] private ModuleSystemInfo module_info;
    [SerializeField] private Toggle toggle;
    [SerializeField] private Text label;

    private void Start() {
        toggle.onValueChanged.AddListener((t) => { Toggled(t); });
    }

    private void Toggled(bool val) {
        if (module_info != null) { module_info.active = val; }
    }

    public void SetModule(ModuleSystemInfo m) {
        this.module_info = m;
        if (this.module_info != null) {
            SetToggleNameAndValue(this.module_info.name, this.module_info.active, this.module_info.current_health);
        }
    }

    public void SetToggleNameAndValue(string Name, bool val, float health, bool is_malfunctioning = false) {
        toggle.isOn = val;
        label.text = Name.Replace("(Clone)", "") + " " + health.ToString() + "%";
        if (health > 0) {
            if (is_malfunctioning == true) {
                label.color = UnityFunctions.Color_Green();
            } else {
                label.color = UnityFunctions.Color_Orange();
            }
           
        } else {
            label.color = UnityFunctions.Color_Green(); 
        }
    }
}