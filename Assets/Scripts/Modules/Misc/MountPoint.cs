using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MountPoint : MonoBehaviour {
    public Enums.emun_zone zone;

    //public Enums.emun_mount_point_type mount_point_type;
    [Header("Mounting type")]
    [SerializeField] public bool mount_type_util_top = true;

    [SerializeField] public bool is_command_module = true;
    [SerializeField] public bool mount_type_util_side = true;
    [SerializeField] public bool mount_type_thruster = true;
    [SerializeField] public bool mount_type_engine = true;
    [SerializeField] public bool mount_type_internal = true;
    [SerializeField] public bool set_mount_color = false;
    public int index;

    //public string  name;
    public int max_mounting;

    [SerializeField] public int render_order = 100;
    public GameObject associated_mountpoint;

    [SerializeField] public List<KeyMappingModel> key_mappings = new List<KeyMappingModel>();

    private void Start() {
        SetColor();
    }

    public void SetColor() {
        if (set_mount_color) {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();

            float red = 0;
            float green = 0;
            float blue = 0;

            
            if (mount_type_engine) { red = red + 75; }
            if (mount_type_thruster) { red = red + 150; }
            if (mount_type_util_top) { green = green + 150; }
            if (mount_type_util_side) { blue = blue + 150; }

            sr.color = new  Color(red / 255f, green / 255f, blue / 255f);

        }
    }

    public void SetValues(MountPoint mp) {
        max_mounting = mp.max_mounting;
        index = mp.index;
        zone = mp.zone;
        SetIndex(mp.index.ToString());
        SetText(mp.name.ToString());
    }

    public void SetSize(Vector2 size) {
        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null) {
            rt.sizeDelta = size;
        }
    }

    public void SetIndex(string text) {
        Transform go = gameObject.transform.Find("txtIndex");
        Text t = go.gameObject.GetComponent<Text>();
        if (t != null) {
            t.text = text;
        }
    }

    public void SetText(string text) {
        Transform go = gameObject.transform.Find("txtName");
        Text t = go.gameObject.GetComponent<Text>();
        if (t != null) {
            t.text = text;
        }
    }
}