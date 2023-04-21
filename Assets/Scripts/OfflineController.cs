using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OfflineController : MonoBehaviour
{
    //Objetos del canvas
    public GameObject loginPanel, profilePanel;
    //Text
    public TMP_Text profileUserName_Text, profileEmail_Text;
    public void OpenProfilePanel()
    {
        loginPanel.SetActive(false);
        profilePanel.SetActive(true);
    }
    public void PlayLikeGuest()
    {
        PlayerPrefs.SetString("UserName", "Guest");
        PlayerPrefs.SetString("UserEmail", "Guest");
        profileUserName_Text.text = "Guest";
        profileEmail_Text.text = "Guest";
        OpenProfilePanel();
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
