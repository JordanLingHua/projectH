using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileScript : MonoBehaviour {

	//
	//protected Animator animator;

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

	//
	//public float targetTileX, targetTileZ;

	
	public GameProcess gp;
	AudioManager am;
	public List<GameObject> AoETiles = new List<GameObject> ();

	GameManager gm;
	void Start () {
		am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();	
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
	}

	public void OnMouseOver(){
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

		//Change attack animation back to idle since the
		/*
		if(this.GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 8)
			this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 0);
		else if(this.GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 9)
			this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 1);
		else if(this.GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 10)
			this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 2);
		else if(this.GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 11)
			this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 3);
			*/

	}

	//taken from user dcarrier on unity forums
	//http://answers.unity3d.com/questions/14279/make-an-object-move-from-point-a-to-point-b-then-b.html
	IEnumerator movePiece (GameObject move, Vector3 start, Vector3 end, float time){

		//new
		//Step 1)  Before you do anything, Transition from neutral_states in the post_attack version, to the actual neutral states
		//NOTE:  the post_attack neutral states don't get signified by a mode_and_dir.  occured at exit time of attack.  So mode_and_dir is still 
		//== the attack state it left off at
		if(this.GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 8)
			this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 0);
		else if(this.GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 9)
			this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 1);
		else if(this.GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 10)
			this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 2);
		else if(this.GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 11)
			this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 3);
		//NOTE:  I will use the above switch state as the preamble to nearly every switch statement like below
		
		
		
		float i = 0.0f;
		float rate = 1.0f / time;
		//1.0f
		bool isOpponentPiece = false;//Temporarily here.  Will make global after non-opponent
		//logic implemented
		//1.0f
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			move.transform.position = Vector3.Lerp(start, end, i);

			/*Choose animation*/
			if(isOpponentPiece == false){




				//old
				//Step 2 (regular transition trigger)
				if(end.z > start.z)
					this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 4);
				else if(end.z < start.z)
					this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 5);
				else if(end.x < start.x)
					this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 6);
				else if(end.x > start.x)
					this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 7);
			}
			else{
			}


			yield return null; 
		}
	}

	IEnumerator getPath(Node current, Unit movingUnit){
		this.transform.parent.GetComponent<TileManager>().clearAllTiles();
		Stack<GameObject> tiles = new Stack<GameObject>();
		tiles.Push(this.gameObject);

		while (current.parent != null){
			tiles.Push(current.parent.myNode.gameObject);
			current = current.parent;
		}
		
		Vector3 newPos;
		//Vector3 misterPos;

		float tilePeekX;
		float tilePeekZ;
		Vector3 movingUnitTransPos;

		while (tiles.Count !=0){

			tilePeekX = tiles.Peek().transform.position.x;
			tilePeekZ = tiles.Peek().transform.position.z;
			movingUnitTransPos = movingUnit.transform.position;

			newPos = new Vector3(tiles.Peek().transform.position.x,5f,tiles.Peek().transform.position.z);
			yield return StartCoroutine(movePiece(movingUnit.gameObject,movingUnit.transform.position,newPos,1.0f));//0.28f

			tiles.Pop();




			//Set unit back to neutral animation now that it has moved to the final tile
			if(this.GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 4)
				this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 0);
			else if(this.GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 5)
				this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 1);
			else if(this.GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 6)
				this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 2);
			else if(this.GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 7)
				this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 3);

		}


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
		if (gm.selectedUnit != null) {
			if (gm.gs == GameManager.gameState.playerMv) {
				moveToTile ();
			} else if (gm.gs == GameManager.gameState.playerAtk) {
				attackTile ();
			}
		}
	}

	public void moveToTile(){

		
		//move unit selected to this tile if it can go there
		if (gm.turn && 
			gm.accessibleTiles.Contains (this) && 
			this.objectOccupyingTile == null && 
			!gm.selectedUnit.GetComponent<Unit> ().mvd &&
			gm.pMana >= gm.selectedUnit.GetComponent<Unit> ().mvCost && 
			((gm.selectedUnit.alleg == Unit.allegiance.playerOne && gp.playerNumber == 1) || (gm.selectedUnit.alleg == Unit.allegiance.playerTwo && gp.playerNumber == 2))) {

			gp.returnSocket ().SendTCPPacket ("move\\" + gm.selectedUnit.unitID + "\\" + this.x + "\\" + this.y);
			print ("Sent move packet");
						/*
			if(this.x == 0 && this.y > 0)
				this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 4);
			else if(this.x == 0 && this.y < 0)
				this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 5);
			else if(this.x < 0 && this.y == 0)
				this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 6);
			else if(this.x > 0 && this.y == 0)
				this.GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 7);
			*/
			am.playButtonSFX ();
		} else {
			am.playErrorSFX ();
			if(!gm.turn){
				gm.showErrorMessage("It's not your turn!");
			}else if ((gm.selectedUnit.alleg == Unit.allegiance.playerOne && gp.playerNumber == 2) || (gm.selectedUnit.alleg == Unit.allegiance.playerTwo && gp.playerNumber == 1)){
				gm.showErrorMessage("Cannot move an opponent's piece!");
			}else if (gm.selectedUnit.GetComponent<Unit>().mvd){
				gm.showErrorMessage("That unit has already moved this turn!");
			}else if (gm.pMana < gm.selectedUnit.mvCost){
				gm.showErrorMessage("Not enough mana to move!");
			}else if (!gm.accessibleTiles.Contains(this)){
				gm.showErrorMessage("Cannot move there!");
			}else if (this.objectOccupyingTile != null && this.objectOccupyingTile == gm.selectedUnit){
				gm.showErrorMessage("That unit is already there!");
			}else if (this.objectOccupyingTile != null){
				gm.showErrorMessage("Theres a unit there already!");
			}
		}
	}
	

	public void attackTile(){
		if (gm.turn &&
		    gm.accessibleTiles.Contains (this) && 
		    gm.pMana >= gm.selectedUnit.GetComponent<Unit> ().atkCost && 
		    !gm.selectedUnit.GetComponent<Unit>().atkd &&
		    ((gm.selectedUnit.alleg == Unit.allegiance.playerOne && gp.playerNumber == 1) || (gm.selectedUnit.alleg == Unit.allegiance.playerTwo && gp.playerNumber == 2))) {
			gp.returnSocket ().SendTCPPacket ("attack\\" + gm.selectedUnit.unitID + "\\" + this.x + "\\" + this.y);
			print ("Sent attack packet");

			//extract the info of the tile selected to be attacked, so that the attacking tile can know which attack direction animation to play
			GameObject.Find ("GameProcess").GetComponent<GameProcess>().targetTileX = this.x;
			GameObject.Find ("GameProcess").GetComponent<GameProcess>().targetTileZ = this.y;
			//targetTileX and targetTileZ are then used inside GameProcess when attack is called

			am.playButtonSFX();
		} else {
			am.playErrorSFX();
			if(!gm.turn){
				gm.showErrorMessage("It's not your turn!");
			}else if (((gm.selectedUnit.alleg == Unit.allegiance.playerOne && gp.playerNumber == 2) || (gm.selectedUnit.alleg == Unit.allegiance.playerTwo && gp.playerNumber == 1))){
				gm.showErrorMessage("Cannot attack with an opponent's piece!");
			}else if (gm.selectedUnit.GetComponent<Unit>().atkd){
				gm.showErrorMessage("That unit has already attacked this turn!");
			}else if (gm.pMana < gm.selectedUnit.atkCost){
				gm.showErrorMessage("Not enough mana to attack!");
			}else if (gm.accessibleTiles.Contains(this)){
				gm.showErrorMessage("Cannot attack there!");
			}
		}
	}

}
