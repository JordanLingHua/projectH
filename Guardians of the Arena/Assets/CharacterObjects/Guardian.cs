using UnityEngine;
using System.Collections;

#pragma warning disable 0114
public class Guardian :Unit {


	void Start(){
		base.Start ();
		levelBonusShort [0] = "Hardened Skin";
		levelBonusShort [1] = "Thorns";
		levelBonusLong [0] = "Cannot take more than 10 damage when attacked";
		levelBonusLong [1] = "Deals 5 damage to attackers";
		description = "A melee unit that protects the Soulstone. If this unit dies the player's Soulstone becomes vulnerable to death.";
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

		//yield return new WaitForSeconds (1.0f);

		string unitAffectedPlayer = ((gp.playerNumber ==  1 && unitAttacking.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && unitAttacking.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		if (!this.invincible) {
			

			//taking damage
			if (amt > 0){
				//thorns
				if (unitLevel == 3){
					unitAttacking.takeDmg (this,5);
				}

				if (unitLevel >=2 && amt > 10){
					hp -=10 ;
					gm.addLogToCombatLog(unitAffectedPlayer + unitAttacking.unitName +" attacked "+ player + unitName + " for " + 10 + " damage!");
				}else{
					this.hp -= (amt - this.armor);
					gm.addLogToCombatLog(unitAffectedPlayer + unitAttacking.unitName +" attacked "+ player + unitName + " for " + amt + " damage!");
					showPopUpText("-" + (amt - this.armor),Color.red);
				}
			}else{
				//getting healed
				this.hp -= amt;
				//if healed up dont let it have more than max hp
				if (hp > maxHP){
					hp = maxHP;
				}
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
}
