using UnityEngine;
using System.Collections;

public class CameraScript2 : MonoBehaviour {

	void OnLevelWasLoaded()
	{
		if (GameObject.Find ("GameProcess").GetComponent<GameProcess> ().playerNumber == 1){
			GetComponent<Camera>().enabled = false;
			GetComponent<AudioListener>().enabled = false;
			
		}
	}
}
