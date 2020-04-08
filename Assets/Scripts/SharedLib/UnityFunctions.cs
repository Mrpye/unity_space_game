using UnityEngine;
using UnityEngine.EventSystems;

public class UnityFunctions {
    public static GameObject modules;
    public static GameObject stored_modules;
    public static GameObject game_elements;
    public static GameObject stored_upgrades;
    public static GameObject player;
    public static void PopulateCommonVariables() {
        UnityFunctions.modules = GameObject.Find("Modules");
        UnityFunctions.stored_modules = GameObject.Find("Stored_Modules");
        UnityFunctions.game_elements = GameObject.Find("GameElements");
        UnityFunctions.stored_upgrades = GameObject.Find("Stored_Upgrades");
        UnityFunctions.player = GameObject.Find("Player");
    }

    public static void LookAt2D(Transform obj, Transform theTarget, float speed, Enums.enum_facing_direction facing) {
        Vector3 vectorToTarget = theTarget.position - obj.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        angle -= (float)facing;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, q, Time.deltaTime * speed);
    }

    public static Color RGBA(float R,float G,float B,float A) {
        return new Color(R/255f,G/255, B/255,A/255f);
    }
    public static void CameraShake(float duration = 0.1f, float magnitude = 0.3f) {
        magnitude = Mathf.Clamp(magnitude, 0.05f, 0.5f);
        GameObject go = GameObject.Find("Main Camera");
        CameraShake shake = go.GetComponent<CameraShake>();
        if (shake != null) { shake.ShakeCamera(duration, magnitude); ; }
    }

    public static void SendAlert(Enums.enum_status status, string msg) {
        GameObject go = GameObject.Find("Alerts");
        if (go != null) { go.GetComponent<Alert>().RaiseAlert(status, msg); }
        
    }

    public static float normValue(float x, float min, float max) {
        return (x - min) / (max - min);
    }

    public static void Move_RB_Random(Rigidbody2D rb, float speed_min, float speed_max) {
        float move_speed_x = Random.Range(speed_min, speed_max);
        float move_speed_y = Random.Range(speed_min, speed_max);
        rb.velocity = new Vector3(Random.Range(-move_speed_x, move_speed_x), Random.Range(-move_speed_y, move_speed_y), 0);
    }

    public static GameObject FireProjectile(GameObject projectile_prefab, GameObject fire_point, int sort_order, float speed = 0, int damage = 2) {
        GameObject laser = Object.Instantiate(projectile_prefab, fire_point.transform.position, fire_point.transform.rotation, UnityFunctions.game_elements.transform) as GameObject;
        laser.GetComponent<DamageDealer>().SetDamage(damage);
        laser.GetComponent<SpriteRenderer>().sortingOrder = 100;
        laser.GetComponent<Rigidbody2D>().velocity = fire_point.transform.TransformDirection(Vector3.up * speed);
        return laser;
    }

    public static void Trust_At_Point(Rigidbody2D rb, Transform thrust_point, float thrust) {
        Vector2 force = -1 * thrust_point.up * Time.deltaTime * thrust;
        rb.AddForceAtPosition(force, thrust_point.position, ForceMode2D.Impulse);
    }

    public static float Calc_Kinetic_Energy(Rigidbody2D rb) {
        return 0.5f * rb.mass * rb.velocity.sqrMagnitude;
    }

    public static void AddListener(ref EventTrigger trigger, EventTriggerType eventType, System.Action<PointerEventData> listener) {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(data => listener.Invoke((PointerEventData)data));
        trigger.triggers.Add(entry);
    }

    public static ModuleSystemInfo[] GetModules() {
        return UnityFunctions.modules.GetComponentsInChildren<ModuleSystemInfo>();
    }

    public static ModuleSystemInfo[] GetStoredModules() {
        return UnityFunctions.stored_modules.GetComponentsInChildren<ModuleSystemInfo>();
    }

    public Upgrade_Settings[] GetStoredUpgrades() {
        return UnityFunctions.stored_modules.GetComponentsInChildren<Upgrade_Settings>();
    }

    public static Color Color_Green() {
        return UnityFunctions.RGBA(45, 195, 52, 255);
    }
    public static Color Color_Red() {
        return Color.red;
    }
    public static Color Color_Orange() {
        return UnityFunctions.RGBA(45, 195, 52, 255);
    }
}