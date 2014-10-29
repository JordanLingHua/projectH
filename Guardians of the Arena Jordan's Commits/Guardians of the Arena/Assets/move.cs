using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {

	private Vector3 screenPoint;


	// Use this for initialization
	void Start () {


	
	}
	
	// Update is called once per frame
	void Update () {

		//Remember this sort of logic?!?!?!?!?  
		/*
		if Input.GetMouseButtonDown(0)
		{
		}
		 */


	}


	//Alternative loops to UPDATE method.  Remember Unity has BUILT-IN 
	//c# methods that you can re-implement!!!!

	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(transform.position);
	}

	void OnMouseDrag()
	{
		Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentPos = Camera.main.ScreenToWorldPoint(currentScreenPoint);

		transform.position = currentPos;
	}




}
