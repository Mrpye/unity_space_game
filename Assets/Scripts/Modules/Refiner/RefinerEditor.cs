using UnityEditor;

//[CustomEditor(typeof(Refiner))]

public class RefinerEditor : Editor {
    public string[] avaliableSysinfoProperties = new string[] {
        //"idle_heat",
        //"min_idle_heat" ,
       // "usage_factor_heat" ,
       // "min_usage_factor_heat",
        //"idle_power",
        //"min_idle_power" ,
        //"min_power_usage_factor" ,
        //"power_usage_factor",
         "min_idle_fuel",
         "idle_fuel",
        "fuel_usage_factor" ,
        "min_fuel_usage_factor" ,
        //"cpu",
        //"min_cpu",
        //"health",
        //"health_malfunction" ,
        //"health_offline" ,
        //"mass",
        //"min_mass",
        "max_thrust",
        "thrust",
        "max_Damage",
        "damage",
        //"max_action_speed",
        //"min_action_speed",
        //"action_speed",
        "max_speed",
        "min_speed",
        "speed",
        //"max_items",
        //"items",
        "max_range",
        "range",
        //"max_ammount",
       // "ammount",
        "min_offline_malfunction_time",
        "max_offline_malfunction_time",
        "min_online_malfunction_time",
        "max_online_malfunction_time",
    };

    public override void OnInspectorGUI() {
        Refiner obj = (Refiner)target;

        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, avaliableSysinfoProperties);

        foreach (string e in avaliableSysinfoProperties) {
            System.Reflection.FieldInfo n = obj.GetType().GetField(e);
            var a = n.FieldType;
            n.SetValue(obj, 0);
        }


        serializedObject.ApplyModifiedProperties();
    }
}
