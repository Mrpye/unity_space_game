using UnityEngine;

public class Propulsion : ModuleSystemInfo {
   // []private float current_Thrust = 10f;
    private GameObject player_obj;
    private Rigidbody2D player_rb;
    private ParticleSystem fx;

    private void Start() {
        if (!this.is_in_storage) {
            fx = GetComponent<ParticleSystem>();
            this.Run_Start();
            this.StartMonitor();
        }
    }

    public void Set_PlayerObj(GameObject obj) {
        player_obj = obj;
        player_rb = obj.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    private void Update() {
        if (!this.is_in_storage) {
            UpdateUsage();
        }
    }

    public void Deactivate() {
        fx.Stop();
        StopUsage();
    }

    public void ResetCurrentThrust() {
    }

        public void Activate(float overthrust=-1) {
        if (Is_Malfunctioning()&& this.is_online && this.active) {
            //if (overthrust < current_Thrust && this.IsInUse()) { return; }


            if (overthrust > -1) { current_thrust = overthrust; }
            if (current_thrust > settings.Thrust_start) {
                current_thrust = settings.Thrust_start;
            }
            fx = GetComponent<ParticleSystem>();
            fx.Play();

            ParticleSystem.ShapeModule shape = fx.shape;
            if (player_obj != null) {
                shape.scale = player_obj.transform.localScale;
            }

            ParticleSystem.MainModule s = fx.main;
            s.startLifetime = UnityFunctions.normValue(current_thrust + 20, 0, settings.Thrust_start);

            StartUsage();

            if (player_rb == null) { return; }

            UnityFunctions.Trust_At_Point(player_rb, transform, current_thrust);
          
        }
    }


   
}