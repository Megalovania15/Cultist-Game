using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public GameObject explosion;

    public Sprite[] sprites;

    public float explosionForce;
    public float timeTillDetonate;

    public bool startTimer = false;

    private bool soundPlayed = false;

    private float currentDetonateTime;
    
    private SpriteRenderer currentSprite;

    private SoundManager soundManager;

	// Use this for initialization
	void Start ()
    {
        soundManager = FindObjectOfType<SoundManager>();
        
        currentSprite = GetComponent<SpriteRenderer>();

        currentDetonateTime = timeTillDetonate;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.parent != null)
            startTimer = true;

        if (startTimer)
        {
            currentDetonateTime -= Time.deltaTime;

            float detonatePercentage = currentDetonateTime / timeTillDetonate;

            UpdateSprite(detonatePercentage);

            if (!soundPlayed)
            {
                soundManager.PlaySound("Ignite Bomb");
                soundPlayed = true;
            }

            if (currentDetonateTime <= 0)
            {
                Detonate();
                startTimer = false;
                gameObject.SetActive(false);
            }
        }
	}

    void UpdateSprite(float percentage)
    {
        int currentPercentage = (int)(percentage * 6);

        currentSprite.sprite = sprites[currentPercentage];
    }

    public void Detonate()
    {
        soundManager.StopSound();
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
