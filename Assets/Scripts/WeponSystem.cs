using System.Collections;
using UnityEngine;

public class WeponSystem : MonoBehaviour {

    public enum enum_wepon_type {
        single_blaster,
        double_blaster,
        beam,
    }

    [Header("Player Projectile")]
    [SerializeField] private GameObject prefab_blaster_laser;
    [SerializeField] private GameObject laser_hit_sprite;
    [SerializeField] public enum_wepon_type wepon_type = enum_wepon_type.single_blaster;
    [SerializeField] public float laser_range = 10f;
    [SerializeField] public float projectile_speed = 10f;
    [SerializeField] public float projectileFiringPeriod = 0.1f;
    [SerializeField] public float double_blaster_distance = 0.5f;
    [SerializeField] private AudioClip laserAudio;

    private Coroutine fire_method;
    private LineRenderer line_renderer;
    private SpriteRenderer laser_hit_sprite_renderer;

    // Start is called before the first frame update
    private void Start() {
        line_renderer = GetComponent<LineRenderer>();
        line_renderer.enabled = false;
        line_renderer.useWorldSpace = true;
        if (laser_hit_sprite != null) {
            laser_hit_sprite_renderer = laser_hit_sprite.GetComponent<SpriteRenderer>();
            laser_hit_sprite_renderer.enabled = false;
        }
        
    }

    private void Update() {
        //if (wepon_type == enum_wepon_type.beam) {
            Fire_Beam();
        //} else {
            Fire_Blaster();
        //}
    }

    private void Fire_Beam() {
        if (Input.GetButton("Fire2")) {
           
            int hit_mask = (1 << LayerMask.NameToLayer("Asteroid")) | (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Material"));

           
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, transform.up, laser_range, hit_mask);
            if (hit2D) {
                laser_hit_sprite.transform.position = hit2D.point;
                laser_hit_sprite_renderer.enabled = true;
                //test if has a meterial spawner
                MaterialSpawner ms = hit2D.collider.gameObject.GetComponent<MaterialSpawner>();
                if (ms != null) {
                    ms.Hit(laser_hit_sprite.transform.position);
                }
            } else {
                laser_hit_sprite.transform.position = transform.position + transform.up * laser_range;
                laser_hit_sprite_renderer.enabled = false;
            }

            line_renderer.SetPosition(0, transform.position);
            line_renderer.SetPosition(1, laser_hit_sprite.transform.position);
            line_renderer.enabled = true;
        } else {
            line_renderer.enabled = false;
            laser_hit_sprite_renderer.enabled = false;
        }
    }

    private void Fire_Blaster() {
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
            if (wepon_type == enum_wepon_type.double_blaster) {
                Vector3 left_laser = new Vector3(transform.position.x - double_blaster_distance, transform.position.y, transform.position.z);
                Vector3 right_laser = new Vector3(transform.position.x + double_blaster_distance, transform.position.y, transform.position.z);

                GameObject laser1 = Instantiate(prefab_blaster_laser, left_laser, transform.rotation) as GameObject;
                laser1.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * projectile_speed);

                GameObject laser2 = Instantiate(prefab_blaster_laser, right_laser, transform.rotation) as GameObject;
                laser2.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * projectile_speed);
            } else if (wepon_type == enum_wepon_type.single_blaster) {
                GameObject laser = Instantiate(prefab_blaster_laser, transform.position, transform.rotation) as GameObject;
                laser.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.up * projectile_speed);
            }
            if (laserAudio != null) {
                AudioSource.PlayClipAtPoint(laserAudio, new Vector3(0, 0, 0));
            }
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }
}