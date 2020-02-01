using System.Collections;
using UnityEngine;

public class ProceduralGenrator : MonoBehaviour {
    private int seed;
    [SerializeField] private GameObject Asteroidprefab;
    [SerializeField] private GameObject maerialPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject player;
    private const int size = 500;
    private const int offset = 250;
    //private GameObject[,] map = new GameObject[size, size];

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
        int c_width = 50;
        int c_height = 30;

        int h_width = c_height;
        int h_height = c_width;

        for (int y = (c_y - h_height - 1); y < c_y + c_height + 1; y ++) {
            for (int x = (c_x - h_width - 1); x < c_x + h_width + 1; x ++) {
                int seed = ((x + 1000) * y);
                Random.InitState(seed);

                double res = Random.Range(1, 1000);
                int newres = (int)res;

                if (newres == 5 || newres == 1) {
                    string key = (x).ToString() + "-" + (y).ToString();
                    object t = items[key];
                    if (t == null) { items.Add(key,true); Instantiate(Asteroidprefab, new Vector3(x, y, 0), transform.rotation); }
                } else if (newres == 3 || newres == 7 || newres == 2) {
                    string key = (x).ToString() + "-" + (y).ToString();
                    object t = items[key];
                    if (t == null) { items.Add(key, true); Instantiate(maerialPrefab, new Vector3(x, y, 0), transform.rotation); }
                } else if (newres == 4 || newres == 8 || newres == 2) {
                    string key = (x).ToString() + "-" + (y).ToString();
                    object t = items[key];
                    if (t == null) { items.Add(key, true); Instantiate(enemyPrefab, new Vector3(x, y, 0), transform.rotation); }
                }
            }
        }
        //Lets randomize so other randoms arnt the same
        Random.InitState(System.Environment.TickCount);
    }
}