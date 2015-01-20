using UnityEngine;
using System.Collections;

public class Swordsman : Unit {

	int atkCharges = 2;

	void Start(){
		base.Start ();
		unitType = 7;
		unitName = "Swordsman";
		hp = 38;
		maxHP = 38;	
		atk = 10;
		mvRange = 3;
		mvCost = 1;
		atkRange = 1;
		atkCost = 1;
		
		unitRole = 505;//MeleeTank
		renderer.material.color = new Color32(102,51,0,1);
	}

	public override void attackUnit(Unit unitAffected){
		bool players = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo));
		string player = players ? "Your " : "Opponent's ";
		string unitAffectedPlayer = ((gp.playerNumber ==  1 && unitAffected.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && unitAffected.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		//WIN-dfury
		if (unitLevel == 3) {
			if (atkCharges > 0){
				atkCharges --;
				if (atkCharges == 0){
					atkd = true;
				}
			}
		} else {
			atkd = true;		
		}

		if (!unitAffected.invincible){
			if (unitAffected.unitType == 2){
				(unitAffected as Mystic).revertStatsOfFocused();
			}
			unitAffected.hp -= this.atk;
			unitAffected.showPopUpText("-" +this.atk,Color.red);
			if (pum.clo == PopUpMenuNecro.combatLogOption.all){

			}
			gm.addLogToCombatLog(player + this.unitName +" attacked "+ unitAffected.unitName + " for " + this.atk + " damage!");
			if (unitLevel >= 2){
				this.hp += 5;
				showPopUpText("+5",Color.green);
				gm.addLogToCombatLog(player + this.unitName + " gained 5 health from lifesteal!");
				//dont overheal on lifesteal
				if (hp > maxHP){
					this.hp = maxHP;
				}
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
				gm.addLogToCombatLog(unitAffectedPlayer + unitAffected.unitName + " was killed!");
				//Kill unit and remove from game
				gm.units.Remove(unitAffected.unitID);
				unitAffected.transform.parent.GetComponent<TileScript>().objectOccupyingTile = null;
				Destroy(unitAffected.gameObject);
			}
		}else{
			unitAffected.showPopUpText("Invincible!",Color.red);
			gm.addLogToCombatLog(player + this.unitName +" attacked "+ unitAffected.unitName + " but it was invincible!");
		}

		//clean up the board colors checks atkd here for windfury
		if (atkd) {
			gm.accessibleTiles.Clear ();
		}
	}

	public override void resetUnitAbilities ()
	{
		base.resetUnitAbilities ();
		atkCharges = 2;
	}


	void Update () {
	
	}
}
