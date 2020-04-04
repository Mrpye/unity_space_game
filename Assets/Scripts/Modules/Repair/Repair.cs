using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : ModuleSystemInfo {

    private GameObject modules_game_object;
    private ModuleSystemInfo current_item = null;
    private float repair_ammount = 1;
    private float max_repair = 1;
    private float time_between_repair= 1;
    private Coroutine coroutine;
    private void Start() {
        if (!this.is_in_storage) {
            modules_game_object = GameObject.Find("Modules");
            this.Run_Start();
            this.repair_ammount = this.settings.Action_speed;
            this.time_between_repair = this.settings.Action_speed2;
            this.max_repair = this.settings.Ammount;
            coroutine = StartCoroutine(RepairItem());
        }
    }

    private IEnumerator RepairItem() {
        while (true) {
            this.repair_ammount = this.settings.Action_speed;
            this.time_between_repair = this.settings.Action_speed2;
            this.max_repair = this.settings.Ammount;
            if (current_item == null) {
                float smallest_health = 999999;
                //find somthing that needs repairing
                ModuleSystemInfo[]  items = GeEquipedItems();
                foreach(ModuleSystemInfo i in items) {
                    if(i.Is_Destroyed()==false && i.current_health< smallest_health && i.current_health< this.max_repair) {
                        current_item = i;
                        smallest_health = i.current_health;
                    }
                }
                if (current_item != null) {
                    StartUsage();
                    UnityFunctions.SendAlert(Enums.enum_status.Info, "Repairing " + current_item.name);
                }
            } else {
                if (current_item.current_health>= this.max_repair) {
                    current_item = null;
                    UnityFunctions.SendAlert(Enums.enum_status.Info,  current_item.name + " Repaired ");
                } else {
                    current_item.current_health += repair_ammount;
                    if(current_item.current_health > this.max_repair) {
                        current_item.current_health = this.max_repair;
                        current_item = null;
                        StopUsage();
                    }
                }
            }
            yield return new WaitForSeconds(time_between_repair);
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
