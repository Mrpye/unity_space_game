using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade_Settings", menuName = "Upgrade_Settings", order = 1)]
public class Upgrade_Settings : ScriptableObject {
    #region Fields
    [SerializeField] private List< Enums.enum_item> avalible_for;
    [SerializeField] private Sprite sprite;
    [SerializeField] private float cpu = 1;
    #endregion




    public bool IsAvalible(Enums.enum_item  item) {
        foreach (Enums.enum_item  e in avalible_for) {
            if(e== item) {
                return true;
            }
        }
        return false;
    }


    #region Properties

    public List<Enums.enum_item> Basic_info_system_class { get => avalible_for; set => avalible_for = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public float Cpu { get => cpu; set => cpu = value; }
    public string Full_name {
        get { return "Upgrades\\" + this.name; }
        
    }

    #endregion
}