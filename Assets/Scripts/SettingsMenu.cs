using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [Header("Este es el mezclador de audio")]
    public AudioMixer audioMixer;

    [Header("Configuracion de resolucion")]
    public TMPro.TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    // 0 - veary low _ 1 - low _ 2 - medium  _ 3 - high
    public void SetQuality(int qualityIndex)
    {

        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log(QualitySettings.GetQualitySettings().ToString());
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }
    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        //Sirve para saber que reolucion esta activada actualmente
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            //Asigna cual es el currente index
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
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
