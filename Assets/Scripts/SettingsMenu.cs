using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Este es el slider de sonido")]
    public Slider slidersound;
    [Header("Este es el mezclador de audio")]
    public AudioMixer audioMixer;

    [Header("Configuracion de resolucion")]
    public TMPro.TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    [Header("Parametros para quitar si se ejecuta en android")]
    public TMPro.TMP_Text fullscreenText;
    public Toggle fullscreenToggle;
    [Header("Configuracion de calidad")]
    public TMPro.TMP_Dropdown qualityDropdown;
    [Header("Configuracion de creditos")]
    public GameObject creditsPanel;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
    }
    // 0 - veary low _ 1 - low _ 2 - medium  _ 3 - high
    public void SetQuality(int qualityIndex)
    {
        PlayerPrefs.SetInt("quality", qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void Exit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif   
    }
    public void OpenCredits() {
        creditsPanel.SetActive(true);
    }
    public void CloseCredits() {
        creditsPanel.SetActive(false);
    }
    private void Awake()
    {
        //Si ha sido guardado el valor en los player pref se asigna automaticamente
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
        qualityDropdown.value = PlayerPrefs.GetInt("quality");
        CloseCredits();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Si la aplicacion se ejecuta en android, no queremos dejar ver las opciones de fullscreen y de las resoluciones
        if (Application.platform == RuntimePlatform.Android)
        {
            GameObject gameObjectResolution = resolutionDropdown.gameObject;
            gameObjectResolution.SetActive(false);
            GameObject gameObjectFullscreen = fullscreenText.gameObject;
            gameObjectFullscreen.SetActive(false);
            GameObject gameObjectToggle = fullscreenToggle.gameObject;
            gameObjectToggle.SetActive(false);
        }
        //Debug.Log(PlayerPrefs.GetFloat("volume"));
        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("volume"));
        slidersound.value = PlayerPrefs.GetFloat("volume");
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        //Sirve para saber que reolucion esta activada actualmente
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            //Asigna cual es el currente index
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        //refresca el dropdown para que se active
        resolutionDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
