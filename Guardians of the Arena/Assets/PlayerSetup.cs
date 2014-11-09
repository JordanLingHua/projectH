using UnityEngine;
using System.Collections;

public class PlayerSetup : MonoBehaviour {


	//Essentially a class containing the Physical Player itself. 
	//Should be called "player profile" actually.  

	//Out for now:
	//public int money_Limit = 10;
	//public int used_money;//set to 0 in constructor.  Then increment/decrement later

	public enum placement{ONFIELD, OFFFIELD};


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


	public int boardCapacity = 5;


	//Add to this array everytime a piece is dropped onto a board
	public ArrayList playerPieces;


	//These special tiles seperate from the half of the game board (tiles) stores 
	//the units that are dragged off the game board.  
	public GameObject[,] unit_storage_tiles;
	private int xStorage_Tiles = 16;
	private int yStorage_Tiles = 2;
	public GameObject storage_tile;


	//
	//We only need 1 prevPosition because we are only moving one piece at a time.  
	//public static Vector3 prevPosition;//use this in move.cs
	public Vector3 prevPosition;
	//


	private int xTiles = 16;
	private int yTiles = 5;
	public GameObject[,] tiles;
	public GameObject tile;
	//public GameObject ball;//test for prefab instantiation 1




	GameObject addUnit(placement placementType, int x, int y,int type, int unitID){

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
//		placeTile.objectOccupyingTile = unit;

		return unit;
	}







	void Start () {
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



		//Initialize:  

		//Pieces creation (add pieces into the array above)

		//There should be 15 pieces total.  Look at the unit design spreadsheet!  
		//GameObject u1_1 = (GameObject)Instantiate(Resources.Load("UnitOnePrefab"));
		//GameObject u2_1 = (GameObject)Instantiate(Resources.Load("UnitTwoPrefab"));
		//GameObject u3_1 = (GameObject)Instantiate(Resources.Load("UnitThreePrefab"));

		providedPieces = new GameObject[11];

		//GameObject temp = (GameObject)Instantiate(ball, new Vector3(0, 0, 0), new Quaternion(0,0,0,1));//test for prefab instantiation 1
		//providedPieces[0] = temp;//test for prefab instantiation 1


//		providedPieces[0] = (GameObject)Instantiate(unit1, new Vector3(0, 5, -10), new Quaternion(0,0,0,1));
//		providedPieces[1] = (GameObject)Instantiate(unit2, new Vector3(10, 5, -10), new Quaternion(0,0,0,1));
//		providedPieces[2] = (GameObject)Instantiate(unit3, new Vector3(20, 5, -10), new Quaternion(0,0,0,1));
//		providedPieces[3] = (GameObject)Instantiate(unit3, new Vector3(30, 5, -10), new Quaternion(0,0,0,1));
//		providedPieces[4] = (GameObject)Instantiate(unit4, new Vector3(40, 5, -10), new Quaternion(0,0,0,1));
//		providedPieces[5] = (GameObject)Instantiate(unit5, new Vector3(50, 5, -10), new Quaternion(0,0,0,1));
//		providedPieces[6] = (GameObject)Instantiate(unit6, new Vector3(60, 5, -10), new Quaternion(0,0,0,1));
//		providedPieces[7] = (GameObject)Instantiate(unit7, new Vector3(70, 5, -10), new Quaternion(0,0,0,1));
//		providedPieces[8] = (GameObject)Instantiate(unit7, new Vector3(80, 5, -10), new Quaternion(0,0,0,1));
//		providedPieces[9] = (GameObject)Instantiate(unit7, new Vector3(90, 5, -10), new Quaternion(0,0,0,1));
//		providedPieces[10] = (GameObject)Instantiate(unit8, new Vector3(0, 5, -20), new Quaternion(0,0,0,1));
//		providedPieces[11] = (GameObject)Instantiate(unit9, new Vector3(10, 5, -20), new Quaternion(0,0,0,1));
//		providedPieces[12] = (GameObject)Instantiate(unit9, new Vector3(20, 5, -20), new Quaternion(0,0,0,1));
//		providedPieces[13] = (GameObject)Instantiate(unit10, new Vector3(30, 5, -20), new Quaternion(0,0,0,1));
//		providedPieces[14] = (GameObject)Instantiate(unit11, new Vector3(40, 5, -20), new Quaternion(0,0,0,1));


		

		providedPieces[0] = addUnit (placement.OFFFIELD, 0,0,1,1);
		providedPieces[1] = addUnit (placement.OFFFIELD, 1,0,2,2);
		providedPieces[2] = addUnit (placement.OFFFIELD, 2,0,3,3);
		providedPieces[3] = addUnit (placement.OFFFIELD, 3,0,3,4);
		providedPieces[4] = addUnit (placement.OFFFIELD, 4,0,4,5);
		providedPieces[5] = addUnit (placement.OFFFIELD, 5,0,5,6);
		providedPieces[6] = addUnit (placement.OFFFIELD, 6,0,5,7);
		providedPieces[7] = addUnit (placement.OFFFIELD, 7,0,5,8);
		providedPieces[8] = addUnit (placement.OFFFIELD, 8,0,6,9);
		providedPieces[9] = addUnit (placement.ONFIELD, 10,3,7,10);
		providedPieces[10] = addUnit (placement.ONFIELD, 9,3,8,11);
		//providedPieces[11] = addUnit (1,1,16,12);
		//providedPieces[12] = addUnit (2,1,16,13);
		//providedPieces[13] = addUnit (3,1,16,14);
		//providedPieces[14] = addUnit (4,1,16,15);




		
		
		//guardian and soulstone
		//addUnit (7,15,19, 18);
		//addUnit (8,15,20, 19);
		
		//Add a move script to each, just for this scene though (this scene should be the only scene that calls this script)
		//Position each of the pieces just made onto the board: 
		for(int i = 0; i < providedPieces.Length - 1; i++)
		{
			providedPieces[i].AddComponent("move"); 
		}

		playerPieces = new ArrayList(boardCapacity);
		playerPieces.Add (providedPieces[9]);
		playerPieces.Add (providedPieces[10]);

	
	}




	//Drag and drop
	// Update is called once per frame
	void Update () {


	
	}
}
