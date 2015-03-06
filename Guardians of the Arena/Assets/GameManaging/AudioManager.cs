using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource bgMusic,gameMusic,unitSFX,buttonSFX,errorSFX,lobbyMusic,turnEndSFX,turnStartSFX,victorySFX,defeatSFX;
	float prevMasterVolume;
	//public AudioClip buttonSFX; 
	public float masterVolume,musicVolume,sfxVolume;
	void Start () {
		masterVolume = 1f;
		sfxVolume = 1f;
		this.transform.position = Camera.main.transform.position;
		musicVolume = 1.0f;
		lobbyMusic = gameObject.AddComponent<AudioSource> ();
		gameMusic = gameObject.AddComponent<AudioSource> ();
		buttonSFX = gameObject.AddComponent<AudioSource> ();
		errorSFX = gameObject.AddComponent<AudioSource> ();
		turnEndSFX = gameObject.AddComponent<AudioSource> ();
		turnStartSFX = gameObject.AddComponent<AudioSource> ();
		victorySFX = gameObject.AddComponent<AudioSource> ();
		defeatSFX = gameObject.AddComponent<AudioSource> ();

		gameMusic.clip = Resources.Load ("gameMusic") as AudioClip;
		lobbyMusic.clip = Resources.Load ("lobbyMusic") as AudioClip;
		bgMusic = lobbyMusic;
		bgMusic.Play ();
		bgMusic.loop = true;
		bgMusic.volume = musicVolume;

		buttonSFX.clip = Resources.Load ("buttonSFX") as AudioClip;
		errorSFX.clip = Resources.Load ("errorSFX") as AudioClip;
		turnEndSFX.clip = Resources.Load ("turnEndSFX") as AudioClip;
		turnStartSFX.clip = Resources.Load ("turnStartSFX") as AudioClip;
		victorySFX.clip = Resources.Load ("victorySFX") as AudioClip;
		defeatSFX.clip = Resources.Load ("errorSFX") as AudioClip;
	}

//	void OnApplicationFocus(bool focusStatus){
//		if (!focusStatus) {
//			prevMasterVolume = masterVolume;
//			masterVolume = 0;
//			bgMusic.volume = musicVolume * masterVolume;
//			setSFXVolume(masterVolume);
//		} else {
//			try{
//				masterVolume = prevMasterVolume;
//				bgMusic.volume = musicVolume * masterVolume;
//				setSFXVolume(masterVolume);
//			}catch(UnassignedReferenceException e){
//				print (e.Message + " Music still loading;;");
//			}
//		}
//	}
	

	void OnLevelWasLoaded(int level){
		try{
			this.transform.position = Camera.main.transform.position;
			if (level == 3) {
				GameProcess gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
				if (gp.playerNumber == 2){
					this.transform.position = new Vector3(5.5f,215f,97f);
				}
				if(bgMusic != gameMusic){
					bgMusic.Stop();
					bgMusic = gameMusic;
					bgMusic.loop = true;
					bgMusic.Play();
				}
			}else if (level == 5){
				if(bgMusic != gameMusic){
					bgMusic.Stop();
					bgMusic = gameMusic;
					bgMusic.loop = true;
					bgMusic.Play(); 
				}

			}else if (level != 0){
				if(bgMusic != lobbyMusic){
					bgMusic.Stop();
					bgMusic = lobbyMusic;
					bgMusic.loop = true;
					bgMusic.Play();
				}
			}
		}catch(UnityException e){
			print (e);
		}
	}

	public void setSFXVolume(float volume){
		buttonSFX.volume = volume * masterVolume;
		errorSFX.volume = volume * masterVolume;
		turnEndSFX.volume = volume * masterVolume;
		turnStartSFX.volume = volume * masterVolume;
		victorySFX.volume = volume * masterVolume;
		defeatSFX.volume = volume * masterVolume;
	}

	public void playButtonSFX(){
		buttonSFX.Play ();
	}

	public void playErrorSFX(){
		errorSFX.Play ();
	}

	public void playTurnEndSFX(){
		turnEndSFX.Play ();
	}

	public void playTurnStartSFX(){
		turnStartSFX.Play ();
	}

	public void playVictorySFX(){
		victorySFX.Play ();
	}

	public void playDefeatSFX(){
		defeatSFX.Play ();
	}
}
