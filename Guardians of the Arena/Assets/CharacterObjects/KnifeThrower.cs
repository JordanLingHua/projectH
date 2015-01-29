﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KnifeThrower : Unit {

	void Start () {
		base.Start ();
		levelBonusShort [0] = "Farsight";
		levelBonusShort [1] = "Rain of Knives";
		levelBonusLong [0] = "Gain +1 range";
		levelBonusLong [1] = "Gain an area of affect attack\n(area range of 1)";
		unitType = 1;
		unitName = "Knife Thrower";
		hp = 25;
		maxHP = 25;
		atk = 18;  
		mvRange = 4;
		mvCost = 1;
		atkRange = 4;
		atkCost = 2;
		//unitRole = "Ranged";
		unitRole = 500;//ranged
		renderer.material.color = new Color32(255,153,204,1);
	}
	
	//gain xp add attack range for lvl 2
	public override void gainXP(){
		xp += 5;
		if (xp >= XP_TO_LEVEL [unitLevel - 1]) {
			xp = 0;
			unitLevel ++;
			if (unitLevel == 2) {
				atkRange++;
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

	//show new AoE ability for level 3
	public override List<GameObject> showAoEAffectedTiles(TileScript tile){
		List <GameObject> ret = new List<GameObject> ();
		if (unitLevel == 3) {
			if (tile.up != null){
				ret.Add (tile.up);
				tile.up.renderer.material.color = new Color(1f,0.7f,0f, 0f);
			}
			if (tile.down != null){
				ret.Add (tile.down);
				tile.down.renderer.material.color = new Color(1f,0.7f,0f, 0f);
			}
			if (tile.left != null){
				ret.Add (tile.left);
				tile.left.renderer.material.color = new Color(1f,0.7f,0f, 0f);
			}
			if (tile.right != null){
				ret.Add (tile.right);
				tile.right.renderer.material.color = new Color(1f,0.7f,0f, 0f);
			}

		}
		return ret;
	}

	public override HashSet<TileScript> getAtkAccessibleTiles ()
	{
		HashSet<TileScript> tileSet = new HashSet<TileScript>();
		TileScript tileS = this.transform.parent.GetComponent<TileScript> ();
		TileScript temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.up != null){
				tileSet.Add (temp.up.GetComponent<TileScript>());
				temp = temp.up.GetComponent<TileScript>();
			}
		}
		temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.down != null){
				tileSet.Add (temp.down.GetComponent<TileScript>());
				temp = temp.down.GetComponent<TileScript>();
			}
		}
		temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.left != null){
				tileSet.Add (temp.left.GetComponent<TileScript>());
				temp = temp.left.GetComponent<TileScript>();
			}
		}
		temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.right != null){
				tileSet.Add (temp.right.GetComponent<TileScript>());
				temp = temp.right.GetComponent<TileScript>();
			}
		}
		return tileSet;
	}

}
