using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {

	private int xTiles = 16;
	private int yTiles = 16;
	private GameObject[,] tiles;

	public GameObject tile;

	void Start () {

		tiles = new GameObject[xTiles, yTiles];

		int startingY = 0;
		int startingX = 0;
		
		
		
		//Tile Creation
		for (int i = 0; i < xTiles; i++)
		{
			for (int j = 0; j < yTiles; j++)
			{
				Vector3 position = new Vector3((10 * i), 0, (10 * j));
				GameObject newtile = (GameObject)Instantiate(tile,
				                                             position, 
				                                             new Quaternion(0,0,0,0));

				tiles[i,j] = newtile;
				//newtile.transform.parent = this.transform;
			}
		}

		//loop through the array of tiles and assign neighbors accordingly
		for (int i = 0; i < xTiles; i++) 
		{
			for (int j = 0; j < yTiles; j++)
			{
				TileScript script = tiles[i, j].GetComponent<TileScript>();

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
	
	void Update () {
	
	}
}
