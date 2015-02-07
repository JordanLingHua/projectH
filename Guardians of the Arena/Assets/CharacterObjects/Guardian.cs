using UnityEngine;
using System.Collections;

#pragma warning disable 0114
public class Guardian :Unit {


	void Start(){
		base.Start ();
		levelBonusShort [0] = "Hardened Skin";
		levelBonusShort [1] = "Executioner";
		levelBonusLong [0] = "Cannot take more than 10\ndamage when attacked";
		levelBonusLong [1] = "Instantly kills units below\n50% health on attack";
		description = "A melee unit that protects the Soulstone. If this\n" +
						"unit dies the player's Soulstone becomes\n" +
						"vulnerable to death.";
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
		string unitAffectedPlayer = ((gp.playerNumber ==  1 && unitAttacking.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && unitAttacking.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		if (!this.invincible) {

			if (unitLevel >=2 && amt > 10){
				hp -= 10;
				showPopUpText("-10 "+ (unitAttacking.atk-10) + "blocked",Color.red);
			}else{
				this.hp -= amt;
			}
			
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
				if (this.alleg == allegiance.playerOne){
					playerSSKillable();
				}else{
					enemySSKillable();
				}

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

	
	public override void attackUnit(Unit unitAffected){
		atkd = true;

		if (unitLevel == 3 && (((float)unitAffected.hp/unitAffected.maxHP) < 0.5)){
			unitAffected.takeDmg(this,unitAffected.hp);
		}else{
			unitAffected.takeDmg(this,this.atk);
		}
		//clean up the board colors
		gm.accessibleTiles.Clear();
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
	}
}
