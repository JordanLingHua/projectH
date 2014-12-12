using UnityEngine;
using System.Collections;

public class setTransparencyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<SpriteRenderer> ().material.shader = Shader.Find ("Unlit/Transparent");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
