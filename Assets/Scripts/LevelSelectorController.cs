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
    public void OpenLevelPanel()
    {
        levelPanel.SetActive(true);
        profilePanel.SetActive(false);
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
    public void SelectLevel1() {
        SceneManager.LoadScene(1);
    }

    public void OpenLeaderBoardPanel() {
        profilePanel.SetActive(false);
        leaderBoardPanel.SetActive(false);
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
