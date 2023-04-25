using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Velocidad de movimiento
    public float moveSpeed;
    //fuerza de salto
    public float jumpForce;
    //el objeto player
    private Rigidbody2D rb;
    //caja de colision
    private BoxCollider2D boxCollider;
    //moviment mode
    private int movementMode = 0;
    //fuerza de vuelo
    public float flyforce;
    private float timeToFall = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Ponerlo a 60fps
        Application.targetFrameRate = 60;
        //Se inicializa
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        // Mover el cubo hacia la derecha
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        //Si el movimiento esta modo volar entonces
        if (IsFly())
        {
            movementMode = 1;

        }
        //si el movimiento no esta en modo volar entonces
        else
        {
            movementMode = 0;
        }
        // Saltar cuando detecte que esta en el suelo tanto con spacio como tocar la pantalla
        if ((Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0) && IsGrounded() && movementMode == 0)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //Modo volar manteniendo el spacio o pulsando la pantalla
        if (movementMode == 1)
        {
            Debug.Log("Fly");
            if ((Input.GetKey(KeyCode.Space) || Input.touchCount > 0))
            {
                rb.AddForce(Vector2.up * flyforce, ForceMode2D.Impulse);
                timeToFall = 0;
                
            }
            else
            {
                timeToFall += Time.deltaTime;
                if (timeToFall > 1)
                {
                    rb.AddForce(Vector2.down * 0.2f, ForceMode2D.Impulse);
                }
            }
        }

    }
    /*
     * NOTEERROR: Si ponemos el codigo de fixedupdate ne el update el personaje salta demasiadas veces en tan poco tiempo debido al internvalo de ejecucion de update, sin embargo con update el intervalo es fij y siempoire el mismo por lo que no genera ese error
     * Error: si se pulsa muchas veces aun asi sigue saltando el doble
     * FixedUpdate() es un método que se utiliza en Unity para actualizar físicas y movimiento de objetos.
     * Se ejecuta en intervalos de tiempo fijos y constantes, independientemente de la frecuencia de cuadros (fps) de la aplicación. 
     * Esto significa que el FixedUpdate() se ejecutará un número fijo de veces por segundo, independientemente de si el juego se está ejecutando a 30 fps o 100 fps.
     * Se puede utilizar Update() para la lógica de juego no relacionada con la física, mientras que FixedUpdate() se utiliza para actualizaciones de física
     */
    private void FixedUpdate()
    {

    }
    // Función para detectar si el cubo está tocando el suelo
    private bool IsGrounded()
    {
        //distancia
        float extraHeightText = 0.5f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraHeightText, LayerMask.GetMask("Ground"));
        return raycastHit.collider != null;
    }
    private bool IsFly()
    {
        //distancia
        float height = 0.1f;
        Vector2 direction = new Vector2(1, 1);
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, direction, height, LayerMask.GetMask("FlyZone"));
        return raycastHit.collider != null;
    }
}
