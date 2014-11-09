using UnityEngine;
using System.Collections;


public class move : MonoBehaviour {


	public PlayerSetup playerSetup;
	SetupTileScript oldScript;
	//drag and drop
	private Ray ray = new Ray();
	private RaycastHit hit = new RaycastHit();

	//Needed to avoid getting the dragged object stuck when mouse is released up
	private bool isTouched = false;

	//nearest slot; will keep UPDATING!!!!
	Transform slot;

	//slot tag's name 
	public string tagName = "Tile";


	void Start()
	{
		playerSetup = GameObject.Find ("PlayerSetup").GetComponent<PlayerSetup>(); 
	}


	void OnMouseDown()
	{
		//
		//PlayerSetup.prevPosition = transform.position;//pass by value hopefully
		playerSetup.prevPosition = transform.position;//pass by value hopefully
		//

		isTouched = true;
	}
	
	void OnMouseUp()
	{

		isTouched = false;
		slot = findNearest();
		//if there is a slot
		Debug.Log (slot);
		if (slot != null && !slot.GetComponent<SetupTileScript>().occupied)
		{
			Debug.Log ("7 or 8" + (gameObject.GetComponent<Unit>().unitRole));
			if(gameObject.GetComponent<Unit>().unitRole == 505 || gameObject.GetComponent<Unit>().unitRole == 506){
				if(slot.GetComponent<SetupTileScript>().tt == SetupTileScript.TileType.ONFIELD)
					{
						this.transform.parent.GetComponent<SetupTileScript>().occupied = false;
						this.transform.parent = slot;
						transform.position = new Vector3(slot.transform.position.x, 5.0f, slot.position.z);
						slot.GetComponent<SetupTileScript>().occupied = true;
					}
				else
					transform.position = playerSetup.prevPosition;
			}

			else if(playerSetup.playerPieces.Count < playerSetup.boardCapacity)
			{
				if(slot.GetComponent<SetupTileScript>().tt == SetupTileScript.TileType.ONFIELD)
				{
					if(!playerSetup.playerPieces.Contains(gameObject))
						playerSetup.playerPieces.Add(gameObject);

					this.transform.parent.GetComponent<SetupTileScript>().occupied = false;
					this.transform.parent = slot;
					transform.position = new Vector3(slot.transform.position.x, 5.0f, slot.position.z);
					slot.GetComponent<SetupTileScript>().occupied = true;
				}

			
				else 
				{
					if(playerSetup.playerPieces.Contains(gameObject))
						playerSetup.playerPieces.Remove(gameObject);

					this.transform.parent.GetComponent<SetupTileScript>().occupied = false;
					this.transform.parent = slot;
					transform.position = new Vector3(slot.transform.position.x, 5.0f, slot.position.z);
					slot.GetComponent<SetupTileScript>().occupied = true;
				}
			}

			else
			{
				if(slot.GetComponent<SetupTileScript>().tt == SetupTileScript.TileType.ONFIELD)
				{
					Debug.Log (playerSetup.playerPieces.Contains(gameObject));
					if(playerSetup.playerPieces.Contains(gameObject))
					{
						this.transform.parent.GetComponent<SetupTileScript>().occupied = false;
						this.transform.parent = slot;
						transform.position = new Vector3(slot.transform.position.x, 5.0f, slot.position.z);
						slot.GetComponent<SetupTileScript>().occupied = true;
					}

					else
						transform.position = playerSetup.prevPosition;

				}

				else if(playerSetup.playerPieces.Contains(gameObject))
				{
					playerSetup.playerPieces.Remove(gameObject);

					this.transform.parent.GetComponent<SetupTileScript>().occupied = false;
					this.transform.parent = slot;
					transform.position = new Vector3(slot.transform.position.x, 5.0f, slot.position.z);
					slot.GetComponent<SetupTileScript>().occupied = true;
				}
			}


			//Special if dragging object to field-type tile:

			Debug.Log(playerSetup.playerPieces.Count);




		}
		else
		
		{

			//transform.position = PlayerSetup.prevPosition;
			transform.position = playerSetup.prevPosition;

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
