using UnityEngine;

public class MovmentVector : MonoBehaviour {
    [SerializeField] private GameObject Prefab;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    private void Start() {
        rb = Prefab.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update() {
        //Draw Vector arrow
        Vector2 dir = rb.velocity;
        float s = 1 * dir.magnitude + 1;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (s <= 1.1f) {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            sr.enabled = false;
        } else {
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            sr.enabled = true;
        }
    }
}