using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propulsion : ModuleSystemInfo
{
    [SerializeField] private float trust = 10f;
    private GameObject player_obj;
    private Rigidbody2D player_rb;

    private ParticleSystem fx;
    void Start()
    {
       // this.StoreItem();
        
    }

    public void Set_PlayerObj(GameObject obj) {
        player_obj = obj;
        player_rb = obj.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
   
    void Update()
    {
        UpdateUsage();
        trust = Get_Thrust();
    }
    public void Deactivate() {
        fx = GetComponent<ParticleSystem>();
        fx.Stop();
        StopUsage();
    }

    public void Activate(float overthrust) {
        fx = GetComponent<ParticleSystem>();
        fx.Play();
        ParticleSystem.ShapeModule shape = fx.shape;
        if (player_obj != null) {
            shape.scale = player_obj.transform.localScale;
        }
       
        StartUsage();
        if(player_rb == null) { return; }
        Vector2 force = -1*transform.up * Time.deltaTime * overthrust;
        player_rb.AddForceAtPosition(force, transform.position,ForceMode2D.Impulse);
  
    }
}
