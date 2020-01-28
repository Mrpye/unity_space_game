using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{

    [SerializeField]int damage =100;

    public int GetDamage() {return damage;}

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }

    public void Hit( ) {
        Destroy(gameObject);
    }

}
