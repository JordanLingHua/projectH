using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0114
public class Priest : Unit {

	void Start () {
		base.Start ();
		levelBonusShort [0] = "Mighty Heal";
		levelBonusShort [1] = "Rain of Light";
		levelBonusLong [0] = "Heal now heals the target to\nfull health";
		levelBonusLong [1] = "Healing a unit now heals units\nnear it for 10 health";
		unitName = "Priest";
		unitType = 8;
		hp = 20;
		maxHP = 20;
		atk = -20;
		mvRange = 3;
		mvCost = 1;
		atkRange = 4;
		atkCost = 4;
		
		unitRole = 506;//Healer
		renderer.material.color = new Color32(204,255,153,1);
	}

	public override void gainXP(){
		xp += 5;
		if (xp >= XP_TO_LEVEL[unitLevel-1]){
			xp = 0;
			unitLevel ++;
			//Max heal
			//just sets attack to heal for more then any hp in game atm
			if (unitLevel == 2){
				atk = -500;
			}
			if ((alleg == allegiance.playerOne && gp.playerNumber == 1) || (alleg == allegiance.playerTwo && gp.playerNumber == 2)){
				gm.addLogToCombatLog("Your " + unitName + " has leveled up to level " + unitLevel + "!");
			}
			if ((alleg == allegiance.playerTwo && gp.playerNumber == 1) || (alleg == allegiance.playerOne && gp.playerNumber == 2)){
				gm.addLogToCombatLog("Opponent's " + unitName + " has leveled up to level " + unitLevel + "!");
			}
			showPopUpText("Leveled Up!",Color.yellow);
		}else {
			showPopUpText("XP+5!",Color.magenta);
		}
	}


	public override List<GameObject> showAoEAffectedTiles(TileScript tile){
		List <GameObject> ret = new List<GameObject> ();
		if (unitLevel == 3) {
			rangeAoE (ret,tile,1);
		}
		return ret;
	}

	//Recursive call to show AoE tiles around unit healed for 2
	void rangeAoE(List<GameObject> list,TileScript tile, int num){
		tile.renderer.material.color = new Color(1f,0.7f,0f, 0f);
		TileScript tileS = tile.transform.GetComponent<TileScript>();
		if (num != 0){
			if (tileS.up != null){
				rangeAoE(list,tileS.up.GetComponent<TileScript>(),num-1);
				list.Add(tileS.up);
			}
			if (tileS.down != null){
				rangeAoE(list,tileS.down.GetComponent<TileScript>(),num-1);
				list.Add(tileS.down);
			}
			if (tileS.left != null){
				rangeAoE(list,tileS.left.GetComponent<TileScript>(),num-1);
				list.Add(tileS.left);
			}
			if (tileS.right != null){
				rangeAoE(list,tileS.right.GetComponent<TileScript>(),num-1);
				list.Add(tileS.right);
			}
		}
	
	}

}
