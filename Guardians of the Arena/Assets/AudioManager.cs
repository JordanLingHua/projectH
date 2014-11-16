using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource bgMusic,unitSFX,buttonSFX,errorSFX;
	//public AudioClip buttonSFX;
	public float masterVolume,musicVolume,sfxVolume;
	void Start () {
		masterVolume = 1f;
		sfxVolume = 1f;
		this.transform.position = Camera.main.transform.position;
		musicVolume = 0.5f;
		bgMusic = gameObject.AddComponent<AudioSource> ();
		buttonSFX = gameObject.AddComponent<AudioSource> ();
		errorSFX = gameObject.AddComponent<AudioSource> ();

		bgMusic.clip = Resources.Load ("bgMusic") as AudioClip;
		bgMusic.Play ();
		bgMusic.loop = true;
		bgMusic.volume = musicVolume;

		buttonSFX.clip = Resources.Load ("buttonSFX") as AudioClip;
		errorSFX.clip = Resources.Load ("errorSFX") as AudioClip;
	}

	void OnLevelWasLoaded(int level){
		this.transform.position = Camera.main.transform.position;
		if (level == 3) {
			GameProcess gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
			if (gp.playerNumber == 2){
				this.transform.position = new Vector3(75f,144.8f,167.8f);
			}
		}
	}

	public void setSFXVolume(float volume){
		buttonSFX.volume = volume * masterVolume;
		errorSFX.volume = volume * masterVolume;
	
	}

	public void playButtonSFX(){
		buttonSFX.Play ();
	}
	public void playErrorSFX(){
		errorSFX.Play ();
	}
}
