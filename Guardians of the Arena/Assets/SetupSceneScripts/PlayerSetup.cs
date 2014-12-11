using UnityEngine;
using System.Collections;

public class PlayerSetup : MonoBehaviour {

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
	public int maxUnitCount = 10;

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
				Vector3 position = new Vector3((10 * i), 0, (10 * j)-25);//each square is 10 points away from each other
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
	
		//Create five page objects for storing unit positions
		pages = new Page[5];
		for(int i = 0; i < 5; i++)
		{
			pages[i] = new Page();
		}

		//The stupd screen defaults to the first page (index "0" in the pages array, "1" on the server)
		activePage = 0;
		gp.returnSocket().SendTCPPacket("getBoardData\\1\\" + gp.playerName);
	}

	// Update is called once per frame
	void Update () {	
	}
}
