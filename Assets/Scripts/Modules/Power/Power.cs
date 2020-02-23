using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : ModuleSystemInfo {
   
    override public void Set_Values(float heat, float max_heat, float power, float max_power, float fuel,float max_fuel) {
        if (power< max_power) {
            StartUsage();
        } else {
            StopUsage();
        }
    }

    void Update() {
        UpdateUsage();
    }
}
