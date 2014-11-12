using UnityEngine;
using System.Collections;

public class UnitFour : Unit {




	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		unitType = 4;
		name = "AoE Unit";
		hp = 28;
		maxHP = 28;
		armor = 10;
		atk = 10;
		mvRange = 4;
		mvCost = 1;
		atkRange = 1;
		atkCost = 2;
		//unitRole = "AOE";
		unitRole = 502;//AOE
		renderer.material.color = new Color32(0,255,0,1);
	}


	void Update () {
	
	}
}
