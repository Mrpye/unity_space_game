using UnityEngine;

/// <summary>
/// This is assigned to blaster
/// </summary>
public class DamageDealer : MonoBehaviour {
    [SerializeField] private int damage = 100;
    [SerializeField] private GameObject particleSystem;
    public void SetDamage(int damage) {
        this.damage= damage;
    }

    public int GetDamage() {
        return damage;
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }

    public void Hit() {
        //**************************
        //Draw some particle effects
        //**************************
        GameObject p = Instantiate(particleSystem);
        ParticleSystem particle= p.GetComponent<ParticleSystem>();
        p.transform.position = gameObject.transform.position;
        particle.Play();
        Destroy(particle.gameObject,2);
        Destroy(gameObject);
    }
}