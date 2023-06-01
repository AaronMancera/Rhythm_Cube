using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public  class NotificationController
{
    public  GameObject  notificationPanel;
    public  TMP_Text notif_Title_Text, notif_Message_Text;

    public NotificationController(GameObject notificationPanel, TMP_Text notif_Title_Text, TMP_Text notif_Message_Text) {
        this.notificationPanel = notificationPanel;
        this.notif_Message_Text = notif_Message_Text;
        this.notif_Title_Text = notif_Title_Text;
    }
    //Creacion de la notificacion
    public void showNotificationMessage(string title, string message)
    {
        notif_Title_Text.text = "" + title;
        notif_Message_Text.text = "" + message;
        notificationPanel.SetActive(true);
    }
    //Cerrar la notificacion
    public void CloseNotif_Panel()
    {
        notif_Title_Text.text = "";
        notif_Message_Text.text = "";
        notificationPanel.SetActive(false);
    }
}
