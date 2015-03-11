using UnityEngine;
using System.Collections;

public class CameraScript1 : MonoBehaviour {

	void OnLevelWasLoaded()
	{
		if (Screen.width < 1280){
			Camera.main.orthographicSize = 60;
		}
		if (GameObject.Find ("GameProcess").GetComponent<GameProcess> ().playerNumber == 2){
			GetComponent<Camera>().enabled = false;
			GetComponent<AudioListener>().enabled = false;
		}
	}
}
