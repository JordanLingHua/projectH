using UnityEngine;
using System.Collections;

public class CameraScript2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//this.camera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void enable()
	{
		GetComponent<Camera>().enabled = false;
	}

	void OnLevelWasLoaded()
	{
		//UnityEngine.Debug.Log ("2playernumber: " + GameObject.Find ("GameProcess").GetComponent<GameProcess> ().playerNumber);
		if (GameObject.Find ("GameProcess").GetComponent<GameProcess> ().playerNumber == 1)
			enable ();
	}
}
