using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorController : MonoBehaviour
{
    //Objetos del canvas
    public GameObject profilePanel, levelPanel, leaderBoardPanel, notificationPanel;
    //Text
    public TMP_Text profileUserName_Text, leaderboardUserName_Text;
    //Nombre de usuario
    private string userName;
    //Database
    Firebase.Database.DatabaseReference database;
    //LeaderBoard
    public GameObject scrollViewContent;
    public GameObject prefabLeaderPlayer;
    //Notifications
    private NotificationController notificationController;
    public TMP_Text notif_Title_Text, notif_Message_Text;

    private void Awake()
    {
        notificationController = new NotificationController(notificationPanel, notif_Title_Text, notif_Message_Text);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenLevelPanel()
    {
        levelPanel.SetActive(true);
        profilePanel.SetActive(false);
        leaderBoardPanel.SetActive(false);
        //Para que muestre el nombre
        userName = PlayerPrefs.GetString("UserName");
        profileUserName_Text.text = userName;
        //NOTE: Por si vuelve del leaderboard que elimine todos los objetos anteriores
        // Recorrer todos los hijos del objeto
        for (int i = 0; i < scrollViewContent.transform.childCount; i++)
        {
            // Obtener el hijo actual
            GameObject childObject = scrollViewContent.transform.GetChild(i).gameObject;

            // Destruir el hijo actual
            Destroy(childObject);
        }
        if (PlayerPrefs.GetString("UserName") != "Guest")
        {
            InitialBestRecord();
        }
    }
    public void OpenProfilePanel()
    {
        levelPanel.SetActive(false);
        profilePanel.SetActive(true);
    }
    public void SelectLeve0()
    {
        SceneManager.LoadScene(0);
    }
    public void SelectLevel1()
    {
        SceneManager.LoadScene(1);
    }
    public void SelectLevel2()
    {
        if (PlayerPrefs.GetInt("score_1") != 100) {
            notificationController.showNotificationMessage("Error","You must complete the previous level!!!");
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }
    public void SelectLevel3()
    {
        if (PlayerPrefs.GetInt("score_2") != 100)
        {
            notificationController.showNotificationMessage("Error", "You must complete the previous level!!!");
        }
        else
        {
            SceneManager.LoadScene(3);
        }
    }

    public void OpenLeaderBoardPanel()
    {
        levelPanel.SetActive(false);
        leaderBoardPanel.SetActive(true);
        List<User> listLeaderBoard;
        listLeaderBoard = new List<User>();
        LoadUserDataScore1(listLeaderBoard);

    }

  
   
    //Metodo de descargar los datos de todos los jugadores para la LeaderBoard
    private void LoadUserDataScore1(List<User> listLeaderBoard)
    {
        FirebaseDatabase.DefaultInstance
            //Esto cogera los de puntuacion ordenados menor a mayor (por defecto y no se puede cambiar)
            .GetReference("users").OrderByChild("global")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                //Si entra como guest, simplemente sera capturado aqui el error
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    listLeaderBoard.Clear();
                    DataSnapshot snapshot = task.Result;
                    //Debug.Log(snapshot.GetRawJsonValue());
                    // Recorrer los hijos del nodo
                    foreach (DataSnapshot childSnapshot in snapshot.Children)
                    {
                        //Debug.Log(childSnapshot.GetRawJsonValue());
                        // Obtener los datos de cada hijo
                        string campo1 = childSnapshot.Child("email").Value.ToString();
                        //NOTA: Metodo para dar la facilidad de implementacion de nuevos niveles
                        //int campoNuevo = 0;
                        //if (childSnapshot.HasChild("score_x"))
                        //{
                        //    campoNuevo = int.Parse(childSnapshot.Child("score_x").Value.ToString());
                        //}
                        int campo2 = 0;
                        if (childSnapshot.HasChild("score_1"))
                        {
                            campo2 = int.Parse(childSnapshot.Child("score_1").Value.ToString());
                        }
                        int campo3 = 0;
                        if (childSnapshot.HasChild("score_2"))
                        {
                           campo3 = int.Parse(childSnapshot.Child("score_2").Value.ToString());
                        }
                        int campo4 = 0;
                        if (childSnapshot.HasChild("score_3"))
                        {
                            campo4 = int.Parse(childSnapshot.Child("score_3").Value.ToString());
                        }
                        string campo5 = childSnapshot.Child("username").Value.ToString();
                        //NOTE: campo1 - email _ campo2 - score_1 _ campo3 - score_2 _ campo4 - score_3 _ campo5 - username
                        //Recoger el score del 2 y el 3
                        User newUser = new User(campo1, campo2, campo3, campo4, campo5);
                        listLeaderBoard.Add(newUser);

                    }
                    //Invertimos la lista
                    listLeaderBoard.Reverse();
                    for (int i = 0; i < listLeaderBoard.Count; i++)
                    {
                        //Debug.Log(i + " " + listLeaderBoard[i].toStringLeaderBoard());
                        GameObject newPlayer = (GameObject)Instantiate(prefabLeaderPlayer);
                        //Note: Tiene que ser un TextMeshProUGUI, o sino dara error y no se ejecutara la parte de abajo del codigo
                        TextMeshProUGUI textMesh = (TextMeshProUGUI)newPlayer.GetComponent<TMP_Text>();
                        int pos = i;
                        textMesh.text = "Nº" + (pos + 1) + " : " + listLeaderBoard[i].toStringLeaderBoard();
                        newPlayer.transform.SetParent(scrollViewContent.transform);
                        newPlayer.transform.SetPositionAndRotation(new Vector3(0, 0 + i * 5), new Quaternion());
                        if (listLeaderBoard[i].getUsername() == PlayerPrefs.GetString("UserName"))
                        {

                            leaderboardUserName_Text.text = "Nº" + (pos + 1) + " : " + listLeaderBoard[i].toStringLeaderBoard();

                        }
                    }
                    //--
                }
            });



    }
    //NOTA: Se ha cambiado el lugar de esta funcion, ya que desde aqui cumple las dos funcionalides
    //1. Limita la progresion del jugador
    //2. Guarda los valores en el PlayerPrefs
    void InitialBestRecord()
    {
        FirebaseDatabase.DefaultInstance
           //Esto cogera los de puntuacion ordenados menor a mayor (por defecto y no se puede cambiar)
           .GetReference("users").Child(PlayerPrefs.GetString("UserId"))
           .GetValueAsync().ContinueWithOnMainThread(task =>
           {
               if (task.IsFaulted)
               {
                   // Handle the error...
               }
               else if (task.IsCompleted)
               {

                   DataSnapshot snapshot = task.Result;
                   //Debug.Log(snapshot.GetRawJsonValue());
                   //TODO: Hacer un condicional para cada nivel de la base de datos
                   if (snapshot.HasChild("score_1"))
                   {
                       var dictionary = snapshot.Value as Dictionary<string, object>;
                       if (dictionary != null)
                       {
                           int bestScore = 0;
                           bestScore = int.Parse(dictionary["score_1"].ToString());
                           PlayerPrefs.SetInt("score_1", bestScore);
                           Debug.Log(bestScore);



                       }
                   }
                   else
                   {
                       PlayerPrefs.SetInt("score_1", 0);
                   }
                   if (snapshot.HasChild("score_2"))
                   {
                       var dictionary = snapshot.Value as Dictionary<string, object>;
                       if (dictionary != null)
                       {
                           int bestScore = 0;
                           bestScore = int.Parse(dictionary["score_2"].ToString());
                           PlayerPrefs.SetInt("score_2", bestScore);
                           Debug.Log(bestScore);



                       }
                   }
                   else {
                       PlayerPrefs.SetInt("score_2", 0);
                   }
                   if (snapshot.HasChild("score_3"))
                   {
                       var dictionary = snapshot.Value as Dictionary<string, object>;
                       if (dictionary != null)
                       {
                           int bestScore = 0;

                           bestScore = int.Parse(dictionary["score_3"].ToString());
                           PlayerPrefs.SetInt("score_3", bestScore);
                           Debug.Log(bestScore);


                       }
                   }
                   else
                   {
                       PlayerPrefs.SetInt("score_3", 0);
                   }
                  
               }
           });
    }
}
