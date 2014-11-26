using UnityEngine;
using System.Collections;

public class Swordsman : Unit {

	void Start(){
		base.Start ();
		unitType = 7;
		unitName = "Swordsman";
		hp = 38;
		maxHP = 38;	
		atk = 10;
		mvRange = 3;
		mvCost = 1;
		atkRange = 1;
		atkCost = 1;
		
		unitRole = 505;//MeleeTank
		renderer.material.color = new Color32(102,51,0,1);
	}


	void Update () {
	
	}
}
