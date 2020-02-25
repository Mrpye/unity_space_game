using System.Collections;
using UnityEngine;

public class Turret : Targeting {
    [SerializeField] private GameObject fire_point1;
    [SerializeField] private GameObject prefab_blaster_laser;
    [SerializeField] public float fire_range;
    [SerializeField] public float fire_speed = 30;
    Coroutine fireing;
    private IEnumerator FireConinuous() {
        while (true) {
            UnityFunctions.FireProjectile(prefab_blaster_laser, fire_point1,100, fire_speed);
            yield return new WaitForSeconds(1);
        }
    }

    // Update is called once per frame
    private void Update() {
        TargetingRange(transform.position, fire_range);
        if (target_object != null) {
            UnityFunctions.LookAt2D(transform, target_object.transform, 2f, Enums.enum_facing_direction.Up);
            if(fireing == null) { fireing = StartCoroutine(FireConinuous()); }
           
        } else {
            if (fireing != null) {
                StopCoroutine(fireing);
            }
        }
    }

   
}