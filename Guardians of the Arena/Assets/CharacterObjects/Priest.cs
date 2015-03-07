using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0114
public class Priest : Unit {

	void Start () {
		base.Start ();
		unitPortrait = Resources.Load("PriestPortrait") as Texture2D;
		levelBonusShort [0] = "Mighty Heal";
		levelBonusShort [1] = "Rain of Light";
		levelBonusLong [0] = "Heal now heals the target to full health";
		levelBonusLong [1] = "Heals allies near target for 10 health";
		description = "A great support healing unit that can help her allies from a distance";
		unitName = "Priest";
		unitType = 8;
		hp = 20;
		maxHP = 20;
		atk = -20;
		mvRange = 3;
		mvCost = 1;
		atkRange = 4;
		atkCost = 3;
		
		unitRole = 506;//Healer
		renderer.material.color = new Color32(204,255,153,1);
	}

	public override void attackUnit(Unit unitAffected){
		if (unitLevel == 3 && atkd){
			unitAffected.takeDmg(this,-10);
		}else{
			unitAffected.takeDmg(this,this.atk);
		}
		//clean up the board colors
		gm.accessibleTiles.Clear();
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
		atkd = true;
	}


	public override void gainLevelTwoBonus ()
	{
		atk = -500;
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
