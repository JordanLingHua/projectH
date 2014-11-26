using UnityEngine;
using System.Collections;

public class RangedUnit : Unit {

	//NOT IN DEVELOPMENT


	void Start () {
		base.Start ();
		unitType = 9;
		unitName = "Ranged Unit";
		hp = 22;
		maxHP = 22;
		atk = 12;
		mvRange = 3;
		mvCost = 2;
		atkRange = 4;
		atkCost = 3;
		
		unitRole = 500;//Ranged
		renderer.material.color = new Color32(96,96,96,1);
	}


	void Update () {
	
	}
}
