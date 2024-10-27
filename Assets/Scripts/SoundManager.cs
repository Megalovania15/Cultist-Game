using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	//method by Tiane Erwee

	public AudioSource source;

	public AudioClip[] allClips;
	public AudioClip chosenClip;



	public void PlaySound (string soundName) 
	{
		chosenClip = null;

		switch (soundName) 
		{
            case "Hurt":
                chosenClip = allClips[Random.Range(0, 2)];
                break;
            case "Destroyed":
                chosenClip = allClips[3];
                break;
            case "Death":
                chosenClip = allClips[4];
                break;
            case "Contact Hit":
                chosenClip = allClips[5];
                break;
            case "Respawn":
                chosenClip = allClips[6];
                break;
            case "Ignite Bomb":
                chosenClip = allClips[7];
                break;
            case "Build Piece Disappears":
                chosenClip = allClips[8];
                break;
            case "Start Button":
                chosenClip = allClips[9];
                break;
            default:
                break;
        }

		source.PlayOneShot(chosenClip);
	}

    public void StopSound()
    {
        source.Stop();
    }
}
