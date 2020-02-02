using UnityEngine;

public class DamageDealer : MonoBehaviour {
    [SerializeField] private int damage = 100;

    public int GetDamage() {
        return damage;
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }

    public void Hit() {
        Destroy(gameObject);
    }
}