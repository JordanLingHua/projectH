using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
	
	//Describes what is occupying the tile, 0 for empty, 1 for a friendly unit, 2 for neutral, 3 for enemy
	public int occupied = 0;
	public GameObject environmentObject, cp;
		
	public GameObject left, right, down, up;
	public GameObject objectOccupyingTile;
	public int x,y;

	public GameProcess gp;
	
	GameManager gm;
	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		this.GetComponent<BaseObject>().type = BaseObject.ObjectType.Tile;

		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
	}
		
	//pathfinder algorithm for moving pieces
//	void pathFinder(){
//		//use tiles
//		Queue<GameObject> open = new Queue<GameObject>();
//		Queue<GameObject> closed = new Queue<GameObject>();
//		bool done = false;
//		
//		//start node
//		open.Enqueue(gm.selectedUnit.transform.parent.gameObject);
//		
//		
//		//endnode
//		GameObject end = this.gameObject;
//		
//		while(!done){
//		}		
//	}
	
	void OnMouseDown(){
		//set tile selected coord in GM script
		gm.tsx = x;
		gm.tsy = y;
		
		//move unit selected to this tile if it can access it
		if (gm.accessibleTiles.Contains(gameObject) && gm.gameState == 1){
			
			//enough mana to move piece
			if (gm.pMana >= gm.selectedUnit.GetComponent<Unit>().mvCost){
				gm.pMana -= gm.selectedUnit.mvCost;
				//move unit to clicked tile
				int x1 = gm.selectedUnit.transform.parent.GetComponent<TileScript>().x;
				int y1 = gm.selectedUnit.transform.parent.GetComponent<TileScript>().y;

				this.occupied = gm.selectedUnit.transform.parent.GetComponent<TileScript>().occupied;
				Vector3 newPos = new Vector3(this.transform.position.x,0,this.transform.position.z);
				gm.selectedUnit.transform.position = newPos;

				gm.accessibleTiles.Clear();
				this.objectOccupyingTile = gm.selectedUnit.gameObject;
				gm.selectedUnit.GetComponent<Unit>().mvd = true;

				int x2 = this.x;
				int y2 = this.y;
				gp.returnSocket().SendTCPPacket("game\\move\\" +gp.playerNumber + "\\" + gm.selectedUnit.unitID + "\\" + x1 + "\\" + y1 + "\\" + x2 + "\\" + y2);
				
				//remove unit from previous tile
				gm.selectedUnit.transform.parent.GetComponent<TileScript>().occupied = 0;
				gm.selectedUnit.transform.parent = gameObject.transform;
				gm.selectedUnit.transform.parent.GetComponent<TileScript>().objectOccupyingTile = null;
				
				this.transform.parent.GetComponent<TileManager>().clearAllTiles();

			}else{
				gm.combatLog.text = "Combat Log:\nNot enough mana";
			}
		}
	}




	
}
