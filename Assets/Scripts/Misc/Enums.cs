using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class to store enum types
/// </summary>
public class Enums 
{
    public enum enum_item {
        material_gold,
        material_iron,
        material_ice,
        material_ilium,
        material_neutonium,
        material_plastica,
        module_storage,
        module_single_blaster,
        module_double_blaster,
        module_mining_laser,
        module_power_reactor,
        module_command_module_type1,
        module_main_engine,
        module_thruster,
        module_refiner,
        module_tracktor_beam
    }
    public enum emun_Side {
        front,
        right,
        bottom,
        left,
        Top
    }

    public enum emun_inventory {
        Selected,
        Unselected
    }
}
