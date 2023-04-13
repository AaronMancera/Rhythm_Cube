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
    public GameObject loginPanel, singupPanel, profilePanel, forgetPasswordPanel;
    //TextField
    public TMP_InputField loginEmail, loginPassword, singupEmail, singupPassword, singupCPasswprd, singupUsername, forgetPassEmail;
    //Control de que paneles mostrar en el canvas
    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        singupPanel.SetActive(false);
        singupPanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenSingUpPanel()
    {
        loginPanel.SetActive(false);
        singupPanel.SetActive(true);
        singupPanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenProfilePanel()
    {
        loginPanel.SetActive(false);
        singupPanel.SetActive(false);
        singupPanel.SetActive(true);
        forgetPasswordPanel.SetActive(false);
    }

    public void OpenForgetPassPanel()
    {
        loginPanel.SetActive(false);
        singupPanel.SetActive(false);
        singupPanel.SetActive(false);
        forgetPasswordPanel.SetActive(true);
    }

    //Inicio de sesion
    public void LoginUser()
    {
        if(string.IsNullOrEmpty(loginEmail.text)&&string.IsNullOrEmpty(loginPassword.text))
        {
            return;
        }
        //TODO: Hacer el login
    }
    //Registro
    public void SingUpUser()
    {
        if(string.IsNullOrEmpty(singupEmail.text)&&string.IsNullOrEmpty(singupPassword.text)&&string.IsNullOrEmpty(singupCPasswprd.text))
        {
            return;
        }
        //TODO: Hacer el registro
    }
    //Contraseña olvidad
    public void ForgetPass()
    {
        if(string.IsNullOrEmpty(forgetPassEmail.text))
        {
            return;
        }
        //TODO: Hacer la recuperacion de contraseña
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
