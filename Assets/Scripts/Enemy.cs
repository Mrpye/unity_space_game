using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    void OnMouseDown() {
        // this object was clicked - do something

        transform.Find("Circle").gameObject.SetActive(true);
    }


    private void OnBecameInvisible() {
        this.enabled=false;
    }
    private void OnBecameVisible() {
        this.enabled = false;
    }


}
