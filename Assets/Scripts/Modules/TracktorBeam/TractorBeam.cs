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
        shape.position = new Vector3(0, this.Get_Range() + 0.1f, 0);
        main.startSpeed = 1.8f * this.Get_Range();
        tractor_collider.offset = new Vector2(0, this.Get_Range() * 0.5f);
        tractor_collider.size = new Vector2(width, this.Get_Range());
        tractor_pull.forceMagnitude = this.Get_Ammount();
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
        /* if (Input.GetButton("Fire1")) {
             Vector2 ray =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
             Debug.DrawRay(transform.position, ray * 100, Color.green);
             RaycastHit2D hit2D = Physics2D.Raycast(transform.position, ray, 100);
             if (hit2D) {
                 if (hit2D.transform.tag == "Pullable") {       // move the object hit!
                     target.GetComponent<Rigidbody2D>().velocity = Vector2.up;

                     hit2D.transform.position = Vector3.MoveTowards(hit2D.transform.position, target.position, Time.deltaTime * 5);
                    // print("I'm looking at " + hit2D.transform.name);
                 }
             }
         }*/
    }
}