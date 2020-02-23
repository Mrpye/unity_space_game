using System.Collections;
using UnityEngine;

public class DamageSystem : MonoBehaviour {
    [SerializeField] private float health = 100f;

    [Header("Destruction")]
    [SerializeField] private GameObject deathFX;

    [SerializeField] private GameObject shockwave;
    [SerializeField] private float durationOfExplosion = 0.5f;

    private CircleCollider2D explosion_force_collider;
    private PointEffector2D explosion_force_effector;
    private bool death_running = false;

    private void Start() {
        explosion_force_collider = gameObject.GetComponent<CircleCollider2D>();
        explosion_force_effector = gameObject.GetComponent<PointEffector2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        ShipManagment ship_managment = other.GetComponent<ShipManagment>();
        if (ship_managment != null) {
            //If near a ship appy force and damage
            float dist = Vector3.Distance(other.transform.position, transform.position) * (explosion_force_effector.forceMagnitude * 0.20f);
            float factor = (explosion_force_effector.forceMagnitude - dist);
            if (factor > 0) {
                ship_managment.DamageShip(factor * 0.2f);
            }
        } else {
            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            if (damageDealer) {
                ProcessHit(damageDealer);
            }
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
        ApplyExplosionForce();
        SpriteRenderer sr = null;


        //***************
        //Disable Sprites
        //***************
        sr = gameObject.GetComponent<SpriteRenderer>();
        if (sr != null) { sr.enabled = false; }
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

    private void ApplyExplosionForce() {
        if (explosion_force_collider != null && explosion_force_effector != null) {
            explosion_force_collider.enabled = true;
            explosion_force_effector.enabled = true;
        }
    }

    private void Die() {
        if (death_running == false) { StartCoroutine(DeathProcesses()); }
    }
}