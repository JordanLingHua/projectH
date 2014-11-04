using UnityEngine;
using System.Collections;

public class UnitSeven : Unit {

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		name = "Melee Tank";
		hp = 25;
		maxHP = 25;
		armor = 35;
		atk = 8;
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
