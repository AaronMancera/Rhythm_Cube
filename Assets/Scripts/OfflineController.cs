using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OfflineController : MonoBehaviour
{
    //Objetos del canvas
    public GameObject loginPanel, profilePanel, notificatonPanel;
    //Text
    public TMP_Text profileUserName_Text, profileEmail_Text;
    int aviso;
    //Notification
    private NotificationController notificationController;
    public TMP_Text notif_Title_Text, notif_Message_Text;

    private void Awake()
    {
        notificationController = new NotificationController(notificatonPanel, notif_Title_Text, notif_Message_Text);
        aviso = 0;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenProfilePanel()
    {
        loginPanel.SetActive(false);
        profilePanel.SetActive(true);
    }
    public void PlayLikeGuest()
    {
        if (aviso == 0)
        {
            notificationController.showNotificationMessage("WARNING", "When you play offline you will lose your progress if you login with an account");
            aviso = 1;
        }
        else
        {
            PlayerPrefs.DeleteKey("UserId");
            PlayerPrefs.SetString("UserName", "Guest");
            PlayerPrefs.SetString("UserEmail", "Guest");
            profileUserName_Text.text = "Guest";
            profileEmail_Text.text = "Guest";
            OpenProfilePanel();
        }
    }
    

}
