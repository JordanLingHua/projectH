using UnityEngine;
using System.Collections;

#pragma warning disable 0114
public class Mystic: Unit {
	
	public Unit unitFocused;
	public int levelOnFocus;

	void Start () {
		base.Start ();
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
		atk = 0;//change later
		mvRange = 2;
		mvCost = 1;
		atkRange = 3;
		atkCost = 4;	
		//unitRole = "BuffDebuff";
		unitRole = 504;//BuffDebuff
		renderer.material.color = new Color32(255,128,0,1);
	}

	public void revertStatsOfFocused(){
		if (unitFocused != null) {
			string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
			string unitAffectedPlayer = ((gp.playerNumber ==  1 && unitFocused.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && unitFocused.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
			//allied units get only increased mv range and attack damage
			unitFocused.showPopUpText("No Longer Focused!",Color.yellow);

			if (unitFocused.alleg == this.alleg){

				if(levelOnFocus == 3){
					unitFocused.armor -= 3;
				}
				if (levelOnFocus >= 2){
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
		}
	
	}

	public override void takeDmg(Unit unitAttacking,int amt){
		string unitAffectedPlayer = ((gp.playerNumber ==  1 && unitAttacking.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && unitAttacking.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		if (!this.invincible) {
			this.hp -= (amt - this.armor);
			revertStatsOfFocused();
			//if healed up dont let it have more than max hp
			if (hp > maxHP){
				hp = maxHP;
			}
			
			if (amt > 0){
				//taking damage
				gm.addLogToCombatLog(unitAffectedPlayer + unitAttacking.unitName +" attacked "+ player + unitName + " for " + (amt - this.armor) + " damage!");
				showPopUpText("-" + amt,Color.red);
			}else{
				//getting healed
				gm.addLogToCombatLog(unitAffectedPlayer + unitAttacking.unitName +" healed "+ player + unitName + " for " + (-1*amt));
				showPopUpText("+" + (-1*amt),Color.green);
			}
			
			if (this.hp <= 0) {				
				//Kill unit and remove from game
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
				if (unitLevel >= 2){
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
}
