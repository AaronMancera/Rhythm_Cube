using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Reconocer los textfield
using UnityEngine.UI;
//Using a TMP
using TMPro;

public class FirebaseController : MonoBehaviour
{
    //Objetos del canvas
    public GameObject loginPanel, singupPanel, profilePanel, forgetPasswordPanel, notificationPanel;
    //TextField
    public TMP_InputField loginEmail, loginPassword, singupEmail, singupPassword, singupCPasswprd, singupUsername, forgetPassEmail;
    //Text
    public TMP_Text notif_Title_Text, notif_Message_Text, profileUserName_Text, profileEmail_Text;
    //Toggle
    public Toggle remeberMe;
    //Control de que paneles mostrar en el canvas
    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        singupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenSingUpPanel()
    {
        loginPanel.SetActive(false);
        singupPanel.SetActive(true);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenProfilePanel()
    {
        loginPanel.SetActive(false);
        singupPanel.SetActive(false);
        profilePanel.SetActive(true);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenForgetPassPanel()
    {
        loginPanel.SetActive(false);
        singupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(true);
    }

    //Inicio de sesion
    public void LoginUser()
    {
        if(string.IsNullOrEmpty(loginEmail.text)&&string.IsNullOrEmpty(loginPassword.text))
        {
            showNotificationMessage("Error", "Fields Empty!\nPlease Input All Details");
            return;
        }
        //TODO: Hacer el login
    }
    //Cerrar sesion
    public void LogOut()
    {
        profileUserName_Text.text="";
        profileEmail_Text.text="";
        OpenLoginPanel();
    }
    //Registro
    public void SingUpUser()
    {
        if(string.IsNullOrEmpty(singupEmail.text)&&string.IsNullOrEmpty(singupPassword.text)&&string.IsNullOrEmpty(singupCPasswprd.text))
        {
            showNotificationMessage("Error", "Fields Empty!\nPlease Input All Details");
            return;
        }
        //TODO: Hacer el registro
    }
    //Contraseña olvidad
    public void ForgetPass()
    {
        if(string.IsNullOrEmpty(forgetPassEmail.text))
        {
            showNotificationMessage("Error", "Forget Email Empty");
            return;
        }
        //TODO: Hacer la recuperacion de contraseña
    }
    //Creacion de la notificacion
    private void showNotificationMessage(string title, string message)
    {
        notif_Title_Text.text=""+title;
        notif_Message_Text.text=""+message;
        notificationPanel.SetActive(true);

    }
    //Cerrar la notificacion
    public void CloseNotif_Panel()
    {
        notif_Title_Text.text="";
        notif_Message_Text.text="";
        notificationPanel.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
