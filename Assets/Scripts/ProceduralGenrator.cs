using System.Collections;
using UnityEngine;

public class ProceduralGenrator : MonoBehaviour {
    private const int a = 16807;
    private const int m = 2147483647;
    private const int q = 127773;
    private const int r = 2836;
    private int seed;
    [SerializeField] private GameObject Asteroidprefab;
    [SerializeField] private GameObject maerialPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject player;
    private const int size = 500;
    private const int offset = 250;
    private GameObject[,] map = new GameObject[size, size];

    private int l_c_x = 0;
    private int l_c_y = 0;
    private int l_c_width = 0;
    private int l_c_height = 0;

    private Hashtable items = new Hashtable();
   

    private void Start() {
        l_c_x = (int)player.transform.position.x;
        l_c_y = (int)player.transform.position.y;
        l_c_width = Camera.main.scaledPixelWidth / 40;
        l_c_height = Camera.main.scaledPixelHeight / 40;
        Calc_Objects();
        items.Clear();
    }

    private void Update() {
        Calc_Objects();
    }

    private void Calc_Objects() {
        int c_x = (int)player.transform.position.x;
        int c_y = (int)player.transform.position.y;
        int c_width = 10;// Camera.main.scaledPixelWidth / 10;
        int c_height = 10;// Camera.main.scaledPixelHeight / 10;

        int h_width = c_height / 2;
        int h_height = c_width / 2;
/*
        Hashtable tempitems = new Hashtable();
        foreach (DictionaryEntry e in items) {
            GameObject a = (GameObject)e.Value;
            if (a != null) {
                if (a.transform.position.x < c_x- h_width || a.transform.position.x > c_x + h_width || a.transform.position.y < c_y- h_height || a.transform.position.y > c_y + h_height) {
                    Destroy(a.gameObject);
                    a = null;
                } else {
                    tempitems.Add(e.Key, e.Value);
                }
            }
        }
     
        items = tempitems;*/
        for (int y = (c_y- h_height-1); y < c_y + c_height + 1; y++) {
            for (int x = (c_x- h_width - 1); x < c_x + h_width + 1; x++) {
                int seed = ((x + 1000) * y);
                Random.InitState(seed);

                double res = Random.Range(1, 1000);
                int newres = (int)res;

                string key = (x).ToString() + "-" + (y).ToString();
                object t = items[key];

                if (newres == 5 || newres == 1) {
                    if (t == null) { items.Add(key, Instantiate(Asteroidprefab, new Vector3(x, y, 0), transform.rotation)); }
                } else if (newres == 3 || newres == 7 || newres == 2) {
                    if (t == null) { items.Add(key, Instantiate(maerialPrefab, new Vector3(x, y, 0), transform.rotation)); }
                } else if (newres == 4 || newres == 8 || newres == 2) {
                    if (t == null) { items.Add(key, Instantiate(enemyPrefab, new Vector3(x, y, 0), transform.rotation)); }
                }
            }
        }
    }
}