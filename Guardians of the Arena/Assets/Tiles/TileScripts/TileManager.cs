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
	public GameProcess gp;
	public PopUpMenuNecro pum;
	
	void Start () {
		pum =  GameObject.Find("PopUpMenu").GetComponent<PopUpMenuNecro>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
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

	}

	public void displayHPBars(int choice){
		
		switch (choice) {
		//All units
		case 0:
			foreach (int key in gm.units.Keys) {
				gm.units [key].displayHPBar = true;
			}
			break;
		//Friendly units
		case 1:
			foreach (int key in gm.units.Keys) {
				if ((gp.playerNumber == 1 && gm.units [key].alleg == Unit.allegiance.playerOne) ||(gp.playerNumber == 2 && gm.units [key].alleg == Unit.allegiance.playerTwo )) {
							gm.units [key].displayHPBar = true;
					} else {
							gm.units [key].displayHPBar = false;
					}
			}
			break;
		//enemy units
		case 2:
			foreach (int key in gm.units.Keys) {
					if ((gp.playerNumber == 2 && gm.units [key].alleg == Unit.allegiance.playerOne) ||(gp.playerNumber == 1 && gm.units [key].alleg == Unit.allegiance.playerTwo )) {
							gm.units [key].displayHPBar = true;
					} else {
							gm.units [key].displayHPBar = false;
					}
			}
			break;
			//off
		case 3:
			foreach (int key in gm.units.Keys) {
					gm.units [key].displayHPBar = false;
			}
			break;
		}
	}

	public void displayXPBars(int choice){
		
		switch (choice) {
			//All units
		case 0:
			foreach (int key in gm.units.Keys) {
				if (gm.units [key].alleg != Unit.allegiance.neutral){
					gm.units [key].displayXPBar = true;
				}
			}
			break;
			//Friendly units
		case 1:
			foreach (int key in gm.units.Keys) {
				if ((gp.playerNumber == 1 && gm.units [key].alleg == Unit.allegiance.playerOne) ||(gp.playerNumber == 2 && gm.units [key].alleg == Unit.allegiance.playerTwo )) {
					gm.units [key].displayXPBar = true;
				} else {
					gm.units [key].displayXPBar = false;
				}
			}
			break;
			//enemy units
		case 2:
			foreach (int key in gm.units.Keys) {
				if ((gp.playerNumber == 2 && gm.units [key].alleg == Unit.allegiance.playerOne) ||(gp.playerNumber == 1 && gm.units [key].alleg == Unit.allegiance.playerTwo )) {
					gm.units [key].displayXPBar = true;
				} else {
					gm.units [key].displayXPBar = false;
				}
			}
			break;
			//off
		case 3:
			foreach (int key in gm.units.Keys) {
				gm.units [key].displayXPBar = false;
			}
			break;
		}
	}
	public void addRock(int x, int y,int unitID){
		TileScript placeTile = tiles[x,y].GetComponent<TileScript>();
		GameObject rock = (GameObject)Instantiate(cp, 
		                                          new Vector3(placeTile.transform.position.x, 0, placeTile.transform.position.z), 
		                                          new Quaternion());
		rock.renderer.material.color = new Color32(139,69,19,255);
		rock.transform.parent = placeTile.transform;
		rock.GetComponent<Unit> ().makeRock ();
		placeTile.objectOccupyingTile = rock;

		placeTile.gameObject.renderer.material.color = Color.gray;
		
		gm.units.Add (unitID, rock.GetComponent<Unit>());
		
	}

	public void addTree(int x, int y,int unitID){
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
	
	public GameObject addUnit(int x, int y, int type, int pNum, int unitID){
		TileScript placeTile = tiles[x,y].GetComponent<TileScript>();
		GameObject unit; 

		switch(type){
		case 1:
			unit = (GameObject)Instantiate(UnitOne, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 2:
			unit = (GameObject)Instantiate(UnitTwo, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 3:
			unit = (GameObject)Instantiate(UnitThree, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 4:
			unit = (GameObject)Instantiate(UnitFour, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 5:
			unit = (GameObject)Instantiate(UnitFive, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 6:
			unit = (GameObject)Instantiate(UnitSix, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 7:
			unit = (GameObject)Instantiate(UnitSeven, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 8:
			unit = (GameObject)Instantiate(UnitEight, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 9:
			unit = (GameObject)Instantiate(UnitNine, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 10:
			unit = (GameObject)Instantiate(UnitTen, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		case 11:
			unit = (GameObject)Instantiate(UnitEleven, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;

		default:
			unit = (GameObject)Instantiate(cp, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		}


		unit.transform.parent = placeTile.transform;
		placeTile.objectOccupyingTile = unit;
		unit.GetComponent<Unit> ().alleg = pNum == 1? Unit.allegiance.playerOne : Unit.allegiance.playerTwo;
		unit.GetComponent<Unit> ().unitID = unitID;

		gm.units.Add(unitID,unit.GetComponent<Unit>());
		if (gp.playerNumber == 2) {
			unit.transform.eulerAngles = new Vector3(4.5f,180f,0f);
		}

		return unit;
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
					Color newTileColor = Color.blue;
					//newTileColor.
					if (gm.pMana > 0){
						if (gm.turn && gp.playerNumber == 1 && !tiles[i,k].GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().mvd){
							newTileColor.g += 0.3f;
						}
						if (gm.turn && gp.playerNumber == 1 && !tiles[i,k].GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().atkd){
							newTileColor.g += 0.3f;
						}
					}
					tiles[i, k].renderer.material.color = newTileColor;
				//neutral unit tile (shrubbery)
				}else if (tiles[i,k].GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.neutral){
					tiles[i, k].renderer.material.color = Color.gray;
				//enemy unit tile
				}else if (tiles[i,k].GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerTwo) {
					Color newTileColor = Color.red;
					if (gm.pMana > 0){
						if (gm.turn && gp.playerNumber == 2 && !tiles[i,k].GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().mvd){
							newTileColor.g += 0.3f;
						}
						if (gm.turn && gp.playerNumber == 2 && !tiles[i,k].GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().atkd){
							newTileColor.g += 0.3f;
						}
					}
					tiles[i, k].renderer.material.color = newTileColor;
				}
			}
		}
	}
	
}
