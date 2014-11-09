using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource bgMusic;
	public float volume;
	
	void Start () {
		this.transform.position = Camera.main.transform.position;
		volume = 1;
		bgMusic = gameObject.AddComponent<AudioSource> ();
		bgMusic.clip = Resources.Load ("bgMusic") as AudioClip;
		bgMusic.Play ();
		bgMusic.loop = true;
	}

	void OnLevelWasLoaded(int level){
		this.transform.position = Camera.main.transform.position;
	}

	void Update () {
	}
}
