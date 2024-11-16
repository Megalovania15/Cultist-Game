using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public GameObject explosion;

    public Sprite[] sprites;

    public float explosionForce;

    [field: SerializeField]
    public float TimeTillDetonate { get; private set; }

    public bool startTimer = false;

    public bool IsDetonating { get; set; }

    private bool soundPlayed = false;

    private float currentDetonateTime;
    
    private SpriteRenderer currentSprite;

    private SoundManager soundManager;

	// Use this for initialization
	void Start ()
    {
        soundManager = FindObjectOfType<SoundManager>();
        
        currentSprite = GetComponent<SpriteRenderer>();

        currentDetonateTime = TimeTillDetonate;
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*if (transform.parent != null)
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
        }*/
	}

    void UpdateSprite(float percentage)
    {
        int currentPercentage = (int)(percentage * 5);

        currentSprite.sprite = sprites[currentPercentage];

        //Debug.Log("Bomb detonation percentage: " + currentPercentage);
    }

    public void Detonate()
    {
        soundManager.StopSound();
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public IEnumerator DetonateBomb()
    {
        if (IsDetonating)
        {
            yield return null;
        }

        IsDetonating = true;
        float detonationTick = 0.1f;
        float temp = 0;

        if (!soundPlayed)
        {
            Debug.Log("Bomb sound played");
            soundManager.PlaySound("Ignite Bomb");
            soundPlayed = true;
        }

        while (temp < TimeTillDetonate)
        {
            temp += detonationTick;
            float detonatePercentage = temp / TimeTillDetonate;
            UpdateSprite(detonatePercentage);
            yield return new WaitForSeconds(detonationTick);
        }

        Detonate();
    }

    /*void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Bomb picked up");
            StartCoroutine(DetonateBomb(TimeTillDetonate));

            if (!soundPlayed)
            {
                soundManager.PlaySound("IgniteBomb");
                soundPlayed = true;
            }
        }
    }*/
}
