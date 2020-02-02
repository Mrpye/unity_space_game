using UnityEngine;

public class SpaceShipMovment : MonoBehaviour {
    private Camera cam;

    [Header("Zoom")]
    [SerializeField] private float zoom_speed = 20f;

    [SerializeField] private float min_zoom = 2f;
    [SerializeField] private float max_zoom = 8.4f;

    [Header("Rotation")]
    [SerializeField] private bool assited_turning = false;

    [SerializeField] private float assited_rotate_speed = 50f;
    [SerializeField] private float rotate_speed = 10f;
    [SerializeField] private bool rotate_player_mode = false;
    [SerializeField] private float rotate_break_drag = 2f;

    [Header("Speed")]
    [SerializeField] private float speed_x = 50f;

    [SerializeField] private float speed_y = 100f;
    [SerializeField] private float speed_break_drag = 1f;

    [Header("Game Objects")]
    [SerializeField] private GameObject star_continer;

    [SerializeField] private GameObject pointer;

    private Rigidbody2D rb;
    private float movement_y;
    private float movement_x;
    private float rotaion_movement;

    private ParticleSystem main_eng_fx;

    private ParticleSystem front_eng_left_fx;
    private ParticleSystem front_eng_right_fx;

    private ParticleSystem left_eng_top_fx;
    private ParticleSystem left_eng_bottom_fx;

    private ParticleSystem right_eng_top_fx;
    private ParticleSystem right_eng_bottom_fx;

    private Vector3 oldEulerAngles;

    [Header("Debug Info")]
    [SerializeField] private float rotationDelta;

    [SerializeField] private Vector3 verlocity;
    [SerializeField] private float s;
    [SerializeField] private float mag;

    private void Start() {
        //**********************
        //Setup the Game Objects
        //**********************
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        main_eng_fx = GameObject.Find("MainEngine").GetComponent<ParticleSystem>();
        front_eng_left_fx = GameObject.Find("FrontEngine_Left").GetComponent<ParticleSystem>();
        front_eng_right_fx = GameObject.Find("FrontEngine_Right").GetComponent<ParticleSystem>();

        left_eng_top_fx = GameObject.Find("LeftEngine_Top").GetComponent<ParticleSystem>();
        left_eng_bottom_fx = GameObject.Find("LeftEngine_Bottom").GetComponent<ParticleSystem>();

        right_eng_top_fx = GameObject.Find("RightEngine_Top").GetComponent<ParticleSystem>();
        right_eng_bottom_fx = GameObject.Find("RightEngine_Bottom").GetComponent<ParticleSystem>();

        Vector3 oldEulerAngles = transform.rotation.eulerAngles;
    }

    private void Update() {
        ZoomHandler();//Handle the zoom

        Get_Player_Input();

        /*  Debug.Log(movement_y);
          if (Input.GetKeyDown(KeyCode.M)) {
              Vector2 dir = rb.velocity;

              float s = 1 * dir.magnitude + 1;
              pointer.transform.localScale = new Vector3(s, s, 1);
              float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
              transform.Rotate(new Vector3(0, 0, angle - 270));
              //transform.rotation = Quaternion.AngleAxis(angle - 270, Vector3.forward);
          }*/
    }

    private void Get_Player_Input() {
        //***************************
        //This handles players inputs
        //***************************
        movement_y = Input.GetAxis("Vertical");
        movement_x = Input.GetAxis("Strife");
        rotaion_movement = Input.GetAxis("Horizontal");
        rotationDelta = oldEulerAngles.z - transform.rotation.eulerAngles.z;
        verlocity = transform.InverseTransformDirection(rb.velocity);
        mag = rb.velocity.magnitude;
        //***************
        //Breaking system
        //***************
        if (Input.GetKey(KeyCode.X)) {
            if (rotationDelta > -0.01f && rotationDelta < 0.01f) {
                rb.angularDrag = rotate_break_drag * 4;
            } else {
                rb.angularDrag = rotate_break_drag;
            }
            if (rb.velocity.magnitude < 0.2) {
                rb.drag = speed_break_drag * 100;
            } else {
                rb.drag = speed_break_drag;
            }
            EngineParticles(-verlocity.x, -verlocity.y, -rotationDelta);
        } else if (Input.GetKey(KeyCode.Z)) {
            rb.angularDrag = rotate_break_drag;
            EngineParticles(0, 0, -rotationDelta);
        } else if (Input.GetKey(KeyCode.C)) {
            rb.drag = speed_break_drag;
            EngineParticles(-verlocity.x, -verlocity.y, 0);
        } else {
            rb.drag = 0f;
            rb.angularDrag = 0f;
        }

        oldEulerAngles = transform.rotation.eulerAngles;
    }

    private void FixedUpdate() {
        MoveChar(movement_x, movement_y, rotaion_movement);
        EngineParticles(movement_x, movement_y, rotaion_movement);
    }

    private void Start_Usage(GameObject go) {
        ModuleSystemInfo ms = go.GetComponent<ModuleSystemInfo>();
        if (ms != null) {
            ms.StartUsage();
        }
    }

    private void Stop_Usage(GameObject go) {
        ModuleSystemInfo ms = go.GetComponent<ModuleSystemInfo>();
        if (ms != null) {
            ms.StopUsage();
        }
    }

    private void EngineParticles(float movement_x, float movement_y, float rotaion_movement) {
        //****************
        //Handle y movment
        //****************
        if (movement_y > 0) {
            main_eng_fx.Play();
            Start_Usage(main_eng_fx.gameObject);
        } else if (movement_y < 0) {
            front_eng_left_fx.Play();
            front_eng_right_fx.Play();
            Start_Usage(front_eng_left_fx.gameObject);
            Start_Usage(front_eng_right_fx.gameObject);
        } else {
            if (main_eng_fx.isPlaying) { main_eng_fx.Stop(); Stop_Usage(main_eng_fx.gameObject); }
            if (front_eng_left_fx.isPlaying) { front_eng_left_fx.Stop(); Stop_Usage(front_eng_left_fx.gameObject); }
            if (front_eng_right_fx.isPlaying) { front_eng_right_fx.Stop(); Stop_Usage(front_eng_right_fx.gameObject); }
        }

        //****************
        //Handle x movment
        //****************
        if (movement_x > 0) {
            left_eng_top_fx.Play();
            left_eng_bottom_fx.Play();
            Start_Usage(left_eng_top_fx.gameObject);
            Start_Usage(left_eng_bottom_fx.gameObject);
        } else if (movement_x < 0) {
            right_eng_top_fx.Play();
            right_eng_bottom_fx.Play();
            Start_Usage(right_eng_top_fx.gameObject);
            Start_Usage(right_eng_bottom_fx.gameObject);
        } else {
            if (left_eng_top_fx.isPlaying) { left_eng_top_fx.Stop(); Stop_Usage(left_eng_top_fx.gameObject); }
            if (left_eng_bottom_fx.isPlaying) { left_eng_bottom_fx.Stop(); Stop_Usage(left_eng_bottom_fx.gameObject); }
            if (right_eng_top_fx.isPlaying) { right_eng_top_fx.Stop(); Stop_Usage(right_eng_top_fx.gameObject); }
            if (right_eng_bottom_fx.isPlaying) { right_eng_bottom_fx.Stop(); Stop_Usage(right_eng_bottom_fx.gameObject); }
        }

        //***********************
        //Handle rotation movment
        //***********************
        if (rotaion_movement > 0) {
            left_eng_top_fx.Play();
            right_eng_bottom_fx.Play();

            Start_Usage(left_eng_top_fx.gameObject);
            Start_Usage(right_eng_bottom_fx.gameObject);

        } else if (rotaion_movement < 0) {
            right_eng_top_fx.Play();
            left_eng_bottom_fx.Play();

            Start_Usage(right_eng_top_fx.gameObject);
            Start_Usage(left_eng_bottom_fx.gameObject);
        } else if (movement_x == 0 && rotaion_movement == 0) {
            if (left_eng_top_fx.isPlaying) { left_eng_top_fx.Stop(); Stop_Usage(left_eng_top_fx.gameObject); }
            if (left_eng_bottom_fx.isPlaying) { left_eng_bottom_fx.Stop(); Stop_Usage(left_eng_bottom_fx.gameObject); }
            if (right_eng_top_fx.isPlaying) { right_eng_top_fx.Stop(); Stop_Usage(right_eng_top_fx.gameObject); }
            if (right_eng_bottom_fx.isPlaying) { right_eng_bottom_fx.Stop(); Stop_Usage(right_eng_bottom_fx.gameObject); }
        }
    }

    private void MoveChar(float movement_x, float movement_y, float rotaion_movement) {
        //***************************************
        //This Handles Forward and reverse Motion
        //***************************************
        if (movement_y > 0) { movement_y = movement_y * 2; }
        Vector2 force = Vector2.up * Time.deltaTime * speed_y * movement_y;
        rb.AddRelativeForce(force);

        Vector2 force2 = Vector2.right * Time.deltaTime * speed_x * movement_x;
        rb.AddRelativeForce(force2);

        //********************
        //This handles turning
        //********************
        float z_rotation = 0;
        if (assited_turning) {
            z_rotation = (rotaion_movement * assited_rotate_speed) * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, -z_rotation));
        } else {
            z_rotation = (rotaion_movement * rotate_speed) * Time.deltaTime;
            rb.AddTorque(-z_rotation);
        }

        if (rotate_player_mode == true) {
            cam.transform.rotation = Quaternion.Euler(0, 0, 0);
        } else {
            star_continer.GetComponent<Transform>().Rotate(0, 0, z_rotation);
        }

        //Draw Vector arrow
        /*Vector2 dir = rb.velocity;
         s = 1 * dir.magnitude + 1;
       // pointer.transform.localScale = new Vector3(s, s, 1);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        pointer.transform.rotation = Quaternion.AngleAxis(angle -90, Vector3.forward);
        if (s <= 1) {
            pointer.SetActive(false);
        } else {
            pointer.SetActive(true);
        }*/

        /* transform.Rotate(new Vector3(0, 0, Input.GetAxis("Horizontal") * rotate_speed) * Time.deltaTime);
         if (Input.GetKeyDown(KeyCode.W)) {
             Rigidbody2D rb = GetComponent<Rigidbody2D>();
             rb.AddForce(-transform.forward * 500);
         }*/
    }

    private void ZoomHandler() {
        //**************************
        //This Handles the zoom Mode
        //**************************
        float zoom = cam.orthographicSize;
        float scroll_data = Input.GetAxis("Mouse ScrollWheel");
        zoom += scroll_data * zoom_speed;
        zoom = Mathf.Clamp(zoom, min_zoom, max_zoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, Time.deltaTime * zoom_speed);
    }
}