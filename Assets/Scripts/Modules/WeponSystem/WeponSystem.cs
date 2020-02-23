using System.Collections;
using UnityEngine;

/// <summary>
/// This scipt handles wepons
/// Currently there are 3 types
/// 1:Mining beam on fire 2
/// 2:Single Blaster on fire 1
/// 3:Double Blaster on fire 1
/// </summary>
public class WeponSystem : ModuleSystemInfo {

    [Header("Game Objects")]
    [SerializeField] private GameObject prefab_blaster_laser;
    [SerializeField] private GameObject laser_hit_sprite;
    [SerializeField] private AudioClip laserAudio;

    [Header("Wepon Attibutes")]
    [SerializeField] public Enums.enum_wepon_type wepon_type = Enums.enum_wepon_type.single_blaster;

    private float laser_range = 10f;
    private float projectile_speed = 10f;
    private float projectileFiringPeriod = 0.1f;
    [SerializeField] private float double_blaster_distance = 0.5f;

    private Coroutine fire_method;
    private LineRenderer line_renderer;
    private SpriteRenderer laser_hit_sprite_renderer;
    [SerializeField] private GameObject fire_point1;
    [SerializeField] private GameObject fire_point2;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start() {
        line_renderer = GetComponent<LineRenderer>();
        fire_point1 = gameObject.transform.Find("FirePoint").gameObject;
        Transform wt = gameObject.transform.Find("FirePoint2");
        if (wt != null) { fire_point2 = wt.gameObject; }

        Transform t = gameObject.transform.Find("HitPoint");
        if (t != null) {
            laser_hit_sprite_renderer = t.GetComponent<SpriteRenderer>();
        }

        line_renderer.enabled = false;
        line_renderer.useWorldSpace = true;
        if (laser_hit_sprite != null) {
            laser_hit_sprite_renderer = laser_hit_sprite.GetComponent<SpriteRenderer>();
            laser_hit_sprite_renderer.enabled = false;
        }
        //Lets get module info if exist
        UpdateModuleStats();
        StartMonitor();
    }

    /// <summary>
    /// This gets Moduleinfo settigs and apply to the specific module
    /// </summary>
    public void UpdateModuleStats() {
        laser_range = this.Get_Range();
        projectile_speed = this.Get_Speed();
        projectileFiringPeriod = this.Get_ActionSpeed();
    }

    private void Update() {
        if (is_in_storage == true) { return; }
        if (this.Is_Online()) {
            if (wepon_type == Enums.enum_wepon_type.beam) {
                Fire_Beam();
            } else {
                Fire_Blaster();
            }
        }
        UpdateUsage();
    }

    /// <summary>
    /// This fires the beam lazer
    /// this is the mining beam
    /// </summary>
    private void Fire_Beam() {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
            
        if (Input.GetButton("Fire2") && Is_Online()) {
            int hit_mask = (1 << LayerMask.NameToLayer("Asteroid")) | (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Material"));
            StartUsage();
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, transform.up, laser_range, hit_mask);
            if (hit2D) {
                laser_hit_sprite.transform.position = hit2D.point;
                laser_hit_sprite_renderer.enabled = true;
                //test if has a meterial spawner
                ItemSpawner ms = hit2D.collider.gameObject.GetComponent<ItemSpawner>();
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
            line_renderer.sortingOrder = this.order_layer - 1;
        } else {
            line_renderer.enabled = false;
            if (laser_hit_sprite_renderer != null) { laser_hit_sprite_renderer.enabled = false; }
            StopUsage();
        }
    }

    /// <summary>
    /// fire the blaster
    /// </summary>
    private void Fire_Blaster() {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
        if (Input.GetButtonDown("Fire1") && Is_Online()) {
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
            SingleUpdateUsage();
            if (wepon_type == Enums.enum_wepon_type.double_blaster) {
                UnityFunctions.FireProjectile(prefab_blaster_laser, fire_point1, this.order_layer - 1);
                UnityFunctions.FireProjectile(prefab_blaster_laser, fire_point2, this.order_layer - 1);
            } else if (wepon_type == Enums.enum_wepon_type.single_blaster) {
                UnityFunctions.FireProjectile(prefab_blaster_laser, fire_point1, this.order_layer - 1);
            }
            if (laserAudio != null) {
                AudioSource.PlayClipAtPoint(laserAudio, new Vector3(0, 0, 0));
            }
            yield return new WaitForSeconds(projectileFiringPeriod);
            if (this.is_in_storage == true) { break; }
        }
    }
}