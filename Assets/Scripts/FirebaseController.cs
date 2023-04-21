using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Reconocer los textfield
using UnityEngine.UI;
//Using a TMP
using TMPro;
//Firebase

using Firebase.Extensions;
/*
Datos de pruebas
username: Demo
email: demo@gmail.com
password: demo123
*/
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
    //Firebase
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    bool IsSingIn = false;
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    // Update is called once per frame
    bool IsSinged = false;
    void Update()
    {
        if (IsSingIn)
        {
            if (!IsSinged)
            {
                IsSinged = true;
                //Esto guarda estos valores se guardan entre sesiones de juego
                PlayerPrefs.SetString("UserName", user.DisplayName);
                PlayerPrefs.SetString("UserEmail", user.Email);
                profileUserName_Text.text = "" + user.DisplayName;
                profileEmail_Text.text = "" + user.Email;
                OpenProfilePanel();
            }
        }

    }
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
        if (string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text))
        {
            showNotificationMessage("Error", "Fields Empty!\nPlease Input All Details");
            return;
        }
        //TODO: Hacer el login
        SingInUser(loginEmail.text, loginPassword.text);
    }
    //Cerrar sesion
    public void LogOut()
    {
        //Cierra la sesion en el sistema
        auth.SignOut();
        profileUserName_Text.text = "";
        profileEmail_Text.text = "";
        OpenLoginPanel();
    }
    //Registro
    public void SingUpUser()
    {
        if (string.IsNullOrEmpty(singupEmail.text) && string.IsNullOrEmpty(singupPassword.text) && string.IsNullOrEmpty(singupCPasswprd.text))
        {
            showNotificationMessage("Error", "Fields Empty!\nPlease Input All Details");
            return;
        }
        //TODO: Hacer el registro
        CreateUser(singupEmail.text, singupPassword.text, singupUsername.text);
    }
    //Contraseña olvidad
    public void ForgetPass()
    {
        if (string.IsNullOrEmpty(forgetPassEmail.text))
        {
            showNotificationMessage("Error", "Forget Email Empty");
            return;
        }
        //TODO: Hacer la recuperacion de contraseña
    }
    //Creacion de la notificacion
    private void showNotificationMessage(string title, string message)
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

    /*
        Siguiendo el getStarted de https://firebase.google.com/docs/auth/unity/start?hl=es-419
    */
    //Creacion de usuario de firebase auth
    void CreateUser(string email, string password, string username)
    {
        //se utiliza ContinueWithOnMainThread para que no se generen concurrencias al realizarlo en una rama inferior a la principal
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            //Primero se crea el usuario con email y contraseña y luego se le asigna el nombre
            UpdateUserProfile(username);
        });
    }
    //Iniciar sesion de usuario de firebase auth
    void SingInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            //Asignacion de valores para el panel profile
            profileUserName_Text.text = "" + newUser.DisplayName;
            profileEmail_Text.text = "" + newUser.Email;
            OpenProfilePanel();
        });
    }
    //Inicializacion de firebase
    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                IsSingIn = true;

            }
        }
    }
    //Cuando se cierre la escena
    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
    //Actualizar usuario
    void UpdateUserProfile(string UserName)
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = UserName,
                PhotoUrl = new System.Uri("https://dummyimage.com/150"),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
                showNotificationMessage("Alert", "Account Successful Created");

            });
        }
    }
    // Start is called before the first frame update

}
