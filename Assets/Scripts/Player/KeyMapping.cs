using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMapping : ScriptableObject
{
    public string Key;
    public GameObject module;
    public Enums.enum_movment_type movement_type=  Enums.enum_movment_type.none;
    public float value;
}
