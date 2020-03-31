using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipModule : ModuleSystemInfo {

    [Header("Mount Points")]
    [SerializeField] public List<MountPoint> mount_points = new List<MountPoint>();
    





    public void ShowMountPoints() {
        foreach(MountPoint mp in mount_points) {
            mp.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    public void HideMountPoints() {
        foreach (MountPoint mp in mount_points) {
            mp.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    private void Start() {
        this.Run_Start();
        //LoadMountPoints();
    }

    public void LoadMountPoints() {
        mount_points.Clear();
        MountPoint[] mp = gameObject.GetComponentsInChildren<MountPoint>();
        var result = from MountPoint n in mp orderby n.index select n;
        foreach (MountPoint n in result) {
            mount_points.Add(n);
        }
    }
}