using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //audio
    [Header("Audio")]
    public AudioSource audioSource;
    //paneles
    [Header("Panel")]
    public GameObject pausePanel, gamePanel, endingPanel, optionsPanel;
    //textos
    [Header("Text")]
    public TMP_Text usernameText, scoreText, bestScoreText, pauseUsernameText, pauseScoreText, pauseBestScoreText, endingUserName, endingScoreText, endingBestScoreText;
    //puntuacion
    [Header("Score")]
    public int score, bestScore;
    private float time;
    //bool fin del jego
    [Header("Ending")]
    private bool end = false;
    public BoxCollider2D boxCollider;
    public GameObject player;
    private Vector3 spawnPlayer;
    //este es el script de camra que desde el game controller vamos a realizar un metodo
    [Header("Camera")]
    public CameraController cameraController;
    private float endTime;
    [Header("Dead")]
    //bool ha muerto
    private bool dead = false;
    [Header("Respawn")]
    public GameObject prefab; // asigna el prefab en el inspector
    public Button jumpButton;
    //Firebase
    //DataBase
    Firebase.Database.DatabaseReference database;
    //Auth
    Firebase.Auth.FirebaseAuth auth;



    private void Awake()
    {
        //metodo que buscara en la escena automaticamente el objeto cameracontroller
        cameraController = FindObjectOfType<CameraController>();
        //Si la aplicacion se esta ejecutando en widnows entonces se deshabilita la opcion de saltar pulsando la pantalla
        //Note: En ordenador no se puede volar pulsando la pantalla
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            // Código para Windows
            GameObject gameObject = jumpButton.gameObject;
            gameObject.SetActive(false);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        //Coger los datos de la base de datos
        //Nada maz aparecer que recoga primero el best score
        if (PlayerPrefs.GetString("UserName") != "Guest")
        {
            InitialBestRecord();
        }
        //Ponerlo a 60fps TODO: Mas adelante configurar desde opciones
        Application.targetFrameRate = 60;
        audioSource.Play();
        Time.timeScale = 1f;
        spawnPlayer = player.transform.position;
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
        //Si se ha sobreescrito se pone en todos los textos
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
    void InitialBestRecord()
    {
        Debug.Log("Hola");
        FirebaseDatabase.DefaultInstance
           //Esto cogera los de puntuacion ordenados menor a mayor (por defecto y no se puede cambiar)
           .GetReference("users").Child(PlayerPrefs.GetString("UserId"))
           .GetValueAsync().ContinueWithOnMainThread(task =>
           {
               if (task.IsFaulted)
               {
                   // Handle the error...
               }
               else if (task.IsCompleted)
               {
                   Debug.Log("Hola2");

                   DataSnapshot snapshot = task.Result;
                   //Debug.Log(snapshot.GetRawJsonValue());
                   var dictionary = snapshot.Value as Dictionary<string, object>;
                   if (dictionary != null)
                   {
                       bestScore = int.Parse(dictionary["score_1"].ToString());
                       Debug.Log(bestScore);
                       PlayerPrefs.SetInt("score_1", bestScore);
                       InitialText();
                       Debug.Log("Hola3");


                   }
               }
           });
    }
    // Update is called once per frame
    void Update()
    {
        //establecer el volumen Note: Se encarga el settingsMenu de esto
        //audioSource.volume = 0.5f;
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
    public void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);

    }
    public void CloseOptionsPanel()
    {
        optionsPanel.SetActive(false);

    }
    //NOTE: Si el reset se hace con el mismo algoritmo que el abajo, no se cierra el menu de pause y no se inicia hasta que le demos continue
    public void ResetButton() {
        Resume();
        Destroy(player);
        audioSource.Stop();
        // Esperamos 3 segundos
        Invoke("ReaparecerJugador", 0f);
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
        // Esperamos 3 segundos
        Invoke("ReaparecerJugador", 3f);


    }
    void ReaparecerJugador()
    {
        // Creamos una nueva instancia del objeto en la posición de reaparición
        GameObject nuevoObjeto = Instantiate(prefab, spawnPlayer, Quaternion.identity);
        //al reaparecer le asignamos al boton del ui la funcion de saltar
        //Si la aplicacion esta ejecutandose en el movil entonces poner el el click de saltar en el boton
        if (Application.platform == RuntimePlatform.Android)
        {
            // Código para Android
            PlayerController playerController = nuevoObjeto.GetComponent<PlayerController>();
            jumpButton.onClick.AddListener(playerController.jumpClick);
        }
       

        // Asignamos el nuevo objeto a la variable para poder eliminarlo en el futuro

        player = nuevoObjeto;
        boxCollider = player.GetComponent<BoxCollider2D>();
        dead = false;
        //Asignacion del best score
        if (score > bestScore)
        {
            bestScore = score;
            bestScoreText.text = bestScore.ToString();
            pauseBestScoreText.text = bestScore.ToString();
            endingScoreText.text = bestScore.ToString();

            //Database realtime actualiza el campo de score_1
            //Todas las previsiones posibles para que no haya fallos en la subida
            if (PlayerPrefs.GetString("UserId") != null && PlayerPrefs.GetString("UserName") !="Guest" && auth != null)
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
    // El score debe guardarse en las PlayerPrefs para que se comparta los valores del menu de juego y el menu de pause
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

    //Cuando se quita la aplicacion esto detecta si le ha dado a recuerdame o no
    private void OnApplicationQuit()
    {

        if (PlayerPrefs.GetInt("RemenberMe") == 0)
        {
            auth.SignOut();
        }

    }
}
