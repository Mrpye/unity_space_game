/// <summary>
/// Class to store enum types
/// </summary>
public class Enums {
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

    public enum enum_item {
        material_gold,
        material_iron,
        material_ice,
        material_ilium,
        material_neutronium,
        material_plastica,
        module_storage,
        module_single_blaster,
        module_double_blaster,
        module_turret,
        module_mining_laser,
        module_power_reactor,
        module_command_module_type1,
        module_main_engine,
        module_thruster,
        module_refiner,
        module_tracktor_beam,

        asset_asteroid_large,
        asset_asteroid_med,
        asset_small_enemy,
        asset_large_enemy,
        asset_player
    }

    public enum emun_zone {
        intern = 0,
        front = 1,
        right = 2,
        bottom = 3,
        left = 4,
        Top = 5
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
        heat,
        power,
        cpu,
        fuel,
        health,
        flight_assist,
        landing_zone,
        refiner,
    }
}