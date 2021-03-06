﻿/// <summary>
/// Class to store enum types
/// </summary>
public class Enums {

    public enum enum_status {
        Info = 0,
        Warning = 1,
        Danger = 2
    }

    public enum enum_read_type {
        storage = 0,
        replicator_make = 1,
        replicator_que = 2,
        replicator_time = 3,
        use = 4
    }

    public enum enum_staus_type {
        Online,
        Malfunction,
        Offline,
        Destroyed
    }

    public enum enum_class {
        Class_A,
        Class_B,
        Class_C,
        Class_D,
    }

    public enum enum_wepon_type {
        single_blaster,
        double_blaster,
        beam,
        rotary
    }

    public enum enum_resorce_type {
        module,
        material,
        asset,
        upgrade,
        blueprint,
        pickup
    }

    public enum enum_item {
        material_gold = 0,
        material_iron = 1,
        material_ice = 2,
        material_ilium = 3,
        material_neutronium = 4,
        material_plastica = 5,
        module_storage = 6,
        module_single_blaster = 7,
        module_double_blaster = 8,
        module_turret = 9,
        module_mining_laser = 10,
        module_power_reactor = 11,
        module_command_module_type1 = 12,
        module_main_engine = 13,
        module_thruster = 14,
        module_refiner = 15,
        module_tracktor_beam = 16,
        asset_asteroid_large = 17,
        asset_asteroid_med = 18,
        asset_small_enemy = 19,
        asset_large_enemy = 20,
        asset_player = 21,
        pickup = 22,
        None = 23,
        module_repair = 24,
        module_cooling = 25,
        module_shield = 26,
        material_fuel = 27,
        blueprint_fuel = 28,
        module_replicator = 29,

        component_wire = 30,
        component_circuit = 31,
        component_coil = 32,
        component_matrix = 33,
        component_capacitor = 34,
        component_insulation = 35,
        component_focus_crystal = 36,
        component_glass = 37,
        component_vents = 38,
        component_cooling_pump = 39,

        blueprint_wire = 40,
        blueprint_circuit = 41,
        blueprint_coil = 42,
        blueprint_matrix = 43,
        blueprint_capacitor = 44,
        blueprint_insulation = 45,
        blueprint_focus_crystal = 46,
        blueprint_glass = 47,
        blueprint_vents = 48,
        blueprint_cooling_pump = 49,

        component_money = 50,
        blueprint_money = 51,

        component_plastic = 52,
        blueprint_plastic = 53,
        component_metal = 54,
        blueprint_metal = 55,
    }

    public enum emun_zone {
        Command = 0,
        intern = 1,
        front = 2,
        right = 3,
        bottom = 4,
        left = 5,
        Top = 6
    }

    public enum emun_mount_point_type {
        propulsion = 0,
        utility = 1,
        intern = 3
    }

    public enum enum_facing_direction {
        Down = 270,
        Right = 180,
        Left = 0,
        Up = 90
    }

    public enum emun_inventory {
        Selected,
        Unselected
    }

    public enum enum_movment_type {
        none,
        rotation,
        strife,
        forward_backward,
    }

    public enum enum_system_info {
        heat = 0,
        power = 1,
        cpu = 2,
        fuel = 3,
        health = 4,
        flight_assist = 5,
        landing_zone = 6,
        refiner = 7,
        shield = 8
    }
}