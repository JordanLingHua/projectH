using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitEight : Unit {



	void Start () {
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		unitName = "Priest";
		unitType = 8;
		hp = 20;
		maxHP = 20;
		atk = -8;//not final
		mvRange = 3;
		mvCost = 3;
		atkRange = 0;//not final
		atkCost = 0;
		
		unitRole = 506;//Healer
		renderer.material.color = new Color32(204,255,153,1);
	}

	public override void showAtkTiles(){
		if (!atkd) {
			//only this one
			gm.accessibleTiles.Add(transform.parent.GetComponent<TileScript>());		
			transform.parent.gameObject.renderer.material.color = new Color(1f,0.4f,0f, 0f);
		}
	}

	
	public override List<GameObject> showAoEAffectedTiles(TileScript tile){
		List <GameObject> ret = new List<GameObject> ();

		foreach (int key in gm.units.Keys) {
			if (alleg == allegiance.ally && gm.units[key].alleg == allegiance.ally && this != gm.units[key]){
				gm.units[key].transform.parent.gameObject.renderer.material.color =  new Color(1f,0.4f,0f, 0f);
				ret.Add (gm.units[key].transform.parent.gameObject);
			}	
			if (alleg == allegiance.enemy && gm.units[key].alleg == allegiance.enemy && this != gm.units[key]){
				gm.units[key].transform.parent.gameObject.renderer.material.color =  new Color(1f,0.4f,0f, 0f);
				ret.Add (gm.units[key].transform.parent.gameObject);
			}	
		}
		return ret;
	}
	
}
