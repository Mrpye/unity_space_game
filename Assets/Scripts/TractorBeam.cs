using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    private ParticleSystem tractor_beam_fx;
    private PointEffector2D tractor_pull;
    private void Start() {
        tractor_beam_fx = GameObject.Find("TracktorBeam").GetComponent<ParticleSystem>();
        tractor_pull = GameObject.Find("TracktorBeam").GetComponent<PointEffector2D>();

    }

   
   

   private  void Update() {
        if (Input.GetKey(KeyCode.T)) {
            tractor_pull.enabled = true;
            tractor_beam_fx.Play();
        } else {
            tractor_pull.enabled = false;
            tractor_beam_fx.Stop();
        }
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
