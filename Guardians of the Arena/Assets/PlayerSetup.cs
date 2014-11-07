using UnityEngine;
using System.Collections;

public class PlayerSetup : MonoBehaviour {


	//Essentially a class containing the Physical Player itself. 
	//Should be called "player profile" actually.  

	public int money_Limit = 10;
	public int used_money;//set to 0 in constructor.  Then increment/decrement later


	public GameObject[] providedPieces;
	public GameObject unit1;
	public GameObject unit2;
	public GameObject unit3;
	public GameObject unit4;
	public GameObject unit5;
	public GameObject unit6;
	public GameObject unit7;
	public GameObject unit8;
	public GameObject unit9;
	public GameObject unit10;
	public GameObject unit11;


	public int max_num_pieces = 15;
	public int num_pieces_used;//set to 0 in constructor.  Then increment/decrement later



	//Add to this array everytime a piece is dropped onto a board
	public GameObject[] playerPieces;




	private int xTiles = 16;
	private int yTiles = 5;
	public GameObject[,] tiles;
	public GameObject tile;
	//public GameObject ball;//test for prefab instantiation 1
	
	void Start () {
		//Tile Creation
		tiles = new GameObject[xTiles, yTiles];
		for (int i = 0; i < xTiles; i++)
		{
			for (int j = 0; j < yTiles; j++)
			{
				Vector3 position = new Vector3((10 * i), 0, (10 * j));
				GameObject newtile = (GameObject)Instantiate(tile,
				                                             position, 
				                                             new Quaternion(0,0,0,0));

				newtile.AddComponent("SetupTileScript");
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



		//Initialize:  
		used_money = 0;
		num_pieces_used = 0;


		//Pieces creation (add pieces into the array above)

		//There should be 15 pieces total.  Look at the unit design spreadsheet!  
		//GameObject u1_1 = (GameObject)Instantiate(Resources.Load("UnitOnePrefab"));
		//GameObject u2_1 = (GameObject)Instantiate(Resources.Load("UnitTwoPrefab"));
		//GameObject u3_1 = (GameObject)Instantiate(Resources.Load("UnitThreePrefab"));

		providedPieces = new GameObject[15];

		//GameObject temp = (GameObject)Instantiate(ball, new Vector3(0, 0, 0), new Quaternion(0,0,0,1));//test for prefab instantiation 1
		//providedPieces[0] = temp;//test for prefab instantiation 1


		providedPieces[0] = (GameObject)Instantiate(unit1, new Vector3(0, 0, -10), new Quaternion(0,0,0,1));
		providedPieces[1] = (GameObject)Instantiate(unit2, new Vector3(10, 0, -10), new Quaternion(0,0,0,1));
		providedPieces[2] = (GameObject)Instantiate(unit3, new Vector3(20, 0, -10), new Quaternion(0,0,0,1));
		providedPieces[3] = (GameObject)Instantiate(unit3, new Vector3(30, 0, -10), new Quaternion(0,0,0,1));
		providedPieces[4] = (GameObject)Instantiate(unit4, new Vector3(40, 0, -10), new Quaternion(0,0,0,1));
		providedPieces[5] = (GameObject)Instantiate(unit5, new Vector3(50, 0, -10), new Quaternion(0,0,0,1));
		providedPieces[6] = (GameObject)Instantiate(unit6, new Vector3(60, 0, -10), new Quaternion(0,0,0,1));
		providedPieces[7] = (GameObject)Instantiate(unit7, new Vector3(70, 0, -10), new Quaternion(0,0,0,1));
		providedPieces[8] = (GameObject)Instantiate(unit7, new Vector3(80, 0, -10), new Quaternion(0,0,0,1));
		providedPieces[9] = (GameObject)Instantiate(unit7, new Vector3(90, 0, -10), new Quaternion(0,0,0,1));
		providedPieces[10] = (GameObject)Instantiate(unit8, new Vector3(0, 0, -20), new Quaternion(0,0,0,1));
		providedPieces[11] = (GameObject)Instantiate(unit9, new Vector3(10, 0, -20), new Quaternion(0,0,0,1));
		providedPieces[12] = (GameObject)Instantiate(unit9, new Vector3(20, 0, -20), new Quaternion(0,0,0,1));
		providedPieces[13] = (GameObject)Instantiate(unit10, new Vector3(30, 0, -20), new Quaternion(0,0,0,1));
		providedPieces[14] = (GameObject)Instantiate(unit11, new Vector3(40, 0, -20), new Quaternion(0,0,0,1));

		/*
		providedPieces[0] = (GameObject)Instantiate(Resources.Load("UnitOnePrefab"), new Vector3(0, 0, 0), new Quaternion(0,0,0,1));
		providedPieces[1] = (GameObject)Instantiate(Resources.Load("UnitTwoPrefab"));
		providedPieces[2] = (GameObject)Instantiate(Resources.Load("UnitThreePrefab"));
		providedPieces[3] = (GameObject)Instantiate(Resources.Load("UnitThreePrefab"));
		providedPieces[4] = (GameObject)Instantiate(Resources.Load("UnitFourPrefab"));
		providedPieces[5] = (GameObject)Instantiate(Resources.Load("UnitFivePrefab"));
		providedPieces[6] = (GameObject)Instantiate(Resources.Load("UnitSixPrefab"));
		providedPieces[7] = (GameObject)Instantiate(Resources.Load("UnitSevenPrefab"));
		providedPieces[8] = (GameObject)Instantiate(Resources.Load("UnitSevenPrefab"));
		providedPieces[9] = (GameObject)Instantiate(Resources.Load("UnitSevenPrefab"));
		providedPieces[10] = (GameObject)Instantiate(Resources.Load("UnitEightPrefab"));
		providedPieces[11] = (GameObject)Instantiate(Resources.Load("UnitNinePrefab"));
		providedPieces[12] = (GameObject)Instantiate(Resources.Load("UnitNinePrefab"));
		providedPieces[13] = (GameObject)Instantiate(Resources.Load("UnitTenPrefab"));
		providedPieces[14] = (GameObject)Instantiate(Resources.Load("UnitElevenPrefab"));
		*/


		/*
		//Add a move script to each, just for this scene though (this scene should be the only scene that calls this script)
		//Position each of the pieces just made onto the board: 
		for(int i = 0; i < 15; i++)
		{
			providedPieces[i].AddComponent("move"); 



		}


		//NOTE:  position of each tile is ---->  Vector3 position = new Vector3((10 * i), 0, (10 * j));

		//int myX, myY, myZ = 0;
		for(int i = 0; i < 15; i++)
		{
			//providedPieces[i].transform.position.Set (myX, myY, myZ);


			
		}
		*/


		providedPieces[0].transform.position.Set (80,50,25);
		//providedPieces[1].transform.position.Set (160,0,-10);

		 


		//After this, in the update loop, allow the player to rearrange the pieces there.  


		//in progress....


		//while 
		//for(int i = 0; i < 15; i++)
		//{
		//
		//}



	
	}


	//Drag and drop
	//highlight the square that the curser is in.  
	// Update is called once per frame
	void Update () {


		//highlight the square that the curser is in. 



	
	}





















}
