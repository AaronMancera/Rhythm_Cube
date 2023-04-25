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
    // Función para detectar si el cubo está tocando el suelo
    private bool IsGrounded()
    {
        //distancia
        float extraHeightText = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, extraHeightText, LayerMask.GetMask("Ground"));
        return raycastHit.collider != null;
    }
}
