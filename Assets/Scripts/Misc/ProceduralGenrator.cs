using System.Collections;
using UnityEngine;

public class ProceduralGenrator : MonoBehaviour {
    private int seed;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject temp_prefab;

    private const int size = 500;
    private const int offset = 250;
    private EnvSaveModels model_save;

    //private GameObject[,] map = new GameObject[size, size];
    private GameObject gameenelmets_object;

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
        //Calc_Objects();
        items.Clear();
        gameenelmets_object = GameObject.Find("GameElements");
        Load();
    }

    private void Update() {
       Calc_Objects();
    }

    public void Save() {
        model_save = new EnvSaveModels();
        model_save.ReadItems(gameenelmets_object.transform);
        model_save.ReadKeyPair(items);
        model_save.Save();
    }

    public void Load() {
        //Delete all old objects
        foreach (Transform item in gameenelmets_object.transform) {
            Destroy(item);
        }
        items.Clear();

        //Load
        model_save = EnvSaveModels.Load();
        if (model_save != null) {
            foreach (DataItem d in model_save.generated) {
                items.Add(d.Key, d.Value);
            }

            foreach (EnvSaveModel d in model_save.items) {
                //Load the resorces
                GameObject refab = Resources.Load(d.Module_Resource.ToString()) as GameObject;
                if (refab != null) {
                    GameObject obj_module = Instantiate(refab, d.position, d.rotation, gameenelmets_object.transform) as GameObject;
                   // refab.transform.localScale = d.scale;
                }
            }
        }
    }

    private void Calc_Objects() {
        int c_x = (int)player.transform.position.x;
        int c_y = (int)player.transform.position.y;
        int c_width = 50;
        int c_height = 30;

        int h_width = c_height;
        int h_height = c_width;

        for (int y = (c_y - h_height - 1); y < c_y + c_height + 1; y++) {
            for (int x = (c_x - h_width - 1); x < c_x + h_width + 1; x++) {
                int seed = ((x + 2000) * y);
                Random.InitState(seed);

                double res = Random.Range(1, 1000);
                int newres = (int)res;

                if (newres == 5 || newres == 1) {
                    string key = (x).ToString() + "x" + (y).ToString();
                    object t = items[key];
                    temp_prefab  = Resources.Load("GameAssets\\Asteroid\\Asteroid_large") as GameObject;
                    if (t == null) { items.Add(key, true); Instantiate(temp_prefab, new Vector3(x, y, 0), transform.rotation, gameenelmets_object.transform); }
                } else if (newres == 3 || newres == 7 || newres == 2) {
                    string key = (x).ToString() + "x" + (y).ToString();
                    object t = items[key];
                    temp_prefab = Resources.Load("GameAssets\\Asteroid\\Asteroid_med") as GameObject;
                    if (t == null) { items.Add(key, true); Instantiate(temp_prefab, new Vector3(x, y, 0), transform.rotation, gameenelmets_object.transform); }
                } else if (newres == 4 || newres == 8 || newres == 2) {
                    string key = (x).ToString() + "x" + (y).ToString();
                    object t = items[key];
                    temp_prefab = Resources.Load("GameAssets\\Enemy\\SmallEnemy") as GameObject;
                    if (t == null) { items.Add(key, true); Instantiate(temp_prefab, new Vector3(x, y, 0), transform.rotation, gameenelmets_object.transform); }
                }
            }
        }
        //Lets randomize so other randoms arnt the same
        Random.InitState(System.Environment.TickCount);
    }
}