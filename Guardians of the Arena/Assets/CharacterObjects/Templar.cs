using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0114
public class Templar : Unit{
	
	void Start(){
		base.Start ();
		levelBonusShort [0] = "Mighty Swing";
		levelBonusShort [1] = "Healing Blade";
		levelBonusLong [0] = "Attacks now deal an additional\n5 damage to full health units";
		levelBonusLong [1] = "Attacks that hit allies now heal\nthem instead of dealing damage";
		unitType = 3;
		unitName = "Templar";
		hp = 38;//hp can --//all other stats cannot --//some other stats ++ for growth
		maxHP = 38;//max hp is constant//use as reference to hp
		atk = 13;
		mvRange = 3;
		mvCost = 1;
		atkRange = 1;
		atkCost = 3;
		//unitRole = "AOE";//O is NOT a zero.  it is capital O
		unitRole = 502;//AOE
		renderer.material.color = new Color32(255,255,0,1);
	}

	public override void attackUnit(Unit unitAffected){
		atkd = true;	
		//level 3 Heal ally units
		if (unitLevel == 3 && ((alleg == Unit.allegiance.playerOne && unitAffected.alleg == Unit.allegiance.playerOne) || (alleg == Unit.allegiance.playerTwo && unitAffected.alleg == Unit.allegiance.playerTwo))){
			unitAffected.takeDmg (this,-1*this.atk);
		//Deal extra dmg to full hp units at level 2+
		}else if (unitLevel >= 2 && unitAffected.hp == unitAffected.maxHP) {
			unitAffected.takeDmg (this,this.atk+5);
		}else{
			unitAffected.takeDmg(this,this.atk);
		}
		gm.accessibleTiles.Clear();
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
	}


	public override List<GameObject> showAoEAffectedTiles(TileScript tile){
		List <GameObject> ret = new List<GameObject> ();
		if (tile.up != null && tile.gameObject == gm.selectedUnit.transform.parent.GetComponent<TileScript>().up) {
			tile.up.renderer.material.color = new Color(1f,0.7f,0f, 0f);
			ret.Add(tile.up);
			if (tile.up.GetComponent<TileScript>().right != null){
				tile.up.GetComponent<TileScript>().right.renderer.material.color = new Color(1f,0.7f,0f, 0f);
				ret.Add (tile.up.GetComponent<TileScript>().right);
			}
			if (tile.up.GetComponent<TileScript>().left != null){
				tile.up.GetComponent<TileScript>().left.renderer.material.color = new Color(1f,0.7f,0f, 0f);
				ret.Add (tile.up.GetComponent<TileScript>().left);
			}

		}

		if (tile.down != null && tile.gameObject == gm.selectedUnit.transform.parent.GetComponent<TileScript>().down) {
			tile.down.renderer.material.color = new Color(1f,0.7f,0f, 0f);
			ret.Add (tile.down);
			if (tile.down.GetComponent<TileScript>().right != null){
				tile.down.GetComponent<TileScript>().right.renderer.material.color = new Color(1f,0.7f,0f, 0f);
				ret.Add (tile.down.GetComponent<TileScript>().right);
			}
			if (tile.down.GetComponent<TileScript>().left != null){
				tile.down.GetComponent<TileScript>().left.renderer.material.color = new Color(1f,0.7f,0f, 0f);
				ret.Add (tile.down.GetComponent<TileScript>().left);
			}
		}
		if (tile.right != null && tile.gameObject == gm.selectedUnit.transform.parent.GetComponent<TileScript>().right) {
			tile.right.renderer.material.color = new Color(1f,0.7f,0f, 0f);
			ret.Add (tile.right);
			if (tile.right.GetComponent<TileScript>().up != null){
				tile.right.GetComponent<TileScript>().up.renderer.material.color = new Color(1f,0.7f,0f, 0f);
				ret.Add (tile.right.GetComponent<TileScript>().up);
			}
			if (tile.right.GetComponent<TileScript>().down != null){
				tile.right.GetComponent<TileScript>().down.renderer.material.color = new Color(1f,0.7f,0f, 0f);
				ret.Add (tile.right.GetComponent<TileScript>().down);
			}
		}

		if (tile.left != null && tile.gameObject == gm.selectedUnit.transform.parent.GetComponent<TileScript>().left) {
			tile.left.renderer.material.color = new Color(1f,0.7f,0f, 0f);
			ret.Add (tile.left);
			if (tile.left.GetComponent<TileScript>().up != null){
				tile.left.GetComponent<TileScript>().up.renderer.material.color = new Color(1f,0.7f,0f, 0f);
				ret.Add (tile.left.GetComponent<TileScript>().up);
			}
			if (tile.left.GetComponent<TileScript>().down != null){
				tile.left.GetComponent<TileScript>().down.renderer.material.color = new Color(1f,0.7f,0f, 0f);
				ret.Add (tile.left.GetComponent<TileScript>().down);
			}
		}
		return ret;
	}
}
