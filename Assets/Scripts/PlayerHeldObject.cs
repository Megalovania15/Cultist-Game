using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeldObject : MonoBehaviour
{
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("On trigger stay happened");

        if (other.gameObject.tag == "Crate" || other.gameObject.tag == "Rock" || other.gameObject.tag == "Altar Piece" ||
            other.gameObject.tag == "Bucket" || other.gameObject.tag == "Candle" || other.gameObject.tag == "Bomb")
        {
            playerController.holdable = other.gameObject;

            /*if (playerController.child != null)
            {
                Debug.Log("Child object: " + playerController.child.name);
            }*/
            

            //Debug.Log("Holdable = " + other.gameObject.name);
            /*if (other.gameObject.tag == "Bomb" && playerController.child != null)
            {
                print("The other gameObject: " + other.gameObject.name);
                print("The child or held game object: " + playerController.child.name);

                Bomb bombComponent = other.gameObject.GetComponent<Bomb>();
                bombComponent.StartCoroutine(bombComponent.DetonateBomb(bombComponent.TimeTillDetonate));
            }*/
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (playerController.holdable == other.gameObject)
        {
            playerController.holdable = null;
        }
    }
}
