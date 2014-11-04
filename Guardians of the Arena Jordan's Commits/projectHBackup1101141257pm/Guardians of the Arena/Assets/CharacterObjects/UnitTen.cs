using UnityEngine;
using System.Collections;

public class UnitTen :Unit {

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		name = "Guardian";
		hp = 40;
		maxHP = 40;
		armor = 60;
		atk = 8;
		mvRange = 1;
		mvCost = 3;
		atkRange = 1;
		atkCost = 1;
		
		unitRole = 505;//MeleeTank
		renderer.material.color = new Color32(0,0,0,1);
	}

	void Update () {
	
	}
}
