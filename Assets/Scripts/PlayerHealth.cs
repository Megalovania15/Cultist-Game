using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public Slider healthBar;
    public SpriteRenderer spriteRenderer;

    public GameObject parent;
    public GameObject destroyParticle;
    public GameObject[] gravestones;

    public bool invulnerabilityActive;
    public bool isDead;
    public bool startRespawnTimer;

    public int maxHealth;
    public int currentHealth;

    private SoundManager soundManager;

	// Use this for initialization
	void Start ()
    {
        soundManager = FindObjectOfType<SoundManager>();

        healthBar.value = maxHealth;
        currentHealth = maxHealth;

        invulnerabilityActive = false;
        isDead = false;
        startRespawnTimer = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        SetHealthBar();
        PlayerDeath();

        if (invulnerabilityActive)
        {
            StartCoroutine(MakeInvulnerable());
        }
    }

    void SetHealthBar()
    {
        healthBar.value = currentHealth;
    }

    void PlayerDeath()
    {
        if (currentHealth <= 0)
        {
            soundManager.PlaySound("Death");
            Instantiate(destroyParticle, new Vector3(transform.position.x, transform.position.y, -2f), transform.rotation);
            Instantiate(gravestones[Random.Range(0, gravestones.Length)], gameObject.transform.position, Quaternion.identity);
            isDead = true;
            startRespawnTimer = true;
            parent.SetActive(false);
        }
    }

    //makes the character invulnerable for a set amount of time
    IEnumerator MakeInvulnerable()
    {

        gameObject.layer = LayerMask.NameToLayer("Invulnerable Player");

        for (int i = 0; i < 10; i++)
        {
            spriteRenderer.color = Color.white;

            yield return new WaitForSeconds(0.2f);

            spriteRenderer.color = new Color32(255, 255, 255, 120);

            yield return new WaitForSeconds(0.2f);
        }

        spriteRenderer.color = Color.white;
        gameObject.layer = LayerMask.NameToLayer("Player");
        invulnerabilityActive = false;
    }

    public void LoseHealth(int DMGDealt)
    {
        soundManager.PlaySound("Hurt");
        currentHealth -= DMGDealt;
    }
}
