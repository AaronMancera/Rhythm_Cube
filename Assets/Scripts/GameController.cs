using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //audio
    public AudioSource audioSource;
    //paneles
    public GameObject pausePanel, gamePanel;
    //textos
    public TMP_Text usernameText, scoreText, bestScoreText;
    //pntuacion
    public int score;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.Play();

    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Poner una configuracion para el volumen
        //establecer el volumen
        audioSource.volume = 0.5f;
        //tiempo vivo en el nivel
        time += Time.deltaTime;
        Score();
        
    }
    //Pausa el juego
    public void Pause()
    {
        pausePanel.SetActive(true);
        gamePanel.SetActive(false);
        Time.timeScale = 0f;
        audioSource.Pause();

    }
    //Reanuda el juego
    public void Resume()
    {
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        audioSource.Play();
        Time.timeScale = 1f;
    }
    //Resetear el nivel
    public void Reset()
    {
        // Obtener el índice de la escena actual
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Cargar de nuevo la misma escena
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1f;
    }
    //Puntuacion
    //TODO: El score debe guardarse en las PlayerPrefs para que se comparta los valores del menu de juego y el menu de pause
    private void Score() {
        score = (int)time;
        scoreText.text = score.ToString();
    }

}
