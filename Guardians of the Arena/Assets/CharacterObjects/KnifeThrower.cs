using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0114
public class KnifeThrower : Unit {

	void Start () {
		base.Start ();
		levelBonusShort [0] = "Farsight";
		levelBonusShort [1] = "Piercing Light";
		levelBonusLong [0] = "Gain +1 attack range";
		levelBonusLong [1] = "Attacks all units to\ntargeted tile";
		description = "Ranged unit that will hit any unit that gets\n" +
						"in its way";
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

	public override void gainLevelTwoBonus ()
	{
		atkRange ++;
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
