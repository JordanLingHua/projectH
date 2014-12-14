using UnityEngine;
using System.Collections;

public class unitSetupScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<SpriteRenderer> ().material.shader = Shader.Find ("Unlit/Transparent");
		//gameObject.GetComponent<SpriteRenderer> ().material.shader = Shader.Find ("Toon/Basic");
		transform.localScale = new Vector3 (1.25f, 12.5f, 1.25f);
	}

	public void rotateForPlayerOne()
	{
		transform.Rotate(new Vector3 (4.5f, 0f, 0f));
	}

	public void rotateForSetupScreen()
	{
		transform.Rotate(new Vector3 (4.5f, 0f, 0f));
	}

	public void rotateForPlayerTwo()
	{
		Debug.Log(transform.localRotation.x);
		//transform.Rotate(new Vector3 (4.5f, 180f, 0f));
		//Debug.Log(transform.localRotation.x);
		//transform.localRotation = new Quaternion (-4.5f, 180f, 0f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
