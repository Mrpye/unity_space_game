using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelFilter : MonoBehaviour {
    public void FilterItems(bool intern, bool thruster,bool engine, bool utility_top, bool utility_side,bool command) {

        
        MountPoint[] mount_points= gameObject.GetComponentsInChildren<MountPoint>();
        foreach(MountPoint m in mount_points) {
            Disable(m.gameObject);
            if (intern == true && m.mount_type_internal ) {
                Enable(m.gameObject);
            } else if (thruster == true && m.mount_type_thruster ) {
                Enable(m.gameObject);
            } else if (engine == true && m.mount_type_engine) {
                Enable(m.gameObject);
            } else if (utility_top == true && m.mount_type_util_top) {
                Enable(m.gameObject);
            } else if (utility_side == true && m.mount_type_util_side) {
                Enable(m.gameObject);
            } else if (command == true && m.is_command_module) {
                Enable(m.gameObject);
            }


        }
    }
    private void Disable(GameObject go) {
        Image img = go.GetComponent<Image>();
        EnabledDisabled enable_disable = go.GetComponent<EnabledDisabled>();
        if(enable_disable != null) {
            enable_disable.Is_Enabled = false;
        }
        
        img.color = Color.grey;
        
    }
    private void Enable(GameObject go) {
        Image img = go.GetComponent<Image>();
        img.color = new Color(1f, 0.58f, 0,1);
        EnabledDisabled enable_disable = go.GetComponent<EnabledDisabled>();
        if (enable_disable != null) {
            enable_disable.Is_Enabled = true;
        }

        //FF9400

    }
}