using UnityEngine;
using System.Collections;

public class Mystic: Unit {
	
	public Unit unitFocused;
	public int oldMvRange, oldAtkRange,oldAtkDmg;

	void Start () {
		base.Start ();
		unitType = 2;
		unitName = "Mystic";
		hp = 30;
		maxHP = 30;
		atk = 0;//change later
		mvRange = 2;
		mvCost = 2;
		atkRange = 4;
		atkCost = 4;	
		//unitRole = "BuffDebuff";
		unitRole = 504;//BuffDebuff
		renderer.material.color = new Color32(255,128,0,1);
	}

	public void revertStatsOfFocused(){
		if (unitFocused != null) {
			//allied units get only increased mv range and attack damage
			if((gp.playerNumber == 1 && unitFocused.alleg == Unit.allegiance.playerOne) || (gp.playerNumber == 2 && unitFocused.alleg == Unit.allegiance.playerTwo)){
				unitFocused.mvRange = oldMvRange;
				unitFocused.atk = oldAtkDmg;
			//enemy units get mv and attack range reduced and atk
			}else if ((gp.playerNumber == 1 && unitFocused.alleg == Unit.allegiance.playerTwo) || (gp.playerNumber == 2 && unitFocused.alleg == Unit.allegiance.playerOne)){
				unitFocused.mvRange = oldMvRange;
				unitFocused.mvRange = oldAtkRange;
				unitFocused.atk = oldAtkDmg;
			}
			unitFocused = null;
		}
	
	}
	
	//Unit Focused needs to take dmg after turn ends if level 3 and enemy unit
	public virtual void attackUnit(Unit unitAffected){
		atkd = true;
		if (!invincible){
			//save variables for reverting later
			unitFocused = unitAffected;
			oldMvRange = unitAffected.mvRange;
			oldAtkRange = unitAffected.atkRange;
			oldAtkDmg = unitAffected.atk;

			if ((gp.playerNumber == 1 && unitFocused.alleg == Unit.allegiance.playerOne) || (gp.playerNumber == 2 && unitFocused.alleg == Unit.allegiance.playerTwo)){
				unitAffected.mvRange += 3;
				if (unitLevel == 2){
					unitAffected.atk += 5;
				}
			//Enemy units are frozen
			}else if ((gp.playerNumber == 1 && unitFocused.alleg == Unit.allegiance.playerTwo) || (gp.playerNumber == 2 && unitFocused.alleg == Unit.allegiance.playerOne)){
				unitAffected.atkRange = 0;
				unitAffected.mvRange = 0;
				unitAffected.atk = 0;
			}


		}else{
			gm.combatLog.text = "Combat Log:\nTarget is invincible!";
		}
		//clean up the board colors
		gm.accessibleTiles.Clear();
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
	}

	void Update () {
		
	}
}
