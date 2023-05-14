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
    Dictionary<string, object> data = new Dictionary<string, object>();


    public void OpenLevelPanel()
    {
        levelPanel.SetActive(true);
        profilePanel.SetActive(false);
        leaderBoardPanel.SetActive(false);
        //Para que muestre el nombre
        userName = PlayerPrefs.GetString("UserName");
        profileUserName_Text.text = userName;
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
        LoadUserData();
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
    private void LoadUserData()
    {
        Debug.Log("Dentro Dentro");

        FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    data.Clear();
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot.GetRawJsonValue());
                    // Do something with snapshot...
                    //Convertimos la recogida en un json
                    

                    //Converttimos la recogida en un diccionario
                    //foreach (DataSnapshot childSnapshot in snapshot.Children)
                    //{
                    //    data.Add(childSnapshot.Key, childSnapshot.Value);
                    //    Dictionary<string, object> aux = new Dictionary<string, object>();
                    //    aux = (Dictionary<string, object>)childSnapshot.Value;
                        
                    //}
             
                }
            });


    }
}
