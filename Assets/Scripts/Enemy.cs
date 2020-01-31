using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   


    private void OnBecameInvisible() {
        this.enabled=false;
    }
    private void OnBecameVisible() {
        this.enabled = false;
    }


}
