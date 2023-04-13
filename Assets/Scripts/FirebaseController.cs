using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Reconocer los textfield
using UnityEngine.UI;

public class FirebaseController : MonoBehaviour
{
    //Objetos del canvas
    public GameObject loginPanel, singupPanel, profilePanel;
    //TextField
    public InputField loginEmail, loginPassword, singupEmail, singupPassword, singupCPasswprd, singupUsername;
    //Control de que paneles mostrar en el canvas
    public void OpenLoginPanel(){
        loginPanel.SetActive(true);
        singupPanel.SetActive(false);
        singupPanel.SetActive(false);
    }

    public void OpenSingUpPanel(){
        loginPanel.SetActive(false);
        singupPanel.SetActive(true);
        singupPanel.SetActive(false);
    }

    public void OpenProfilePanel(){
        loginPanel.SetActive(false);
        singupPanel.SetActive(false);
        singupPanel.SetActive(true);
    }

    public void LoginUser(){
        if(string.IsNullOrEmpty(loginEmail.text)&&string.IsNullOrEmpty(loginPassword.text))
        {
            return;
        }
        //TODO: Hacer el login
    }

    public void SingUpUser(){
        if(string.IsNullOrEmpty(singupEmail.text)&&string.IsNullOrEmpty(singupPassword.text)&&string.IsNullOrEmpty(singupCPasswprd.text))
        {
            return;
        }
        //TODO: Hacer el registro
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
