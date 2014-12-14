using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Templar : Unit{
	


	void Start(){
		base.Start ();
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
		
		if (!unitAffected.invincible){

			//level 3 Heal ally units
			if (unitLevel == 3 && ((alleg == Unit.allegiance.playerOne && unitAffected.alleg == Unit.allegiance.playerOne) || (alleg == Unit.allegiance.playerTwo && unitAffected.alleg == Unit.allegiance.playerTwo))){
				unitAffected.hp += this.atk;
				unitAffected.showPopUpText("+" + this.atk,Color.green);
				if (unitAffected.hp > unitAffected.maxHP){
					unitAffected.hp = unitAffected.maxHP;
				}
			//Deal extra dmg to full hp units at level 2+
			}else if (unitLevel >= 2 && unitAffected.hp == unitAffected.maxHP) {

				if (unitAffected.unitType == 10 && unitAffected.unitLevel >=2){
					unitAffected.hp -= 10;
					unitAffected.showPopUpText("-10 "+ (this.atk-5) + "blocked",Color.red);
				}else{
					if(unitAffected.unitType == 2){
						(unitAffected as Mystic).revertStatsOfFocused();
					}
					unitAffected.hp -= this.atk +5;
					unitAffected.showPopUpText("-" + (this.atk+5)+"!",Color.red);
				}
			}else{
				if (unitAffected.unitType == 10 && unitAffected.unitLevel >=2){
					unitAffected.hp -= 10;
					unitAffected.showPopUpText("-10 "+ (this.atk-10) + "blocked",Color.red);
				}else{
					if(unitAffected.unitType == 2){
						(unitAffected as Mystic).revertStatsOfFocused();
					}
					unitAffected.hp -= this.atk;
					unitAffected.showPopUpText("-" + this.atk,Color.red);
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
			unitAffected.showPopUpText("Invincible!",Color.red);
		}
		//clean up the board colors
		gm.accessibleTiles.Clear();
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
	}


	public override List<GameObject> showAoEAffectedTiles(TileScript tile){
		List <GameObject> ret = new List<GameObject> ();
		if (tile.gameObject == gm.selectedUnit.transform.parent.GetComponent<TileScript>().up && tile.up != null) {
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

		if (tile.gameObject == gm.selectedUnit.transform.parent.GetComponent<TileScript>().down && tile.down != null) {
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
		if (tile.gameObject == gm.selectedUnit.transform.parent.GetComponent<TileScript>().right && tile.right != null) {
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

		if (tile.gameObject == gm.selectedUnit.transform.parent.GetComponent<TileScript>().left && tile.left != null) {
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


	void showAtkAccessibleTiles(TileScript tile, int num){
		tile.renderer.material.color = new Color (1f, 0.4f, 0f, 0f);
		TileScript tileS = tile.transform.GetComponent<TileScript>();
		if (num != 0){
			if (tileS.up != null){
				showAtkAccessibleTiles(tileS.up.GetComponent<TileScript>(),num-1);
				gm.accessibleTiles.Add(tileS.up.GetComponent<TileScript>());
			}
			if (tileS.down != null){
				showAtkAccessibleTiles(tileS.down.GetComponent<TileScript>(),num-1);
				gm.accessibleTiles.Add(tileS.down.GetComponent<TileScript>());
			}
			if (tileS.left != null){
				showAtkAccessibleTiles(tileS.left.GetComponent<TileScript>(),num-1);
				gm.accessibleTiles.Add(tileS.left.GetComponent<TileScript>());
			}
			if (tileS.right != null){
				showAtkAccessibleTiles(tileS.right.GetComponent<TileScript>(),num-1);
				gm.accessibleTiles.Add(tileS.right.GetComponent<TileScript>());
			}
		}
	}
	void Update () {
	
	}
}
