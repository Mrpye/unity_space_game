using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipMovment : MonoBehaviour {
    private Camera cam;

    [Header("Zoom")]
    [SerializeField] private float zoom_speed = 20f;
    [SerializeField] private float min_zoom = 2f;
    [SerializeField] private float max_zoom = 8.4f;

    [Header("Rotation")]
    // [SerializeField] private bool assited_turning = false;
    //  [SerializeField] private float assited_rotate_speed = 50f;
    // [SerializeField] private float rotate_speed = 10f;
    [SerializeField] private bool rotate_player_mode = false;

    [SerializeField] private float rotate_break_drag = 2f;

    [Header("Speed")]
    // [SerializeField] private float speed_x = 50f;
    // [SerializeField] private float speed_y = 100f;
    [SerializeField] private float speed_break_drag = 1f;

    [Header("Game Objects")]
    [SerializeField] private GameObject star_continer;

    [SerializeField] private GameObject pointer;

    [Header("Key Binding")]
    [SerializeField] private Hashtable key_bindings = new Hashtable();

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float movement_y;
    [SerializeField] private float movement_x;
    [SerializeField]  private float rotaion_movement;

 
    private Vector3 oldEulerAngles;

    [Header("Debug Info")]
    [SerializeField] private float rotationDelta;

    [SerializeField] private Vector3 verlocity;
    [SerializeField] private float s;
    [SerializeField] private float mag;

    public void AddKeyBinding(KeyMappingModel mapping, GameObject module) {
        if (!string.IsNullOrEmpty(mapping.Key)){



            Propulsion p= module.GetComponent<Propulsion>();
            if (p != null) { p.Set_PlayerObj(gameObject); }



            if (key_bindings.ContainsKey(mapping.Key)) {
                List<KeyMapping> mappings = key_bindings[mapping.Key] as List<KeyMapping>;
                KeyMapping map = new KeyMapping();
                map.Key = mapping.Key;
                map.value = mapping.value;
                map.mapping_value = mapping.mapping_value;
                map.module = module;
                Debug.Log("Updating Mapping" + mapping.Key);
                mappings.Add(map);
            } else {
                List<KeyMapping> mappings = new List<KeyMapping>();
                KeyMapping map = new KeyMapping();
                map.Key = mapping.Key;
                map.value = mapping.value;
                map.mapping_value = mapping.mapping_value;
                map.module = module;
                Debug.Log("Adding Mapping" + mapping.Key);
                mappings.Add(map);
                key_bindings.Add(mapping.Key, mappings);
            }
        }
    }

    private void Start() {
        //**********************
        //Setup the Game Objects
        //**********************
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        Vector3 oldEulerAngles = transform.rotation.eulerAngles;
    }

    private Propulsion GetPropulsion(string name) {
        GameObject go = GameObject.Find(name);
        if (go != null) {
            Propulsion p = go.GetComponentInChildren<Propulsion>();
            p.Set_PlayerObj(gameObject);
            return p;
        } else {
            return null;
        }
    }

    private void Update() {
        ZoomHandler();//Handle the zoom
        Get_Player_Input();
    }

    private void Get_Player_Input() {
        //***************************
        //This handles players inputs
        //***************************
        movement_y = Input.GetAxis("Vertical");
        movement_x = Input.GetAxis("Strife");
        rotaion_movement = Input.GetAxis("Horizontal");
        //****************
        //Get infomation
        //****************
        rotationDelta = oldEulerAngles.z - transform.rotation.eulerAngles.z;
        verlocity = transform.InverseTransformDirection(rb.velocity);
        mag = rb.velocity.magnitude;
        oldEulerAngles = transform.rotation.eulerAngles;

        BreakSystem();
    }

    private void BreakSystem() {
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
            rb.angularDrag = rotate_break_drag;
        } else if (Input.GetKey(KeyCode.C)) {
            rb.drag = speed_break_drag;
        } else {
            rb.drag = 0f;
            rb.angularDrag = 0f;
        }
    }

    private void ActivateModule(string key) {
        if (key_bindings.ContainsKey(key)) {
            List<KeyMapping> key_mappings = key_bindings[key] as List<KeyMapping>;
            foreach (KeyMapping m in key_mappings) {
                Propulsion p = m.module.GetComponentInChildren<Propulsion>();
                if (p != null) {
                    p.Activate();
                }
            }
        }
    }

    private void DeActivateModule(string key) {
        if (key_bindings.ContainsKey(key)) {
            List<KeyMapping> key_mappings = key_bindings[key] as List<KeyMapping>;
            foreach (KeyMapping m in key_mappings) {
                Propulsion p = m.module.GetComponentInChildren<Propulsion>();
                if (p != null) {
                    p.Deactivate();
                }
            }
        }
    }

    private void MoveChar(float movement_x, float movement_y, float rotaion_movement) {
        if (movement_y > 0) {
            ActivateModule("FORWARD");
        } else if (movement_y < 0) {
            ActivateModule("BACK");
        } else {
            //main_eng.Deactivate();
            DeActivateModule("FORWARD");
            DeActivateModule("BACK");
        }

        if (movement_x > 0) {
            ActivateModule("STRIFE_RIGHT");
        } else if (movement_x < 0) {
            ActivateModule("STRIFE_LEFT");
        } else {
            DeActivateModule("STRIFE_RIGHT");
            DeActivateModule("STRIFE_LEFT");
        }

        if (rotaion_movement > 0) {
            ActivateModule("ROTATE_RIGHT");
        } else if (rotaion_movement < 0) {
            ActivateModule("ROTATE_LEFT");
        } else {
            DeActivateModule("ROTATE_RIGHT");
            DeActivateModule("ROTATE_LEFT");
        }

        //if (rotate_player_mode == true) {
        cam.transform.rotation = Quaternion.Euler(0, 0, 0);
        //} else {
        // star_continer.GetComponent<Transform>().Rotate(0, 0, z_rotation);
        // }
    }

    private void FixedUpdate() {
        MoveChar(movement_x, movement_y, rotaion_movement);
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

    /*
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
        */
    //****************
    //Handle x movment
    //****************
    // if (movement_x > 0) {
    // left_eng_top_fx.Play();
    // left_eng_bottom_fx.Play();
    // Start_Usage(left_eng_top_fx.gameObject);
    // Start_Usage(left_eng_bottom_fx.gameObject);
    //} else if (movement_x < 0) {
    // right_eng_top_fx.Play();
    //  right_eng_bottom_fx.Play();
    //  Start_Usage(right_eng_top_fx.gameObject);
    // Start_Usage(right_eng_bottom_fx.gameObject);
    //   } else {
    // if (left_eng_top_fx.isPlaying) { left_eng_top_fx.Stop(); Stop_Usage(left_eng_top_fx.gameObject); }
    //  if (left_eng_bottom_fx.isPlaying) { left_eng_bottom_fx.Stop(); Stop_Usage(left_eng_bottom_fx.gameObject); }
    //  if (right_eng_top_fx.isPlaying) { right_eng_top_fx.Stop(); Stop_Usage(right_eng_top_fx.gameObject); }
    //  if (right_eng_bottom_fx.isPlaying) { right_eng_bottom_fx.Stop(); Stop_Usage(right_eng_bottom_fx.gameObject); }
}

//***********************
//Handle rotation movment
//***********************
//  if (rotaion_movement > 0) {
//  left_eng_top_fx.Play();
//  right_eng_bottom_fx.Play();

//   Start_Usage(left_eng_top_fx.gameObject);
//   Start_Usage(right_eng_bottom_fx.gameObject);

// } else if (rotaion_movement < 0) {
//   right_eng_top_fx.Play();
//   left_eng_bottom_fx.Play();

// Start_Usage(right_eng_top_fx.gameObject);
// Start_Usage(left_eng_bottom_fx.gameObject);
//    } else if (movement_x == 0 && rotaion_movement == 0) {
//  if (left_eng_top_fx.isPlaying) { left_eng_top_fx.Stop(); Stop_Usage(left_eng_top_fx.gameObject); }
//   if (left_eng_bottom_fx.isPlaying) { left_eng_bottom_fx.Stop(); Stop_Usage(left_eng_bottom_fx.gameObject); }
//  if (right_eng_top_fx.isPlaying) { right_eng_top_fx.Stop(); Stop_Usage(right_eng_top_fx.gameObject); }
//    if (right_eng_bottom_fx.isPlaying) { right_eng_bottom_fx.Stop(); Stop_Usage(right_eng_bottom_fx.gameObject); }
//    }
// }

//   private void MoveChar(float movement_x, float movement_y, float rotaion_movement) {
//***************************************
//This Handles Forward and reverse Motion
//***************************************
//if (movement_y > 0) { movement_y = movement_y * 2; }
//  Vector2 force = Vector2.up * Time.deltaTime * speed_y * movement_y;
//  rb.AddRelativeForce(force);

//   Vector2 force2 = Vector2.right * Time.deltaTime * speed_x * movement_x;
//   rb.AddRelativeForce(force2);

//********************
//This handles turning
//********************
//    float z_rotation = 0;
//   if (assited_turning) {
//       z_rotation = (rotaion_movement * assited_rotate_speed) * Time.deltaTime;
//      transform.Rotate(new Vector3(0, 0, -z_rotation));
//  } else {
//       z_rotation = (rotaion_movement * rotate_speed) * Time.deltaTime;
//      rb.AddTorque(-z_rotation);
//    }

//  if (rotate_player_mode == true) {
//       cam.transform.rotation = Quaternion.Euler(0, 0, 0);
//    } else {
//      star_continer.GetComponent<Transform>().Rotate(0, 0, z_rotation);
//   }

//   }

//}