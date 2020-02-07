using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propulsion : ModuleSystemInfo
{
    [SerializeField] private float trust = 10f;
    //private GameObject player_obj;
    private Rigidbody2D player_rb;

    private ParticleSystem fx;
    void Start()
    {
        
        
    }

    public void Set_PlayerObj(GameObject obj) {
        //player_obj = obj;
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

    public void Activate() {
        fx = GetComponent<ParticleSystem>();
        fx.Play();
        StartUsage();
        Vector2 force = transform.up * Time.deltaTime * trust;
        player_rb.AddForceAtPosition(force, transform.position,ForceMode2D.Impulse);
  
    }
}
