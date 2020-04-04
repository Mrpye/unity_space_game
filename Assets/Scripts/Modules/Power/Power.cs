using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : ModuleSystemInfo {

    private void Start() {
        if (!this.is_in_storage) {
            this.Run_Start();
        }
    }





    override public void Set_Values(float heat, float max_heat, float power, float max_power, float fuel,float max_fuel) {
        if (!this.is_in_storage) {
            if (power < max_power) {
                StartUsage();
            } else {
                StopUsage();
            }
        }
    }

    void Update() {
        if (!this.is_in_storage) {
            UpdateUsage();
        }
    }
}
