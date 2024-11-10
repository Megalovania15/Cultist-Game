using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altar : MonoBehaviour {

    public int requiredAltarPieces, requiredBuckets, requiredCandles;

    //progress items
    public Slider mainProgressBar;
    public Text progressPercentage;
    public Sprite[] sprites;

    //immediate build progress
    public GameObject buildProgressBar;
    public GameObject spiritedAwayParticle;

    public bool startBuildTimer = false;

    public float totalBuildTime;

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

        mainProgressBar.value = 0;
        
        buildProgressBarFill.localScale = new Vector3(0f, 1f, 1f);

        buildProgressBar.SetActive(false);
        playerWonBlurb.SetActive(false);
        listOfItems.SetActive(false);

        UpdateProgressBar();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Build();
        
        UpdateItemCount();
        EndGame();
	}

    //updates the main progress bar that shows the progress towards completion of the alter
    void UpdateProgressBar()
    {
        float currentCollectedPieces = currentAltarPieces + currentBuckets + currentCandles;
        percentage = (currentCollectedPieces / totalAltarPieces) * 100;

        progressPercentage.text = Mathf.RoundToInt(percentage).ToString() + "%";

        mainProgressBar.value = percentage;

        UpdateAltarSprite();
    }

    //updates the number of items collected
    void UpdateItemCount()
    {
        numberOfAltarPieces = requiredAltarPieces - currentAltarPieces;
        numberOfBuckets = requiredBuckets - currentBuckets;
        numberOfCandles = requiredCandles - currentCandles;

        altarPieceCount.text = "x" + numberOfAltarPieces.ToString();
        bucketCount.text = "x" + numberOfBuckets.ToString();
        candleCount.text = "x" + numberOfCandles.ToString();
    }

    //updates the alter's sprite to show the progress towards completion
    void UpdateAltarSprite()
    {
        Debug.Log("Sprite updated");
        int currentPercentage = (int)(percentage / 100 * 8);

        Debug.Log(currentPercentage);

        currentSprite.sprite = sprites[currentPercentage];
        
    }

    //updates the counts of items... this can maybe happen somewhere else and a bit differently
    void BuildAltarProgress()
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

    /*void StartBuild()
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
    }*/

    //forces the player to drop an item and shows a progress bar over the altar when we have an item it likes
    void Build()
    {
        if (buildItem != null)
        {
            if (canBuild && player.holdingBuildObj)
            {
                player.Drop();
                var buildItemPos = new Vector2(buildItem.transform.position.x, buildItem.transform.position.y);
                Instantiate(spiritedAwayParticle, buildItemPos, Quaternion.Euler(-90f,0f,0f));
                soundManager.PlaySound("Build Piece Disappears");
                StartCoroutine(BuildBarProgress(totalBuildTime));
                BuildAltarProgress();
                Destroy(buildItem);
            }
        }
    }

    //a coroutine to demonstrate that the altar is being built, can be adapted later for more cthulu things
    IEnumerator BuildBarProgress(float currentBuildProgress)
    {
        float buildProgressTick = 1f;
        float temp = 0;
        buildProgressBar.SetActive(false);
        Debug.Log(temp);

        while (temp < currentBuildProgress)
        {
            Debug.Log("Build timer in progress");
            buildProgressBar.SetActive(true);
            temp += buildProgressTick;
            Debug.Log(temp);
            UpdateBuildBar(temp);
            yield return new WaitForSecondsRealtime(buildProgressTick);
            
        }
        UpdateProgressBar();
        buildProgressBarFill.localScale = new Vector3(0f, 1f, 1f);
        buildProgressBar.SetActive(false);
        
    }

    //changes the scale of the progress bar directly above the altar when we build.
    void UpdateBuildBar(float buildProgress)
    {
        var buildProgressTime = buildProgress / totalBuildTime;

        buildProgressBarFill.localScale = new Vector3(Mathf.Clamp(buildProgressTime, 0f, 1f), buildProgressBarFill.localScale.y, buildProgressBarFill.localScale.z);
    }

    void EndGame()
    {
        if (currentAltarPieces == requiredAltarPieces && currentBuckets == requiredBuckets 
            && currentCandles == requiredCandles)
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
        }

        if (other.gameObject.tag == "Altar Piece" || other.gameObject.tag == "Candle" || other.gameObject.tag == "Bucket")
        {
            if (startBuildTimer)
                startBuildTimer = false;
        }
    }
}
