using UnityEngine;
using System.Collections;

public class TestText : MonoBehaviour {

	public GUISkin mySkin;
	void Start () {

	}

	void OnGUI(){
		GUI.skin = mySkin; 
		GUI.Button(new Rect(0,0,100,50),"HI");
	}

	void Update () {
	}
}
