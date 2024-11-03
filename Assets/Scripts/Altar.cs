using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altar : MonoBehaviour {

    public int requiredAltarPieces, requiredBuckets, requiredCandles;

    //progress items
    public Slider progressBar;
    public Text progressPercentage;
    public Sprite[] sprites;

    //immediate build progress
    public GameObject buildProgressBar;
    public GameObject spiritedAwayParticle;

    public bool startBuildTimer = false;

    public float totalBuildTime;

    private float currentBuildTime = 0f;

    private bool playerDroppedObj = false;
    private bool particleSpawned = false;

    private RectTransform buildProgressBarFill;

    //item count
    public GameObject listOfItems;
    public Text altarPieceCount;
    public Text bucketCount;
    public Text candleCount;

    public int numberOfAltarPieces;
    public int numberOfBuckets;
    public int numberOfCandles;

    public int currentAltarPieces, currentBuckets, currentCandles;

    public GameObject playerWonBlurb;
    public Text playerWonText;

    public GameObject destroyParticle;
    public GameObject owner;

    //...
    private float percentage;
    
    private float totalAltarPieces;

    private bool canBuild = false;

    private GameObject buildItem;

    private SpriteRenderer currentSprite;
    
    private GameManager gameManager;
    private SoundManager soundManager;
    private PlayerController player;

    // Use this for initialization
    void Start ()
    {
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
        player = owner.gameObject.GetComponent<PlayerController>();
        currentSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        buildProgressBarFill = buildProgressBar.transform.Find("Fill").gameObject.GetComponent<RectTransform>();

        totalAltarPieces = requiredAltarPieces + requiredBuckets + requiredCandles;

        progressBar.value = 0;
        
        buildProgressBarFill.localScale = new Vector3(0f, 1f, 1f);

        buildProgressBar.SetActive(false);
        playerWonBlurb.SetActive(false);
        listOfItems.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (buildItem != null)
        {
            if (canBuild && player.holdingBuildObj)
                startBuildTimer = true;
        }
        
        StartBuild();
        UpdateProgressBar();
        UpdateItemCount();
        UpdateAltarSprite();
        EndGame();
	}

    void UpdateProgressBar()
    {
        float currentCollectedPieces = currentAltarPieces + currentBuckets + currentCandles;
        percentage = (currentCollectedPieces / totalAltarPieces) * 100;

        progressPercentage.text = Mathf.RoundToInt(percentage).ToString() + "%";

        progressBar.value = percentage;
    }

    void UpdateItemCount()
    {
        numberOfAltarPieces = requiredAltarPieces - currentAltarPieces;
        numberOfBuckets = requiredBuckets - currentBuckets;
        numberOfCandles = requiredCandles - currentCandles;

        altarPieceCount.text = "x" + numberOfAltarPieces.ToString();
        bucketCount.text = "x" + numberOfBuckets.ToString();
        candleCount.text = "x" + numberOfCandles.ToString();
    }

    void UpdateAltarSprite()
    {
        int currentPercentage = (int)(percentage / 100 * 8);

        currentSprite.sprite = sprites[currentPercentage];
        
    }

    void BuildAltar()
    {
        if (buildItem.gameObject.tag == "Altar Piece")
        {
            currentAltarPieces++;
        }

        else if (buildItem.gameObject.tag == "Candle")
        {
            currentCandles++;
        }

        else if (buildItem.gameObject.tag == "Bucket")
        {
            currentBuckets++;
        }
    }

    void StartBuild()
    {
        if (startBuildTimer)
        {
            buildProgressBar.SetActive(true);

            currentBuildTime += Time.deltaTime;

            float elapsedBuildTime = currentBuildTime / totalBuildTime;

            UpdateBuildBar(elapsedBuildTime);

            Vector3 buildItemPos = buildItem.transform.position;

            if (elapsedBuildTime >= 0.4f)
            {
                if (!particleSpawned)
                {
                    Instantiate(spiritedAwayParticle, new Vector3(buildItemPos.x, buildItemPos.y - 0.5f, buildItemPos.z),
                        Quaternion.Euler(-90f, 0f, 0f));
                    soundManager.PlaySound("Build Piece Disappears");
                    particleSpawned = true;
                }
            }

            if (elapsedBuildTime >= 0.5f)
            {
                if (!playerDroppedObj && player.child != buildItem && player.child != null)
                {
                    player.Drop();
                    playerDroppedObj = true;
                }                

                buildItem.SetActive(false);

                canBuild = false;
            }

            if (currentBuildTime >= totalBuildTime)
            {
                BuildAltar();
                Destroy(buildItem);
                //buildItem = null;
                currentBuildTime = 0f;
                buildProgressBar.SetActive(false);
                playerDroppedObj = false;
                particleSpawned = false;
                startBuildTimer = false;
                
            }
        }

        else if (!startBuildTimer && buildItem != null)
        {
            currentBuildTime = 0f;

            buildProgressBar.SetActive(false);
            buildItem.SetActive(true);

            playerDroppedObj = false;
            particleSpawned = false;

            buildItem = null;

        }
    }

    void UpdateBuildBar(float buildProgress)
    {
        buildProgressBarFill.localScale = new Vector3(Mathf.Clamp(buildProgress, 0f, 1f), buildProgressBarFill.localScale.y, buildProgressBarFill.localScale.z);
    }

    void EndGame()
    {
        if (currentAltarPieces == requiredAltarPieces && currentBuckets == requiredBuckets && currentCandles == requiredCandles)
        {
            playerWonBlurb.SetActive(true);
            playerWonBlurb.GetComponent<Menu>().SelectButton();
            playerWonText.text = owner.transform.parent.name + " wins!";
            gameManager.endGame = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject == owner)
        {
            listOfItems.SetActive(true);
            //print("player entered altar area");

            if (other.gameObject.GetComponent<PlayerController>().holdingBuildObj)
            {
                buildItem = other.gameObject.GetComponent<PlayerController>().child;
                canBuild = true;

                if (buildItem != null)
                {
                    if (buildItem.gameObject.tag == "Altar Piece" && currentAltarPieces == requiredAltarPieces)
                    {
                        canBuild = false;
                    }

                    if (buildItem.gameObject.tag == "Candle" && currentCandles == requiredCandles)
                    {
                        canBuild = false;
                    }

                    if (buildItem.gameObject.tag == "Bucket" && currentBuckets == requiredBuckets)
                    {
                        canBuild = false;
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject == owner)
        {
            listOfItems.SetActive(false);

            canBuild = false;

            //buildItem = null;
        }

        if (other.gameObject.tag == "Altar Piece" || other.gameObject.tag == "Candle" || other.gameObject.tag == "Bucket")
        {
            if (startBuildTimer)
                startBuildTimer = false;
        }
    }
}
