using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ShipModule : ModuleSystemInfo {
    [Header("Mount Points")]
    [SerializeField] public List<MountPoint> mount_points = new List<MountPoint>();
    private void Start() {
        //LoadMountPoints();
       
    }
    public void LoadMountPoints() {
        mount_points.Clear();
        MountPoint[] mp = gameObject.GetComponentsInChildren<MountPoint>();
        var result = from MountPoint n in mp orderby n.index select n;
        foreach(MountPoint n in result) {
            mount_points.Add(n);
        }
    }
}
