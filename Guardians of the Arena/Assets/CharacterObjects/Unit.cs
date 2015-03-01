using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Unit    : MonoBehaviour {
	
	//These get set depending on th	e function call used on this class
	public enum allegiance{playerOne,neutral,playerTwo};
	public allegiance alleg;
	//All these are public, so we can modify them all for now.  
	public int unitID, xpos, ypos;//, unitType;
	public int hp,maxHP,atk,mvRange,atkRange,mvCost,atkCost, xp, unitLevel,popUpTextNum,armor;
	public bool atkd, mvd,paralyzed;
	public string unitName = string.Empty;
	public string info = string.Empty;
	public string description = string.Empty;
	public string[] levelBonusShort = {"",""};
	public string[] levelBonusLong = {"",""};
	public bool invincible,displayHPBar,displayXPBar;
	public int barXOffset, barYOffset;
	public Mystic enemyFocusingThis;


	public GameObject popUpText;

	public readonly int[] XP_TO_LEVEL = {20,20,100000};

	//unit cost will be utilized here or elsewhere
	//public string unitRole;//name called in switch statement here or elsewhere
	//change unitRole to int if we can do defines for each unit role in this code or elsewhere
	public int unitType;
	public int unitRole;//compare this int to the ints provided inside gameManager or wherever unitRole is compared




	public Texture2D hpBarBG,hpBarHigh,hpBarMedium,hpBarLow,xpBar,level2Symbol,level3Symbol;
	public GameManager gm;
	public GameProcess gp;
	public PopUpMenuNecro pum;
	public AudioManager am;
	public virtual void Start () {
		barXOffset = 15;
		barYOffset = 25;
		armor = 0;
		unitLevel = 1;
		level2Symbol = Resources.Load("Level2Symbol") as Texture2D;
		level3Symbol = Resources.Load("Level3Symbol") as Texture2D;
		popUpText = GameObject.Find ("popUpText");
		hpBarBG = Resources.Load("HPBarBG") as Texture2D;
		hpBarHigh = Resources.Load("HPBarHigh") as Texture2D;
		hpBarMedium = Resources.Load("HPBarMedium") as Texture2D;
		hpBarLow = Resources.Load("HPBarLow") as Texture2D;
		xpBar = Resources.Load("XPBar") as Texture2D;
		am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		pum = GameObject.Find ("PopUpMenu").GetComponent<PopUpMenuNecro> ();
		if (Application.loadedLevelName.Equals("BoardScene") || Application.loadedLevelName.Equals("AIScene")){
			gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		}
		info = string.Empty;
	}	

	public void showPopUpText(string affect,Color newColor){
		if (gameObject != null) {
			Camera cam = Camera.main;
			GUI.depth = -1;
			Vector3 textPos = cam.WorldToScreenPoint (gameObject.transform.position);
			textPos.x = (textPos.x - 10) / Screen.width;
			textPos.y = (textPos.y + 20 + (10 * popUpTextNum)) / Screen.height;
			textPos.z = 0;
			GameObject text = (GameObject)Instantiate (popUpText, textPos, Quaternion.identity);
			popUpTextNum++;
			text.GetComponent<popUpTextScript> ().StartCoroutine (text.GetComponent<popUpTextScript> ().showText (this, affect, newColor));
		}
	}

	void OnGUI(){
		Camera cam = Camera.main;
		Vector3 unitScreenPos = cam.WorldToScreenPoint(gameObject.transform.position);
//		if (unitLevel == 2){
//			GUI.DrawTexture (new Rect(unitScreenPos.x+10, Screen.height - unitScreenPos.y+10,  15, 15),level2Symbol);
//		}else if (unitLevel == 3){
//			GUI.DrawTexture (new Rect(unitScreenPos.x+10, Screen.height - unitScreenPos.y+10,  15, 15),level3Symbol);
//		}

		if (displayHPBar){
			if (unitType == 20){
				unitScreenPos.y -= 10;
				unitScreenPos.x -= -5;
			}
			GUI.depth = 10;
			GUI.DrawTexture (new Rect(unitScreenPos.x-barXOffset, Screen.height - unitScreenPos.y-barYOffset,  25, 3),hpBarBG);
			float barColorSwitch = (float)hp/maxHP;
			if (barColorSwitch > .6){
				GUI.DrawTexture(new Rect(unitScreenPos.x-barXOffset, Screen.height - unitScreenPos.y-barYOffset, barColorSwitch * 25, 3),hpBarHigh);
			}else if (barColorSwitch > 0.3){
				GUI.DrawTexture(new Rect(unitScreenPos.x-barXOffset, Screen.height - unitScreenPos.y-barYOffset, barColorSwitch * 25, 3),hpBarMedium);
			}else if (barColorSwitch > 0){
				GUI.DrawTexture(new Rect(unitScreenPos.x-barXOffset, Screen.height - unitScreenPos.y-barYOffset, barColorSwitch * 25, 3),hpBarLow);
			}
		}
		if (displayXPBar){
			GUI.DrawTexture (new Rect(unitScreenPos.x-15, Screen.height - unitScreenPos.y-(barYOffset-3),  25, 3),hpBarBG);
			GUI.DrawTexture(new Rect(unitScreenPos.x-15, Screen.height - unitScreenPos.y-(barYOffset-3), ((float)xp/XP_TO_LEVEL[unitLevel-1])* 25, 3),xpBar);
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

	void OnMouseOver(){
		//show unit info when hovering over it

		if (Application.loadedLevelName.Equals("BoardScene") || Application.loadedLevelName.Equals("AIScene")){
			transform.parent.GetComponent<TileScript> ().OnMouseOver ();
			gm.hoverOverUnit = this; 
			if (enemyFocusingThis != null){
				enemyFocusingThis.gameObject.GetComponentInParent<TileScript>().renderer.material.color = enemyFocusingThis.alleg == Unit.allegiance.playerOne ? Color.magenta : Color.black;
			}
		}else{
			//used for setup screen info
			string info ="\nHP: " + hp + "/" + maxHP;
			if (atk > 0){
				info +="\nDamage: " + atk; 
			} else if (atk == 0){
				info += "";
			}else {
				info += "\nHeals for: " + -1*atk;
			}
			info += mvCost > 0? "\nMove Cost: " + mvCost : "";
			info += mvRange >0? "\nMove Range: " + mvRange : "";
			info += atkCost > 0? "\nAttack Cost: " + atkCost : "";
			info += atkRange > 0? "\nAttack Range: " + atkRange: "";


			GameObject.Find ("Level1BonusNameGUIText").GetComponent<GUIText>().text = (unitType == 11)?"" :"Lvl.2 Bonus: " + levelBonusShort[0];
			GameObject.Find ("Level1BonusDescriptionGUIText").GetComponent<GUIText>().text =  wordWrap(25,levelBonusLong[0]);
			GameObject.Find ("Level2BonusNameGUIText").GetComponent<GUIText>().text = (unitType == 11)?"" :"Lvl.3 Bonus: " + levelBonusShort[1];
			GameObject.Find ("Level2BonusDescriptionGUIText").GetComponent<GUIText>().text = wordWrap(25,levelBonusLong[1]);

			GameObject.Find ("SetupScreenUnitInfo").GetComponent<GUIText>().text = info;
			if (unitName == "Soulstone"){
				GameObject.Find("UnitDescription").transform.position = new Vector3(0.7f,0.88f);
			}else if (unitName == "Mystic"){
				GameObject.Find("UnitDescription").transform.position = new Vector3(0.7f,0.62f);
				GameObject.Find("Level2BonusNameGUIText").transform.position = new Vector3(0.82f,0.77f);
				GameObject.Find("Level2BonusDescriptionGUIText").transform.position = new Vector3(0.82f,0.74f);
			}
			GameObject.Find ("UnitNameInfo").GetComponent<GUIText>().text = unitName;
			GameObject.Find("UnitDescription").GetComponent<GUIText>().text = wordWrap(40, description);
		}		
	}

	public string wordWrap (int length,string word){
		string ret = "";
		int tempLength = 0;
		string[] tokens = word.Split(new string[] {" "}, StringSplitOptions.None);
		for (int i = 0; i < tokens.Length; i++)
		{
			if (tokens[i].Contains("\n")){				
				string temp = tokens[i].Substring(0,tokens[i].IndexOf("\n"));
				string temp2 = tokens[i].Substring(tokens[i].IndexOf("\n")+1);
	

				if (tempLength + temp.Length <= length){
					ret+= temp + "\n" + temp2 + " ";
					tempLength = temp2.Length;
				}else{
					ret += "\n"+ temp + "\n" + temp2;
					tempLength = temp2.Length;
				}

			}else {
				if (tempLength + tokens[i].Length <= length){
				ret+= tokens[i] + " ";
				tempLength += tokens[i].Length;	
				}else{
					ret += "\n"+ tokens[i] + " ";
					tempLength = tokens[i].Length;
				}
			}
		}
		return ret;
	}


	void OnMouseExit(){
		//clear unit info when not hovering over it
		if (Application.loadedLevelName.Equals("BoardScene") || Application.loadedLevelName.Equals("AIScene")){
			transform.parent.GetComponent<TileScript> ().OnMouseExit ();
			gm.hoverOverUnit = null;
			gm.clearHoverUnitInfo();
			if (enemyFocusingThis != null){
				enemyFocusingThis.gameObject.GetComponentInParent<TileScript>().renderer.material.color = enemyFocusingThis.alleg == allegiance.playerOne? Color.blue: Color.red;
			}
			//gm.uInfo.text  = "";
		}else {
			GameObject.Find ("SetupScreenUnitInfo").GetComponent<GUIText>().text = "";
			GameObject.Find ("Level1BonusNameGUIText").GetComponent<GUIText>().text = "";
			GameObject.Find ("Level1BonusDescriptionGUIText").GetComponent<GUIText>().text = "";
			GameObject.Find ("Level2BonusNameGUIText").GetComponent<GUIText>().text = "";
			GameObject.Find ("Level2BonusDescriptionGUIText").GetComponent<GUIText>().text = "";
			GameObject.Find ("UnitNameInfo").GetComponent<GUIText>().text = "";
			GameObject.Find("UnitDescription").GetComponent<GUIText>().text = "";
			GameObject.Find("UnitDescription").transform.position = new Vector3(0.7f,0.71f);
			GameObject.Find("Level2BonusNameGUIText").transform.position = new Vector3(0.82f,0.83f);
			GameObject.Find("Level2BonusDescriptionGUIText").transform.position = new Vector3(0.82f,0.8f);
		}
	}


	public virtual List<GameObject> showAoEAffectedTiles(TileScript tile){
		List<GameObject> nothing = new List<GameObject> ();
		return nothing;
	}

	public virtual void gainLevelTwoBonus (){

	}
	public virtual void gainLevelThreeBonus(){

	}
	public virtual void gainXP(){
		xp += 20;

		if (xp >= XP_TO_LEVEL [unitLevel - 1]) {
			xp = 0;
			unitLevel ++;
			if (unitLevel == 2){
				gainLevelTwoBonus();
			}else if (unitLevel == 3){
				gainLevelThreeBonus();
			}
			if ((alleg == allegiance.playerOne && gp.playerNumber == 1) || (alleg == allegiance.playerTwo && gp.playerNumber == 2)){
				gm.addLogToCombatLog("Your " + unitName + " has leveled up to level " + unitLevel + "!");
			}
			if ((alleg == allegiance.playerTwo && gp.playerNumber == 1) || (alleg == allegiance.playerOne && gp.playerNumber == 2)){
				gm.addLogToCombatLog("Opponent's " + unitName + " has leveled up to level " + unitLevel + "!");
			}
			showPopUpText("Leveled Up!",Color.yellow);
		} else {
			if (unitLevel!=3){
				showPopUpText("XP+5!",Color.magenta);
			}
		}
	}

	void OnMouseDown() {
		//Attack this piece if:
		//the game is in attack mode
		//the unit selected is in range of the selected unit

		if (Application.loadedLevelName.Equals("BoardScene") || Application.loadedLevelName.Equals("AIScene")) {
			if (gm.gs == GameManager.gameState.playerAtk && gm.accessibleTiles.Contains (this.transform.parent.GetComponent<TileScript> ())) {
				transform.parent.GetComponent<TileScript>().attackTile ();
			} else {
				if ((this.alleg == allegiance.neutral || (this.alleg == allegiance.playerOne && gp.playerNumber == 1 || this.alleg == allegiance.playerTwo && gp.playerNumber == 2 )) || pum.allowEnemyUnitSelection){
					selectUnit ();
					am.playButtonSFX();
				}
			}
		}
	}



	public virtual void takeDmg(Unit unitAttacking,int amt){

		//yield return new WaitForSeconds (1.0f);

		string unitAffectedPlayer = ((gp.playerNumber == 1 && unitAttacking.alleg == allegiance.playerOne) || (gp.playerNumber == 2 && unitAttacking.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		string player = "";
		if ((gp.playerNumber == 1 && this.alleg == allegiance.playerOne) || (gp.playerNumber == 2 && this.alleg == allegiance.playerTwo)) {
				player = "Your ";
		} else if ((gp.playerNumber == 1 && this.alleg == allegiance.playerTwo) || (gp.playerNumber == 2 && this.alleg == allegiance.playerOne)){
				player = "Opponent's ";
		}
		if (!this.invincible) {
			if (amt > 0){
				//taking damage
				this.hp -= (amt - this.armor);
				gm.addLogToCombatLog(unitAffectedPlayer + unitAttacking.unitName +" attacked "+ player + unitName + " for " + (amt - this.armor) + " damage!");
				showPopUpText("-" + (amt - this.armor),Color.red);
			}else{
				//getting healed
				this.hp -= amt;
				//if healed up dont let it have more than max hp
				if (hp > maxHP){
					hp = maxHP;
				}
				gm.addLogToCombatLog(unitAffectedPlayer + unitAttacking.unitName +" healed "+ player + unitName + " for " + (-1*amt));
				showPopUpText("+" + (-1*amt),Color.green);
			}

			//If the unit's hp is destroyed, we should switch to 
			if (this.hp <= 0) {	

				//If the unit's hp is destroyed
				if(this.unitName == "Barrel")
				{
					this.GetComponent<Animator>().Play("barrel_break");
					gm.addLogToCombatLog (this.unitName + " was killed!");
				}else{
					//Kill unit and remove from game
					gm.addUnitToGraveyard(this.unitName,this.alleg);
					gm.addLogToCombatLog (this.unitName + " was killed!");
					gm.units.Remove (this.unitID);
					this.transform.parent.GetComponent<TileScript> ().objectOccupyingTile = null;
					Destroy (this.gameObject);
				}


			}
		}else{
			showPopUpText("Invincible!",Color.red);

			gm.addLogToCombatLog(unitAttacking.unitName +" attacked "+ unitName + " but it was invincible!");

		}
	}

	public virtual void attackUnit(Unit unitAffected){
		atkd = true;
		unitAffected.takeDmg(this,this.atk);
		//clean up the board colors
		gm.accessibleTiles.Clear();
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();

		/*
		AnimatorStateInfo stateInfo = this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
		int x = 0;
		while(x < stateInfo.normalizedTime*Time.deltaTime)
			x++;
			*/
		/*
		if(this.GetComponent<Animator>().GetInteger("mode_and_dir")==8){
			this.GetComponent<Animator>().SetInteger("mode_and_dir", 0);
		}
		else if(this.GetComponent<Animator>().GetInteger("mode_and_dir")==9){
			this.GetComponent<Animator>().SetInteger("mode_and_dir", 1);
		}	
		else if(this.GetComponent<Animator>().GetInteger("mode_and_dir")==10){
			this.GetComponent<Animator>().SetInteger("mode_and_dir", 2);
		}
		else if(this.GetComponent<Animator>().GetInteger("mode_and_dir")==11){
			this.GetComponent<Animator>().SetInteger("mode_and_dir", 3);
		}
		*/


	}

	public void selectUnit(){
		gm.clearSelection();
		gm.selectedUnit = this;
		gm.accessibleTiles.Clear();
		
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
		//if player is moving a piece
		//if (gm.gs == GameManager.gameState.playerMv){
		if (!mvd || (mvd && atkd)) {
			gm.changeToMoving();
			showMvTiles (alleg == allegiance.playerOne ? allegiance.playerOne : allegiance.playerTwo);
			//if player is attacking with a piece	
		}else{
			gm.changeToAttacking();
		//}else if (gm.gs == GameManager.gameState.playerAtk){
			showAtkTiles();
		}
	}
	
	public virtual void showMvTiles(allegiance ally){
		if (!paralyzed){
			HashSet<TileScript> tiles = getMvAccessibleTiles(ally);
			tiles.Remove(this.transform.parent.GetComponent<TileScript>());
			gm.accessibleTiles = tiles;
			foreach (TileScript tile in tiles){
				if (tile.objectOccupyingTile == null){
				tile.renderer.material.color = Color.green;
				}
			}
		}

	}	

	public virtual void resetUnitAbilities(){
		atkd = false;
		mvd = false;
	}

	public HashSet<TileScript> getMvAccessibleTiles(allegiance ally){
		HashSet<TileScript> tileSet = new HashSet<TileScript>();
		getMvAccessibleTiles(tileSet,this.transform.parent.GetComponent<TileScript>(),mvRange,ally);
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


	public virtual HashSet<TileScript> getAtkAccessibleTiles(){
		HashSet<TileScript> tileSet = new HashSet<TileScript>();
		getAtkAccessibleTiles(tileSet,this.transform.parent.GetComponent<TileScript>(),atkRange);
		return tileSet;
	}

	void getAtkAccessibleTiles(HashSet<TileScript> list,TileScript tile, int num){
		TileScript tileS = tile.transform.GetComponent<TileScript>();
		if (num != 0){
			if (tileS.up != null){
				list.Add (tileS.up.GetComponent<TileScript> ());
				getAtkAccessibleTiles (list, tileS.up.GetComponent<TileScript> (), num - 1);
			}
			if (tileS.down != null){
				list.Add (tileS.down.GetComponent<TileScript> ());
				getAtkAccessibleTiles (list, tileS.down.GetComponent<TileScript> (), num - 1);
			}
			if (tileS.left != null){
				list.Add (tileS.left.GetComponent<TileScript> ());
				getAtkAccessibleTiles (list, tileS.left.GetComponent<TileScript> (), num - 1);
			}
			if (tileS.right != null){
				list.Add (tileS.right.GetComponent<TileScript> ());
				getAtkAccessibleTiles (list, tileS.right.GetComponent<TileScript> (), num - 1);
			}
		}
	}
	
	
	public virtual void showAtkTiles(){
		if (!paralyzed){

			HashSet<TileScript> tiles = getAtkAccessibleTiles();
			//tiles.Remove(this.transform.parent.GetComponent<TileScript>());
			gm.accessibleTiles = tiles;
			foreach (TileScript tile in tiles){
				tile.renderer.material.color = new Color(1f,0.4f,0f, 0f);
			}
		}
	}

	void showAtkAccessibleTiles(TileScript tile, int num){
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

}