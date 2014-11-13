using UnityEngine;
using System.Collections;

public class UnitEight : Unit {



	void Start () {
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		name = "Priest";
		unitType = 8;
		hp = 20;
		maxHP = 20;
		atk = -8;//not final
		mvRange = 3;
		mvCost = 3;
		atkRange = 0;//not final
		atkCost = 6;
		
		unitRole = 506;//Healer
		renderer.material.color = new Color32(204,255,153,1);
	}

	public override void showAtkTiles(){
		if (!atkd){
			showAtkAccessibleTiles(this.transform.parent.GetComponent<TileScript>(),atkRange);
			gm.accessibleTiles.Remove(this.transform.parent.GetComponent<TileScript>());
			
			switch(alleg){
			case allegiance.ally:
				this.transform.parent.renderer.material.color = Color.blue;
				break;
			case allegiance.enemy:
				this.transform.parent.renderer.material.color = Color.red;
				break;
			case allegiance.neutral:
				this.transform.parent.renderer.material.color = Color.gray;
				break;
			}
		}
	}

	void showAtkAccessibleTiles(TileScript tile, int num){
		foreach(int key in gm.units.Keys){
			if (alleg == allegiance.ally && gm.units[key].alleg == allegiance.ally){
				gm.accessibleTiles.Add(gm.units[key].transform.parent.GetComponent<TileScript>());
				gm.units[key].transform.parent.gameObject.renderer.material.color = new Color(1f,0.4f,0f, 0f);
			}

		}
	}

	void Update () {
	
	}
}
