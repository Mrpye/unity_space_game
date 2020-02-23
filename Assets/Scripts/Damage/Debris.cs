using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour {

    [Header("Debris")]
    [SerializeField] private List<GameObject> debri_item;

    [SerializeField] private int debri_item_count = 2;
    [SerializeField] private int debri_move_speed_min = 1;
    [SerializeField] private int debri_move_speed_max = 5;

    // [SerializeField] private int debri_rotate_speed_min = 1;
    // [SerializeField] private int debri_rotate_speed_max = 5;
    [SerializeField] private bool debri_on_death = false;

    [SerializeField] private float time_to_live = 6;

    public void CreateDebris() {
        if (debri_on_death == true) {
            for (int i = 0; i < debri_item_count; i++) {
                GameObject item = debri_item[Random.Range(0, debri_item.Count - 1)];
                GameObject debri = Instantiate(item, transform.position, Quaternion.identity) as GameObject;
                //debri.GetComponent<Rigidbody2D>().rotation = Random.Range(-debri_rotate_speed_min, debri_rotate_speed_max);
                float move_speed = Random.Range(debri_move_speed_min, debri_move_speed_max);
                debri.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-move_speed, move_speed), Random.Range(-move_speed, move_speed), 0);
            }
        }
    }
}