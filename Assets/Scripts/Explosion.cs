using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public int DMGDealt;

    public GameObject explosionParticle;

    private float explosionTime = 1f;

    private SoundManager soundManager;

	// Use this for initialization
	void Start ()
    {
        GameObject explosion = Instantiate(explosionParticle, transform);

        explosion.transform.position = transform.position;

        soundManager = FindObjectOfType<SoundManager>();

        soundManager.PlaySound("Destroyed");
	}
	
	// Update is called once per frame
	void Update ()
    {
        explosionTime -= Time.deltaTime;

        if (explosionTime <= 0)
        {
            Destroy(gameObject,0.2f);
        }
            
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.name + " has been hit");

        if (other.gameObject.tag == "Player")
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            print(other.gameObject.name + " has taken damage");
            playerHealth.invulnerabilityActive = true;
            playerHealth.LoseHealth(DMGDealt);
            if (playerController.child != null)
            {
                playerController.Drop();
            }

        }
        else if (other.gameObject.tag == "Crate")
        {
            other.gameObject.GetComponent<Crate>().DropObject();
            other.gameObject.GetComponent<MoveableObj>().DestroyObj();
        }
        else if (other.gameObject.tag == "Altar")
        {
            print("build progress halted");
            other.gameObject.GetComponent<Altar>().startBuildTimer = false;
        }
    } 
}
