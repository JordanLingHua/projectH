using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Priest : Unit {



	void Start () {
		base.Start ();
		unitName = "Priest";
		unitType = 8;
		hp = 20;
		maxHP = 20;
		atk = -20;//not final
		mvRange = 3;
		mvCost = 3;
		atkRange = 0;//not final
		atkCost = 4;
		
		unitRole = 506;//Healer
		renderer.material.color = new Color32(204,255,153,1);
	}

}
