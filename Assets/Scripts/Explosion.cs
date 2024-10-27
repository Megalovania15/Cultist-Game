using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public int DMGDealt;

    private float explosionTime = 1f;

    private SoundManager soundManager;

	// Use this for initialization
	void Start ()
    {
        soundManager = FindObjectOfType<SoundManager>();

        soundManager.PlaySound("Destroyed");
	}
	
	// Update is called once per frame
	void Update ()
    {
        explosionTime -= Time.deltaTime;

        if (explosionTime <= 0)
            Destroy(gameObject);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.name + " has been hit");

        if (other.gameObject.tag == "Player")
        {
            print(other.gameObject.name + " has taken damage");
            other.gameObject.GetComponent<PlayerHealth>().invulnerabilityActive = true;
            other.gameObject.GetComponent<PlayerHealth>().LoseHealth(DMGDealt);
            if (other.gameObject.GetComponent<PlayerController>().child != null)
                other.gameObject.GetComponent<PlayerController>().Drop();
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
