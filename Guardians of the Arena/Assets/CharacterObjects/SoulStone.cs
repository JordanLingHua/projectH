using UnityEngine;
using System.Collections;
#pragma warning disable 0114
public class SoulStone: Unit {

	void Start () {
		base.Start ();
		unitType = 11;
		unitName = "Soulstone";
		description = "The Soulstone allows the player to control units, if it is destroyed the player loses. It is invincible until the Guardian is killed";
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

		//yield return new WaitForSeconds (1.0f);

		string unitAffectedPlayer = ((gp.playerNumber ==  1 && unitAttacking.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && unitAttacking.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
		string player = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo)) ? "Your " : "Opponent's ";
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
			
			if (this.hp <= 0) {				
				//Kill unit and remove from game
				gm.gameOver = true;
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

	public override void showMvTiles(allegiance ally){
		
	}
}
