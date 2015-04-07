	using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0114
public class Mystic: Unit {
	
	public Unit unitFocused;
	public int levelOnFocus;
	public GameObject partSys;
	public GameObject partSys2;
	public float origX, origY, origZ, offsetX, offsetY, offsetZ, origX2, origY2, origZ2;

	void Start () {
		base.Start ();
		unitPortrait = Resources.Load("MysticPortrait") as Texture2D;
		unitFocused = null;
		levelBonusShort [0] = "Greater Focus";
		levelBonusShort [1] = "Mighty Focus";
		levelBonusLong [0] = "Focusing allies gives them +3 attack damage\nFocusing enemies increases damage they take by 2";
		levelBonusLong [1] = "Focusing allies reduces damage they take by 3\nFocusing enemies increases damage they take by 4";
		description = "A powerful wizard that can paralyze enemy units or give allies increased mobility";
		unitType = 2;
		unitName = "Mystic";
		hp = 30;
		maxHP = 30;
		atk = 0;
		mvRange = 2;
		mvCost = 1;
		atkRange = 3;
		atkCost = 4;	
		//unitRole = "BuffDebuff";
		unitRole = 504;//BuffDebuff
		renderer.material.color = new Color32(255,128,0,1);
	}


	//mystic cannot friendly fire, it buffs allies so it should never show a friendly fire tip
	public override HashSet<TileScript> getFriendlyFireHitTiles(){
		HashSet<TileScript> ret = new HashSet<TileScript>();
		return ret;
	}

	public void revertStatsOfFocused(){
		GameObject.Destroy (partSys);
		GameObject.Destroy (partSys2);

		if (unitFocused != null) {
			string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
			string unitAffectedPlayer = ((gp.playerNumber ==  1 && unitFocused.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && unitFocused.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
			//allied units get only increased mv range and attack damage
			unitFocused.showPopUpText("No Longer Focused!",Color.yellow);

			if (unitFocused.alleg == this.alleg){
				unitFocused.mvRange -= 2;
				if(levelOnFocus == 3){
					unitFocused.armor -= 3;
				}
				if (levelOnFocus >= 2 && unitFocused.unitType != 8){
					unitFocused.atk -= 3;
				}

			}else if (unitFocused.alleg != this.alleg){
				unitFocused.enemyFocusingThis = null;
				unitFocused.paralyzed = false;
				if (levelOnFocus == 3){
					unitFocused.armor += 4;
				}else if (levelOnFocus == 2){
					unitFocused.armor +=2;
				}
			}

			showPopUpText("Lost Focus!",Color.red);
			gm.addLogToCombatLog(player + this.unitName + " lost focus of " + unitAffectedPlayer + unitFocused.unitName);
			unitFocused = null;
		}
	
	}

	public override void takeDmg(Unit unitAttacking,int amt){

		//yield return new WaitForSeconds (1.0f);

		string unitAffectedPlayer = ((gp.playerNumber ==  1 && unitAttacking.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && unitAttacking.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		if (!this.invincible) {

			if (amt > 0){
				//taking damage
				this.hp -= (amt - this.armor);
				gm.addLogToCombatLog(unitAffectedPlayer + unitAttacking.unitName +" attacked "+ player + unitName + " for " + (amt - this.armor) + " damage!");
				showPopUpText("-" + (amt - this.armor),Color.red);
				revertStatsOfFocused();
			
			}else{
				//getting healed
				this.hp -= amt;
				
				//if healed up dont let it have more than max hp
				if (hp > maxHP){
					hp = maxHP;
				}

				gm.addLogToCombatLog(unitAffectedPlayer + unitAttacking.unitName +" healed "+ player + unitName + " for " + (-1*amt));
				if (amt == -500){
					showPopUpText("Full Heal",Color.green);
				}else{
					showPopUpText("+" + (-1*amt),Color.green);
				}
			}
			
			if (this.hp <= 0) {				
				//Kill unit and remove from game
				gm.addUnitToGraveyard(this.unitName,this.alleg);
				gm.addLogToCombatLog (this.unitName + " was killed!");
				gm.units.Remove (this.unitID);
				this.transform.parent.GetComponent<TileScript> ().objectOccupyingTile = null;
				Destroy (this.gameObject);
			}
		}else{
			showPopUpText("Invincible!",Color.red);
			gm.addLogToCombatLog(unitAttacking.unitName +" attacked "+ unitName + " but it was invincible!");

		}
	}

	//Unit Focused needs to take dmg after turn ends if level 3 and enemy unit
	public override void attackUnit(Unit unitAffected){
		
		if (unitFocused != null) {
			revertStatsOfFocused();		
		}

		addParticles (unitAffected);
		string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		string unitAffectedPlayer = ((gp.playerNumber ==  1 && unitAffected.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && unitAffected.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		atkd = true;
		if (!unitAffected.invincible){
			levelOnFocus = unitLevel;
			unitFocused = unitAffected;
			//focusing enemy unit
			if (unitFocused.alleg != this.alleg){
				//focusing enemy mystic
				unitFocused.enemyFocusingThis = this;
				if (unitAffected.unitType == 2){
					(unitAffected as Mystic).revertStatsOfFocused();
				}
				unitFocused.paralyzed = true;
				if (unitLevel == 3){
					unitFocused.armor -= 4;
				}else if (unitLevel == 2){
					unitFocused.armor -= 2;
				}
				unitAffected.showPopUpText("Focused!",Color.red);

				//focusing ally
			}else if (unitFocused.alleg == this.alleg){
				unitAffected.mvRange += 2;
				unitAffected.showPopUpText("Focused!",Color.green);
				if (unitLevel >= 2 && unitAffected.unitType != 8){
					unitAffected.atk += 3;
				}
				if (unitLevel == 3){
					unitAffected.armor += 3;
				}
			}
			gm.addLogToCombatLog(player + unitName +" focused "+ unitAffectedPlayer +  unitAffected.unitName);
		}else{
			unitAffected.showPopUpText("Invincible!",Color.red);
		}
		//clean up the board colors
		gm.accessibleTiles.Clear();
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
	}

	void addParticles(Unit unitFocused)
	{
		int playerNumber = GameObject.Find ("GameProcess").GetComponent<GameProcess> ().playerNumber;
		if (alleg == allegiance.playerOne)
		{
			partSys = (GameObject)Instantiate (Resources.Load ("MysticFocusBlue"));
			partSys2 = (GameObject)Instantiate (Resources.Load ("MysticFocusBlue"));			
		} 

		else 
		{
			partSys = (GameObject)Instantiate (Resources.Load ("MysticFocusRed"));			
			partSys2 = (GameObject)Instantiate (Resources.Load ("MysticFocusRed"));
		}

		 
		partSys.transform.parent = this.transform;
		partSys.transform.position = this.transform.position;
		partSys.transform.Translate (0.0f, 0.0f, -4.0f);
		origX = partSys.transform.position.x;
		origY = partSys.transform.position.y;
		origZ = partSys.transform.position.z;


		partSys2.transform.parent = unitFocused.transform;
		partSys2.transform.position = unitFocused.transform.position;
		partSys2.transform.Translate (0.0f, 0.0f, -4.0f);
		origX2 = partSys2.transform.position.x;
		origY2 = partSys2.transform.position.y;
		origZ2 = partSys2.transform.position.z;

		if (Application.loadedLevelName.Equals ("BoardScene") && (playerNumber == 2))
		{
			Debug.Log("switcheroo!");
			partSys.transform.rotation = this.transform.rotation;
			partSys2.transform.rotation = unitFocused.transform.rotation;

			partSys.transform.Rotate(300.0f, 0.0f, 0.0f);
			partSys.transform.position = new Vector3 (origX, origY, origZ + 5.33f);

			partSys2.transform.Rotate(300.0f, 0.0f, 0.0f);
			partSys2.transform.position = new Vector3 (origX2, origY2, origZ2 + 5.33f);
		}

	}
}
