using UnityEngine;

public class DamageSystem : MonoBehaviour {
    [SerializeField] private float health = 100f;

    [Header("Destruction")]
    [SerializeField] private GameObject deathFX;
    [SerializeField] private GameObject shockwave;

    [SerializeField] private float durationOfExplosion = 0.5f;

    private CircleCollider2D explosion_force_collider;
    private PointEffector2D explosion_force_effector;

    private void Start() {
        explosion_force_collider = gameObject.GetComponent<CircleCollider2D>();
        explosion_force_effector = gameObject.GetComponent<PointEffector2D>();
    }

    private void ApplyExplosionForce() {
        if (explosion_force_collider != null && explosion_force_effector != null) {
            explosion_force_collider.enabled = true;
            explosion_force_effector.enabled = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        ShipManagment sm= other.GetComponent<ShipManagment>();
        if (sm != null) {
            //Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            float dist=  Vector3.Distance(other.transform.position, transform.position)* (explosion_force_effector.forceMagnitude*0.20f);
            float factor=(explosion_force_effector.forceMagnitude- dist);
            if (factor > 0) { 
                sm.DamageShip( factor*0.2f);
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

    private void Die() {
        ApplyExplosionForce();
        Destroy(gameObject, durationOfExplosion);
        if (deathFX == null) { return; }
        GameObject explosion = Instantiate(deathFX, transform.position, transform.rotation);
        if (shockwave != null) {
            Instantiate(shockwave, transform.position, transform.rotation);
        }
        
        
        Destroy(explosion, durationOfExplosion);
    }
}