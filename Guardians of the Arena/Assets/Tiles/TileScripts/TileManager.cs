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
	
	void Start () {
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
		addPresetAllyUnits();
		addPresetEnemyUnits();
		
	}
	
	void addPresetTrees(){
		addTree (0,7);
		addTree (2,7);
		addTree (3,7);
		addTree (4,7);
		
		addTree (15,7);
		addTree (13,7);
		addTree (12,7);
		addTree (11,7);
		
		
		addTree (0,8);
		addTree (2,8);
		addTree (3,8);
		addTree (4,8);
		
		addTree (15,8);
		addTree (13,8);
		addTree (12,8);
		addTree (11,8);
		
		addTree (7,7);
		addTree (7,8);
		addTree (8,7);
		addTree (8,8);
		
	}
	
	void addPresetAllyUnits(){
		//melee units
		addUnit (5,3,16,true, 0);
		addUnit (6,3,16,true, 1);
		addUnit (9,3,16,true, 2);
		addUnit (10,3,16,true, 3);
		
		//ranged units
		addUnit (5,2,10,true, 4);
		addUnit (6,2,10,true, 5);
		addUnit (9,2,10,true, 6);
		addUnit (10,2,10,true, 7);
		
		//guardian and soulstone
		addUnit (7,0,19,true, 8);
		addUnit (8,0,20,true, 9);
	}
	
	void addPresetEnemyUnits(){
		//melee units
		addUnit (5,12,16,false, 10);
		addUnit (6,12,16,false, 11);
		addUnit (9,12,16,false, 12);
		addUnit (10,12,16,false, 13);
		
		//ranged units
		addUnit (5,13,10,false, 14);
		addUnit (6,13,10,false, 15);
		addUnit (9,13,10,false, 16);
		addUnit (10,13,10,false, 17);
		
		//guardian and soulstone
		addUnit (7,15,19,false, 18);
		addUnit (8,15,20,false, 19);
	}
	
	void addTree(int x, int y){
		TileScript placeTile = tiles[x,y].GetComponent<TileScript>();
		GameObject tree = (GameObject)Instantiate(environmentObject, 
		                                          new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
		                                          new Quaternion());
		tree.transform.parent = placeTile.transform;
		tree.GetComponent<Unit> ().makeTree ();
		placeTile.objectOccupyingTile = tree;
		placeTile.gameObject.renderer.material.color = Color.gray;

	}
	
	void addUnit(int x, int y,int type, bool ally, int unitID){
		TileScript placeTile = tiles[x,y].GetComponent<TileScript>();
		GameObject unit; 

		switch(type){
		case 10:
			unit = (GameObject)Instantiate(UnitOne, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 11:
			unit = (GameObject)Instantiate(UnitTwo, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 12:
			unit = (GameObject)Instantiate(UnitThree, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 13:
			unit = (GameObject)Instantiate(UnitFour, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 14:
			unit = (GameObject)Instantiate(UnitFive, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 15:
			unit = (GameObject)Instantiate(UnitSix, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 16:
			unit = (GameObject)Instantiate(UnitSeven, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 17:
			unit = (GameObject)Instantiate(UnitEight, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 18:
			unit = (GameObject)Instantiate(UnitNine, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 19:
			unit = (GameObject)Instantiate(UnitTen, 
			                               new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 20:
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
		
		unit.GetComponent<Unit> ().alleg = ally? Unit.allegiance.ally : Unit.allegiance.enemy;
		unit.GetComponent<Unit> ().unitID = unitID;

		placeTile.gameObject.renderer.material.color = ally? Color.blue : Color.red;
		
		if (ally){
			gm.playerUnits.Add(unit.GetComponent<Unit>());
		}else{
			gm.enemyUnits.Add(unit.GetComponent<Unit>());
		}
	}
	
	//Resets color of tiles
	public void clearAllTiles(){	
		for (int i = 0; i < xTiles; i ++){
			
			for (int k = 0; k < yTiles; k++){
				//empty tile
				if (tiles[i,k].GetComponent<TileScript>().objectOccupyingTile == null){
					tiles[i, k].renderer.material.color = Color.white;
				//ally unit tile
				}else if (tiles[i,k].GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.ally){
					tiles[i, k].renderer.material.color = Color.blue;
				//neutral unit tile (shrubbery)
				}else if (tiles[i,k].GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.neutral){
					tiles[i, k].renderer.material.color = Color.gray;
				//enemy unit tile
				}else if (tiles[i,k].GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.enemy) {
					tiles[i, k].renderer.material.color = Color.red;
				}
			}
		}
	}
	
}
