using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Reconocer los textfield
using UnityEngine.UI;
//Using a TMP
using TMPro;
//Firebase
using Firebase;
using System;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Database;
/*
Datos de pruebas
---------------------
username: Demo
email: demo@gmail.com
password: demo123
---------------------

*/
public class FirebaseController : MonoBehaviour
{
    //Objetos del canvas
    [Header("Panel")]
    public GameObject loginPanel, signupPanel, profilePanel, forgetPasswordPanel, notificationPanel, optionsPanel;
    [Header("Input")]
    //TextField
    public TMP_InputField loginEmail, loginPassword, signupEmail, signupPassword, signupCPasswprd, signupUsername, forgetPassEmail;
    [Header("Text")]
    //Text
    public TMP_Text notif_Title_Text, notif_Message_Text, profileUserName_Text, profileEmail_Text;
    [Header("Toggle")]

    //Toggle
    public Toggle remeberMe;
    //Firebase
    //Firebase Authentication
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    //Firebase RealtimeDatabase
    Firebase.Database.DatabaseReference database;
    //----------------------------------------------------------------------------------------------------------//
    bool IsSignIn = false;
    //Esto es para que cada vez que se inicie de nuevo el juego reinicie estos valores guardados netre sesiones
    //Notifiacitions
    private NotificationController notificationController;
    private void Awake()
    {
        notificationController = new NotificationController(notificationPanel, notif_Title_Text, notif_Message_Text);
    }
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
    //Meter en la base de datos o en prefabs un valor para guardar el remenber me
    void Update()
    {
        if (IsSignIn)
        {
            //Esto guarda estos valores se guardan entre sesiones de juego
            PlayerPrefs.SetString("UserId", user.UserId);
            PlayerPrefs.SetString("UserName", user.DisplayName);
            PlayerPrefs.SetString("UserEmail", user.Email);
            profileUserName_Text.text = "" + user.DisplayName;
            profileEmail_Text.text = "" + user.Email;
            OpenProfilePanel();
            //Recargando el valores del databseRealtime 

        }


    }
    //Control de que paneles mostrar en el canvas
    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
        CleanValues();
    }

    public void OpenSignUpPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(false);
        CleanValues();
    }

    public void OpenProfilePanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(true);
        forgetPasswordPanel.SetActive(false);
        CleanValues();
    }

    public void OpenForgetPassPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        forgetPasswordPanel.SetActive(true);
        CleanValues();
    }
    public void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);

    }
    public void CloseOptionsPanel()
    {
        optionsPanel.SetActive(false);

    }

    private void CleanValues()
    {
        loginEmail.text = "";
        loginPassword.text = "";
        signupEmail.text = "";
        signupPassword.text = "";
        signupCPasswprd.text = "";
        signupUsername.text = "";
        forgetPassEmail.text = "";
    }
    //Inicio de sesion
    public void LoginUser()
    {
        if (string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text))
        {
            createNotificationMessage("Error", "Fields Empty!\nPlease Input All Details");
            return;
        }
        // Hacer el login
        SignInUser(loginEmail.text, loginPassword.text);
        if (!remeberMe.isOn)
        {
            PlayerPrefs.SetInt("RemenberMe", 0);
        }
        else
        {
            PlayerPrefs.SetInt("RemenberMe", 1);
        }
    }
    //Cerrar sesion
    public void LogOut()
    {
        //Importante tener en cuenta el tema del offline
        if (PlayerPrefs.GetString("UserName") != "Guest")
        {
            //Al salir del auth el observador del auth cambia el panel y resetea los textos y valores
            auth.SignOut();
        }
        //Tener en cuenta si es guest que vuelva al panel de login
        else
        {
            OpenLoginPanel();
        }


    }

    //Registro
    public void SignUpUser()
    {
        if (string.IsNullOrEmpty(signupEmail.text) && string.IsNullOrEmpty(signupPassword.text) && string.IsNullOrEmpty(signupCPasswprd.text))
        {
            createNotificationMessage("Error", "Fields Empty!\nPlease Input All Details");
            return;
        }
        // Hacer el registro
        CreateUser(signupEmail.text, signupPassword.text, signupUsername.text);
    }
    //Contraseña olvidad
    public void ForgetPass()
    {
        if (string.IsNullOrEmpty(forgetPassEmail.text))
        {
            createNotificationMessage("Error", "Forget Email Empty");
            return;
        }
        //Metodo de recuperacion de contraseña mediante el email
        forgetPasswordSubmit(forgetPassEmail.text);
    }
    //Creacion de la notificacion
    public void createNotificationMessage(string title, string message)
    {
        
        notificationController.showNotificationMessage(title, message);
    }
    //Cerrar la notificacion
    public void CloseNotif_Panel()
    {
        notificationController.CloseNotif_Panel();
    }

    /*
        Siguiendo el getStarted de https://firebase.google.com/docs/auth/unity/start?hl=es-419
    */
    //Creacion de usuario de firebase auth
    //NOTE: A la hora de crear un usuario, queremos que se cree tanto en Firebas Auth como en nuestra Realtime db
    void CreateUser(string email, string password, string username)
    {
        //se utiliza ContinueWithOnMainThread para que no se generen concurrencias al realizarlo en una rama inferior a la principal
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                //Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                //Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                //Tratamiento de errores
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    GetErrorMessage(exception);
                }
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult authResult = task.Result;
            Firebase.Auth.FirebaseUser newUser = authResult.User;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            //Primero se crea el usuario con email y contraseña y luego se le asigna el nombre
            UpdateUserProfile(username);
        });
    }
    //Iniciar sesion de usuario de firebase auth
    void SignInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                //Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            //Tratamiento de error a la hora de inicar sesion
            if (task.IsFaulted)
            {
                //Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                //Tratamiento de errores
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    GetErrorMessage(exception);
                }
                return;
            }

            Firebase.Auth.AuthResult authResult = task.Result;
            Firebase.Auth.FirebaseUser newUser = authResult.User;
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
        database = FirebaseDatabase.DefaultInstance.RootReference;

    }

    //Metodo observer de auth
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out ");
                IsSignIn = false;
                //Cierra la sesion en el sistema
                profileUserName_Text.text = "";
                profileEmail_Text.text = "";
                remeberMe.isOn = false;
                PlayerPrefs.DeleteKey("UserId");
                PlayerPrefs.DeleteKey("UserName");
                PlayerPrefs.DeleteKey("UserEmail");
                PlayerPrefs.DeleteKey("score_1");
                PlayerPrefs.DeleteKey("score_2");
                PlayerPrefs.DeleteKey("score_3");
                //Quitar el score del nivel 2 y 3
                PlayerPrefs.DeleteKey("RemenberMe");
                OpenLoginPanel();
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                IsSignIn = true;

            }
        }
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
                    //Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    //Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }
                Debug.Log("User profile updated successfully.");
                //Inserccion en el RealtimeDatabse
                writeNewUser(user.UserId, user.DisplayName, user.Email);
                createNotificationMessage("Alert", "Account Successful Created");


            });
        }
    }
    private void writeNewUser(string userId, string name, string email)
    {
        User user = new User(name, email);
        string json = JsonUtility.ToJson(user);

        database.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }
    //Cuando se cierre la escena
    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
    //Cuando se quita la aplicacion esto detecta si le ha dado a recuerdame o no
    private void OnApplicationQuit()
    {

        if (PlayerPrefs.GetInt("RemenberMe") == 0)
        {
            LogOut();
        }

    }
    private void GetErrorMessage(Exception exception)
    {
        Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
        if (firebaseEx != null)
        {
            var errorCode = (AuthError)firebaseEx.ErrorCode;
            createNotificationMessage("Error", GetErrorMessage(errorCode));
        }
    }
    //Recogedor de errores
    private static string GetErrorMessage(AuthError errorCode)
    {
        var message = "";
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                message = "Account Not Exist";
                break;
            case AuthError.MissingPassword:
                message = "Missing Password";
                break;
            case AuthError.WeakPassword:
                message = "Password So Week";
                break;
            case AuthError.WrongPassword:
                message = "Wrong Password";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "Your Email Alredy in User";
                break;
            case AuthError.InvalidEmail:
                message = "Your Email Invalid";
                break;
            case AuthError.MissingEmail:
                message = "Email Missing";
                break;
            default:
                message = "Invalid Error";
                break;
        }
        return message;
    }
    //Metodo para recuperar contraseña
    void forgetPasswordSubmit(String forgetPasswordEmail)
    {
        auth.SendPasswordResetEmailAsync(forgetPasswordEmail).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                //Debug.LogError("SendPasswordResetEmailAsync was canceled");
            }
            if (task.IsFaulted)
            {
                //Tratamiento de errores
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    GetErrorMessage(exception);
                }
                return;
            }
            createNotificationMessage("Alert", "Successfully Send Email");
        });
    }
}
