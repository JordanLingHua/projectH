using UnityEngine;
using System.Collections;

public class PlayerSetup : MonoBehaviour {


	//Essentially a class containing the Physical Player itself. 
	//Should be called "player profile" actually.  

	public int money_Limit = 10;
	public int used_money;//set to 0 in constructor.  Then increment/decrement later

	//public GameObject[,] providedPieces;
	//public GameObject[] providedPieces;
	public PieceStruct[] providedPieces;



	public int max_num_pieces = 15;
	public int num_pieces_used;//set to 0 in constructor.  Then increment/decrement later



	//Add to this array everytime a piece is dropped onto a board
	//public GameObject[] playerPieces;
	//public GameObject[,] playerPieces;
	public PieceStruct[] playerPieces;




	private int xTiles = 16;
	private int yTiles = 5;
	public GameObject[,] tiles;
	public GameObject tile;
	
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
		providedPieces[0] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitOnePrefab")),new UnitOne());
		providedPieces[1] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitTwoPrefab")),new UnitTwo());
		providedPieces[2] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitThreePrefab")),new UnitThree());
		providedPieces[3] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitThreePrefab")),new UnitThree());
		providedPieces[4] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitFourPrefab")),new UnitFour());
		providedPieces[5] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitFivePrefab")),new UnitFive());
		providedPieces[6] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitSixPrefab")),new UnitSix());
		providedPieces[7] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitSevenPrefab")),new UnitSeven());
		providedPieces[8] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitSevenPrefab")),new UnitSeven());
		providedPieces[9] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitSevenPrefab")),new UnitSeven());
		providedPieces[10] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitEightPrefab")),new UnitEight());
		providedPieces[11] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitNinePrefab")),new UnitNine());
		providedPieces[12] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitNinePrefab")),new UnitNine());
		providedPieces[13] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitTenPrefab")),new UnitTen());
		providedPieces[14] = new PieceStruct((GameObject)Instantiate(Resources.Load("UnitElevenPrefab")),new UnitEleven());




		//in progress....


		//while 
		//for(int i = 0; i < 15; i++)
		//{
		//
		//}



	
	}


	//Drag and drop
	// Update is called once per frame
	void Update () {


	
	}
}
