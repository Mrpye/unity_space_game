using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooling : ModuleSystemInfo {
    private GameObject modules_game_object;
    private ModuleSystemInfo current_item = null;
    private float cool_ammount = 1;
    private float time_between_cooling = 1;
    private Coroutine coroutine;
    private void Start() {
        if (!this.is_in_storage) {
            modules_game_object = GameObject.Find("Modules");
            this.Run_Start();
            this.cool_ammount = this.settings.Action_speed;
            this.time_between_cooling = this.settings.Action_speed2;
            coroutine = StartCoroutine(RepairItem());
        }
    }

    private IEnumerator RepairItem() {
        while (true) {
            this.cool_ammount = this.settings.Action_speed;
            this.time_between_cooling = this.settings.Action_speed2;
            if (this.is_online && this.active) {
                if (current_item == null) {
                    ModuleSystemInfo[] items = GeEquipedItems();
                    foreach (ModuleSystemInfo i in items) {
                        if (i.current_heat > 0) {
                            i.current_heat -= cool_ammount;
                            if (i.current_heat < 0) { i.current_heat = 0; }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(time_between_cooling);
        }
    }


    public ModuleSystemInfo[] GeEquipedItems() {
        modules_game_object = GameObject.Find("Modules");
        return modules_game_object.GetComponentsInChildren<ModuleSystemInfo>();
    }


    void Update() {
        if (!this.is_in_storage) {
            UpdateUsage();
        }
    }
}
