using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityFunctions 
{
    public static void LookAt2D(Transform obj, Transform theTarget, float speed, Enums.enum_facing_direction facing) {
        Vector3 vectorToTarget = theTarget.position - obj.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        angle -= (float)facing;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        obj.transform.rotation = Quaternion.Slerp(obj.transform.rotation, q, Time.deltaTime * speed);
    }


    public static float normValue(float x, float min, float max) {
        return (x - min) / (max - min);
    }

    public static void Move_RB_Random(Rigidbody2D rb, float speed_min,float speed_max) {
        float move_speed_x = Random.Range(speed_min, speed_max);
        float move_speed_y = Random.Range(speed_min, speed_max);
        rb.velocity = new Vector3(Random.Range(-move_speed_x, move_speed_x), Random.Range(-move_speed_y, move_speed_y), 0);
    }

    public static GameObject FireProjectile(GameObject projectile_prefab,GameObject fire_point,int sort_order) {
        GameObject laser = Object.Instantiate(projectile_prefab, fire_point.transform.position, fire_point.transform.rotation) as GameObject;
        laser.GetComponent<SpriteRenderer>().sortingOrder = 100;
        laser.GetComponent<Rigidbody2D>().velocity = fire_point.transform.TransformDirection(Vector3.up * 30);
        return laser;
    }


    public static void Trust_At_Point(Rigidbody2D rb,Transform thrust_point,float thrust) {
        Vector2 force = -1 * thrust_point.up * Time.deltaTime * thrust;
        rb.AddForceAtPosition(force, thrust_point.position, ForceMode2D.Impulse);
    }

    public static float Calc_Kinetic_Energy(Rigidbody2D rb) {
        return 0.5f * rb.mass * rb.velocity.sqrMagnitude;
    }
}
