using UnityEngine;
using System.Collections;

public class UnitTen :Unit {



	void Start(){
		base.Start ();
		unitType = 10;
		unitName = "Guardian";
		hp = 45;
		maxHP = 45;
		atk = 23;
		mvRange = 2;
		mvCost = 3;
		atkRange = 1;
		atkCost = 1;
		
		unitRole = 505;//MeleeTank
		renderer.material.color = new Color32(0,0,0,1);
	}



	void Update () {
	
	}
}
