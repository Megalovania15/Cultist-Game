using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObj : MonoBehaviour {

    public GameObject destroyParticle;

    public int minDMG, maxDMG;

    private float impulse;

    private Rigidbody2D rb;

    private SoundManager soundManager;

	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        soundManager = FindObjectOfType<SoundManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        //print("relative velocity: " + other.relativeVelocity.magnitude);

        if (other.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            Rigidbody2D colRB = other.gameObject.GetComponent<Rigidbody2D>();

            impulse = (rb.mass + colRB.mass) * other.relativeVelocity.magnitude / 0.5f;
        }

        else
            impulse = rb.mass * other.relativeVelocity.magnitude / 0.37f;

        //print("impulse: " + impulse);

        if (impulse >= 700f)
        {
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Altar")
            {
                //method by AlwaysSunny, answers.unity3d.com/questions/911736/how-do-you-cause-damage-based-on-object-velocity.html
                float maxImpulseConsidered = 2000f;
                float factor = impulse / maxImpulseConsidered;
                factor = Mathf.Clamp01(factor);
                int DMGDealt = Mathf.RoundToInt(Mathf.Lerp(minDMG, maxDMG, factor));
                //print("factor: " + factor);
                //print("DMG Dealt: " + DMGDealt);

                if (other.gameObject.tag == "Player" && transform.parent != other.gameObject.transform)
                {
                    if (other.gameObject.GetComponent<PlayerController>().child != null)
                    {
                        other.gameObject.GetComponent<PlayerController>().Drop();
                    }

                    other.gameObject.GetComponent<PlayerHealth>().invulnerabilityActive = true;

                    other.gameObject.GetComponent<PlayerHealth>().LoseHealth(DMGDealt);
                }

                else if (other.gameObject.tag == "Altar" && other.gameObject.GetComponent<Altar>().startBuildTimer)
                {
                    other.gameObject.GetComponent<Altar>().startBuildTimer = false;
                }
            }

            if (other.gameObject != gameObject)
            {
                if (other.gameObject.tag == "Crate")
                {
                    other.gameObject.GetComponent<MoveableObj>().DestroyObj();

                    if (gameObject.tag == "Crate")
                    {
                        soundManager.PlaySound("Destroyed");
                        gameObject.GetComponent<Crate>().DropObject();
                        DestroyObj();
                    }
                }

                else if (gameObject.tag == "Crate")
                {
                    if (other.gameObject.tag == "Rock" || other.gameObject.tag == "Altar Piece" || other.gameObject.tag == "Altar" ||
                        other.gameObject.tag == "Wall")
                    {
                        gameObject.GetComponent<Crate>().DropObject();
                        soundManager.PlaySound("Destroyed");
                        DestroyObj();
                    }
                }

                else if (gameObject.tag == "Bomb")
                {
                    if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Altar" || other.gameObject.tag == "Rock" || 
                        other.gameObject.tag == "Altar Piece")
                    {
                        gameObject.GetComponent<Bomb>().startTimer = false;
                        gameObject.GetComponent<Bomb>().Detonate();
                        gameObject.SetActive(false);
                    }
                }
            }
            
        }
    }
    
    public void DestroyObj()
    {
        Instantiate(destroyParticle, new Vector3(transform.position.x, transform.position.y, -2f), transform.rotation);
        gameObject.transform.parent = null;
        gameObject.SetActive(false);
    }
}
