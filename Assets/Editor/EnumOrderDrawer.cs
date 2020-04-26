using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(EnumOrder))]
public class EnumOrderDrawer : PropertyDrawer {

  
    int _choiceIndex = 0;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        label = EditorGUI.BeginProperty(position, label, property);

        string[] a =Enum.GetNames(typeof(Enums.enum_read_type)).ToArray();

        _choiceIndex = EditorGUILayout.Popup("Player", _choiceIndex, a);

        property.intValue = (int)(Enums.enum_read_type)EditorGUI.EnumPopup(position, label, (Enums.enum_read_type)property.intValue);
        EditorGUI.EndProperty();

    }




}