using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KnifeThrower : Unit {

	void Start () {
		base.Start ();
		unitType = 1;
		unitName = "Knife Thrower";
		hp = 20;
		maxHP = 20;
		atk = 18;  
		mvRange = 4;
		mvCost = 2;
		atkRange = 4;
		atkCost = 2;
		//unitRole = "Ranged";
		unitRole = 500;//ranged
		renderer.material.color = new Color32(255,153,204,1);
	}


	 public override void showAtkTiles(){
		if (!atkd){
			showAtkAccessibleTiles(this.transform.parent.GetComponent<TileScript>(),atkRange);
			gm.accessibleTiles.Remove(this.transform.parent.GetComponent<TileScript>());
			
			switch(alleg){
			case allegiance.playerOne:
				this.transform.parent.renderer.material.color = Color.blue;
				break;
			case allegiance.playerTwo:
				this.transform.parent.renderer.material.color = Color.red;
				break;
			case allegiance.neutral:
				this.transform.parent.renderer.material.color = Color.gray;
				break;
			}
		}
	}	

	//gain xp add attack range for lvl 2
	public override void gainXP(){
		xp += 5;
		if (xp >= XP_TO_LEVEL [unitLevel - 1]) {
			unitLevel ++;
			if (unitLevel == 2) {
				atkRange++;
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


	new void showAtkAccessibleTiles(TileScript tile, int num){

		TileScript tileS = tile.transform.GetComponent<TileScript>();

		TileScript temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.up != null){
				gm.accessibleTiles.Add(temp.up.GetComponent<TileScript>());
				temp = temp.up.GetComponent<TileScript>();
				temp.gameObject.renderer.material.color = new Color(1f,0.4f,0f, 0f);
			}
		}
		temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.down != null){
				gm.accessibleTiles.Add(temp.down.GetComponent<TileScript>());
				temp = temp.down.GetComponent<TileScript>();
				temp.gameObject.renderer.material.color = new Color(1f,0.4f,0f, 0f);
			}
		}
		temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.left != null){
				gm.accessibleTiles.Add(temp.left.GetComponent<TileScript>());
				temp = temp.left.GetComponent<TileScript>();
				temp.gameObject.renderer.material.color = new Color(1f,0.4f,0f, 0f);
			}
		}
		temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.right != null){
				gm.accessibleTiles.Add(temp.right.GetComponent<TileScript>());
				temp = temp.right.GetComponent<TileScript>();
				temp.gameObject.renderer.material.color = new Color(1f,0.4f,0f, 0f);
			}
		}
	}

	void Update () {
	
	}
}
