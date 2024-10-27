using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    
    private GameManager gameManager;

	// Use this for initialization
	void Start ()
    {
        gameManager = FindObjectOfType<GameManager>();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject == gameManager.player1)
            {
                //do the thing
                gameManager.player1Respawn = gameObject;
            }

            if (other.gameObject == gameManager.player2)
            {
                //do the thing
                gameManager.player2Respawn = gameObject;
            }
        }
    }
}
