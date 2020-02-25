using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [SerializeField] public GameObject target_object;
    [SerializeField] public List<string> allowed_targets;
    [SerializeField] public float range;

    public void TargetingRange(Vector2 center,float override_fire_range=-1) {
        int hit_mask = 1;
        float fire_range = range;
        if (override_fire_range > -1) {
            fire_range = override_fire_range;
        } 
        foreach(string target_layer in allowed_targets) {
            hit_mask = hit_mask |(1 << LayerMask.NameToLayer(target_layer));
        }
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, range, hit_mask);

        bool found_item = false;
        int i = 0;
        
        float clost_dist = fire_range + 500;

        while (i < hitColliders.Length) {
            //ItemResorce ir = hitColliders[i].GetComponent<ItemResorce>();
            //if (ir != null && ir.GetItemType() == Enums.enum_item.asset_player) {
            float dist = Vector3.Distance(hitColliders[i].gameObject.transform.position, transform.position);
            if (dist < clost_dist) {
                clost_dist = dist;
                target_object = hitColliders[i].gameObject;
                found_item = true;
            }
            i++;
        }
        if (found_item == false) {
            target_object = null;
        } 
    }
}
