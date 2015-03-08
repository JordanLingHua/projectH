using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0114
public class KnifeThrower : Unit {

	void Start () {
		base.Start ();
		unitPortrait = Resources.Load("ArcherPortrait") as Texture2D;
		levelBonusShort [0] = "Farsight";
		levelBonusShort [1] = "Piercing Light";
		levelBonusLong [0] = "Gain +1 attack range";
		levelBonusLong [1] = "Attacks all units to targeted tile";
		description = "Ranged unit that will hit any unit that gets in its way";
		unitType = 1;
		unitName = "Archer";
		hp = 25;
		maxHP = 25;
		atk = 16;  
		mvRange = 3;
		mvCost = 1;
		atkRange = 3;
		atkCost = 2;
		//unitRole = "Ranged";
		unitRole = 500;//ranged
		renderer.material.color = new Color32(255,153,204,1);
	}

	public override HashSet<TileScript> getFriendlyFireHitTiles(){
		HashSet<TileScript> tileSet = new HashSet<TileScript>();
		TileScript tileS = this.transform.parent.GetComponent<TileScript> ();

		TileScript temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.up != null){
				if (temp.up.GetComponent<TileScript>().objectOccupyingTile != null && ((gp.playerNumber == 1 && temp.up.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == allegiance.playerOne ) || (gp.playerNumber == 2 &&  temp.up.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == allegiance.playerTwo ))){
					for (int k = i; k < atkRange; k++){
						if (temp.up != null){
							tileSet.Add (temp.up.GetComponent<TileScript>());
							temp = temp.up.GetComponent<TileScript>();
						}
					}
					break;
				}
				temp = temp.up.GetComponent<TileScript>();
			}
		}
		temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.down != null){
				if (temp.down.GetComponent<TileScript>().objectOccupyingTile != null && ((gp.playerNumber == 1 && temp.down.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == allegiance.playerOne ) || (gp.playerNumber == 2 &&  temp.down.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == allegiance.playerTwo ))){
					for (int k = i; k < atkRange; k++){
						if (temp.down != null){
							tileSet.Add (temp.down.GetComponent<TileScript>());
							temp = temp.down.GetComponent<TileScript>();
						}
					}
					break;
				}
				temp = temp.down.GetComponent<TileScript>();
			}
		}
		temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.left != null){
				if (temp.left.GetComponent<TileScript>().objectOccupyingTile != null && ((gp.playerNumber == 1 && temp.left.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == allegiance.playerOne ) || (gp.playerNumber == 2 &&  temp.left.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == allegiance.playerTwo ))){
					for (int k = i; k < atkRange; k++){
						if (temp.left != null){
							tileSet.Add (temp.left.GetComponent<TileScript>());
							temp = temp.left.GetComponent<TileScript>();
						}
					}
					break;
				}
				temp = temp.left.GetComponent<TileScript>();
			}
		}
		temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.right != null){
				if (temp.right.GetComponent<TileScript>().objectOccupyingTile != null && ((gp.playerNumber == 1 && temp.right.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == allegiance.playerOne ) || (gp.playerNumber == 2 &&  temp.right.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == allegiance.playerTwo ))){
					for (int k = i; k < atkRange; k++){
						if (temp.right != null){
							tileSet.Add (temp.right.GetComponent<TileScript>());
							temp = temp.right.GetComponent<TileScript>();
						}
					}
					break;
				}
				temp = temp.right.GetComponent<TileScript>();
			}
		}
		return tileSet;
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
