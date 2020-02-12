using UnityEngine;

/// <summary>
/// Class to load the mount points from command module
/// </summary>
public class MountPointManager : MonoBehaviour {
    [SerializeField] private GameObject MountpointPrefab;
    [SerializeField] private Enums.emun_Side side;

    // Start is called before the first frame update
    private void Start() {
        GameObject mount_points = GameObject.Find("MountPoints");//This find the moun points on our command module
        ShipModule sys = mount_points.GetComponentInParent<ShipModule>();

        foreach (MountPoint m in sys.mount_points) {
            if (m.side == side) {
                GameObject g = Instantiate(MountpointPrefab, gameObject.transform);
                MountPoint omp = g.GetComponent<MountPoint>();
                omp.SetValues(m);
                omp.SetSize(new Vector2(100 + (m.max_mounting * 50), 50));
                omp.associated_mountpoint = m.gameObject;
                ItemDropHandler dh = g.GetComponentInChildren<ItemDropHandler>();
                dh.enforce_max = true;
                dh.max_items = m.max_mounting;
            }
        }
    }
}