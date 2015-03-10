using UnityEngine;
using System.Collections;

#pragma warning disable 0114
public class Swordsman : Unit {

	int atkCharges = 2;

	 void Start(){
		base.Start ();
		unitPortrait = Resources.Load("SwordsmanPortrait") as Texture2D;
		levelBonusShort [0] = "Lifesteal";
		levelBonusShort [1] = "Swift Strike";
		levelBonusLong [0] = "Heals for 5 health on attack";
		levelBonusLong [1] = "Can attack twice per turn";
		description = "A melee unit specializing in high mobility";
		unitType = 7;
		unitName = "Swordsman";
		hp = 45;
		maxHP = 45;	
		atk = 10;
		mvRange = 3;
		mvCost = 1;
		atkRange = 1;
		atkCost = 1;
		
		unitRole = 505;//MeleeTank
		renderer.material.color = new Color32(102,51,0,1);
	}

	public override void gainLevelThreeBonus ()
	{
		atkCharges = 1;
		atkd = false;
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
		if (unitLevel >= 2 && unitAffected.alleg != this.alleg){
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
