using UnityEngine;
using static Enums;

public class Shield : ModuleSystemInfo {
    private SpriteRenderer shield_SR;
    private CircleCollider2D shield_Col;
    public bool shield_online;
    [SerializeField] private float charge_rate = 0;
    private float total_max_shield;
    private float onnline_at;
    private bool Last_Shield_Action=false;
    private void Start() {
        if (!this.is_in_storage) {
            shield_SR = gameObject.GetComponent<SpriteRenderer>();
            shield_Col = gameObject.GetComponent<CircleCollider2D>();
            shield_SR.enabled = false;
            shield_online = false;
            charge_rate = settings.Speed;
            this.max_shield = settings.Ammount;
            total_max_shield = this.Get_Calculated_Max_Shield_Capacity();
            onnline_at = settings.Action_speed;
            current_shield = this.total_max_shield;
            Run_Start();
        }
    }

    private void ShieldOnline() {
        shield_Col.enabled = true;
        shield_SR.enabled = true;
        this.SendAlert(enum_status.Info, "Shield Online");
        StartUsage();
    }

    private void ShieldOffline() {
        shield_Col.enabled = false;
        shield_SR.enabled = false;
        StopUsage();
       
    }

    private void Update() {
        if (!this.is_in_storage) {
            if (this.is_online && this.active) {
                charge_rate = settings.Action_speed;
                current_shield += (charge_rate * Time.deltaTime) * 0.05f;
                current_shield = Mathf.Clamp(current_shield, 0, this.total_max_shield);

                if (Input.GetKeyDown(KeyCode.J)) {
                    shield_online = !shield_online;
                }

                if (shield_online == true) {
                    Last_Shield_Action = shield_online;
                    if (current_shield <= 0) {
                        ShieldOffline();
                        this.SendAlert(enum_status.Info, "Shield Offline");
                    } else if (current_shield > onnline_at) {
                        ShieldOnline();
                    }
                } else {
                    if (Last_Shield_Action != shield_online) {
                        Last_Shield_Action = shield_online;
                        this.SendAlert(enum_status.Info, "Shield Offline");
                    }
                    ShieldOffline();
                }
                UpdateUsage();
            } else {
                ShieldOffline();
            }
        } else {
            ShieldOffline();
        }
    }
}