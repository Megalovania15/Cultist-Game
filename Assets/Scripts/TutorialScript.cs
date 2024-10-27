using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour {

    public Image[] tutImages;
    public Text[] textItems;

    public bool hasNextTut = false;

	// Use this for initialization
	void Start ()
    {
        foreach (Image tutImage in tutImages)
        {
            tutImage.GetComponent<CanvasRenderer>().SetAlpha(0f);
        }

        foreach (Text textItem in textItems)
        {
            textItem.GetComponent<CanvasRenderer>().SetAlpha(0f);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FadeIn()
    {

    }

    IEnumerator TutorialCycle()
    {
        for (int i = 0; i < tutImages.Length; i++)
        {
            tutImages[i].CrossFadeAlpha(1f, 0.5f, false);

            textItems[i].CrossFadeAlpha(1f, 0.5f, false);

            yield return new WaitForSeconds(3f);

            tutImages[i].CrossFadeAlpha(0f, 0.5f, false);

            textItems[i].CrossFadeAlpha(0f, 0.5f, false);

            if (hasNextTut)
            {
                yield return new WaitForSeconds(3f);
            }
        }
        yield return null;
    }
}
