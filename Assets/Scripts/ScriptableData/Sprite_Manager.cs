using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_Manager : MonoBehaviour
{
    private SpriteRenderer sr;
    public Sprite_Data sprites;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) {
            Sprite s = sprites.Get_Randon_Sprite();
            if (s != null) {
                sr.sprite = sprites.Get_Randon_Sprite();
            } else {
                sr.sprite = sprites.Get_Randon_Sprite();
            }
           
        }
    }

}
