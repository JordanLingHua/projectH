using UnityEngine;
using System.Collections;


//USING TileManager as the MAIN function of the game, since the tiles are instantiated HERE, 
//and don't pre-exist elsewhere.  

public class TileManager : MonoBehaviour {
	Texture2D regularCursor;
	private int xTiles = 9;
	private int yTiles = 9;
	public GameObject[,] tiles;
	
	public GameObject tile;
	public GameObject environmentObject, cp, UnitOne,UnitTwo,UnitThree,UnitFour,UnitFive,UnitSix,UnitSeven,UnitEight,UnitNine,UnitTen,UnitEleven,Barrel,Rock;
	public GameManager gm;
	public GameProcess gp;
	public PopUpMenuNecro pum;
	
	void Start () {
		regularCursor =  Resources.Load("Cursor") as Texture2D;
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

		//rocks for the corners of the board
		addRock(0,0,1000);
		addRock(8,0,1001);
		addRock(8,8,1002);
		addRock(0,8,1003);
	}

	public void displayHPBars(int choice){
		
		switch (choice) {
		//All units
		case 0:
			foreach (int key in gm.units.Keys) {
				if (gm.units [key].unitName != "Rock"){
					gm.units [key].displayHPBar = true;
				}
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
				if (gm.units [key].alleg != Unit.allegiance.neutral && gm.units [key].unitType != 11){
					gm.units [key].displayXPBar = true;
				}
			}
			break;
			//Friendly units
		case 1:
			foreach (int key in gm.units.Keys) {
				if ( gm.units [key].unitType != 11 && ((gp.playerNumber == 1 && gm.units [key].alleg == Unit.allegiance.playerOne) ||(gp.playerNumber == 2 && gm.units [key].alleg == Unit.allegiance.playerTwo ))) {
					gm.units [key].displayXPBar = true;
				} else {
					gm.units [key].displayXPBar = false;
				}
			}
			break;
			//enemy units
		case 2:
			foreach (int key in gm.units.Keys) {
				if ( gm.units [key].unitType != 11 && ((gp.playerNumber == 2 && gm.units [key].alleg == Unit.allegiance.playerOne) ||(gp.playerNumber == 1 && gm.units [key].alleg == Unit.allegiance.playerTwo ))) {
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

	//places a rock at the specified x and y position on the board
	public void addRock(int x, int y,int unitID){
		TileScript placeTile = tiles[x,y].GetComponent<TileScript>();
		GameObject rock = (GameObject)Instantiate(Rock, 
		                                          new Vector3(placeTile.transform.position.x, 5, placeTile.transform.position.z), 
		                                          new Quaternion());
		rock.transform.parent = placeTile.transform;
		rock.transform.localScale = new Vector3(1.25f,12.5f,1.25f);
		rock.transform.eulerAngles = new Vector3(4.5f,0,0f);
		placeTile.objectOccupyingTile = rock;

		placeTile.gameObject.renderer.material.color = Color.gray;
		if (gp.playerNumber == 2) {
			rock.transform.eulerAngles = new Vector3(4.5f,180f,0f);
		}
		gm.units.Add (unitID, rock.GetComponent<Unit>());
		
	}
	//places a tree at the specified x and y position on the board
	public void addTree(int x, int y,int unitID){
		TileScript placeTile = tiles[x,y].GetComponent<TileScript>();
		GameObject tree = (GameObject)Instantiate(Barrel, 
		                                          new Vector3(placeTile.transform.position.x+0.2f, 8, placeTile.transform.position.z+0.1f), 
		                                          new Quaternion());
		tree.transform.parent = placeTile.transform;
		placeTile.objectOccupyingTile = tree;
		placeTile.gameObject.renderer.material.color = Color.gray;

		gm.units.Add (unitID, tree.GetComponent<Unit>());
		tree.transform.localScale = new Vector3(1.25f,12.5f,1.25f);
		tree.transform.eulerAngles = new Vector3(4.5f,0,0f);
		if (gp.playerNumber == 2) {
			tree.transform.eulerAngles = new Vector3(4.5f,180f,0f);
		}
		
	}

	//places a unit at the specified x and y position on the board
	public GameObject addUnit(int x, int y, int type, int pNum, int unitID){
		TileScript placeTile = tiles[x,y].GetComponent<TileScript>();
		GameObject unit; 

		switch(type){
		//archer
		case 1:
			unit = (GameObject)Instantiate(UnitOne, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		//mystic
		case 2:
			unit = (GameObject)Instantiate(UnitTwo, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		//templar
		case 3:
			unit = (GameObject)Instantiate(UnitThree, 
			                               new Vector3(placeTile.transform.position.x, 5.5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		//Not in development
		case 4:
			unit = (GameObject)Instantiate(UnitFour, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		//Not in development
		case 5:
			unit = (GameObject)Instantiate(UnitFive, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		//Not in development
		case 6:
			unit = (GameObject)Instantiate(UnitSix, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		//swordsman
		case 7:
			unit = (GameObject)Instantiate(UnitSeven, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		//priest
		case 8:
			unit = (GameObject)Instantiate(UnitEight, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		//Not in development
		case 9:
			unit = (GameObject)Instantiate(UnitNine, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		//Guardian
		case 10:
			unit = (GameObject)Instantiate(UnitTen, 
			                               new Vector3(placeTile.transform.position.x, 5f, placeTile.transform.position.z), 
			                               new Quaternion());
			break;
		//soulstone
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
		//rotate piece so player two will see it
		if (gp.playerNumber == 2) {
			unit.transform.eulerAngles = new Vector3(4.5f,180f,0f);
		}

		//The following description is on the perspective that these are rendered on player 1's screen
		//Change starting animation to neutral_back
		
		if(unit.GetComponent<Animator>() != null)
		{
			//If this piece belongs to the opponent and the opponent controls player 2, render it facing back on your side
			if(unit.GetComponent<Unit> ().alleg == Unit.allegiance.playerTwo && gp.playerNumber == 1)
			{
				unit.GetComponent<Animator>().SetInteger ("mode_and_dir", 1);
			}
			//If this piece belongs to you and the and you control player 2, render it facing back on their side
			if(unit.GetComponent<Unit> ().alleg == Unit.allegiance.playerOne && gp.playerNumber == 2)
			{
				unit.GetComponent<Animator>().SetInteger ("mode_and_dir", 1);
			}

		}



		return unit;
	}
	
	//Resets color of tiles
	public void clearAllTiles(){
		Cursor.SetCursor(regularCursor,Vector2.zero,CursorMode.Auto);
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
