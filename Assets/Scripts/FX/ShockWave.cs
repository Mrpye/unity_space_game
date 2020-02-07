using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private SpriteRenderer sr;
   [SerializeField] private float star_width = 0.1f;
    [SerializeField] private float start_height = 0.1f;
    [SerializeField]  private float destroy_whenn_width = 10f;
    [SerializeField] private float speed =3f;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector2(star_width, start_height);
       
    }

    // Update is called once per frame
    void Update()
    {
        star_width += star_width * speed * Time.deltaTime;
        start_height += start_height * speed * Time.deltaTime;
        if (start_height > 20) { Destroy(gameObject); }
       transform.localScale =new Vector2(star_width, start_height);
    }

    
  
    
}
