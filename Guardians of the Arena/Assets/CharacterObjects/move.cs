using UnityEngine;
using System.Collections;


public class move : MonoBehaviour {

	GameProcess gp;
	bool pieceMoved;
	bool onField;
	public PlayerSetup playerSetup;
	SetupTileScript oldScript;
	//drag and drop
	private Ray ray = new Ray();
	private RaycastHit hit = new RaycastHit();

	//Needed to avoid getting the dragged object stuck when mouse is released up
	private bool isTouched = false;

	GameObject nearestTile;

	//slot tag's name 
	public string tagName = "Tile";

	void Start()
	{
		gp = GameObject.Find ("GameProcess").GetComponent<GameProcess>();
		pieceMoved = false;
		playerSetup = GameObject.Find ("PlayerSetup").GetComponent<PlayerSetup>(); 
	}

	void OnMouseDown()
	{
		playerSetup.prevPosition = transform.position;
		isTouched = true;
		pieceMoved = false;
	}
	
	void OnMouseUp()
	{
		Debug.Log (playerSetup.activePage);
		isTouched = false;
		oldScript = this.gameObject.GetComponentInParent<SetupTileScript> ();
		nearestTile = findNearestTile();
		//if there is a slot
		Debug.Log (nearestTile);
		if (nearestTile != null && !nearestTile.GetComponent<SetupTileScript>().occupied)
		{
			//guardian and soulstone must be kept on the field
			if((gameObject.GetComponent<Unit>().unitType == 10 || gameObject.GetComponent<Unit>().unitType == 11) &&
				nearestTile.GetComponent<SetupTileScript>().tt == SetupTileScript.TileType.ONFIELD)
			{
				pieceMoved = true;
				onField = true;
			}

			//if there is room on the board for units...
			else if(playerSetup.pages[playerSetup.activePage].onBoardPieces.Count < playerSetup.boardCapacity)
			{
				//...and the unit is being placed on the field...
				if(nearestTile.GetComponent<SetupTileScript>().tt == SetupTileScript.TileType.ONFIELD)
				{
					//...only add the unit if it is not already on the board
					if(!playerSetup.pages[playerSetup.activePage].onBoardPieces.Contains(this.gameObject))
					{
						playerSetup.pages[playerSetup.activePage].offBoardPieces.Remove(this.gameObject);
						playerSetup.pages[playerSetup.activePage].onBoardPieces.Add(this.gameObject);
					}

					pieceMoved = true;
					onField = true;
				}

			
				else //unit is being placed off the field
				{
					//if the unit is being moved off the field (rather than being adjusted off the board)
					if(playerSetup.pages[playerSetup.activePage].onBoardPieces.Contains(gameObject))
					{
						playerSetup.pages[playerSetup.activePage].offBoardPieces.Add(this.gameObject);
						playerSetup.pages[playerSetup.activePage].onBoardPieces.Remove(this.gameObject);
					}

					pieceMoved = true;
					onField = false;
				}
			}

			else //board is at max capacity
			{
				if(nearestTile.GetComponent<SetupTileScript>().tt == SetupTileScript.TileType.ONFIELD)
				{
					//the piece can only be moved in the case that it is already on the board and is being adjusted
					if(playerSetup.pages[playerSetup.activePage].onBoardPieces.Contains(gameObject))
					{
						pieceMoved = true;
						onField = true;
					}
				}

				else //offfield - player is moving a piece when the board is full
				{
					//the piece being moved off the field
					if(playerSetup.pages[playerSetup.activePage].onBoardPieces.Contains(gameObject))
					{
						playerSetup.pages[playerSetup.activePage].offBoardPieces.Add(this.gameObject);
						playerSetup.pages[playerSetup.activePage].onBoardPieces.Remove(this.gameObject);
					}

					pieceMoved = true;
					onField = false;
				}
			}

			Debug.Log(playerSetup.pages[playerSetup.activePage].onBoardPieces.Count);
		}

		if (pieceMoved) 
		{
			int oldX = oldScript.x;
			int oldY = oldScript.y;

			int newX = nearestTile.GetComponent<SetupTileScript>().x;
			int newY = nearestTile.GetComponent<SetupTileScript>().y;

			int unitType = this.gameObject.GetComponent<Unit>().unitType;

			gp.returnSocket().SendTCPPacket("movePiece\\"+ gp.playerName + "\\" + playerSetup.activePage + "\\"
			                                + unitType + "\\" + oldX + "\\" + oldY + "\\" + newX + "\\" + newY + "\\" + onField);

			this.gameObject.GetComponentInParent<SetupTileScript>().occupied = false;
			this.gameObject.transform.parent = nearestTile.transform;
			transform.position = new Vector3(nearestTile.transform.position.x, 5.0f, nearestTile.transform.position.z);
			nearestTile.GetComponent<SetupTileScript>().occupied = true;
		}
		else		
		{
			transform.position = playerSetup.prevPosition;
		}
	}

	GameObject findNearestTile()
	{
		float nearestDistanceTile = Mathf.Infinity;
		GameObject[] taggedGameObjects = GameObject.FindGameObjectsWithTag(tagName);//<---KEY compares all objects near with that tag name!  Works perfectly for a set of tiles!!!
		GameObject nearestObj = null;
		foreach (GameObject obj in taggedGameObjects)
		{
			Vector3 objectPos = obj.transform.position;
			float distanceTile = (objectPos - transform.position).sqrMagnitude;
			
			if (distanceTile < nearestDistanceTile)
			{
				nearestObj = obj;
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
