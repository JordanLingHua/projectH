using UnityEngine;
using System.Collections;
using System.Collections.Generic;//allows use of list, etc.  


public class GameManager : MonoBehaviour {

	public GameObject TilePrefab;

	List <List<Tile>> map = new List<List<Tile>>();

	//size of entire map
	public int mapSize = 11;//set temporary as 11 for now.  modify if increase nxn
	 

	// Use this for initialization
	void Start () {
		generateMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void generateMap()
	{
		//reset map to null
		map = new List<List<Tile>>();

		//2D array traversal
		for(int i = 0; i < mapSize; i++)
		{
			for(int j = 0; j < mapSize; j++)
			{

			}
		}

	}

}
