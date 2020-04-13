using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    private Coroutine s=null;

    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 OrigionalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, OrigionalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = OrigionalPos;
        s = null;
    }

    public void ShakeCamera(float duration, float magnitude) {
        if (s == null) {
            s = StartCoroutine(Shake(duration, magnitude));
        }
       
    }
}