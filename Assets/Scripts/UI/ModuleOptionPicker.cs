using UnityEngine;
using UnityEngine.UI;
public class ModuleOptionPicker : MonoBehaviour {
    [SerializeField] private ModuleSystemInfo[] stored_modules;
    [SerializeField] private GameObject player_prefab;
    [SerializeField] private Dropdown dd;

    private void Start() {
        LoadStorage();
    }

    public void LoadStorage() {
        //dd = gameObject.GetComponent<Dropdown>();
        Storage storage = player_prefab.GetComponent<Storage>();
        stored_modules = storage.GetSToredItems();
        dd.ClearOptions(); // better approach
        foreach (ModuleSystemInfo n in stored_modules) {
            dd.options.Add(new Dropdown.OptionData(n.ModuleName));
        }
        
        //modules = GameObject.Find("Modules");
        //stored_modules = GameObject.Find("Stored_Modules");

    }
}