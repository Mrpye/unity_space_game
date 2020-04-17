using UnityEngine;

public class TractorBeam : ModuleSystemInfo {
    private ParticleSystem tractor_beam_fx;
    private ParticleSystem.ShapeModule shape;
    private ParticleSystem.MainModule main;
    private GameObject FirePoint;
    //private GameObject player;
    private Rigidbody2D prb;
    [Header("Tractorbeam")]
    [SerializeField] private float width = 0.3f;

    private void Start() {
        if (!this.is_in_storage) {
            //Lets set up the range and width
            tractor_beam_fx = gameObject.GetComponent<ParticleSystem>();
            FirePoint = gameObject.transform.Find("FirePoint").gameObject;
            shape = tractor_beam_fx.shape;
            main = tractor_beam_fx.main;
            //player = GameObject.Find("Player");
            if (UnityFunctions.player != null) { prb = UnityFunctions.player.GetComponent<Rigidbody2D>(); }

            this.Run_Start();
        }
    }

    private void UpdateTracktorBeam() {
        float range = this.Get_Calculated_Range();
        shape.position = new Vector3(0, range + 0.1f, 0);
        main.startSpeed = 1.8f * this.settings.Range;

        ApplyExplosionForce(range, this.settings.Ammount);
    }

    private void ApplyExplosionForce(float range, float forceMultiplier) {
        FirePoint.transform.localPosition = new Vector3(0, range * 0.5f);
        int hit_mask = (1 << LayerMask.NameToLayer("game-assets"));
        hit_mask |= (1 << LayerMask.NameToLayer("Material"));
        //hit_mask = ~hit_mask;
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(new Vector2(FirePoint.transform.position.x, FirePoint.transform.position.y), new Vector2(width, range), UnityFunctions.player.transform.rotation.eulerAngles.z, hit_mask);
        if (hitColliders.Length > 0) {
        }

        for (int i = 0; i < hitColliders.Length; i++) {
            SpriteRenderer sr = hitColliders[i].GetComponent<SpriteRenderer>();
            Rigidbody2D rb = hitColliders[i].GetComponent<Rigidbody2D>();
            if (rb != null) {
                Vector2 force;
                if (prb.mass > rb.mass) {
                    force=-(UnityFunctions.player.transform.position - transform.position) * (forceMultiplier * 100);
                    rb.AddForce(force);
                } else {
                    force = -(transform.position- UnityFunctions.player.transform.position ) * (forceMultiplier * 100);
                    prb.AddForce(force);
                }

               
                
            }
        }
    }

    private void Update() {
        if (!this.is_in_storage) {
            if (this.is_online && this.active) {
                if ((Input.GetMouseButton(2) || Input.GetKey(KeyCode.T)) && !UnityFunctions.controls_locked) {
                    if (this.Is_Malfunctioning()) {
                        UpdateTracktorBeam();
                        tractor_beam_fx.Play();
                        StartUsage();
                    }
                } else {
                    tractor_beam_fx.Stop();
                    StopUsage();
                }
            } else {
                tractor_beam_fx.Stop();
                StopUsage();
            }
            UpdateUsage();
        }
    }
}