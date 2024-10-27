using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public GameObject movementTut;
    public GameObject pickUpTut;
    public GameObject throwTut;
    public GameObject findItemTut;
    public GameObject holdItemTut;

    public float currentFadeOutDelay;
    public float tutorialTimer;
    public float timeToNextTut = 2f;

    public int tutRun;

    public bool isTutorial;

    private bool _startFadeOutTimer;
    private bool tutorialTimerOn;
    private bool nextTutTimer;
    private bool showFindItemCalled;

    private float fadeOutDelay = 3f;
    private float currentTimeToNextTut;

    private int tutSequence = 0;

    private Image movementTutIMG;
    private Image pickUpTutIMG;
    private Image throwTutIMG;
    private Image findItemIMG;
    private Image holdItemIMG;

	// Use this for initialization
	void Start ()
    {
        if (findItemTut != null && holdItemTut != null)
        {
            findItemIMG = findItemTut.GetComponent<Image>();
            holdItemIMG = holdItemTut.GetComponent<Image>();

            findItemIMG.GetComponent<CanvasRenderer>().SetAlpha(0f);
            findItemTut.GetComponentInChildren<Text>().GetComponent<CanvasRenderer>().SetAlpha(0f);
            findItemTut.transform.Find("Golden Apple").GetComponent<Image>().GetComponent<CanvasRenderer>().SetAlpha(0f);
            holdItemIMG.GetComponent<CanvasRenderer>().SetAlpha(0f);
            holdItemTut.GetComponentInChildren<Text>().GetComponent<CanvasRenderer>().SetAlpha(0f);
        }

        movementTutIMG = movementTut.GetComponent<Image>();
        pickUpTutIMG = pickUpTut.GetComponent<Image>();
        throwTutIMG = throwTut.GetComponent<Image>();

        pickUpTutIMG.GetComponent<CanvasRenderer>().SetAlpha(0f);
        pickUpTut.GetComponentInChildren<Text>().GetComponent<CanvasRenderer>().SetAlpha(0f);
        throwTutIMG.GetComponent<CanvasRenderer>().SetAlpha(0f);
        throwTut.GetComponentInChildren<Text>().GetComponent<CanvasRenderer>().SetAlpha(0f);
        
        currentFadeOutDelay = fadeOutDelay;
        currentTimeToNextTut = timeToNextTut;

        _startFadeOutTimer = true;
        tutorialTimerOn = true;
        nextTutTimer = false;
        showFindItemCalled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //print(tutorialTimer);

        //print(gameObject.name + " tutSequence: " + tutSequence);

        if (tutorialTimerOn)
        {
            tutorialTimer -= Time.deltaTime;

            if (tutorialTimer <= 0)
            {
                if (findItemTut != null)
                {
                    ShowFindItem();
                    _startFadeOutTimer = true;
                    showFindItemCalled = true;
                    tutorialTimerOn = false;
                }
            }
        }

        if (nextTutTimer)
        {
            currentTimeToNextTut -= Time.deltaTime;

            if (currentTimeToNextTut <= 0)
            {
                if (tutSequence < 1)
                {
                    if (holdItemTut != null)
                    {
                        //ShowHoldItem();
                        HideHoldItem();
                        ShowThrow();
                        currentTimeToNextTut = timeToNextTut;
                        _startFadeOutTimer = true;
                        tutSequence++;
                        tutorialTimerOn = false;
                    }
                }
            }
        }

        if (_startFadeOutTimer)
        {
            //print("fade out timer: " + currentFadeOutDelay);

            currentFadeOutDelay -= Time.deltaTime;

            if (currentFadeOutDelay <= 0f)
            {
                HideMovement();
                HidePickUp();

                if (showFindItemCalled)
                {
                    if (findItemTut != null)
                    {
                        HideFindItem();
                        //print("code has been accessed");
                    }

                    if (holdItemTut != null)
                    {
                        HideHoldItem();
                    }
                }

                currentFadeOutDelay = fadeOutDelay;
                _startFadeOutTimer = false;
            }
        }
	}

    public void ShowPickUp()
    {
        pickUpTutIMG.CrossFadeAlpha(1f, 0.5f, false);
        pickUpTut.GetComponentInChildren<Text>().CrossFadeAlpha(1f, 0.5f, false);

        _startFadeOutTimer = true;
    }

    public void ShowThrow()
    {
        throwTutIMG.CrossFadeAlpha(1f, 0.5f, false);
        throwTut.GetComponentInChildren<Text>().CrossFadeAlpha(1f, 0.5f, false);
    }

    public void ShowFindItem()
    {
        findItemIMG.CrossFadeAlpha(1f, 0.5f, false);
        findItemTut.GetComponentInChildren<Text>().CrossFadeAlpha(1f, 0.5f, false);
        findItemTut.transform.Find("Golden Apple").GetComponent<Image>().CrossFadeAlpha(1f, 0.5f, false);
        //tutSequence++;
    }

    public void ShowHoldItem()
    {
        holdItemIMG.CrossFadeAlpha(1f, 0.5f, false);
        holdItemTut.GetComponentInChildren<Text>().CrossFadeAlpha(1f, 0.5f, false);
        //tutSequence++;
    }

    public void HideMovement()
    {
        movementTutIMG.CrossFadeAlpha(0, 0.5f, false);
        movementTut.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 0.5f, false);
    }

    public void HidePickUp()
    {
        pickUpTutIMG.CrossFadeAlpha(0f, 0.5f, false);
        pickUpTut.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 0.5f, false);
    }

    public void HideThrow()
    {
        throwTutIMG.CrossFadeAlpha(0f, 0.5f, false);
        throwTut.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 0.5f, false);
    }

    public void HideFindItem()
    {
        findItemIMG.CrossFadeAlpha(0f, 0.5f, false);
        findItemTut.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 0.5f, false);
        findItemTut.transform.Find("Golden Apple").GetComponent<Image>().CrossFadeAlpha(0f, 0.5f, false);

        nextTutTimer = true;
    }

    public void HideHoldItem()
    {
        holdItemIMG.CrossFadeAlpha(0f, 0.5f, false);
        holdItemTut.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 0.5f, false);
    }

    public bool StartFadeOutTimer {
        get { return _startFadeOutTimer; }
        set { _startFadeOutTimer = value; }
    }
}
