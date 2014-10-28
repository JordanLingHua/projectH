﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
	
	private struct Node{
		public TileScript parent;
		public TileScript myNode;
		
		public Node(TileScript newParent, TileScript newMyNode){
			parent = newParent;
			myNode = newMyNode;
		}
	};
	
	//Describes what is occupying the tile, 0 for empty, 1 for a friendly unit, 2 for neutral, 3 for enemy
	public enum occupiedBy {nothing,friendly,neutral,enemy};
	
	//public int occupied = 0;
	public occupiedBy occupied = occupiedBy.nothing;
	
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
	//		Queue<TileScript> open = new Queue<TileScript>();
	//		Queue<TileScript> closed = new Queue<TileScript>();
	//		bool done = false;
	//		
	//		//start node
	//		open.Enqueue(gm.selectedUnit.transform.parent.GetComponent<TileScript>());
	//		
	//		//endnode
	//		TileScript end = this;
	//		pathFind(new Node(null,gm.selectedUnit.transform.parent.GetComponent<TileScript>()),end);
	//	}
	
	//	Node pathFind(Node attempt, TileScript end){
	//		if (attempt.myNode == end){
	//			return attempt;
	//		}	
	//		if (attempt.myNode.right != null && gm.accessibleTiles.Contains(attempt.myNode.right.GetComponent<TileScript>())){
	//			return pathFind (new Node(attempt.myNode,attempt.myNode.right.GetComponent<TileScript>()),end);
	//		}
	//		if (attempt.myNode.left != null && gm.accessibleTiles.Contains(attempt.myNode.left.GetComponent<TileScript>())){
	//			return pathFind (new Node(attempt.myNode,attempt.myNode.left.GetComponent<TileScript>()),end);
	//		}
	//		if (attempt.myNode.down != null && gm.accessibleTiles.Contains(attempt.myNode.down.GetComponent<TileScript>())){
	//			return pathFind (new Node(attempt.myNode,attempt.myNode.down.GetComponent<TileScript>()),end);
	//		}
	//		if (attempt.myNode.up != null && gm.accessibleTiles.Contains(attempt.myNode.up.GetComponent<TileScript>())){
	//			return pathFind (new Node(attempt.myNode,attempt.myNode.up.GetComponent<TileScript>()),end);
	//		}
	//		return attempt;
	//	}
	
	
	void OnMouseDown(){
		//set tile selected coord in GM script
		gm.tsx = x;
		gm.tsy = y;
		
		//move unit selected to this tile if it can go there
		if (gm.accessibleTiles.Contains(this) && gm.gs ==  GameManager.gameState.playerMv){
			
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
				//gp.returnSocket().SendTCPPacket("game\\move\\" +gp.playerNumber + "\\" + gm.selectedUnit.unitID + "\\" + x1 + "\\" + y1 + "\\" + x2 + "\\" + y2);
				
				//remove unit from previous tile
				gm.selectedUnit.transform.parent.GetComponent<TileScript>().occupied = occupiedBy.nothing;
				gm.selectedUnit.transform.parent = gameObject.transform;
				gm.selectedUnit.transform.parent.GetComponent<TileScript>().objectOccupyingTile = null;

				this.transform.parent.GetComponent<TileManager>().clearAllTiles();

				//as if attack button was pushed
				gm.gs =  GameManager.gameState.playerAtk;
				gm.buttonOption = "Move";
				if (gm.selectedUnit != null){
					gm.selectedUnit.GetComponent<Unit>().showAtkTiles();
				}
				
			}else{
				gm.combatLog.text = "Combat Log:\nNot enough mana";
			}
		}
	}
	
	
	
	
	
}
