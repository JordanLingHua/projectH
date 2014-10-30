using UnityEngine;
using System.Collections;

public class PlayerSetup : MonoBehaviour {


	//Essentially a class containing the Physical Player itself. 
	//Should be called "player profile" actually.  

	public int purchasingLimit = 10;

	public GameObject[,] providedPieces;

	//Add to this array everytime a piece is dropped onto a board
	public GameObject[,] playerPieces;

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
	
	}


	//Drag and drop
	// Update is called once per frame
	void Update () {


	
	}
}
