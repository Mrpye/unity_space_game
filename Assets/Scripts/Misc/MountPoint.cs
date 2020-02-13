using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MountPoint : MonoBehaviour
{
    
    public Enums.emun_zone zone;
    public int index;
    //public string  name;
    public int max_mounting;
    [SerializeField]public int render_order=100;
    public GameObject associated_mountpoint;


    public void SetValues(MountPoint mp) {
        max_mounting = mp.max_mounting;
        index = mp.index;
        zone = mp.zone;
        SetIndex(mp.index.ToString());
        SetText(mp.name.ToString());
    }
    public void SetSize(Vector2 size) {
        RectTransform rt = GetComponent<RectTransform>();
        if(rt != null) {
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
