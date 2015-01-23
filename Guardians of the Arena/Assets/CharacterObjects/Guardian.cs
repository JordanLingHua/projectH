﻿using UnityEngine;
using System.Collections;

public class Guardian :Unit {



	void Start(){
		base.Start ();
		unitType = 10;
		unitName = "Guardian";
		hp = 45;
		maxHP = 45;
		atk = 23;
		mvRange = 2;
		mvCost = 1;
		atkRange = 1;
		atkCost = 1;
		
		unitRole = 505;//MeleeTank
		renderer.material.color = new Color32(0,0,0,1);
	}

	public override void takeDmg(Unit unitAttacking, int amt){

		string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		if (!this.invincible) {

			if (unitLevel >=2 && amt > 10){
				hp -= 10;
				showPopUpText("-10 "+ (this.atk-10) + "blocked",Color.red);
			}else{
				this.hp -= amt;
			}
			
			//if healed up dont let it have more than max hp
			if (hp > maxHP){
				hp = maxHP;
			}
			
			if (amt > 0){
				//taking damage
				showPopUpText("-" + amt,Color.red);
			}else{
				//getting healed
				showPopUpText("+" + (-1*amt),Color.green);
			}
			
			if (this.hp <= 0) {
				if (this.alleg == allegiance.playerOne){
					playerSSKillable();
				}else{
					enemySSKillable();
				}

				//Kill unit and remove from game
				gm.addLogToCombatLog (player + this.unitName + " was killed!");
				gm.units.Remove (this.unitID);
				this.transform.parent.GetComponent<TileScript> ().objectOccupyingTile = null;
				Destroy (this.gameObject);
			}
		}else{
			showPopUpText("Invincible!",Color.red);
			if ((player == "Your " && (pum.clo == PopUpMenuNecro.combatLogOption.all || pum.clo == PopUpMenuNecro.combatLogOption.playerOnly)) || (player == "Opponent's " && (pum.clo == PopUpMenuNecro.combatLogOption.all || pum.clo == PopUpMenuNecro.combatLogOption.enemyOnly))){
				gm.addLogToCombatLog(unitAttacking.unitName +" attacked "+ unitName + " but it was invincible!");
			}
		}
	}


	//TODO: move this logic to the server
	public override void attackUnit(Unit unitAffected){
		string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		string unitAffectedPlayer = ((gp.playerNumber ==  1 && unitAffected.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && unitAffected.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		atkd = true;

		if (unitLevel == 3 && (((float)unitAffected.hp/unitAffected.maxHP) < 0.5)){
			if ((player == "Your " && (pum.clo == PopUpMenuNecro.combatLogOption.all || pum.clo == PopUpMenuNecro.combatLogOption.playerOnly)) || (player == "Opponent's " && (pum.clo == PopUpMenuNecro.combatLogOption.all || pum.clo == PopUpMenuNecro.combatLogOption.enemyOnly))){
				gm.addLogToCombatLog(player + this.unitName +" executed "+ unitAffected.unitName + " for " + unitAffected.hp + " damage!");
			}
			unitAffected.hp = 0;
			unitAffected.showPopUpText("Executed!",Color.red);
		}else{
			unitAffected.hp -= this.atk;
			if ((player == "Your " && (pum.clo == PopUpMenuNecro.combatLogOption.all || pum.clo == PopUpMenuNecro.combatLogOption.playerOnly)) || (player == "Opponent's " && (pum.clo == PopUpMenuNecro.combatLogOption.all || pum.clo == PopUpMenuNecro.combatLogOption.enemyOnly))){
				gm.addLogToCombatLog(player + this.unitName +" attacked "+ unitAffected.unitName + " for " + this.atk + " damage!");
			}
			unitAffected.showPopUpText("-" + this.atk,Color.red);
		}
		//clean up the board colors
		gm.accessibleTiles.Clear();
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
	}
}
