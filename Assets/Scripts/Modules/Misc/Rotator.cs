using UnityEngine;

public class Rotator : MonoBehaviour {
    [SerializeField] private int max_rotation_speed = 20;
    [SerializeField] private int rotate_speed = 0;
    [SerializeField] private bool manual_set = false;

    public void Set_rotation_Speed(int speed) {
        manual_set = true;
        rotate_speed = speed;
    }

    // Start is called before the first frame update
    private void Start() {
        if (manual_set == false) { rotate_speed = Random.Range(0, max_rotation_speed); }
    }

    // Update is called once per frame
    private void Update() {
        float z_rotation = rotate_speed * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, z_rotation));
    }
}