using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
	
	//Method by Superrodan 2016/04/28, answers.unity3d.com/questions/878382/audio-or-music-to-continue-playing-between-scene.html
	public GameObject musicPlayer;
	public AudioSource backMusic;

	//this is to have the music play throughout the game
	void Awake()
	{
		musicPlayer = GameObject.Find ("Music");
		backMusic = GetComponent<AudioSource>();
		backMusic.Play ();

		//a check to see if the music player exists within the scene, if it does, the duplicate gets deleted
		if (musicPlayer == null)
		{
			musicPlayer = this.gameObject;

			musicPlayer.name = "Music";

			DontDestroyOnLoad(this.gameObject);

		}

		else
		{
			if (this.gameObject.name != "Music")
			{
				Destroy (this.gameObject);
			}
		}
	}
	
}
