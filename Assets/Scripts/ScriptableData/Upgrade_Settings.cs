using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade_Settings", menuName = "Upgrade_Settings", order = 1)]
public class Upgrade_Settings : ScriptableObject {
    #region Fields
    [SerializeField] private List< Enums.enum_item> basic_info_system_class;
    [SerializeField] private Sprite sprite;
    [SerializeField] private float cpu = 1;
    #endregion







    #region Properties

    public List<Enums.enum_item> Basic_info_system_class { get => basic_info_system_class; set => basic_info_system_class = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public float Cpu { get => cpu; set => cpu = value; }
    public string Full_name {
        get { return "Upgrades\\" + this.name; }
        
    }

    #endregion
}