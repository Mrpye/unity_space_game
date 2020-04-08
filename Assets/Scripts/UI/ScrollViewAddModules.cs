using UnityEngine;

public class ScrollViewAddModules : MonoBehaviour {
    [SerializeField] private GameObject module_container;
    [SerializeField] private GameObject content_container;
    [SerializeField] private GameObject taggle_prefab;
    // Start is called before the first frame update

    public ModuleSystemInfo[] GeEquipedItems() {
        return module_container.GetComponentsInChildren<ModuleSystemInfo>();
    }

    public void Clear() {
        ModuleToggleActivate[] list= content_container.GetComponentsInChildren<ModuleToggleActivate>();
        for (int i= 0;i< list.Length; i++) {
            Destroy(list[i].gameObject);
        }
    }

    private void Start() {
        Populate();
    }

    public void Populate() {
        //Lets load the modules
        Clear();
        ModuleSystemInfo[] mod_info = GeEquipedItems();
        foreach (ModuleSystemInfo m in mod_info) {
            GameObject go = Instantiate(taggle_prefab, content_container.transform);
            ModuleToggleActivate tog = go.GetComponent<ModuleToggleActivate>();
            tog.SetModule(m);

            //sort the content heigh out
            RectTransform rec = content_container.GetComponent<RectTransform>();
            float new_height = mod_info.Length * 40;
            rec.sizeDelta = new Vector2(rec.sizeDelta.x, new_height);

        }
    }
    // Update is called once per frame
    private void Update() {
    }
}