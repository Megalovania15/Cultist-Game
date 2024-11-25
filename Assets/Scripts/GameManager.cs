using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {

    public GameObject player1Respawn;
    public GameObject player2Respawn;
    public GameObject player1;
    public GameObject player2;
    public GameObject player1RespawnTimer;
    public GameObject player2RespawnTimer;

    public GameObject player1StartScreen;
    public GameObject player2StartScreen;

    public GameObject pauseMenu;

    public float respawnTimeP1;
    public float respawnTimeP2;

    public bool endGame;
    public bool isTutorial = false;
    public bool isPaused = false;
    public bool canPause = false;

    private float currentRespawnTimeP1;
    private float currentRespawnTimeP2;

    private string player1StartButton;
    private string player2StartButton;
    
    private SoundManager soundManager;

    void Awake()
    {
        Time.timeScale = 1f;
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    }

	// Use this for initialization
	void Start ()
    {
        pauseMenu.SetActive(false);

        soundManager = FindObjectOfType<SoundManager>();
        
        endGame = false;

        currentRespawnTimeP1 = respawnTimeP1;
        currentRespawnTimeP2 = respawnTimeP2;

        player1RespawnTimer.gameObject.SetActive(false);
        player2RespawnTimer.gameObject.SetActive(false);

        //player1StartScreen.SetActive(true);
        //player2StartScreen.SetActive(true);

        player1StartButton = player1.GetComponent<PlayerController>().startButton;
        player2StartButton = player2.GetComponent<PlayerController>().startButton;
	}
	
	// Update is called once per frame
	void Update ()
    {
        RevealScreen();

        /*if (!player1StartScreen.activeInHierarchy && !player2StartScreen.activeInHierarchy)
            GamePaused();*/

        ShowRespawnCountdown();

        if (endGame)
        {
            print("game is over");
            Time.timeScale = 0;
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
        }
        

        if (player1.gameObject.GetComponent<PlayerHealth>().startRespawnTimer)
        {
            currentRespawnTimeP1 -= Time.deltaTime;

            if (currentRespawnTimeP1 <= 0f)
            {
                Respawn();

                player1RespawnTimer.SetActive(false);

                currentRespawnTimeP1 = respawnTimeP1;

                player1.gameObject.GetComponent<PlayerHealth>().startRespawnTimer = false;

            }
        }

        if (player2.gameObject.GetComponent<PlayerHealth>().startRespawnTimer)
        {
            currentRespawnTimeP2 -= Time.deltaTime;

            if (currentRespawnTimeP2 <= 0f)
            {
                Respawn();

                player2RespawnTimer.SetActive(false);

                currentRespawnTimeP2 = respawnTimeP2;

                player2.gameObject.GetComponent<PlayerHealth>().startRespawnTimer = false;
            }
        }
    }


    public void TogglePause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Game was paused");

            if (isPaused)
            {
                InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
                isPaused = false;
                
            }
            else
            {
                InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                isPaused = true;

            }
        }

        /*if (Input.GetButtonDown(player1StartButton) || Input.GetButtonDown(player2StartButton))
        {
            soundManager.PlaySound("Start Button");
            isPaused = !isPaused;
        }*/

    }

    void RevealScreen()
    {
        if (player1.GetComponent<PlayerController>().playerInitiated)
        {
            player1StartScreen.transform.Find("Press Start Text").gameObject.SetActive(false);
            player1StartScreen.GetComponent<RectTransform>().localScale -= new Vector3(0f, 0.7f) * Time.deltaTime;
            if (player1StartScreen.GetComponent<RectTransform>().localScale.y <= 0)
            {
                player1StartScreen.GetComponent<RectTransform>().localScale = new Vector3(0f, 0f);
                player1StartScreen.SetActive(false);
            }
        }

        if (player2.GetComponent<PlayerController>().playerInitiated)
        {
            player2StartScreen.transform.Find("Press Start Text").gameObject.SetActive(false);
            player2StartScreen.GetComponent<RectTransform>().localScale -= new Vector3(0f, 0.7f) * Time.deltaTime;
            if (player2StartScreen.GetComponent<RectTransform>().localScale.y <= 0)
            {
                player2StartScreen.GetComponent<RectTransform>().localScale = new Vector3(0f, 0f);
                player2StartScreen.SetActive(false);
            }
                
        }
    }

    void ShowRespawnCountdown()
    {
        if (player1.gameObject.GetComponent<PlayerHealth>().isDead)
        {
            player1RespawnTimer.SetActive(true);

            player1RespawnTimer.gameObject.GetComponent<Text>().text = "Respawn In: " + Mathf.RoundToInt(currentRespawnTimeP1).ToString();
        }

        if (player2.gameObject.GetComponent<PlayerHealth>().isDead)
        {
            player2RespawnTimer.SetActive(true);

            player2RespawnTimer.gameObject.GetComponent<Text>().text = "Respawn In: " + Mathf.RoundToInt(currentRespawnTimeP2).ToString();
        }
    }

    public void Respawn()
    {
        if (player1.gameObject.GetComponent<PlayerHealth>().isDead)
        {
            //do the thing
            player1.transform.position = player1Respawn.transform.position;
            player1.GetComponent<PlayerHealth>().parent.SetActive(true);
            soundManager.PlaySound("Respawn");
            player1.GetComponent<PlayerHealth>().currentHealth = player1.gameObject.GetComponent<PlayerHealth>().maxHealth;
            player1.GetComponent<PlayerHealth>().invulnerabilityActive = true;
            player1.GetComponent<PlayerHealth>().isDead = false;
            
        }

        if (player2.gameObject.GetComponent<PlayerHealth>().isDead)
        {
            //do the thing
            player2.transform.position = player2Respawn.transform.position;
            player2.GetComponent<PlayerHealth>().parent.SetActive(true);
            soundManager.PlaySound("Respawn");
            player2.GetComponent<PlayerHealth>().currentHealth = player2.gameObject.GetComponent<PlayerHealth>().maxHealth;
            player2.GetComponent<PlayerHealth>().invulnerabilityActive = true;
            player2.GetComponent<PlayerHealth>().isDead = false;
            
        }
    }
}
