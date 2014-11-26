using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit    : MonoBehaviour {
	
	//These get set depending on th	e function call used on this class
	public enum allegiance{playerOne,neutral,playerTwo};
	public allegiance alleg;
	//All these are public, so we can modify them all for now.  
	public int unitID, xpos, ypos;//, unitType;
	public int hp,maxHP,atk,mvRange,atkRange,mvCost,atkCost, xp, unitLevel;
	public bool atkd, mvd;
	public string unitName = string.Empty;
	public string info = string.Empty;
	public bool invincible,displayHPBar;

	public readonly int[] XP_TO_LEVEL = {10,20,30,40,50,60,70,80,90,100};

	//unit cost will be utilized here or elsewhere
	//public string unitRole;//name called in switch statement here or elsewhere
	//change unitRole to int if we can do defines for each unit role in this code or elsewhere
	public int unitType;
	public int unitRole;//compare this int to the ints provided inside gameManager or wherever unitRole is compared


	/*
	 * DO NOT DELETE THIS COMMENT!!!!
	 * Jordan Hua
	 * 
	 * Unit Roles:  
	 * 
	 * DO NOT DELETE THIS COMMENT!!!!//Made them 500 in case we need to send as byte to server and back
	 * Rubric-  
	 * Ranged = 500
	 * BuffDebuff = 501
	 * AOE = 502
	 * Utility = 503
	 * BuffDebuff = 504
	 * MeleeTank = 505
	 * Healer = 506
	 * Kingpin = 507
	 * Temporary unit identification.  For now, it's just colors.  We have to assign them
	 * different models later
	 * 
	 * 
	 * unit1: pink //255,153,204
	 * unit2: orange//255,128,0
	 * unit3: yellow//255,255,0
	 * unit4: green//0,255,0
	 * unit5: cyan//0,255,255
	 * unit6: magenta//255,0,255
	 * unit7: brown//102,51,0
	 * unit8: pear //204,255,153
	 * unit9: grey//96,96,96
	 * unit10: black//0,0,0
	 * unit11: white//255,255,255
	 * 
	 * 
	 * 
	 * DO NOT DELETE THIS COMMENT!!!!//Made them 10 to 20 in case we need to send as byte to server and back
	 * 
	 */

	public Texture2D hpBarBG,hpBarHigh,hpBarMedium,hpBarLow;
	public GameManager gm;
	public GameProcess gp;
	public AudioManager am;
	public virtual void Start () {
		unitLevel = 1;
		hpBarBG = Resources.Load("HPBarBG") as Texture2D;
		hpBarHigh = Resources.Load("HPBarHigh") as Texture2D;
		hpBarMedium = Resources.Load("HPBarMedium") as Texture2D;
		hpBarLow = Resources.Load("HPBarLow") as Texture2D;
		am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		if (Application.loadedLevelName.Equals("BoardScene")){
			gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		}
		info = string.Empty;
	}	
	
	void Update () {
	}

	public IEnumerator showDmgDealt(int dmg){
		gameObject.GetComponent<GUIText> ().enabled = true;
		gameObject.GetComponent<GUIText>().text = dmg <= 0? "+" + dmg: "-" + dmg;
		Color temp = gameObject.GetComponent<GUIText>().material.color;
		yield return new WaitForSeconds(2.0f);
		for (int i =0; i < 60; i ++){
			temp.a -= 0.016f;
			gameObject.GetComponent<GUIText>().material.color = temp;
			yield return new WaitForSeconds(0.016666f);
		}
		gameObject.GetComponent<GUIText> ().enabled = false;
	}

	void OnGUI(){
		if (displayHPBar){
			Camera cam = Camera.main;
			Vector3 HPBarPos = cam.WorldToScreenPoint(gameObject.transform.position);
			GUI.DrawTexture (new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-10 < 0?Screen.height:Screen.height - HPBarPos.y-10,  25, 5),hpBarBG);
			float barColorSwitch = (float)hp/maxHP;
			if (barColorSwitch > .6){
				GUI.DrawTexture(new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-10, barColorSwitch * 25, 5),hpBarHigh);
			}else if (barColorSwitch > 0.3){
				GUI.DrawTexture(new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-10, barColorSwitch * 25, 5),hpBarMedium);
			}else{
				GUI.DrawTexture(new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-10, barColorSwitch * 25, 5),hpBarLow);
			}
		}
	}

	void playerSSKillable(){
		foreach (int key in gm.units.Keys){
			if (gm.units[key].unitType == 11 && gm.units[key].alleg == allegiance.playerOne){
				gm.units[key].invincible = false;
				break;
			}
		}
	}

	void enemySSKillable(){
		foreach (int key in gm.units.Keys){
			if (gm.units[key].unitType == 11 && gm.units[key].alleg == allegiance.playerTwo){
				gm.units[key].invincible = false;
				break;
			}
		}
	}

	void OnMouseEnter(){
		//show unit info when hovering over it

		if (Application.loadedLevelName.Equals("BoardScene")){
			refreshUnitText ();
			transform.parent.GetComponent<TileScript> ().OnMouseEnter ();
		}else{
			string info = "Unit Information:\n" +unitName + "\nHP: " + hp + "/" + maxHP;
			info += "\nMovement Range: " + mvRange;
			info += mvCost > 0? "\nMove Cost: " + mvCost : "";
			info += atkCost > 0? "\nAttack Cost: " + atkCost : "";
			info += atk > 0? "\nDamage: " + atk : "";

			GameObject.Find ("SetupScreenUnitInfo").GetComponent<GUIText>().text = info;
		}		
	}

	void OnMouseExit(){
		//clear unit info when not hovering over it
		if (Application.loadedLevelName.Equals("BoardScene")){
			transform.parent.GetComponent<TileScript> ().OnMouseExit ();
			gm.uInfo.text  = "";
		}else {
			GameObject.Find ("SetupScreenUnitInfo").GetComponent<GUIText>().text = "Unit Information:";
		}
	}


	public virtual List<GameObject> showAoEAffectedTiles(TileScript tile){
		List<GameObject> nothing = new List<GameObject> ();
		return nothing;

	}

	void refreshUnitText()
	{
		info = unitName + "\nHP: " + hp + "/" + maxHP;
		info += "\nLevel: " + unitLevel + " Experience: " + xp + "/" + XP_TO_LEVEL[unitLevel-1];
		info += mvCost > 0? "\nMove Cost: " + mvCost : "";
		info += atkCost > 0? "\nAttack Cost: " + atkCost : "";
		info += atk > 0? "\nDamage: " + atk : "";

		if (invincible){
			info+="\nINVINCIBLE";
		}
		if (gm.gs == GameManager.gameState.playerMv && mvd){
			info += "\nAlready moved";
		}
		if (gm.gs == GameManager.gameState.playerAtk && atkd){
			info += "\nAlready attacked";
		}
		gm.uInfo.text = info;
	}

	public void gainXP(){
		xp += 5;
		if (xp >= XP_TO_LEVEL[unitLevel-1]){
			unitLevel ++;
			hp +=5;
			maxHP += 5;
			atk +=5;
			mvRange += 1;
			mvCost = mvCost <=0 ? 0: mvCost-1;
			atkRange += 1;
			atkCost = atkCost <= 0 ? 0: atkCost-1;
			refreshUnitText();
		}
	}

	void OnMouseDown() {
		//Attack this piece if:
		//the game is in attack mode
		//the unit selected is in range of the selected unit

		if (Application.loadedLevelName.Equals("BoardScene") && gm.turn) {
			if (gm.gs == GameManager.gameState.playerAtk 
					&& gm.accessibleTiles.Contains (this.transform.parent.GetComponent<TileScript> ())) {
				transform.parent.GetComponent<TileScript>().attackTile ();
			} else {
				selectUnit ();
				am.playButtonSFX();
			}
		}
	}

	//TODO: move this logic to the server
	public virtual void attackThisUnit(Unit unitThatAttacked){
			
		if (!invincible){
			//gm.combatLog.text = "Combat Log:\nDealt " + unitThatAttacked.atk + " damage!";
			if (unitThatAttacked.atk > 0){
				this.hp -= unitThatAttacked.atk;
			}else{
				//heal unit if it's being attacked by healer
				this.hp -= unitThatAttacked.atk;
				if (hp > maxHP)
					hp = maxHP;
			}
			//if (unitType == 8
			
			//if the unit attacked was killed, remove it from the board and unit list
			if (this.hp <=0){				

				if (unitType == 10){
					if (alleg == allegiance.playerOne){
						playerSSKillable();
					}else{
						enemySSKillable();
					}
				}else if (unitType == 11){
					gm.gameOver = true;
				}


				gm.units.Remove(unitID);


				this.transform.parent.GetComponent<TileScript>().objectOccupyingTile = null;
				Destroy(gameObject);



			}
		}else{
			gm.combatLog.text = "Combat Log:\nTarget is invincible!";
		}
		//clean up the board colors
		gm.accessibleTiles.Clear();
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
		refreshUnitText();

	}

	void selectUnit(){
		gm.selectedUnit = this;
		gm.accessibleTiles.Clear();
		
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
		//if player is moving a piece
		if (gm.gs == GameManager.gameState.playerMv){
			showMvTiles(alleg == allegiance.playerOne? allegiance.playerOne : allegiance.playerTwo);
		//if player is attacking with a piece	
		}else if (gm.gs == GameManager.gameState.playerAtk){
			showAtkTiles();
		}
	}
	
	public void showMvTiles(allegiance ally){
		if (!mvd){
			showMvAccessibleTiles(this.transform.parent.GetComponent<TileScript>(),mvRange,ally);
			//can't move to the tile it's in
			gm.accessibleTiles.Remove(this.transform.parent.GetComponent<TileScript>());	
		}
	}	

	void showMvAccessibleTiles(TileScript tile, int num,allegiance ally){
		TileScript tileS = tile.transform.GetComponent<TileScript>();
		
		if (tileS.objectOccupyingTile == null){
			tile.renderer.material.color = Color.green;
		}
		
		if (num!=0){
			if (tileS.up != null && (tileS.up.GetComponent<TileScript>().objectOccupyingTile ==null  || tileS.up.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == ((ally == allegiance.playerOne) ? allegiance.playerOne: allegiance.playerTwo))){
				showMvAccessibleTiles(tileS.up.GetComponent<TileScript>(),num-1,ally);
				gm.accessibleTiles.Add(tileS.up.GetComponent<TileScript>());
			}
			if (tileS.down != null && (tileS.down.GetComponent<TileScript>().objectOccupyingTile ==null  || tileS.down.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == ((ally == allegiance.playerOne) ? allegiance.playerOne: allegiance.playerTwo))){
				showMvAccessibleTiles(tileS.down.GetComponent<TileScript>(),num-1,ally);
				gm.accessibleTiles.Add(tileS.down.GetComponent<TileScript>());
			}
			if (tileS.left != null && (tileS.left.GetComponent<TileScript>().objectOccupyingTile ==null || tileS.left.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == ((ally == allegiance.playerOne) ? allegiance.playerOne: allegiance.playerTwo))){
				showMvAccessibleTiles(tileS.left.GetComponent<TileScript>(),num-1,ally);
				gm.accessibleTiles.Add(tileS.left.GetComponent<TileScript>());
			}
			if (tileS.right != null && (tileS.right.GetComponent<TileScript>().objectOccupyingTile ==null || tileS.right.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == ((ally == allegiance.playerOne) ? allegiance.playerOne: allegiance.playerTwo))){
				showMvAccessibleTiles(tileS.right.GetComponent<TileScript>(),num-1,ally);
				gm.accessibleTiles.Add(tileS.right.GetComponent<TileScript>());
			}	
		}
	}

	public HashSet<TileScript> getMvAccessibleTiles(allegiance ally){
		HashSet<TileScript> tileSet = new HashSet<TileScript>();
		if (!mvd){
			getMvAccessibleTiles(tileSet,this.transform.parent.GetComponent<TileScript>(),mvRange,ally);
			//can't move to the tile it's in
			//gm.accessibleTiles.Remove(this.transform.parent.GetComponent<TileScript>());	
		}
		return tileSet;
	}
	
	void getMvAccessibleTiles(HashSet<TileScript> list, TileScript tile, int num,allegiance ally){
		TileScript tileS = tile.transform.GetComponent<TileScript> ();
		//print ("ran getMvTile");
		if (num != 0) {
			if (tileS.up != null && (tileS.up.GetComponent<TileScript> ().objectOccupyingTile == null || tileS.up.GetComponent<TileScript> ().objectOccupyingTile.GetComponent<Unit> ().alleg == ((ally == allegiance.playerOne) ? allegiance.playerOne : allegiance.playerTwo))) {
				list.Add (tileS.up.GetComponent<TileScript> ());
				getMvAccessibleTiles (list, tileS.up.GetComponent<TileScript> (), num - 1, ally);
			}
			if (tileS.down != null && (tileS.down.GetComponent<TileScript> ().objectOccupyingTile == null || tileS.down.GetComponent<TileScript> ().objectOccupyingTile.GetComponent<Unit> ().alleg == ((ally == allegiance.playerOne) ? allegiance.playerOne : allegiance.playerTwo))) {
				list.Add (tileS.down.GetComponent<TileScript> ());
				getMvAccessibleTiles (list, tileS.down.GetComponent<TileScript> (), num - 1, ally);
			}
			if (tileS.left != null && (tileS.left.GetComponent<TileScript> ().objectOccupyingTile == null || tileS.left.GetComponent<TileScript> ().objectOccupyingTile.GetComponent<Unit> ().alleg == ((ally == allegiance.playerOne) ? allegiance.playerOne : allegiance.playerTwo))) {
				list.Add (tileS.left.GetComponent<TileScript> ());
				getMvAccessibleTiles (list, tileS.left.GetComponent<TileScript> (), num - 1, ally);
			}
			if (tileS.right != null && (tileS.right.GetComponent<TileScript> ().objectOccupyingTile == null || tileS.right.GetComponent<TileScript> ().objectOccupyingTile.GetComponent<Unit> ().alleg == ((ally == allegiance.playerOne) ? allegiance.playerOne : allegiance.playerTwo))) {
				list.Add (tileS.right.GetComponent<TileScript> ());
				getMvAccessibleTiles (list, tileS.right.GetComponent<TileScript> (), num - 1, ally);
			}	
		}
	}

	public virtual void showAtkTiles(){
		if (!atkd){
			showAtkAccessibleTiles(this.transform.parent.GetComponent<TileScript>(),atkRange);
			gm.accessibleTiles.Remove(this.transform.parent.GetComponent<TileScript>());

			switch(alleg){
				case allegiance.playerOne:
					this.transform.parent.renderer.material.color = Color.blue;
					break;
				case allegiance.playerTwo:
					this.transform.parent.renderer.material.color = Color.red;
					break;
				case allegiance.neutral:
					this.transform.parent.renderer.material.color = Color.gray;
					break;
			}
		}
	}
	
	public void showAtkAccessibleTiles(TileScript tile, int num){
		tile.renderer.material.color = new Color(1f,0.4f,0f, 0f);
		TileScript tileS = tile.transform.GetComponent<TileScript>();
		if (num != 0){
			if (tileS.up != null){
				showAtkAccessibleTiles(tileS.up.GetComponent<TileScript>(),num-1);
				gm.accessibleTiles.Add(tileS.up.GetComponent<TileScript>());
			}
			if (tileS.down != null){
				showAtkAccessibleTiles(tileS.down.GetComponent<TileScript>(),num-1);
				gm.accessibleTiles.Add(tileS.down.GetComponent<TileScript>());
			}
			if (tileS.left != null){
				showAtkAccessibleTiles(tileS.left.GetComponent<TileScript>(),num-1);
				gm.accessibleTiles.Add(tileS.left.GetComponent<TileScript>());
			}
			if (tileS.right != null){
				showAtkAccessibleTiles(tileS.right.GetComponent<TileScript>(),num-1);
				gm.accessibleTiles.Add(tileS.right.GetComponent<TileScript>());
			}
		}
	}
	
	public void makeTree(){
		alleg = allegiance.neutral;
		unitName = "Shrub";
		hp = 1;
		maxHP = 1;
	}
	
}











 