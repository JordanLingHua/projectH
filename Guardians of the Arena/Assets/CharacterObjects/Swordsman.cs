using UnityEngine;
using System.Collections;

#pragma warning disable 0114
public class Swordsman : Unit {

	int atkCharges = 2;

	 void Start(){
		base.Start ();
		levelBonusShort [0] = "Lifesteal";
		levelBonusShort [1] = "Swift Strike";
		levelBonusLong [0] = "Gains +5 HP on each attack";
		levelBonusLong [1] = "Ability to attack twice per turn";
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

	public virtual void gainXP(){
		xp += 5;
		if (xp >= XP_TO_LEVEL [unitLevel - 1]) {
			xp = 0;
			unitLevel ++;
			//allow double attacks for windfury
			if (unitLevel == 3){
				atkd = false;
			}
			if ((alleg == allegiance.playerOne && gp.playerNumber == 1) || (alleg == allegiance.playerTwo && gp.playerNumber == 2)){
				gm.addLogToCombatLog("Your " + unitName + " has leveled up to level " + unitLevel + "!");
			}
			if ((alleg == allegiance.playerTwo && gp.playerNumber == 1) || (alleg == allegiance.playerOne && gp.playerNumber == 2)){
				gm.addLogToCombatLog("Opponent's " + unitName + " has leveled up to level " + unitLevel + "!");
			}
			showPopUpText("Leveled Up!",Color.yellow);
		} else {
			showPopUpText("XP+5!",Color.magenta);
		}
	}

	public override void attackUnit(Unit unitAffected){
		bool players = ((gp.playerNumber ==  1 && this.alleg == allegiance.playerOne) || (gp.playerNumber ==  2 && this.alleg == allegiance.playerTwo));
		string player = players ? "Your " : "Opponent's ";
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
		unitAffected.takeDmg(this,this.atk);
		if (unitLevel >= 2){
			this.hp += 5;
			showPopUpText("+5",Color.green);
			gm.addLogToCombatLog(player + this.unitName + " gained 5 health from lifesteal!");
			//dont overheal on lifesteal
			if (hp > maxHP){
				this.hp = maxHP;
			}
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
	
}
