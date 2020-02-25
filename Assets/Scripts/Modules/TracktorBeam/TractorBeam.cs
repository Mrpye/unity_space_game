using UnityEngine;

public class TractorBeam : ModuleSystemInfo {
    private ParticleSystem tractor_beam_fx;
    private PointEffector2D tractor_pull;
    private BoxCollider2D tractor_collider;
    private ParticleSystem.ShapeModule shape;
    private ParticleSystem.MainModule main;

    [Header("Tractorbeam")]
    [SerializeField] private float width = 0.3f;

    private void Start() {
        //Lets set up the range and width
        tractor_beam_fx = gameObject.GetComponent<ParticleSystem>();
        tractor_pull = gameObject.GetComponent<PointEffector2D>();
        tractor_collider = gameObject.GetComponent<BoxCollider2D>();
        shape = tractor_beam_fx.shape;
        main = tractor_beam_fx.main;
    }

    private void UpdateTracktorBeam() {
        shape.position = new Vector3(0, this.settings.Range + 0.1f, 0);
        main.startSpeed = 1.8f * this.settings.Range;
        tractor_collider.offset = new Vector2(0, this.settings.Range * 0.5f);
        tractor_collider.size = new Vector2(width, this.settings.Range);
        tractor_pull.forceMagnitude = this.settings.Ammount;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.T)) {
            if (this.Is_Online()) {
                UpdateTracktorBeam();
                tractor_pull.enabled = true;
                tractor_beam_fx.Play();
                StartUsage();
            }
        } else {
            tractor_pull.enabled = false;
            tractor_beam_fx.Stop();
            StopUsage();
        }
        UpdateUsage();
       
    }
}