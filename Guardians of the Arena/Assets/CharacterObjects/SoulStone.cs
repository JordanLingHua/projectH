﻿using UnityEngine;
using System.Collections;

public class SoulStone: Unit {
	


	void Start () {
		base.Start ();
		unitType = 11;
		unitName = "Soulstone";
		hp = 1;
		maxHP = 1;
		atk = 0;
		mvRange = 0;
		mvCost = 0;
		atkRange = 0;
		atkCost = 0;
		
		invincible = true;//can be changed later
		unitRole = 507;//Kingpin
		renderer.material.color = new Color32(255,255,255,1);
	}

	public override void takeDmg(Unit unitAttacking, int amt){
		string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		if (!this.invincible) {
			this.hp -= amt;
			
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
				//Kill unit and remove from game
				gm.gameOver = true;
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

	public override void showMvTiles(allegiance ally){
		
	}

	void Update () {
	
	}
}
