using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {
	
	private class Node{
		public Node parent;
		public TileScript myNode;
		
		public Node(Node newParent, TileScript newMyNode){
			parent = newParent;
			myNode = newMyNode;
		}
	};
	
	public GameObject environmentObject, cp;
	
	public GameObject left, right, down, up;
	public GameObject objectOccupyingTile = null;
	public int x,y;
	
	public GameProcess gp;
	
	GameManager gm;
	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
	}
	
	IEnumerator movePiece(Node current){
		this.transform.parent.GetComponent<TileManager>().clearAllTiles();
		Stack<GameObject> tiles = new Stack<GameObject>();
		tiles.Push(this.gameObject);
		
		current.myNode.gameObject.renderer.material.color = Color.green;
		while (current.parent != null){
			tiles.Push(current.parent.myNode.gameObject);
			current = current.parent;
		}
		
		Vector3 newPos;
		gm.movingPiece = true;
		
		while (tiles.Count !=0){
			newPos = new Vector3(tiles.Peek().transform.position.x,0,tiles.Peek().transform.position.z);

			tiles.Peek().renderer.material.color = gm.selectedUnit.alleg == Unit.allegiance.ally? Color.blue : Color.red;

			gm.selectedUnit.transform.position = newPos;
			yield return new WaitForSeconds(0.28f);
			if (tiles.Count != 1){
				if (tiles.Peek ().GetComponent<TileScript>().objectOccupyingTile == null){
					tiles.Peek ().renderer.material.color = Color.white;
					//ally unit tile
				}else if (tiles.Peek ().GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.ally){
					tiles.Peek ().renderer.material.color = Color.blue;
					//neutral unit tile (shrubbery)
				}else if (tiles.Peek ().GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.neutral){
					tiles.Peek ().renderer.material.color = Color.gray;
					//enemy unit tile
				}else if (tiles.Peek ().GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.enemy) {
					tiles.Peek ().renderer.material.color = Color.red;
				}

			} 
			tiles.Pop();
		}
		gm.movingPiece = false;
		objectOccupyingTile = gm.selectedUnit.gameObject;


		
		//as if attack button was pushed
		gm.gs = gm.gs == GameManager.gameState.playerMv ? GameManager.gameState.playerAtk : GameManager.gameState.opponentAtk;
		gm.buttonOption = "Move";
		if (gm.selectedUnit != null){
			gm.selectedUnit.GetComponent<Unit>().showAtkTiles();
		}
	}

	//pathfinder algorithm for moving pieces
	void pathFinder(){
		//use tiles
		Queue<Node> open = new Queue<Node>();
		Queue<Node> closed = new Queue<Node>();

		//start node
		open.Enqueue(new Node(null,gm.selectedUnit.transform.parent.GetComponent<TileScript>()));
		
		//endnode
		TileScript end = this;
		
		
		while (open.Count != 0){
			Node current = open.Dequeue();
			
			//print path!
			if (current.myNode.Equals (end)){
				
				
				StartCoroutine(movePiece(current));
				break;
				
			}
			closed.Enqueue(current);
			
			if (current.myNode.right != null && gm.accessibleTiles.Contains(current.myNode.right.GetComponent<TileScript>()) && !closed.Contains(new Node (current,current.myNode.right.GetComponent<TileScript>()))){
				if (!open.Contains(new Node(current,current.myNode.right.GetComponent<TileScript>()))){
					open.Enqueue(new Node(current,current.myNode.right.GetComponent<TileScript>()));
				}
			}
			if (current.myNode.left != null && gm.accessibleTiles.Contains(current.myNode.left.GetComponent<TileScript>()) && !closed.Contains(new Node (current,current.myNode.left.GetComponent<TileScript>()))){
				if (!open.Contains(new Node(current,current.myNode.left.GetComponent<TileScript>()))){
					open.Enqueue(new Node(current,current.myNode.left.GetComponent<TileScript>()));
				}
			}
			if (current.myNode.up != null && gm.accessibleTiles.Contains(current.myNode.up.GetComponent<TileScript>()) && !closed.Contains(new Node (current,current.myNode.up.GetComponent<TileScript>()))){
				if (!open.Contains(new Node(current,current.myNode.up.GetComponent<TileScript>()))){
					open.Enqueue(new Node(current,current.myNode.up.GetComponent<TileScript>()));
				}
			}
			if (current.myNode.down != null && gm.accessibleTiles.Contains(current.myNode.down.GetComponent<TileScript>()) && !closed.Contains(new Node (current,current.myNode.down.GetComponent<TileScript>()))){
				if (!open.Contains(new Node(current,current.myNode.down.GetComponent<TileScript>()))){
					open.Enqueue(new Node(current,current.myNode.down.GetComponent<TileScript>()));
				}
			}
			
		} 
	}

	
	void OnMouseDown(){

		if (gm != null){
		//set tile selected coord in GM script
		gm.tsx = x;
		gm.tsy = y;
		
		//move unit selected to this tile if it can go there
		if (gm.accessibleTiles.Contains(this) && this.objectOccupyingTile == null && (gm.gs ==  GameManager.gameState.playerMv ||gm.gs ==  GameManager.gameState.opponentMv  )){
			
			//enough mana to move piece
			if (gm.pMana >= gm.selectedUnit.GetComponent<Unit>().mvCost){
				gm.pMana -= gm.selectedUnit.mvCost;

				pathFinder ();

				//remove unit from previous tile
				gm.selectedUnit.transform.parent.GetComponent<TileScript>().objectOccupyingTile = null;
				gm.selectedUnit.transform.parent = gameObject.transform;


				//move unit to clicked tile
				int x1 = gm.selectedUnit.transform.parent.GetComponent<TileScript>().x;
				int y1 = gm.selectedUnit.transform.parent.GetComponent<TileScript>().y;
				
				gm.accessibleTiles.Clear();
				this.objectOccupyingTile = gm.selectedUnit.gameObject;
				gm.selectedUnit.GetComponent<Unit>().mvd = true;
				
				int x2 = this.x;
				int y2 = this.y;
				//gp.returnSocket().SendTCPPacket("game\\move\\" +gp.playerNumber + "\\" + gm.selectedUnit.unitID + "\\" + x1 + "\\" + y1 + "\\" + x2 + "\\" + y2);				
			}else{
				gm.combatLog.text = "Combat Log:\nNot enough mana";
			}
		}
		}
	}
	
	
	
	
	
}
