using UnityEngine;

public class EnemyAI : MonoBehaviour {
    private Transform target_transform;
    private Transform enemy_transform;
    [SerializeField ]private GameObject fire_system;

    //AI stuff
    public float rotationSpeed;

    public float maxDist;
    public float fireDist;

    //AI Speeds
    public float moveSpeed;

    private enum FacingDirection {
        UP = 270,
        DOWN = 90,
        LEFT = 180,
        RIGHT = 0
    }

    // public Transform target;
    private Vector3 v_diff;

    private float atan2;

    private void Start() {
        target_transform = GameObject.Find("Player").transform;
        enemy_transform = gameObject.transform;
       
    }

    private void Update() {
        if (IsPlayerWithinApproachRange()) {
            RotateTowardsPlayer();
            MoveForward();
            if (fire_system != null) { 
            Turret w= fire_system.GetComponent<Turret>();
            if (w != null) {
                w.fire_range = fireDist;
            }
            }
        }
    }

    private void RotateTowardsPlayer() {
        if (enemy_transform == null) { return; }
        LookAt2D(target_transform, 2f, FacingDirection.UP);
    }

    private void LookAt2D(Transform theTarget, float theSpeed, FacingDirection facing) {
        Vector3 vectorToTarget = theTarget.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        angle -= (float)facing;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * theSpeed);
    }

    private void MoveForward() {
        if (enemy_transform == null) { return; }
        enemy_transform.position = Vector2.MoveTowards(enemy_transform.position, target_transform.position, moveSpeed * Time.deltaTime);
    }

    private bool IsPlayerWithinApproachRange() {
        if (target_transform == null) { return false; }
        float distance = Vector3.Distance(target_transform.position, enemy_transform.position);
        return distance < maxDist;
    }

    private bool IsPlayerWithinAttackRange() {
        if (target_transform == null) { return false; }
        float distance = Vector3.Distance(target_transform.position, enemy_transform.position);
        return distance < fireDist;
    }

    private void Fire() {
        // Fire code here
    }
}