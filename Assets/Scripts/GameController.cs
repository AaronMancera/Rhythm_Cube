using Firebase.Database;
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
    public GameObject pausePanel, gamePanel, endingPanel;
    //textos
    public TMP_Text usernameText, scoreText, bestScoreText, pauseUsernameText, pauseScoreText, pauseBestScoreText, endingUserName, endingScoreText, endingBestScoreText;
    //puntuacion
    public int score, bestScore;
    private float time;
    //bool fin del jego
    private bool end = false;
    public BoxCollider2D boxCollider;
    public GameObject player;
    private Vector3 spawnPlayer;

    //este es el script de camra que desde el game controller vamos a realizar un metodo
    public CameraController cameraController;
    private float endTime;
    //bool ha muerto
    private bool dead = false;
    public GameObject prefab; // asigna el prefab en el inspector
    //Firebase
    //DataBase
    Firebase.Database.DatabaseReference database;
    //Auth
    Firebase.Auth.FirebaseAuth auth;



    private void Awake()
    {
        //metodo que buscara en la escena automaticamente el objeto cameracontroller
        cameraController = FindObjectOfType<CameraController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Ponerlo a 60fps TODO: Mas adelante configurar desde opciones
        Application.targetFrameRate = 60;
        audioSource.Play();
        Time.timeScale = 1f;
        spawnPlayer = player.transform.position;
        //TODO: Coger de la base de datos el mejor score
        //Inicializa el score de manera local
       
        InitialText();
        InitializeFirebase();
    }
    //Firebase Initial
    void InitializeFirebase()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    void InitialText()
    {
        usernameText.text = PlayerPrefs.GetString("UserName");
        pauseUsernameText.text = PlayerPrefs.GetString("UserName");
        endingUserName.text = PlayerPrefs.GetString("UserName");
        if (PlayerPrefs.GetInt("score_1") != 0)
        {
            bestScore = PlayerPrefs.GetInt("score_1");
            bestScoreText.text = bestScore.ToString();
            pauseBestScoreText.text = bestScore.ToString();
            endingScoreText.text = bestScore.ToString();
        }
        else
        {
            bestScore = 0;
        }

    }
    // Update is called once per frame
    void Update()
    {
        //TODO: Poner una configuracion para el volumen
        //establecer el volumen
        audioSource.volume = 0.5f;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            //End
            if (end)
            {
                //el metodo que realizara si se ha activado al menos una vez el trigger
                //camara deja de seguir al objeto
                cameraController.stopFollow();
                //y espera 3 segundos para mostrar el menu
                if (endTime >= 3f)
                {
                    endingPanel.SetActive(true);
                    pausePanel.SetActive(false);
                    gamePanel.SetActive(false);
                    player.SetActive(false);
                }
                else
                {
                    endTime += Time.deltaTime;
                    Debug.Log(endTime);
                }
            }
            //trigger
            else if (IsEnd())
            {
                end = true;

            }
            //score in gameplay
            else
            {
                //tiempo vivo en el nivel
                time += Time.deltaTime;
                Score();
                //Muerte
                if (dead)
                {
                    Reset();
                }

                else if (IsDead())
                {
                    dead = true;
                }
            }
        }

    }
    //Pausa el juego
    public void Pause()
    {
        endingPanel.SetActive(false);
        pausePanel.SetActive(true);
        gamePanel.SetActive(false);
        Time.timeScale = 0f;
        audioSource.Pause();

    }
    //Reanuda el juego
    public void Resume()
    {
        endingPanel.SetActive(false);
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        audioSource.Play();
        Time.timeScale = 1f;
    }
    //Resetear el nivel
    public void Reset()
    {
        // Obtener el índice de la escena actual
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Cargar de nuevo la misma escena
        //SceneManager.LoadScene(currentSceneIndex);
        //Time.timeScale = 1f;

        //player.SetActive(true);
        //Cuando muera eliminara el game object y esperara 5 segundos con la musica pausada para respaunear otro jugador
        Destroy(player);
        audioSource.Stop();
        // Esperamos 5 segundos
        Invoke("ReaparecerJugador", 3f);


    }
    void ReaparecerJugador()
    {
        // Creamos una nueva instancia del objeto en la posición de reaparición
        GameObject nuevoObjeto = Instantiate(prefab, spawnPlayer, Quaternion.identity);
        // Asignamos el nuevo objeto a la variable para poder eliminarlo en el futuro

        player = nuevoObjeto;
        boxCollider = player.GetComponent<BoxCollider2D>();
        dead = false;
        if (score > bestScore)
        {
            bestScore = score;
            bestScoreText.text = bestScore.ToString();
            pauseBestScoreText.text = bestScore.ToString();
            endingScoreText.text = bestScore.ToString();

            //Database realtime actualiza el campo de score_1
            Debug.Log(PlayerPrefs.GetString("UserId"));

            if (PlayerPrefs.GetString("UserId") != null)
            {
                WriteScoreInDatabase(PlayerPrefs.GetString("UserId"), bestScore);
            }
            //Guarda en los datos guardados
            PlayerPrefs.SetInt("score_1", bestScore);
        }
        time = 0;
        scoreText.text = score.ToString();


        // Reiniciamos el AudioSource
        audioSource.Play();
    }
    //Metodo de base de datos apra actualizar el valor del primero score
    private void WriteScoreInDatabase(string userId, int score)
    {
        /*
        https://tfgrhythmcube-default-rtdb.europe-west1.firebasedatabase.app/
                                                                             users/
                                                                                   id/
                                                                                      score_1
        */
        Debug.Log("UserIdDespues: "+userId);

        database.Child("users").Child(userId).Child("score_1").RunTransaction(mutableData =>
        {
            // if the data isn't an int or is null, just make it 0

            mutableData.Value = score;
            return TransactionResult.Success(mutableData);
        });
    }
    //Salir al menu
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
    //Puntuacion
    //TODO: El score debe guardarse en las PlayerPrefs para que se comparta los valores del menu de juego y el menu de pause
    private void Score()
    {
        score = (int)time;
        scoreText.text = score.ToString();
        pauseScoreText.text = score.ToString();
        endingScoreText.text = score.ToString();

        //PlayerPrefs.SetFloat("Score_lvl1", score);

    }
    //Metodo que detecta cuando toca un layer End
    private bool IsEnd()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask("End"));
        return raycastHit.collider != null;
    }
    //Metodo que detecta cuando toca un layer DeadZone
    private bool IsDead()
    {
        //Vextor que detecta eje y y eje x
        Vector2 direction = new Vector2(1, 1);
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, direction, 0.1f, LayerMask.GetMask("DeadZone"));
        return raycastHit.collider != null;
    }
    private void OnApplicationQuit()
    {

        if (PlayerPrefs.GetInt("RemenberMe") == 0)
        {
            auth.SignOut();
        }

    }
}
