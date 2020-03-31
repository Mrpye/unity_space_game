using System.Collections;
using UnityEngine;

public class DamageSystem : MonoBehaviour {
    [SerializeField] private float health = 100f;

    [Header("Destruction")]
    [SerializeField] private GameObject deathFX;

    [SerializeField] private GameObject shockwave;
    [SerializeField] private float durationOfExplosion = 0.5f;
    [SerializeField] private float explosion_range = 10f;
    [SerializeField] private float force_of_explosion = 10f;
    [SerializeField] private float shockwave_damagefactor = 10f;
    [SerializeField] private bool shockwave_force;
    private bool death_running = false;

    private void OnTriggerEnter2D(Collider2D other) {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer) {
            ProcessHit(damageDealer);
        }
    }

    private void ProcessHit(DamageDealer damageDealer) {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0) {
            Die();
        }
    }

    private IEnumerator DeathProcesses() {
        death_running = true;
        if (shockwave_force == true) { ApplyExplosionForce(transform.position, explosion_range, force_of_explosion); }
        //AppyExplosionForce();
        SpriteRenderer sr = null;

        //***************
        //Disable Sprites
        //***************
        sr = gameObject.GetComponent<SpriteRenderer>();
        if (sr != null) { sr.enabled = false; }
        SpriteRenderer[] sr_col = gameObject.GetComponentsInChildren<SpriteRenderer>();
        if (sr_col != null) {
            foreach (SpriteRenderer s in sr_col) {
                s.enabled = false;
            }
        }
        Transform obj = gameObject.transform.Find("Sprint");
        if (obj != null) { if (sr != null) { sr.enabled = false; } }

        //*****************
        //Disable Collision
        //*****************
        PolygonCollider2D pcol = gameObject.GetComponent<PolygonCollider2D>();
        if (pcol != null) { pcol.enabled = false; }

        BoxCollider2D bcol = gameObject.GetComponent<BoxCollider2D>();
        if (bcol != null) { bcol.enabled = false; }

        //********************
        //Throw som edebri out
        //********************
        Debris debris = gameObject.GetComponent<Debris>();
        if (debris != null) { debris.CreateDebris(); }

        if (deathFX != null) {
            GameObject explosion = Instantiate(deathFX, transform.position, transform.rotation);
            if (shockwave != null) {
                Instantiate(shockwave, transform.position, transform.rotation);
            }
            Destroy(explosion, durationOfExplosion);
        }

        yield return new WaitForSeconds(durationOfExplosion);
        Destroy(gameObject);
    }

    /*private void ApplyExplosionForce() {
        if (explosion_force_collider != null && explosion_force_effector != null) {
            explosion_force_collider.enabled = true;
            explosion_force_effector.enabled = true;
        }
    }*/

    private void Die() {
        if (death_running == false) { StartCoroutine(DeathProcesses()); }
    }

    //******************************************
    //Lets see whats in range and give it damage
    //******************************************
    private void ApplyExplosionForce(Vector2 center, float radius, float forceMultiplier) {
        int hit_mask = (1 << LayerMask.NameToLayer("Player"));
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius, hit_mask);
        int i = 0;
        //Apply force
        if (hitColliders.Length>0) {
            GameObject player = GameObject.Find("Player");
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            Vector2 force = (player.transform.position - transform.position) * (forceMultiplier * 100);
            rb.AddForce(force);
        }


        while (i < hitColliders.Length) {
            ModuleSystemInfo ir = hitColliders[i].GetComponent<ModuleSystemInfo>();
            if (ir != null) {
                ApplyDamageByForce(hitColliders[i].gameObject, transform); 
            }
            i++;
        }
    }

    private void ApplyDamageByForce(GameObject game_obj, Transform source_transform = null) {
        ModuleSystemInfo ship_managment = game_obj.GetComponent<ModuleSystemInfo>();
        if (ship_managment != null && source_transform != null) {
            float ratio = 1 / (source_transform.position - game_obj.transform.position).sqrMagnitude;
            if (ratio > 0) {
                ship_managment.DamageShip(shockwave_damagefactor * ratio);
            }
        } else {
            DamageDealer damageDealer = game_obj.gameObject.GetComponent<DamageDealer>();
            if (damageDealer) {
                ProcessHit(damageDealer);
            }
        }
    }
}