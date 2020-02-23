using UnityEngine;

public class Propulsion : ModuleSystemInfo {
    private float current_Thrust = 10f;
    private GameObject player_obj;
    private Rigidbody2D player_rb;
    private ParticleSystem fx;

    private void Start() {
        fx = GetComponent<ParticleSystem>();
        this.StartMonitor();
    }

    public void Set_PlayerObj(GameObject obj) {
        player_obj = obj;
        player_rb = obj.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    private void Update() {
        UpdateUsage();
    }

    public void Deactivate() {
        fx.Stop();
        StopUsage();
    }

    public void ResetCurrentThrust() {
    }

    public void Activate(float overthrust) {
        if (Is_Online()) {
            if (overthrust < current_Thrust && this.IsInUse()) { return; }
            current_Thrust = overthrust;
            if (current_Thrust > max_thrust) {
                current_Thrust = max_thrust;
            }
            fx = GetComponent<ParticleSystem>();
            fx.Play();

            ParticleSystem.ShapeModule shape = fx.shape;
            if (player_obj != null) {
                shape.scale = player_obj.transform.localScale;
            }

            ParticleSystem.MainModule s = fx.main;
            s.startLifetime = UnityFunctions.normValue(current_Thrust + 20, 0, max_thrust);

            StartUsage();

            if (player_rb == null) { return; }

            UnityFunctions.Trust_At_Point(player_rb, transform, current_Thrust);
          
        }
    }


   
}