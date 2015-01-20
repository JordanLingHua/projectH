using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit    : MonoBehaviour {
	
	//These get set depending on th	e function call used on this class
	public enum allegiance{playerOne,neutral,playerTwo};
	public allegiance alleg;
	//All these are public, so we can modify them all for now.  
	public int unitID, xpos, ypos;//, unitType;
	public int hp,maxHP,atk,mvRange,atkRange,mvCost,atkCost, xp, unitLevel,popUpTextNum;
	public bool atkd, mvd;
	public string unitName = string.Empty;
	public string info = string.Empty;
	public bool invincible,displayHPBar,displayXPBar;
	public Mystic mysticFocusingThis;


	public GameObject popUpText;

	public readonly int[] XP_TO_LEVEL = {20,20,100000};

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

	public Texture2D hpBarBG,hpBarHigh,hpBarMedium,hpBarLow,xpBar;
	public GameManager gm;
	public GameProcess gp;
	public PopUpMenuNecro pum;
	public AudioManager am;
	public virtual void Start () {
		unitLevel = 1;
		popUpText = GameObject.Find ("popUpText");
		hpBarBG = Resources.Load("HPBarBG") as Texture2D;
		hpBarHigh = Resources.Load("HPBarHigh") as Texture2D;
		hpBarMedium = Resources.Load("HPBarMedium") as Texture2D;
		hpBarLow = Resources.Load("HPBarLow") as Texture2D;
		xpBar = Resources.Load("XPBar") as Texture2D;
		am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		pum = GameObject.Find ("PopUpMenu").GetComponent<PopUpMenuNecro> ();
		if (Application.loadedLevelName.Equals("BoardScene")){
			gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		}
		info = string.Empty;
	}	
	
	void Update () {
	}

	public void showPopUpText(string affect,Color newColor){
		Camera cam = Camera.main;
		GUI.depth = -1;
		Vector3 textPos = cam.WorldToScreenPoint(gameObject.transform.position);
		textPos.x = (textPos.x - 10) / Screen.width;
		textPos.y = (textPos.y + 20 + (10 * popUpTextNum)) / Screen.height;
		textPos.z = 0;
		GameObject text = (GameObject) Instantiate(popUpText,textPos,Quaternion.identity);
		popUpTextNum++;
		text.GetComponent<popUpTextScript> ().StartCoroutine (text.GetComponent<popUpTextScript> ().showText (this, affect, newColor));
	}

	void OnGUI(){
		if (displayHPBar){
			Camera cam = Camera.main;
			Vector3 HPBarPos = cam.WorldToScreenPoint(gameObject.transform.position);
			GUI.DrawTexture (new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-20,  25, 3),hpBarBG);
			float barColorSwitch = (float)hp/maxHP;
			if (barColorSwitch > .6){
				GUI.DrawTexture(new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-20, barColorSwitch * 25, 3),hpBarHigh);
			}else if (barColorSwitch > 0.3){
				GUI.DrawTexture(new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-20, barColorSwitch * 25, 3),hpBarMedium);
			}else{
				GUI.DrawTexture(new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-20, barColorSwitch * 25, 3),hpBarLow);
			}
		}
		if (displayXPBar){
			Camera cam = Camera.main;
			Vector3 HPBarPos = cam.WorldToScreenPoint(gameObject.transform.position);
			GUI.DrawTexture (new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-17,  25, 3),hpBarBG);
			GUI.DrawTexture(new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-17, ((float)xp/XP_TO_LEVEL[unitLevel-1])* 25, 3),xpBar);
		}
	}

	public void playerSSKillable(){
		foreach (int key in gm.units.Keys){
			if (gm.units[key].unitType == 11 && gm.units[key].alleg == allegiance.playerOne){
				gm.units[key].invincible = false;
				break;
			}
		}
	}

	public void enemySSKillable(){
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
			transform.parent.GetComponent<TileScript> ().OnMouseEnter ();
		// this was for hovered over unit info - not used
		//	refreshUnitText ();
		}else{
			//used for setup screen info
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
			//gm.uInfo.text  = "";
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
		info += "\nLevel: " + unitLevel;
		info += unitLevel == 3? "" :" Experience: " + xp + "/" + XP_TO_LEVEL[unitLevel-1];
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
	//	gm.uInfo.text = info;
	}

	public virtual void gainXP(){
		xp += 5;

		if (xp >= XP_TO_LEVEL [unitLevel - 1]) {
			xp = 0;
			unitLevel ++;
			refreshUnitText ();
			showPopUpText("Leveled Up!",Color.yellow);
		} else {
			showPopUpText("XP+5!",Color.magenta);
		}
	}

	void OnMouseDown() {
		//Attack this piece if:
		//the game is in attack mode
		//the unit selected is in range of the selected unit

		if (Application.loadedLevelName.Equals("BoardScene")) {
			if (gm.gs == GameManager.gameState.playerAtk && gm.accessibleTiles.Contains (this.transform.parent.GetComponent<TileScript> ())) {
				transform.parent.GetComponent<TileScript>().attackTile ();
			} else {
				if ((this.alleg == allegiance.playerOne && gp.playerNumber == 1 || this.alleg == allegiance.playerTwo && gp.playerNumber == 2 ) || pum.allowEnemyUnitSelection){
					selectUnit ();
					am.playButtonSFX();
				}
			}
		}
	}

	//TODO: move this logic to the server
	public virtual void attackUnit(Unit unitAffected){
		string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		string unitAffectedPlayer = ((gp.playerNumber ==  1 && unitAffected.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && unitAffected.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		atkd = true;

		if (!unitAffected.invincible){
			//gm.combatLog.text = "Combat Log:\nDealt " + unitThatAttacked.atk + " damage!";
			if (this.atk > 0){
				//block dmg if killing guardian lvl 2
				if (unitAffected.unitType == 10 && unitAffected.unitLevel >=2 && this.atk > 10){
					unitAffected.hp -= 10;
					unitAffected.showPopUpText("-10 "+ (this.atk-10) + "blocked",Color.red);
				}else{
					if(unitAffected.unitType == 2){
						Mystic x = unitAffected as Mystic;
						x.revertStatsOfFocused();
					}	
					unitAffected.hp -= this.atk;
					unitAffected.showPopUpText("-" + this.atk,Color.red);
				}

			}else{
				unitAffected.hp -= this.atk;
				unitAffected.showPopUpText("+" + (-1*this.atk),Color.green);
			}

			//if healed up dont let it have more than max hp
			if (unitAffected.hp > unitAffected.maxHP){
				unitAffected.hp = unitAffected.maxHP;
			}
			
			//if the unit attacked was killed, remove it from the board and unit list
			if (unitAffected.hp <=0){				

				//Kill Guardian then SS vulnerable
				if (unitAffected.unitType == 10){
					if (unitAffected.alleg == allegiance.playerOne){
						playerSSKillable();
					}else{
						enemySSKillable();
					}
				
				}else if (unitAffected.unitType == 11){
					gm.gameOver = true;
				}

				//Kill unit and remove from game
				gm.addLogToCombatLog(unitAffectedPlayer + unitAffected.unitName + " was killed!");
				gm.units.Remove(unitAffected.unitID);
				unitAffected.transform.parent.GetComponent<TileScript>().objectOccupyingTile = null;
				Destroy(unitAffected.gameObject);
			}
		}else{
			unitAffected.showPopUpText("Invincible!",Color.red);
			gm.addLogToCombatLog(this.unitName +" attacked "+ unitAffected.unitName + " but it was invincible!");
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
	
	public virtual void showMvTiles(allegiance ally){
		showMvAccessibleTiles(this.transform.parent.GetComponent<TileScript>(),mvRange,ally);
		//can't move to the tile it's in
		gm.accessibleTiles.Remove(this.transform.parent.GetComponent<TileScript>());	

	}	

	public virtual void resetUnitAbilities(){
		atkd = false;
		mvd = false;
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











 