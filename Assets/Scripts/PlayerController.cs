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
        // Saltar cuando detecte que esta en el suelo tanto con spacio como tocar la pantalla
        if ((Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0) && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
        float extraHeightText = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraHeightText, LayerMask.GetMask("Ground"));
        return raycastHit.collider != null;
    }
}
