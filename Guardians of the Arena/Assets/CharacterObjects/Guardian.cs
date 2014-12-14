using UnityEngine;
using System.Collections;

public class Guardian :Unit {



	void Start(){
		base.Start ();
		unitType = 10;
		unitName = "Guardian";
		hp = 45;
		maxHP = 45;
		atk = 23;
		mvRange = 2;
		mvCost = 1;
		atkRange = 1;
		atkCost = 1;
		
		unitRole = 505;//MeleeTank
		renderer.material.color = new Color32(0,0,0,1);
	}
	
	//TODO: move this logic to the server
	public override void attackUnit(Unit unitAffected){
		atkd = true;
		
		if (!unitAffected.invincible){
			if (unitLevel == 3 && (((float)unitAffected.hp/unitAffected.maxHP) < 0.5)){
				unitAffected.hp = 0;
				unitAffected.showPopUpText("Executed!",Color.red);
			}else{
				unitAffected.hp -= this.atk;
				unitAffected.showPopUpText("-" + this.atk,Color.red);
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
}
