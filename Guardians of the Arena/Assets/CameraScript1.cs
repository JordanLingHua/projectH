using UnityEngine;
using System.Collections;

public class CameraScript1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//this.camera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnLevelWasLoaded()
	{

		if (GameObject.Find ("GameProcess").GetComponent<GameProcess> ().playerNumber == 2){
			GetComponent<Camera>().enabled = false;
			GetComponent<AudioListener>().enabled = false;
		}
	}
}
