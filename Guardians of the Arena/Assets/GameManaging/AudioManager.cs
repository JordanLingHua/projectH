using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource bgMusic,unitSFX,buttonSFX,errorSFX;
	float prevMasterVolume;
	//public AudioClip buttonSFX;
	public float masterVolume,musicVolume,sfxVolume;
	void Start () {
		masterVolume = 1f;
		sfxVolume = 1f;
		this.transform.position = Camera.main.transform.position;
		musicVolume = 1.0f;
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

	void OnApplicationFocus(bool focusStatus){
		if (!focusStatus) {
			prevMasterVolume = masterVolume;
			masterVolume = 0;
			bgMusic.volume = musicVolume * masterVolume;
			setSFXVolume(masterVolume);
		} else {
			try{
				masterVolume = prevMasterVolume;
				bgMusic.volume = musicVolume * masterVolume;
				setSFXVolume(masterVolume);
			}catch(UnassignedReferenceException e){
				print ("Music still loading;;");
			}
		}
	}


	void OnLevelWasLoaded(int level){
		this.transform.position = Camera.main.transform.position;
		if (level == 3) {
			GameProcess gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
			if (gp.playerNumber == 2){
				this.transform.position = new Vector3(0f,160f,97f);
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
