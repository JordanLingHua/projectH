using UnityEngine;
using System.Collections;


public class move : MonoBehaviour {
	AudioManager am;
	GameProcess gp;
	GameObject oldTile;
	GameObject currentTile;
	bool pieceMoved;
	bool onField;
	bool trackMouse;
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

	//for error message
	GameObject popUpText;

	void Start()
	{
		am = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		popUpText = GameObject.Find ("ErrorPopUpText");
		gp = GameObject.Find ("GameProcess").GetComponent<GameProcess>();
		pieceMoved = false;
		trackMouse = false;
		playerSetup = GameObject.Find ("PlayerSetup").GetComponent<PlayerSetup>(); 
	}

	void OnMouseDown()
	{
		//Save the last position of the piece in case in needs to get snapped back
		playerSetup.prevPosition = transform.position;
		isTouched = true;
		trackMouse = true;

	}

	public void showErrorMessage(string error){
		GUI.depth = -1;
		Vector3 textPos = new Vector3((Screen.width*0.08f)/Screen.width,(Screen.height-((float)Screen.height*0.05f))/Screen.height,0);
		
		//TODO: Error with different error text overlapping
		if (GameObject.Find ("ErrorPopUpText(Clone)")!=null){
			Destroy (GameObject.Find ("ErrorPopUpText(Clone)"));
		}
		GameObject text = (GameObject) Instantiate(popUpText,textPos,Quaternion.identity);
		text.GetComponent<ErrorPopUpTextScript> ().StartCoroutine (text.GetComponent<ErrorPopUpTextScript> ().showText (error, Color.red));
	}
	
	void OnMouseUp()
	{
		currentTile.renderer.material.shader = Shader.Find ("Toon/Basic");
		trackMouse = false;
		pieceMoved = false;
		isTouched = false;
		oldScript = this.gameObject.GetComponentInParent<SetupTileScript> ();
		nearestTile = findNearestTile();

		//if the tile closest to where the piece was dropped exists and is unoccupied
		if (nearestTile != null && !nearestTile.GetComponent<SetupTileScript>().occupied)
		{
			//guardian and soulstone must be kept on the field
			if(gameObject.GetComponent<Unit>().unitType == 10 || gameObject.GetComponent<Unit>().unitType == 11)
			{
				if (nearestTile.GetComponent<SetupTileScript>().tt == SetupTileScript.TileType.ONFIELD)
				{
					pieceMoved = true;
					onField = true;
				}else {
					if (gameObject.GetComponent<Unit>().unitType == 10){
						showErrorMessage("Guardian must be on the field");
					}else if(gameObject.GetComponent<Unit>().unitType == 11) {
						showErrorMessage("Soulstone must be on the field");
					}
					am.playErrorSFX();
				}
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
					}else{
						showErrorMessage("Board at maximum capacity");
						am.playErrorSFX();
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
			//Debug.Log(playerSetup.pages[playerSetup.activePage].onBoardPieces.Count);
		}

		//if a piece was moved to a new valid tile (i.e. didn't snap back to old position)...
		if (pieceMoved) 
		{
			int oldX = oldScript.x;
			int oldY = oldScript.y;

			int newX = nearestTile.GetComponent<SetupTileScript>().x;
			int newY = nearestTile.GetComponent<SetupTileScript>().y;

			int unitType = this.gameObject.GetComponent<Unit>().unitType;

			//send the server movePiece\\playerName\\activePageNumber\\unitType\\oldX\\oldY\\newX\\newY\\onOrOffField
			gp.returnSocket().SendTCPPacket("movePiece\\" + gp.playerName + "\\" + (playerSetup.activePage + 1) + "\\"
			                                + unitType + "\\" + oldX + "\\" + oldY + "\\" + newX + "\\" + newY + "\\" + onField);

			//set the current tile to unoccupied (the piece is moving away from this tile)
			this.gameObject.GetComponentInParent<SetupTileScript>().occupied = false;

			//attach the new tile's transform to be the parent of the unit
			this.gameObject.transform.parent = nearestTile.transform;

			//move the unit to it's new tile and set that tile to occupied
			transform.position = new Vector3(nearestTile.transform.position.x, 5.0f, nearestTile.transform.position.z);
			nearestTile.GetComponent<SetupTileScript>().occupied = true;
		}

		//this happens if the piece move attempt was illegal/not valid, the piece is returned to it's original position
		else		
		{
			transform.position = playerSetup.prevPosition;

		}
	}

	//returns the tile nearest to where the unit is dropped
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
		
		if (isTouched)
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				transform.position = new Vector3(hit.point.x, 5.0f, hit.point.z);
			}
		}

		if (trackMouse) 
		{
			if (oldTile != null)
				oldTile.renderer.material.shader = Shader.Find ("Toon/Basic");

			currentTile = findNearestTile();
			this.gameObject.transform.position = new Vector3(currentTile.transform.position.x, currentTile.transform.position.y + 5f, currentTile.transform.position.z);
			currentTile.renderer.material.shader = Shader.Find ("Toon/Lighted");



			oldTile = currentTile;



		}


	}
}
