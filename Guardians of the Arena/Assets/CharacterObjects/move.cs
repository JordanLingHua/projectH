using UnityEngine;
using System.Collections;


public class move : MonoBehaviour {



	//drag and drop
	private Ray ray = new Ray();
	private RaycastHit hit = new RaycastHit();

	//Needed to avoid getting the dragged object stuck when mouse is released up
	private bool isTouched = false;

	//nearest slot; will keep UPDATING!!!!
	Transform slot;

	//slot tag's name 
	public string tagName = "Tile";
	
	void OnMouseDown()
	{
		isTouched = true;
	}
	
	void OnMouseUp()
	{
		isTouched = false;
		slot = findNearest();
		//if there is a slot
		if (slot != null)
		{
			transform.position = new Vector3(slot.transform.position.x, 5.0f, slot.position.z);
		}
	}
	
	Transform findNearest()
	{
		float nearestDistanceTile = Mathf.Infinity;
		GameObject[] taggedGameObjects = GameObject.FindGameObjectsWithTag(tagName);//<---KEY compares all objects near with that tag name!  Works perfectly for a set of tiles!!!
		Transform nearestObj = null;
		foreach (GameObject obj in taggedGameObjects)
		{
			Vector3 objectPos = obj.transform.position;
			float distanceTile = (objectPos - transform.position).sqrMagnitude;
			
			if (distanceTile < nearestDistanceTile)
			{
				nearestObj = obj.transform;
				nearestDistanceTile = distanceTile;
			}
		}
		
		return nearestObj;
	}
	
	void Update()
	{
		//drag drop how:  
		if (isTouched)
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				transform.position = new Vector3(hit.point.x, 5.0f, hit.point.z);
			}
		}
	}

}
