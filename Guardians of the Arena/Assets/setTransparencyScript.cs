using UnityEngine;
using System.Collections;

public class setTransparencyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<SpriteRenderer> ().material.shader = Shader.Find ("Unlit/Transparent");
		transform.localScale = new Vector3 (1.25f, 12.5f, 1.25f);
		transform.Rotate(new Vector3 (4.5f, 0f, 0f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
