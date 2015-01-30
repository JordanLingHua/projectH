using UnityEngine;
using System.Collections;

#pragma warning disable 0114
public class Mystic: Unit {
	
	public Unit unitFocused;
	public int oldMvRange, oldAtkRange,oldAtkDmg;

	void Start () {
		base.Start ();
		unitFocused = null;
		levelBonusShort [0] = "Enchant Might";
		levelBonusShort [1] = "Crippling Focus";
		levelBonusLong [0] = "Allied focused units now gain\n+5 attack damage";
		levelBonusLong [1] = "Enemy focused units now take 8\ndamage at the end of every your turn";
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
			unitFocused.mysticFocusingThis = null;
			unitFocused.showPopUpText("No Longer Focused!",Color.yellow);
			unitFocused.mvRange = oldMvRange;
			unitFocused.atkRange = oldAtkRange;
			unitFocused.atk = oldAtkDmg;

			if (unitFocused.alleg != this.alleg){
				unitFocused.paralyzed = false;
			}
			showPopUpText("Lost Focus!",Color.red);
			gm.addLogToCombatLog(player + this.unitName + " lost focus of " + unitAffectedPlayer + unitFocused.unitName);
		}
	
	}

	public override void takeDmg(Unit unitAttacking,int amt){
		string unitAffectedPlayer = ((gp.playerNumber ==  1 && unitAttacking.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && unitAttacking.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		if (!this.invincible) {
			this.hp -= amt;
			revertStatsOfFocused();
			//if healed up dont let it have more than max hp
			if (hp > maxHP){
				hp = maxHP;
			}
			
			if (amt > 0){
				//taking damage
				gm.addLogToCombatLog(unitAffectedPlayer + unitAttacking.unitName +" attacked "+ player + unitName + " for " + unitAttacking.atk + " damage!");
				showPopUpText("-" + amt,Color.red);
			}else{
				//getting healed
				gm.addLogToCombatLog(unitAffectedPlayer + unitAttacking.unitName +" healed "+ player + unitName + " for " + (-1*unitAttacking.atk));
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
			//save variables for reverting later
			unitFocused = unitAffected;
			unitFocused.mysticFocusingThis = this;
			if (unitFocused.alleg != this.alleg){
				unitFocused.paralyzed = true;
			}
			if (unitAffected.unitType == 2){
				(unitAffected as Mystic).revertStatsOfFocused();
			}

			gm.addLogToCombatLog(player + unitName +" focused "+ unitAffectedPlayer +  unitAffected.unitName);

			oldMvRange = unitAffected.mvRange;
			oldAtkRange = unitAffected.atkRange;
			oldAtkDmg = unitAffected.atk;

			if ((alleg == Unit.allegiance.playerOne  && unitFocused.alleg == Unit.allegiance.playerOne) || (alleg == Unit.allegiance.playerTwo && unitFocused.alleg == Unit.allegiance.playerTwo)){
				unitAffected.mvRange += 3;
				unitAffected.showPopUpText("Focused!",Color.green);
				if (unitLevel == 2){
					unitAffected.atk += 5;
				}
			//Enemy units are frozen
			}else if ((alleg == Unit.allegiance.playerOne && unitFocused.alleg == Unit.allegiance.playerTwo) || (alleg == Unit.allegiance.playerTwo && unitFocused.alleg == Unit.allegiance.playerOne)){
				unitAffected.atkRange = 0;
				unitAffected.mvRange = 0;
				unitAffected.atk = 0;
				unitAffected.showPopUpText("Focused!",Color.red);
			}


		}else{
			unitAffected.showPopUpText("Invincible!",Color.red);
		}
		//clean up the board colors
		gm.accessibleTiles.Clear();
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
	}

	public override void resetUnitAbilities ()
	{
		base.resetUnitAbilities ();

//		//deal 8 dmg to unit every time if level 3 and focusing enemy unit
		if (unitFocused != null && ((alleg ==Unit.allegiance.playerOne && unitFocused.alleg == Unit.allegiance.playerTwo) || (alleg == Unit.allegiance.playerTwo && unitFocused.alleg == Unit.allegiance.playerOne)) && unitLevel == 3) {
			if (!invincible){
				unitFocused.hp -= 8;
				unitFocused.showPopUpText("-8",Color.red);

				//if the unit attacked was killed, remove it from the board and unit list
				if (unitFocused.hp <=0){				
					
					//Kill Guardian then SS vulnerable
					if (unitFocused.unitType == 10){
						if (unitFocused.alleg == allegiance.playerOne){
							playerSSKillable();
						}else{
							enemySSKillable();
						}
						
					}else if (unitFocused.unitType == 11){
						gm.gameOver = true;
					}
					
					//Kill unit and remove from game
					gm.addLogToCombatLog(unitFocused.unitName + " was killed!");
					gm.units.Remove(unitFocused.unitID);
					unitFocused.transform.parent.GetComponent<TileScript>().objectOccupyingTile = null;
					Destroy(unitFocused.gameObject);
				}
			}else{
				gm.addLogToCombatLog("Mystic couldn't deal damage it was invincible!");
			}
		}
	}
}
