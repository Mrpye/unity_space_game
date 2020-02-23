using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour {
    [SerializeField] private GameObject fire_point1;
    [SerializeField] private GameObject prefab_blaster_laser;
    [SerializeField] public GameObject Player_Prefab;
    [SerializeField] public float fire_range;
    Coroutine fireing;
    private IEnumerator FireConinuous() {
        while (true) {
            UnityFunctions.FireProjectile(prefab_blaster_laser, fire_point1, 100);
            yield return new WaitForSeconds(1);
        }
    }

    private void Start() {
        //fire_point1 = transform.Find("FirePoint").gameObject;
    }

    // Update is called once per frame
    private void Update() {
        TargetingRange(transform.position, fire_range);
        if (Player_Prefab != null) {
            UnityFunctions.LookAt2D(transform, Player_Prefab.transform, 2f, Enums.enum_facing_direction.Up);
            if(fireing == null) { fireing = StartCoroutine(FireConinuous()); }
           
        } else {
            if (fireing != null) {
                StopCoroutine(fireing);
            }
        }
    }

    private void TargetingRange(Vector2 center, float radius) {
        int hit_mask = (1 << LayerMask.NameToLayer("Player"));
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center,radius, hit_mask);
        
        bool found_item = true;
        int i = 0;
        while (i < hitColliders.Length) {
            ItemResorce ir = hitColliders[i].GetComponent<ItemResorce>();
            if (ir != null && ir.GetItemType() == Enums.enum_item.asset_player) {
                Player_Prefab = hitColliders[i].gameObject;
                found_item = true;
                break;
            }
            i++;
        }
        if (found_item == false) {
            Player_Prefab = null;
        }
    }
}