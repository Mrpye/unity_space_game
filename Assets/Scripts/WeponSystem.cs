using System.Collections;
using UnityEngine;

public class WeponSystem : MonoBehaviour {

    [Header("Player Projectile")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject PrefabLaser;
    [SerializeField] private bool douple_fire = false;
    [SerializeField] private float projectile_speed = 10f;
    [SerializeField] private float projectileFiringPeriod = 0.1f;
    [SerializeField] private float double_laser_distance = 0.5f;
    [SerializeField] public AudioClip laserAudio;

    private Coroutine fire_method;
    // Start is called before the first frame update

    private void Update() {
        Fire();
    }
    private void Fire() {
        if (Input.GetButtonDown("Fire1")) {
            fire_method = StartCoroutine(FireConinuous());
        }
        if (Input.GetButtonUp("Fire1")) {
            if (fire_method != null) {
                StopCoroutine(fire_method);
            }
        }
    }



    private IEnumerator FireConinuous() {
        while (true) {
            if (douple_fire == true) {
                Vector3 left_laser = new Vector3(firepoint.position.x - double_laser_distance, firepoint.position.y, firepoint.position.z);
                Vector3 right_laser = new Vector3(firepoint.position.x + double_laser_distance, firepoint.position.y, firepoint.position.z);

                GameObject laser1 = Instantiate(PrefabLaser, left_laser, firepoint.rotation) as GameObject;
                laser1.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * projectile_speed);

                GameObject laser2 = Instantiate(PrefabLaser, right_laser, firepoint.rotation) as GameObject;
                laser2.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * projectile_speed);
            } else {
                GameObject laser = Instantiate(PrefabLaser, transform.position, firepoint.rotation) as GameObject;
                laser.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * projectile_speed); 
            }
            if (laserAudio != null) {
                AudioSource.PlayClipAtPoint(laserAudio, new Vector3(0, 0, 0));
            }
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }
}