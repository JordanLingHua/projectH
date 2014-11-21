using UnityEngine;
using System.Collections;

public class PlayerSetup : MonoBehaviour {


	//Essentially a class containing the Physical Player itself. 
	//Should be called "player profile" actually.  

	//Out for now:
	//public int money_Limit = 10;
	//public int used_money;//set to 0 in constructor.  Then increment/decrement later

	public enum placement{ONFIELD, OFFFIELD};
	public GameProcess gp;


	public GameObject[] providedPieces;
	public GameObject UnitOne;
	public GameObject UnitTwo;
	public GameObject UnitThree;
	public GameObject UnitFour;
	public GameObject UnitFive;
	public GameObject UnitSix;
	public GameObject UnitSeven;
	public GameObject UnitEight;
	public GameObject UnitNine;
	public GameObject UnitTen;
	public GameObject UnitEleven;
	public GameObject cp;


	public int boardCapacity = 10;
	//10 //Leave out 10 for now since the player can drag all the pieces onto the board in that case

	//Add to this array everytime a piece is dropped onto a board
	public ArrayList playerPieces;


	//These special tiles seperate from the half of the game board (tiles) stores 
	//the units that are dragged off the game board.  
	public GameObject[,] unit_storage_tiles;
	private int xStorage_Tiles = 11;
	private int yStorage_Tiles = 2;
	public GameObject storage_tile;
	
	//We only need 1 prevPosition because we are only moving one piece at a time.  
	public Vector3 prevPosition;

	private int xTiles = 11;
	private int yTiles = 4;
	public GameObject[,] tiles;
	public GameObject tile;

	public int activePage;

	public Page[] pages;

	public void deleteAllUnits(){
		foreach (GameObject piece in GameObject.FindGameObjectsWithTag("Unit")) 
		{
			piece.GetComponentInParent<SetupTileScript>().occupied = false;
			GameObject.Destroy (piece);
		}
	}
	
	//Unit Type and Unit ID are the exact same thing
	public GameObject addUnit(placement placementType, int x, int y,int type){//, int unitID){

		SetupTileScript placeTile;
		if(placementType == placement.OFFFIELD)
			 placeTile = unit_storage_tiles[x,y].GetComponent<SetupTileScript>();
		else
			placeTile = tiles[x,y].GetComponent<SetupTileScript>();

		placeTile.GetComponent<SetupTileScript>().occupied = true;
		GameObject unit; 
		
		switch(type){
		case 1:
			unit = (GameObject)Instantiate(UnitOne, 
			                               new Vector3(placeTile.transform.position.x, 5.0f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 2:
			unit = (GameObject)Instantiate(UnitTwo, 
			                               new Vector3(placeTile.transform.position.x, 5.0f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 3:
			unit = (GameObject)Instantiate(UnitThree, 
			                               new Vector3(placeTile.transform.position.x, 5.0f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 4:
			unit = (GameObject)Instantiate(UnitFour, 
			                               new Vector3(placeTile.transform.position.x, 5.0f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 5:
			unit = (GameObject)Instantiate(UnitFive, 
			                               new Vector3(placeTile.transform.position.x, 5.0f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 6:
			unit = (GameObject)Instantiate(UnitSix, 
			                               new Vector3(placeTile.transform.position.x, 5.0f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 7:
			unit = (GameObject)Instantiate(UnitSeven, 
			                               new Vector3(placeTile.transform.position.x, 5.0f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 8:
			unit = (GameObject)Instantiate(UnitEight, 
			                               new Vector3(placeTile.transform.position.x, 5.0f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 9:
			unit = (GameObject)Instantiate(UnitNine, 
			                               new Vector3(placeTile.transform.position.x, 5.0f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 10:
			unit = (GameObject)Instantiate(UnitTen, 
			                               new Vector3(placeTile.transform.position.x, 5.0f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 11:
			unit = (GameObject)Instantiate(UnitEleven, 
			                               new Vector3(placeTile.transform.position.x, 5.0f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
			
		default:
			unit = (GameObject)Instantiate(cp, 
			                               new Vector3(placeTile.transform.position.x, 5.0f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		}

		unit.transform.parent = placeTile.transform;
		return unit;
	}

	void Start () {
		gp = GameObject.Find ("GameProcess").GetComponent<GameProcess> ();

		//Tile Creation
		tiles = new GameObject[xTiles, yTiles];
		for (int i = 0; i < xTiles; i++)
		{
			for (int j = 0; j < yTiles; j++)
			{
				Vector3 position = new Vector3((10 * i), 0, (10 * j));//each square is 10 points away from each other
				//-60 offsets all these tiles to appear below the game board along the z direction
				GameObject newtile = (GameObject)Instantiate(tile,
				                                             position, 
				                                             new Quaternion(0,0,0,0));


				newtile.AddComponent("SetupTileScript");
				newtile.GetComponent<SetupTileScript>().tt = SetupTileScript.TileType.ONFIELD;			
				tiles[i,j] = newtile;
				newtile.transform.parent = this.transform;
			}
		}
		
		//loop through the array of tiles and assign neighbors accordingly
		for (int i = 0; i < xTiles; i++) 
		{
			for (int j = 0; j < yTiles; j++)
			{
				SetupTileScript script = tiles[i, j].GetComponent<SetupTileScript>();
				
				//set tile id e.g. 5,2
				script.x = i;
				script.y = j;
				
				if (i != 0)
				{
					script.down = tiles[i - 1, j];
				}
				if (i != xTiles - 1)
				{
					script.up = tiles[i + 1, j];
				}
				if (j != 0)
				{
					script.right = tiles[i, j - 1];
				}
				if (j != yTiles - 1)
				{
					script.left = tiles[i, j + 1];
				}
			}
		}


		//_________Create the unit_storage_tiles's tiles:_________  

		//Tile Creation
		unit_storage_tiles = new GameObject[xStorage_Tiles, yStorage_Tiles];
		for (int i = 0; i < xStorage_Tiles; i++)
		{
			for (int j = 0; j < yStorage_Tiles; j++)
			{
				Vector3 position = new Vector3((10 * i), 0, (10 * j)-20);//each square is 10 points away from each other
				//-20 offsets all these tiles to appear below the game board along the z direction

				//NOTICE how a global variable ("public GameObject tile" in this case) has to have prefab attached to it in editor
				//THEN, to create a game object of that prefab, you must use the following syntax below:
				GameObject newtile = (GameObject)Instantiate(storage_tile,
				                                             position, 
				                                             new Quaternion(0,0,0,0));

				
				newtile.AddComponent("SetupTileScript");


				newtile.GetComponent<SetupTileScript>().tt = SetupTileScript.TileType.OFFFIELD;


				unit_storage_tiles[i,j] = newtile;


				newtile.transform.parent = this.transform;
			}
		}
		
		//loop through the array of tiles and assign neighbors accordingly
		for (int i = 0; i < xStorage_Tiles; i++) 
		{
			for (int j = 0; j < yStorage_Tiles; j++)
			{
				SetupTileScript script = unit_storage_tiles[i, j].GetComponent<SetupTileScript>();
				
				//set tile id e.g. 5,2
				script.x = i;
				script.y = j;
				
				if (i != 0)
				{
					script.down = unit_storage_tiles[i - 1, j];
				}
				if (i != xStorage_Tiles - 1)
				{
					script.up = unit_storage_tiles[i + 1, j];
				}
				if (j != 0)
				{
					script.right = unit_storage_tiles[i, j - 1];
				}
				if (j != yStorage_Tiles - 1)
				{
					script.left = unit_storage_tiles[i, j + 1];
				}
			}
		}

		//_________________________________________________________


		//THIS IS SUBJECT TO CHANGE.  WHEN YOU CHANGE THIS, MAKE SURE YOU CHANGE THE ARRAY SIZE ABOVE!!!!
		//providedPieces = new GameObject[11];
		providedPieces = new GameObject[10];


		//THIS IS SUBJECT TO CHANGE.  WHEN YOU CHANGE THIS, MAKE SURE YOU CHANGE THE ARRAY SIZE ABOVE!!!!
		//The 4th arg in addUnit defines the unitType
//		providedPieces[0] = addUnit (placement.OFFFIELD, 0,0,1);
//		providedPieces[1] = addUnit (placement.OFFFIELD, 1,0,2);
//		providedPieces[2] = addUnit (placement.OFFFIELD, 2,0,3);
//		providedPieces[3] = addUnit (placement.OFFFIELD, 3,0,3);
//		providedPieces[4] = addUnit (placement.OFFFIELD, 4,0,7);
//		providedPieces[5] = addUnit (placement.OFFFIELD, 5,0,7);
//		providedPieces[6] = addUnit (placement.OFFFIELD, 6,0,7);
//		providedPieces[7] = addUnit (placement.OFFFIELD, 7,0,8);
//		providedPieces[8] = addUnit (placement.ONFIELD, 10,3,10);
//		providedPieces[9] = addUnit (placement.ONFIELD, 9,3,11);

		//Add a move script to each, just for this scene though (this scene should be the only scene that calls this script)
		//Position each of the pieces just made onto the board: 
		//for(int i = 0; i < providedPieces.Length - 1; i++)
//		for(int i = 0; i < providedPieces.Length; i++)
//		{
//			providedPieces[i].AddComponent("move"); 
//		}

		playerPieces = new ArrayList(boardCapacity);

		//CHANGE the indices of the providedPieces in 

//		playerPieces.Add (providedPieces[8]);
//		playerPieces.Add (providedPieces[9]);


		activePage = 0;

		pages = new Page[5];
		for(int i = 0; i < 5; i++)
		{
			pages[i] = new Page();
		}
	
//		pages[0].onBoardPieces.Add(providedPieces[8]);
//		pages[0].onBoardPieces.Add(providedPieces[9]);

		gp.returnSocket().SendTCPPacket("getBoardData\\1\\" + gp.playerName);
	}


//	void updatePageModifier()
//	{
//		switch(activePage)
//		{
//		case 1:
//
//			pages[0].modified = true;
//			break;
//		case 2:
//			pages[1].modified = true;
//			break;
//		case 3:
//			pages[2].modified = true;
//			break;
//		case 4:
//			pages[3].modified = true;
//			break;
//		case 5:
//			pages[4].modified = true;
//			break;
//			
//		}
//	}

	//Drag and drop
	// Update is called once per frame
	void Update () {


	
	}
}
