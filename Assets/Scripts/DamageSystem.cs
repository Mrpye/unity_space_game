using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{

    [SerializeField] private float health = 100f;

    [Header("Destruction")]
    [SerializeField] private GameObject deathFX;
    [SerializeField] private float durationOfExplosion = 1f;

  
    private void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }


    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
       
        Destroy(gameObject);
        if (deathFX == null) { return; }
        GameObject explosion = Instantiate(deathFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        

    }
}
