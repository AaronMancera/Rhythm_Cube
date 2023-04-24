using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public AudioSource audioSource;

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
    }
}
