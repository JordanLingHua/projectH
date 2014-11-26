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
	AudioManager am;
	public List<GameObject> AoETiles = new List<GameObject> ();

	GameManager gm;
	void Start () {
		am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();	
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
	}

	public void OnMouseEnter(){
		renderer.material.shader = Shader.Find ("Toon/Lighted");
		if (gm.selectedUnit != null && gm.gs == GameManager.gameState.playerAtk && gm.accessibleTiles.Contains (this)) {
			AoETiles = gm.selectedUnit.showAoEAffectedTiles(this);
		}
	}

	public void OnMouseExit(){
		renderer.material.shader = Shader.Find ("Toon/Basic");
		if (gm.selectedUnit != null && gm.gs == GameManager.gameState.playerAtk && gm.accessibleTiles.Contains (this)) {
			clearAoEAffectedTiles();
		}
	}

	
	public void clearAoEAffectedTiles(){
		foreach (GameObject tile in AoETiles) {
			if(gm.accessibleTiles.Contains (tile.GetComponent<TileScript>())){
				tile.renderer.material.color = new Color(1f,0.4f,0f, 0f);
			}else if (tile.GetComponent<TileScript>().objectOccupyingTile == null){
				tile.renderer.material.color = Color.white;			
			}else if (tile.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerOne){
				tile.renderer.material.color = Color.blue;
			}else if (tile.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.neutral){
				tile.renderer.material.color = Color.gray;
			}else if (tile.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerTwo){
				tile.renderer.material.color = Color.red;
			}
		}
		AoETiles.Clear ();
	}

	//taken from user dcarrier on unity forums
	//http://answers.unity3d.com/questions/14279/make-an-object-move-from-point-a-to-point-b-then-b.html
	IEnumerator movePiece (GameObject move, Vector3 start, Vector3 end, float time){
		float i = 0.0f;
		float rate = 1.0f / time;

		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			move.transform.position = Vector3.Lerp(start, end, i);
			yield return null; 
		}
	}

	IEnumerator getPath(Node current, Unit movingUnit){
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

			tiles.Peek().renderer.material.color = movingUnit.alleg == Unit.allegiance.playerOne? Color.blue : Color.red;
			yield return StartCoroutine(movePiece(movingUnit.gameObject,movingUnit.transform.position,newPos,0.28f));
			if (tiles.Count != 1){
				if (tiles.Peek ().GetComponent<TileScript>().objectOccupyingTile == null){
					tiles.Peek ().renderer.material.color = Color.white;
					//ally unit tile
				}else if (tiles.Peek ().GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerOne){
					tiles.Peek ().renderer.material.color = Color.blue;
					//neutral unit tile (shrubbery)
				}else if (tiles.Peek ().GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.neutral){
					tiles.Peek ().renderer.material.color = Color.gray;
					//enemy unit tile
				}else if (tiles.Peek ().GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerTwo) {
					tiles.Peek ().renderer.material.color = Color.red;
				}

			} 
			tiles.Pop();
		}
		gm.movingPiece = false;
		objectOccupyingTile = movingUnit.gameObject;

	}

	//pathfinder algorithm for moving pieces
	public void pathFinder(Unit movingUnit){
		//use tiles
		Queue<Node> open = new Queue<Node>();
		Queue<Node> closed = new Queue<Node>();

		//start node
		open.Enqueue(new Node(null,movingUnit.transform.parent.GetComponent<TileScript>()));
		HashSet<TileScript> tileList = movingUnit.getMvAccessibleTiles (movingUnit.alleg == Unit.allegiance.playerOne ? Unit.allegiance.playerOne : Unit.allegiance.playerTwo);
		
		//endnode
		TileScript end = this;
		
		
		while (open.Count != 0){
			Node current = open.Dequeue();
			if (current.myNode.Equals (end)){
				StartCoroutine(getPath(current,movingUnit));
				break;
				
			}
			closed.Enqueue(current);
			
			if (current.myNode.right != null && tileList.Contains(current.myNode.right.GetComponent<TileScript>()) && !closed.Contains(new Node (current,current.myNode.right.GetComponent<TileScript>()))){
				if (!open.Contains(new Node(current,current.myNode.right.GetComponent<TileScript>()))){
					open.Enqueue(new Node(current,current.myNode.right.GetComponent<TileScript>()));
				}
			}
			if (current.myNode.left != null && tileList.Contains(current.myNode.left.GetComponent<TileScript>()) && !closed.Contains(new Node (current,current.myNode.left.GetComponent<TileScript>()))){
				if (!open.Contains(new Node(current,current.myNode.left.GetComponent<TileScript>()))){
					open.Enqueue(new Node(current,current.myNode.left.GetComponent<TileScript>()));
				}
			}
			if (current.myNode.up != null && tileList.Contains(current.myNode.up.GetComponent<TileScript>()) && !closed.Contains(new Node (current,current.myNode.up.GetComponent<TileScript>()))){
				if (!open.Contains(new Node(current,current.myNode.up.GetComponent<TileScript>()))){
					open.Enqueue(new Node(current,current.myNode.up.GetComponent<TileScript>()));
				}
			}
			if (current.myNode.down != null && tileList.Contains(current.myNode.down.GetComponent<TileScript>()) && !closed.Contains(new Node (current,current.myNode.down.GetComponent<TileScript>()))){
				if (!open.Contains(new Node(current,current.myNode.down.GetComponent<TileScript>()))){
					open.Enqueue(new Node(current,current.myNode.down.GetComponent<TileScript>()));
				}
			}
		} 
	}

	
	void OnMouseDown(){
		if (gm.gs ==  GameManager.gameState.playerMv && gm.turn){
			moveToTile ();
		}else if (gm.gs == GameManager.gameState.playerAtk){
			attackTile ();
		}
	}

	public void moveToTile(){
		
		
		//move unit selected to this tile if it can go there
		if (gm.accessibleTiles.Contains(this) && this.objectOccupyingTile == null && 
		    gm.pMana >= gm.selectedUnit.GetComponent<Unit>().mvCost){

			gp.returnSocket().SendTCPPacket("move\\" + gm.selectedUnit.unitID+ "\\" + this.x + "\\" + this.y);
			print ("Sent move packet");
			am.playButtonSFX();
		}else{
			am.playErrorSFX();
			gm.combatLog.text = "Combat Log:\nNot enough mana to move";
		}
	}
	

	public void attackTile(){
		if (gm.accessibleTiles.Contains (this) && gm.pMana >= gm.selectedUnit.GetComponent<Unit> ().atkCost) {
			gp.returnSocket ().SendTCPPacket ("attack\\" + gm.selectedUnit.unitID + "\\" + this.x + "\\" + this.y);
			print ("Sent attack packet");
		} else {
			am.playErrorSFX();
			gm.combatLog.text = "Combat Log:\nNot enough mana to attack";
		}


	}
	
	
	
	
	
}
