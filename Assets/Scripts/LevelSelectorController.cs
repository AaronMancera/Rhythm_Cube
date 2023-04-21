using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelectorController : MonoBehaviour
{
    //Objetos del canvas
    public GameObject profilePanel, levelPanel;
    //Text
    public TMP_Text profileUserName_Text;
    //Nombre de usuario
    private string userName;
    public void OpenLevelPanel()
    {
        levelPanel.SetActive(true);
        profilePanel.SetActive(false);
    }
    public void OpenProfilePanel()
    {
        levelPanel.SetActive(false);
        profilePanel.SetActive(true);
    }
   
    //Utilizamos el metodo awake para que realice esta accion nada mas ser invocado
    private void Awake()
    {
        userName = PlayerPrefs.GetString("UserName");
        profileUserName_Text.text = userName;
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
