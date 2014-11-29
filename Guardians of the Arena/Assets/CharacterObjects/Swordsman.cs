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

		if (!invincible){
			unitAffected.hp -= this.atk;
			unitAffected.showPopUpText("-" +this.atk,Color.red);
			if (unitLevel >= 2){
				this.hp += 5;
				showPopUpText("+5",Color.green);
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
				
				//Kill unit and remove from game
				gm.units.Remove(unitAffected.unitID);
				unitAffected.transform.parent.GetComponent<TileScript>().objectOccupyingTile = null;
				Destroy(unitAffected.gameObject);
			}
		}else{
			gm.combatLog.text = "Combat Log:\nTarget is invincible!";
		}
		//clean up the board colors
		gm.accessibleTiles.Clear();
	}

	public override void resetUnitAbilities ()
	{
		base.resetUnitAbilities ();
		atkCharges = 2;
	}


	void Update () {
	
	}
}
