using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0114
public class Templar : Unit{
	
	void Start(){
		base.Start ();
		unitPortrait = Resources.Load("TemplarPortrait") as Texture2D;
		levelBonusShort [0] = "Mighty Swing";
		levelBonusShort [1] = "Healing Blade";
		levelBonusLong [0] = "Attacking full health units deals 5 additional damage";
		levelBonusLong [1] = "Attacking allied units heals them";
		description = "A melee unit that deals damage to all units in a cone in front of him";
		unitType = 3;
		unitName = "Templar";
		hp = 40;//hp can --//all other stats cannot --//some other stats ++ for growth
		maxHP = 40;//max hp is constant//use as reference to hp
		atk = 11;
		mvRange = 3;
		mvCost = 1;
		atkRange = 1;
		atkCost = 3;
		//unitRole = "AOE";//O is NOT a zero.  it is capital O
		unitRole = 502;//AOE
		renderer.material.color = new Color32(255,255,0,1);
	}

	public override HashSet<TileScript> getFriendlyFireHitTiles(){
		HashSet<TileScript> ret = new HashSet<TileScript>();
		TileScript tileS = this.transform.parent.GetComponent<TileScript> ();
		HashSet<TileScript> tempSet = new HashSet<TileScript>();
		TileScript temp = tileS;
		if (unitLevel != 3) {
			//check up
			if (temp.up != null) {
					tempSet.Add (temp.up.GetComponent<TileScript> ());
					temp = temp.up.GetComponent<TileScript> ();
					if (temp.up != null) {
							tempSet.Add (temp.up.GetComponent<TileScript> ());
							temp = temp.up.GetComponent<TileScript> ();
							if (temp.right != null) {
									tempSet.Add (temp.right.GetComponent<TileScript> ());
							}
							if (temp.left != null) {
									tempSet.Add (temp.left.GetComponent<TileScript> ());
							}
					}
			}
			foreach (TileScript tile in tempSet) {
					if (tile.objectOccupyingTile != null && ((gp.playerNumber == 1 && tile.objectOccupyingTile.GetComponent<Unit> ().alleg == allegiance.playerOne) || (gp.playerNumber == 2 && tile.GetComponent<TileScript> ().objectOccupyingTile.GetComponent<Unit> ().alleg == allegiance.playerTwo))) {
							ret.Add (tileS.up.GetComponent<TileScript> ());
							break;
					}
			}
			temp = tileS;
			tempSet.Clear ();

			//check down
			if (temp.down != null) {
					tempSet.Add (temp.down.GetComponent<TileScript> ());
					temp = temp.down.GetComponent<TileScript> ();
					if (temp.down != null) {
							tempSet.Add (temp.down.GetComponent<TileScript> ());
							temp = temp.down.GetComponent<TileScript> ();
							if (temp.right != null) {
									tempSet.Add (temp.right.GetComponent<TileScript> ());
							}
							if (temp.left != null) {
									tempSet.Add (temp.left.GetComponent<TileScript> ());
							}
					}
			}
			foreach (TileScript tile in tempSet) {
					if (tile.objectOccupyingTile != null && ((gp.playerNumber == 1 && tile.objectOccupyingTile.GetComponent<Unit> ().alleg == allegiance.playerOne) || (gp.playerNumber == 2 && tile.GetComponent<TileScript> ().objectOccupyingTile.GetComponent<Unit> ().alleg == allegiance.playerTwo))) {
							ret.Add (tileS.down.GetComponent<TileScript> ());
							break;
					}
			}
			temp = tileS;
			tempSet.Clear ();
			//check left
			if (temp.left != null) {
					tempSet.Add (temp.left.GetComponent<TileScript> ());
					temp = temp.left.GetComponent<TileScript> ();
					if (temp.left != null) {
							tempSet.Add (temp.left.GetComponent<TileScript> ());
							temp = temp.left.GetComponent<TileScript> ();
							if (temp.up != null) {
									tempSet.Add (temp.up.GetComponent<TileScript> ());
							}
							if (temp.down != null) {
									tempSet.Add (temp.down.GetComponent<TileScript> ());
							}
					}
			}
			foreach (TileScript tile in tempSet) {
				if (tile.objectOccupyingTile != null && ((gp.playerNumber == 1 && tile.objectOccupyingTile.GetComponent<Unit> ().alleg == allegiance.playerOne) || (gp.playerNumber == 2 && tile.GetComponent<TileScript> ().objectOccupyingTile.GetComponent<Unit> ().alleg == allegiance.playerTwo))) {
					ret.Add (tileS.left.GetComponent<TileScript> ());
					break;
				}
			}
			temp = tileS;
			tempSet.Clear ();
			//check right
			if (temp.right != null) {
				tempSet.Add (temp.right.GetComponent<TileScript> ());
				temp = temp.right.GetComponent<TileScript> ();
				if (temp.right != null) {
					tempSet.Add (temp.right.GetComponent<TileScript> ());
					temp = temp.right.GetComponent<TileScript> ();
					if (temp.up != null) {
							tempSet.Add (temp.up.GetComponent<TileScript> ());
					}
					if (temp.down != null) {
							tempSet.Add (temp.down.GetComponent<TileScript> ());
					}
				}
			}
			foreach (TileScript tile in tempSet) {
				if (tile.objectOccupyingTile != null && ((gp.playerNumber == 1 && tile.objectOccupyingTile.GetComponent<Unit> ().alleg == allegiance.playerOne) || (gp.playerNumber == 2 && tile.GetComponent<TileScript> ().objectOccupyingTile.GetComponent<Unit> ().alleg == allegiance.playerTwo))) {
					ret.Add (tileS.right.GetComponent<TileScript> ());
					break;
				}
			}
		}
		return ret;
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
