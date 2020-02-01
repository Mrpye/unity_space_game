using UnityEngine;

public class StarFieldUV : MonoBehaviour {
    [Range(1, 64)] [SerializeField] private float parralax = 1;
    [SerializeField] private bool warp = false;

    private void Update() {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material mat = mr.material;
        Vector2 offset = mat.mainTextureOffset;

        if (warp == true) {
            offset.x = transform.position.x / transform.localScale.x * (parralax * 0.02f);
            offset.y = transform.position.y / transform.localScale.y * (parralax * 0.02f);
        } else {
            offset.x = transform.position.x / transform.localScale.x / parralax;
            offset.y = transform.position.y / transform.localScale.y / parralax;
        }

        mat.mainTextureOffset = offset;
    }
}