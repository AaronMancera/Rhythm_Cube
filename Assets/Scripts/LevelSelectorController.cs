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
    public GameObject profilePanel, levelPanel, leaderBoardPanel;
    //Text
    public TMP_Text profileUserName_Text, leaderboardUserName_Text;
    //Nombre de usuario
    private string userName;
    //Database
    Firebase.Database.DatabaseReference database;
    //LeaderBoard
    public GameObject scrollViewContent;
    public GameObject prefabLeaderPlayer;

    public void OpenLevelPanel()
    {
        levelPanel.SetActive(true);
        profilePanel.SetActive(false);
        leaderBoardPanel.SetActive(false);
        //Para que muestre el nombre
        userName = PlayerPrefs.GetString("UserName");
        profileUserName_Text.text = userName;
        //Por si vuelve del leaderboard
        // Recorrer todos los hijos del objeto
        for (int i = 0; i < scrollViewContent.transform.childCount; i++)
        {
            // Obtener el hijo actual
            GameObject childObject = scrollViewContent.transform.GetChild(i).gameObject;

            // Destruir el hijo actual
            Destroy(childObject);
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

    public void OpenLeaderBoardPanel()
    {
        levelPanel.SetActive(false);
        leaderBoardPanel.SetActive(true);
        List<User> listLeaderBoard;
        listLeaderBoard = new List<User>();
        LoadUserDataScore1(listLeaderBoard);

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //Metodo de descargar los datos de todos los jugadores para la LeaderBoard
    private void LoadUserDataScore1(List<User> listLeaderBoard)
    {
        FirebaseDatabase.DefaultInstance
            //Esto cogera los de puntuacion ordenados menor a mayor (por defecto y no se puede cambiar)
            .GetReference("users").OrderByChild("score_1")
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
                        Debug.Log(childSnapshot.GetRawJsonValue());
                        // Obtener los datos de cada hijo
                        string campo1 = childSnapshot.Child("email").Value.ToString();
                        int campo2 = int.Parse(childSnapshot.Child("score_1").Value.ToString());
                        string campo5 = childSnapshot.Child("username").Value.ToString();
                        //NOTE: campo1 - email _ campo2 - score_1 _ campo3 - score_2 _ campo4 - score_3 _ campo5 - username
                        //Recoger el score del 2 y el 3
                        User newUser = new User(campo1, campo2, 0, 0, campo5);
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
}
