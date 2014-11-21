using UnityEngine;
using System.Collections;


//USING TileManager as the MAIN function of the game, since the tiles are instantiated HERE, 
//and don't pre-exist elsewhere.  

public class TileManager : MonoBehaviour {
	
	private int xTiles = 16;
	private int yTiles = 16;
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
		addTree (0,7,20);
		addTree (2,7,21);
		addTree (3,7,22);
		addTree (4,7,23);
		
		addTree (15,7,24);
		addTree (13,7,25);
		addTree (12,7,26);
		addTree (11,7,27);
		
		
		addTree (0,8,28);
		addTree (2,8,29);
		addTree (3,8,30);
		addTree (4,8,31);
		
		addTree (15,8,32);
		addTree (13,8,33);
		addTree (12,8,34);
		addTree (11,8,35);
	
		addTree (7,7,36);
		addTree (7,8,37);
		addTree (8,7,38);
		addTree (8,8,39);
		
	}
	
	void addPresetAllyUnits(){
		//melee units
		addUnit (5,3,7,true, 0);
		addUnit (6,3,7,true, 1);
		addUnit (9,3,7,true, 2);
		addUnit (10,3,7,true, 3);
		
		//ranged units
		addUnit (5,2,1,true, 4);
		addUnit (6,2,1,true, 5);
		addUnit (9,2,8,true, 6);
		addUnit (10,2,3,true, 7);
		
		//guardian and soulstone
		addUnit (7,0,10,true, 8);
		addUnit (8,0,11,true, 9);
	}
	
	void addPresetEnemyUnits(){
		//melee units
		addUnit (5,12,7,false, 10);
		addUnit (6,12,7,false, 11);
		addUnit (9,12,7,false, 12);
		addUnit (10,12,7,false, 13);
		
		//ranged units
		addUnit (5,13,1,false, 14);
		addUnit (6,13,1,false, 15);
		addUnit (9,13,8,false, 16);
		addUnit (10,13,3,false, 17);
		
		//guardian and soulstone
		addUnit (7,15,10,false, 18);
		addUnit (8,15,11,false, 19);
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
