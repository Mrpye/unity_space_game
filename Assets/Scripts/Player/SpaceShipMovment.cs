using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceShipMovment : MonoBehaviour {
    private Camera cam;
    public bool is_in_docking_zone;

    [Header("Zoom")]
    [SerializeField] private float zoom_speed = 20f;

    [SerializeField] private float min_zoom = 2f;
    [SerializeField] private float max_zoom = 8.4f;

    [Header("Rotation")]
    [SerializeField] private bool rotate_player_mode = false;

    [SerializeField] private float rotate_break_drag = 2f;

    [Header("Speed")]
    [SerializeField] private float speed_break_drag = 0.2f;

    [SerializeField] private float thrust_rotate_affect = 1f;
    [SerializeField] private float thrust_strife_affect = 1f;
    [SerializeField] private float thrust_forward_affect = 1f;
    [SerializeField] private float cruse_control_speed =20f;
    [SerializeField] private bool cruse_control = false;

    [Header("Game Objects")]
    [SerializeField] private GameObject star_continer;

    [SerializeField] private GameObject pointer;

    [Header("Key Binding")] [SerializeField] private Hashtable key_bindings = new Hashtable();

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float movement_y;
    [SerializeField] private float movement_x;
    [SerializeField] private float rotaion_movement;

    private Vector3 oldEulerAngles;

    [Header("Debug Info")]
    [SerializeField] private float rotationDelta;

    [SerializeField] private Vector3 verlocity;
    [SerializeField] private float s;
    [SerializeField] private float mag;
    [SerializeField] private Vector3 localAngleVelo;
    [SerializeField] public int flight_assist = 1;

    public void AddKeyBinding(KeyMappingModel mapping, GameObject module) {
        if (!string.IsNullOrEmpty(mapping.Key)) {
            Propulsion p = module.GetComponent<Propulsion>();
            if (p != null) { p.Set_PlayerObj(gameObject); }

            if (key_bindings.ContainsKey(mapping.Key)) {
                List<KeyMapping> mappings = key_bindings[mapping.Key] as List<KeyMapping>;
                KeyMapping map = ScriptableObject.CreateInstance<KeyMapping>();
                map.Key = mapping.Key;
                map.value = mapping.value;
                map.movement_type = mapping.movement_type;
                map.module = module;
                Debug.Log("Updating Mapping" + mapping.Key);
                mappings.Add(map);
            } else {
                List<KeyMapping> mappings = new List<KeyMapping>();

                KeyMapping map = ScriptableObject.CreateInstance<KeyMapping>();
                map.Key = mapping.Key;
                map.value = mapping.value;
                map.movement_type = mapping.movement_type;
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
        cam.transform.rotation = Quaternion.Euler(0, 0, (int)-transform.rotation.z);
        ZoomHandler();//Handle the zoom
        Get_Player_Input();
        cam.transform.rotation = Quaternion.Euler(0, 0, (int)-transform.rotation.z);
    }

    public void Launch() {
        SceneManager.LoadScene("Game_Scene");
    }

    private void CalcThrustFactor() {
        thrust_rotate_affect = 1f;
        thrust_strife_affect = 1f;
        thrust_forward_affect = 1f;

        if (Mathf.Abs(movement_y) > 0 && Mathf.Abs(rotaion_movement) > 0) {
            thrust_rotate_affect = 0.1f;
            //thrust_strife_affect = 1f;
            //thrust_forward_affect = 1f;
        }
    }

    private void Get_Player_Input() {
        //***************************
        //This handles players inputs
        //***************************
        movement_y = Input.GetAxis("Vertical");
        movement_x = Input.GetAxis("Strife");
        rotaion_movement = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.K)) {
            cruse_control = !cruse_control;
            this.cruse_control_speed = rb.velocity.magnitude;
        }


        if (movement_y < 0) {
            cruse_control = false;
        } else if( cruse_control == true && mag < cruse_control_speed) {
            movement_y = 1;
        }

       

            CalcThrustFactor();
        if (Input.GetKeyDown(KeyCode.F)) {
            this.flight_assist += 1;
            if (this.flight_assist > 2) {
                this.flight_assist = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.L) && is_in_docking_zone) {
            LoadSave ls = gameObject.GetComponent<LoadSave>();
            if (ls != null) { ls.SavePlayer(); }

            GameObject go = GameObject.Find("ProcduralGenerator");
            if (go != null) {
                ProceduralGenrator pg = go.GetComponent<ProceduralGenrator>();
                if (pg != null) {
                    pg.Save();
                }
            }
            SceneManager.LoadScene("Ship_Editor");
        }
        //****************
        //Get infomation
        //****************
        //rotationDelta = -1*rb.angularVelocity;

        rotationDelta = oldEulerAngles.z - transform.rotation.eulerAngles.z;
        //Vector3 localAngleVelo = transform.InverseTransformVector(transform.GetComponent<Rigidbod2D>().angularVelocity);

        if (rb != null) {
            verlocity = transform.InverseTransformDirection(rb.velocity);
            mag = rb.velocity.magnitude;
        }
        oldEulerAngles = transform.rotation.eulerAngles;

        BreakSystem();
    }

    private float Calc_Rotation_Trust(float max_trust, float reduction = 1) {
        return Mathf.Clamp(Mathf.Abs(rotationDelta) * (max_trust * reduction), 0, max_trust);
    }

    private float Calc_Strife_Trust(float max_trust, float reduction = 1) {
        return Mathf.Clamp(Mathf.Abs(verlocity.x) * (max_trust * reduction), 0, max_trust);
    }

    private float Calc_forward_Trust(float max_trust, float reduction = 1) {
        return Mathf.Clamp(Mathf.Abs(verlocity.y) * (max_trust * reduction), 0, max_trust);
    }

    private void BreakSystem_Stop_Roatation() {
        if (rotationDelta > 0) {
            ActivateModule("ROTATE_LEFT", Enums.enum_movment_type.rotation);
        } else if (rotationDelta < 0) {
            ActivateModule("ROTATE_RIGHT", Enums.enum_movment_type.rotation);
        } else {
            DeActivateModule("ROTATE_RIGHT");
            DeActivateModule("ROTATE_LEFT");
        }
    }

    private void BreakSystem_Stop_Strife() {
        if (verlocity.x > 0) {
            ActivateModule("STRIFE_LEFT", Enums.enum_movment_type.strife);
        } else if (verlocity.x < 0) {
            ActivateModule("STRIFE_RIGHT", Enums.enum_movment_type.strife);
        } else {
            DeActivateModule("STRIFE_RIGHT");
            DeActivateModule("STRIFE_LEFT");
        }
    }

    private void BreakSystem_Stop_Forward() {
        if (verlocity.y > 0) {
            ActivateModule("BACK", Enums.enum_movment_type.forward_backward);
        } else if (verlocity.y < 0) {
            ActivateModule("FORWARD", Enums.enum_movment_type.forward_backward);
        } else {
            DeActivateModule("FORWARD");
            DeActivateModule("BACK");
        }
    }

    private void BreakSystem() {
        //***************
        //Breaking system
        //***************
        if (rb == null) { return; }
        if (Input.GetKey(KeyCode.X)) {
            BreakSystem_Stop_Roatation();
            BreakSystem_Stop_Strife();
            BreakSystem_Stop_Forward();
        } else if (Input.GetKey(KeyCode.Z)) {
            BreakSystem_Stop_Roatation();
        } else if (Input.GetKey(KeyCode.C)) {
            BreakSystem_Stop_Strife();
            BreakSystem_Stop_Forward();
        } else {
            rb.drag = 0f;
            //rb.angularDrag = 0f;
        }
    }

    // private float Thrust_Ammount(float max_trust) {
    // }
    private void ActivateModule(string key, Enums.enum_movment_type movtype = Enums.enum_movment_type.none) {
        if (key_bindings.ContainsKey(key)) {
            List<KeyMapping> key_mappings = key_bindings[key] as List<KeyMapping>;
            foreach (KeyMapping m in key_mappings) {
                Propulsion p = m.module.GetComponentInChildren<Propulsion>();
                if (p != null) {
                    if (movtype != Enums.enum_movment_type.none) {
                        if (movtype == Enums.enum_movment_type.rotation) {
                            p.Activate(Calc_Rotation_Trust(p.Get_Calculated_Thrust(), speed_break_drag) * thrust_rotate_affect);
                        } else if (movtype == Enums.enum_movment_type.strife) {
                            p.Activate(Calc_Strife_Trust(p.Get_Calculated_Thrust(), speed_break_drag) * thrust_strife_affect);
                        } else if (movtype == Enums.enum_movment_type.forward_backward) {
                            p.Activate(Calc_forward_Trust(p.Get_Calculated_Thrust(), speed_break_drag) * thrust_forward_affect);
                        }
                    } else {
                        if (m.movement_type == Enums.enum_movment_type.rotation) {
                            p.Activate(Mathf.Abs(Calc_Rotation_Trust(p.Get_Calculated_Thrust(m.value))));
                        } else if (m.movement_type == Enums.enum_movment_type.strife) {
                            p.Activate(Mathf.Abs(Calc_Strife_Trust(p.Get_Calculated_Thrust(m.value))));
                        } else if (m.movement_type == Enums.enum_movment_type.forward_backward) {
                            p.Activate(Mathf.Abs(Calc_forward_Trust(p.Get_Calculated_Thrust(m.value))));
                        } else {
                            p.Activate(m.value);
                        }
                    }
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

    private string Merge(string a, string b) {
        string bstr = a;

        if (bstr != "" && b != "") {
            bstr = bstr + ":" + b;
        } else {
            if (b != "") {
                bstr = b;
            }
        }

        return bstr;
    }

    private void MoveChar(float movement_x, float movement_y, float rotaion_movement) {
        string key_FB = "";
        string key_R = "";
        string key_S = "";
        string key_all = "";
        bool MultiKey = false;

        if (movement_y > 0) {
            key_FB = "FORWARD";
            DeActivateModule("BACK");
        } else if (movement_y < 0) {
            key_FB = "BACK";
            DeActivateModule("FORWARD");
        }

        if (movement_x > 0) {
            key_S = "STRIFE_RIGHT";
            DeActivateModule("STRIFE_LEFT");
        } else if (movement_x < 0) {
            key_S = "STRIFE_LEFT";
            DeActivateModule("STRIFE_RIGHT");
        }

        if (rotaion_movement > 0) {
            key_R = "ROTATE_RIGHT";
            DeActivateModule("ROTATE_LEFT");
        } else if (rotaion_movement < 0) {
            key_R = "ROTATE_LEFT";
            DeActivateModule("ROTATE_RIGHT");
        }

        key_all = Merge(key_all, key_FB);
        key_all = Merge(key_all, key_S);
        key_all = Merge(key_all, key_R);
        if (key_all != "") {
            ActivateModule(key_all);
            if (key_all.Contains(":")) {
                MultiKey = true;
            }
        }

        if (key_FB == "") {
            DeActivateModule("FORWARD");
            DeActivateModule("BACK");
            if (flight_assist == 1 && MultiKey == false) { BreakSystem_Stop_Forward(); }
        }

        if (key_R == "") {
            DeActivateModule("ROTATE_RIGHT");
            DeActivateModule("ROTATE_LEFT");
            if (flight_assist > 0 && MultiKey == false) {
                BreakSystem_Stop_Roatation();
            }
        }

        if (key_S == "") {
            DeActivateModule("STRIFE_RIGHT");
            DeActivateModule("STRIFE_LEFT");
            if (flight_assist > 0 && MultiKey ==false) {
                BreakSystem_Stop_Strife();
            }
        }
    }

    private void FixedUpdate() {
        MoveChar(movement_x, movement_y, rotaion_movement);
    }

    private void ZoomHandler() {
        if (rb == null) { return; }
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