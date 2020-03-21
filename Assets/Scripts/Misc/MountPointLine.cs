using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountPointLine : MonoBehaviour
{
    LineRenderer line_render;
    MountPoint mount_point;

    // Start is called before the first frame update
    void Start()
    {
        line_render = GetComponent<LineRenderer>();
        line_render.enabled = false;
        mount_point = GetComponent<MountPoint>();
    }
    public void DrawLine() {
        if (mount_point.associated_mountpoint) { 
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        Vector3 pos = Camera.main.ScreenToWorldPoint( new Vector2( rt.rect.width, rt.rect.height));// - start_click_pos;
        line_render.SetPosition(0,new Vector2( transform.position.x+2f, transform.position.y));
        line_render.SetPosition(1, mount_point.associated_mountpoint.transform.position);
        line_render.enabled = true;
        }
    }
    public void HideLine() {
        line_render.enabled = false;
    }

}
