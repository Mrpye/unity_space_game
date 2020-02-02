using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    [Header("Materials")]
    [SerializeField] public List<GameObject> possible_items;

    [SerializeField] public int min_available_item_count = 0;
    [SerializeField] public int max_available_item_count = 10;

    [Header("Movement")]
    [SerializeField] public float item_move_speed_min = 0.2f;

    [SerializeField] public float item_move_speed_max = 2f;
    [SerializeField] public int item_rotate_speed_min = 1;
    [SerializeField] public int item_rotate_speed_max = 5;

    [Header("Spawn")]
    [SerializeField] public float time_to_live = 0;

    [SerializeField] public float min_when_hit_seconds_to_next_spawn = 10;
    [SerializeField] public float max_when_hit_seconds_to_next_spawn = 10;

    private List<GameObject> items = new List<GameObject>();
    private int available_item_count = 0;
    private float seconds_to_next_spawn = 0;

    private void Start() {
        seconds_to_next_spawn = Random.Range(min_when_hit_seconds_to_next_spawn, max_when_hit_seconds_to_next_spawn);
        available_item_count = Random.Range(min_available_item_count, max_available_item_count);
        //this will choose what this will actually spawn
        Select_SubsetOfItems();
    }

    private void Select_SubsetOfItems() {
        //int start_index = Random.Range(0, possible_material_item.Count);
        //int end_index = Random.Range(start_index, possible_material_item.Count);
        for (int i = 0; i < possible_items.Count; i++) {
            if (Random.Range(0, 10) > 5) {
                items.Add(possible_items[i]);
            }
        }
    }

    public void Hit(Vector2 hitpoint) {
        if (seconds_to_next_spawn <= 0) {
            Generatematerials(hitpoint);
            seconds_to_next_spawn = Random.Range(min_when_hit_seconds_to_next_spawn, max_when_hit_seconds_to_next_spawn);
        } else {
            seconds_to_next_spawn -= Time.deltaTime;
        }
    }

    public void Generatematerials(Vector2 hitpoint) {
        if (available_item_count > 0) {
            available_item_count -= 1;
            GameObject item = items[Random.Range(0, items.Count - 1)];
            GameObject material = Instantiate(item, hitpoint, Quaternion.identity) as GameObject;
            Rotator R = material.GetComponent<Rotator>();
            if (R != null) { R.Set_rotation_Speed(Random.Range(-item_rotate_speed_min, item_rotate_speed_max)); }
            float move_speed_x = Random.Range(item_move_speed_min, item_move_speed_max);
            float move_speed_y = Random.Range(item_move_speed_min, item_move_speed_max);
            material.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-move_speed_x, move_speed_x), Random.Range(-move_speed_y, move_speed_y), 0);
            if (time_to_live > 0) {
                Destroy(material, time_to_live);
            }
        }
    }
}