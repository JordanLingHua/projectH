using UnityEngine;
using System.Collections;


//USING TileManager as the MAIN function of the game, since the tiles are instantiated HERE, 
//and don't pre-exist elsewhere.  

public class TileManager : MonoBehaviour {
	
	private int xTiles = 11;
	private int yTiles = 11;
	public GameObject[,] tiles;
	
	public GameObject tile;
	public GameObject environmentObject, cp, UnitOne,UnitTwo,UnitThree,UnitFour,UnitFive,UnitSix,UnitSeven,UnitEight,UnitNine,UnitTen,UnitEleven;
	public GameManager gm;
	public PopUpMenu pum;
	
	void Start () {
		pum =  GameObject.Find("PopUpMenu").GetComponent<PopUpMenu>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		tiles = new GameObject[xTiles, yTiles];
		
		//Tile Creation
		for (int i = 0; i < xTiles; i++)
		{
			for (int j = 0; j < yTiles; j++)
			{
				Vector3 position = new Vector3((10 * i), 0, (10 * j));
				GameObject newtile = (GameObject)Instantiate(tile,
				                                             position, 
				                                             new Quaternion(0,0,0,0));
				newtile.AddComponent("TileScript");
				tiles[i,j] = newtile;
				newtile.transform.parent = this.transform;
			}
		}
		
		//loop through the array of tiles and assign neighbors accordingly
		for (int i = 0; i < xTiles; i++) 
		{
			for (int j = 0; j < yTiles; j++)
			{
				TileScript script = tiles[i, j].GetComponent<TileScript>();
				
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
		
		addPresetTrees();
		//addPresetAllyUnits();
		//addPresetEnemyUnits();
		displayHPBars (pum.selGridInt);
	}

	public void displayHPBars(int choice){
		
		switch (choice) {
		case 0:
			foreach (int key in gm.units.Keys) {
				gm.units [key].displayHPBar = true;
			}
			break;
		case 1:
			foreach (int key in gm.units.Keys) {
					if (gm.units [key].alleg == Unit.allegiance.playerOne) {
							gm.units [key].displayHPBar = true;
					} else {
							gm.units [key].displayHPBar = false;
					}
			}
			break;
		case 2:
			foreach (int key in gm.units.Keys) {
					if (gm.units [key].alleg == Unit.allegiance.playerTwo) {
							gm.units [key].displayHPBar = true;
					} else {
							gm.units [key].displayHPBar = false;
					}
			}
			break;
		case 3:
			foreach (int key in gm.units.Keys) {
					gm.units [key].displayHPBar = false;
			}
			break;
		}
	}
	
	void addPresetTrees(){
		addTree (10,6,20);
		addTree (10,5,21);
		addTree (10,4,22);
		
		addTree (0,4,23);
		addTree (0,5,24);
		addTree (0,6,25);
		
		addTree (5,4,26);
		addTree (4,4,27);
		addTree (3,4,28);
		
		addTree (5,6,29);
		addTree (4,6,30);
		addTree (3,6,31);
	}

	void addTree(int x, int y,int unitID){
		TileScript placeTile = tiles[x,y].GetComponent<TileScript>();
		GameObject tree = (GameObject)Instantiate(environmentObject, 
		                                          new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
		                                          new Quaternion());
		tree.transform.parent = placeTile.transform;
		tree.GetComponent<Unit> ().makeTree ();
		placeTile.objectOccupyingTile = tree;
		placeTile.gameObject.renderer.material.color = Color.gray;

		gm.units.Add (unitID, tree.GetComponent<Unit>());

	}
	
	public void addUnit(int x, int y, int type, bool ally, int unitID){
		TileScript placeTile = tiles[x,y].GetComponent<TileScript>();
		GameObject unit; 

		switch(type){
		case 1:
			unit = (GameObject)Instantiate(UnitOne, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 2:
			unit = (GameObject)Instantiate(UnitTwo, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 3:
			unit = (GameObject)Instantiate(UnitThree, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 4:
			unit = (GameObject)Instantiate(UnitFour, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 5:
			unit = (GameObject)Instantiate(UnitFive, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 6:
			unit = (GameObject)Instantiate(UnitSix, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 7:
			unit = (GameObject)Instantiate(UnitSeven, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 8:
			unit = (GameObject)Instantiate(UnitEight, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 9:
			unit = (GameObject)Instantiate(UnitNine, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 10:
			unit = (GameObject)Instantiate(UnitTen, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 11:
			unit = (GameObject)Instantiate(UnitEleven, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;

		default:
			unit = (GameObject)Instantiate(cp, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		}


		unit.transform.parent = placeTile.transform;
		placeTile.objectOccupyingTile = unit;
		unit.GetComponent<Unit> ().alleg = ally? Unit.allegiance.playerOne : Unit.allegiance.playerTwo;
		unit.GetComponent<Unit> ().unitID = unitID;

		placeTile.gameObject.renderer.material.color = ally? Color.blue : Color.red;

		gm.units.Add(unitID,unit.GetComponent<Unit>());

	}
	
	//Resets color of tiles
	public void clearAllTiles(){	
		for (int i = 0; i < xTiles; i ++){
			
			for (int k = 0; k < yTiles; k++){
				//empty tile
				if (tiles[i,k].GetComponent<TileScript>().objectOccupyingTile == null){
					tiles[i, k].renderer.material.color = Color.white;
				//ally unit tile
				}else if (tiles[i,k].GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerOne){
					tiles[i, k].renderer.material.color = Color.blue;
				//neutral unit tile (shrubbery)
				}else if (tiles[i,k].GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.neutral){
					tiles[i, k].renderer.material.color = Color.gray;
				//enemy unit tile
				}else if (tiles[i,k].GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerTwo) {
					tiles[i, k].renderer.material.color = Color.red;
				}
			}
		}
	}
	
}
