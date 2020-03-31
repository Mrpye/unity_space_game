using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class Alert : MonoBehaviour {

    private class AllertMessage {
        public string message { get; set; }
        public enum_status status { get; set; }

        public AllertMessage(enum_status status, string message) {
            this.message = message;
            this.status = status;
        }
    }

    private List<AllertMessage> alerts = new List<AllertMessage>();
    public int max_messages { get; set; } = 5;
    private enum_status last_status= enum_status.Info;
    private string last_message="";

    public void RaiseAlert(enum_status status, string message) {
        if (status != last_status || message != last_message) {
            if (alerts.Count == max_messages) {
                alerts.RemoveAt(0);
            }
            alerts.Add(new AllertMessage(status, message));
            this.last_status = status;
            this.last_message = message;
            this.UpdateMessage();
        }
    }

    private void UpdateMessage() {
        StringBuilder sb = new StringBuilder();
        foreach (AllertMessage m in alerts) {
            sb.AppendLine(m.status.ToString() + ":" + m.message);
        }
        GetComponent<Text>().text = sb.ToString();
    }
}