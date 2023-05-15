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
    public TMP_Text profileUserName_Text;
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
        Debug.Log("Dentro");
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
    //TODO: Metodo de descargar los datos de todos los jugadores para la LeaderBoard
    private void LoadUserDataScore1(List<User> listLeaderBoard)
    {

        FirebaseDatabase.DefaultInstance
            //Esto cogera los de puntuacion ordenados menor a mayor (por defecto y no se puede cambiar)
            .GetReference("users").OrderByChild("score_1")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    listLeaderBoard.Clear();
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot.GetRawJsonValue());
                    // Recorrer los hijos del nodo
                    foreach (DataSnapshot childSnapshot in snapshot.Children)
                    {
                        Debug.Log(childSnapshot.GetRawJsonValue());
                        // Obtener los datos de cada hijo
                        string campo1 = childSnapshot.Child("email").Value.ToString();
                        int campo2 = int.Parse(childSnapshot.Child("score_1").Value.ToString());
                        string campo3 = childSnapshot.Child("username").Value.ToString();
                        User newUser = new User(campo1, campo2, campo3);
                        listLeaderBoard.Add(newUser);
                        // Mostrar los datos en la consola
                        Debug.Log("Campo 1: " + campo1);
                        Debug.Log("Campo 2: " + campo2);
                        Debug.Log("Campo 3: " + campo3);
                        //
                        Debug.Log(newUser.toStringLeaderBoard());

                        //// Obtener el subnodo
                        //DataSnapshot subSnapshot = childSnapshot.Child("SUBNODO");

                        //// Recorrer los hijos del subnodo
                        //foreach (DataSnapshot subChildSnapshot in subSnapshot.Children)
                        //{
                        //    // Obtener los datos de cada hijo del subnodo
                        //    string subCampo1 = subChildSnapshot.Child("SUB_CAMPO_1").Value.ToString();
                        //    int subCampo2 = int.Parse(subChildSnapshot.Child("SUB_CAMPO_2").Value.ToString());

                        //    // Mostrar los datos en la consola
                        //    Debug.Log("Subcampo 1: " + subCampo1);
                        //    Debug.Log("Subcampo 2: " + subCampo2);
                        //}
                    }
                    //Invertimos la lista
                    listLeaderBoard.Reverse();
                    for (int i = 0; i < listLeaderBoard.Count; i++)
                    {
                        Debug.Log(i + " "+ listLeaderBoard[i].toStringLeaderBoard());
                        GameObject newPlayer = (GameObject)Instantiate(prefabLeaderPlayer);
                        //Note: Tiene que ser un TextMeshProUGUI, o sino dara error y no se ejecutara la parte de abajo del codigo
                        TextMeshProUGUI textMesh = (TextMeshProUGUI)newPlayer.GetComponent<TMP_Text>();
                        int pos = i;
                        textMesh.text = "Nº"+(pos+1)+" : "+listLeaderBoard[i].toStringLeaderBoard();

                        Debug.Log("Hola");

                        newPlayer.transform.SetParent(scrollViewContent.transform);
                        newPlayer.transform.SetPositionAndRotation(new Vector3(0, 0 + i * 5), new Quaternion());
                        
                    }
                    //--
                }
            });


    }
}
