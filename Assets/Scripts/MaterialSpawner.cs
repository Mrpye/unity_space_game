using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSpawner : MonoBehaviour
{
    [Header("Debris")]
    [SerializeField] private List<GameObject> debri_item;
    [SerializeField] private int debri_item_count = 3;
    [SerializeField] private float debri_move_speed_min = 0.2f;
    [SerializeField] private float debri_move_speed_max = 2f;
    [SerializeField] private int debri_rotate_speed_min = 1;
    [SerializeField] private int debri_rotate_speed_max = 5;
    [SerializeField] private float time_to_live = 0;
    [SerializeField] public float min_when_hit_seconds_to_next_spawn = 10;
    [SerializeField] public float max_when_hit_seconds_to_next_spawn = 10;
    [SerializeField] private float seconds_to_next_spawn = 0;

    private void Start() {
        seconds_to_next_spawn = Random.Range(min_when_hit_seconds_to_next_spawn, max_when_hit_seconds_to_next_spawn);
    }
    public void Hit(Vector2 hitpoint) {
        if (seconds_to_next_spawn <= 0){
            GenerateDebris(hitpoint);
            seconds_to_next_spawn = Random.Range(min_when_hit_seconds_to_next_spawn, max_when_hit_seconds_to_next_spawn);
        } else {
            seconds_to_next_spawn -= Time.deltaTime;
        }
    }
    public void GenerateDebris(Vector2 hitpoint) {
        for (int i = 0; i < debri_item_count; i++) {
            GameObject item = debri_item[Random.Range(0, debri_item.Count - 1)];
            GameObject debri = Instantiate(item, hitpoint, Quaternion.identity) as GameObject;
            debri.GetComponent<Rigidbody2D>().rotation = Random.Range(-debri_rotate_speed_min, debri_rotate_speed_max);
            float move_speed_x = Random.Range(debri_move_speed_min, debri_move_speed_max);
            float move_speed_y = Random.Range(debri_move_speed_min, debri_move_speed_max);
            debri.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-move_speed_x, move_speed_x), Random.Range(-move_speed_y, move_speed_y), 0);
            if (time_to_live > 0) {
                Destroy(debri, time_to_live);
            }
           
        }
    }
}
