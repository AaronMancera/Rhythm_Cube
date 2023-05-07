using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //El obejto que va a seguir (player)
    public GameObject player;
    //end
    private bool end = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");

            if (!end)
            {
                transform.position = new Vector3(player.transform.position.x + 3, player.transform.position.y + 2, transform.position.z);
            }
        }
    }
    public void stopFollow()
    {
        end = true;
    }
}
