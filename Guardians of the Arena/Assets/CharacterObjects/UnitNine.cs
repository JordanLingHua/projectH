using UnityEngine;
using System.Collections;

public class UnitNine : Unit {

	//NOT IN DEVELOPMENT


	void Start () {
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		hpBarBG = Resources.Load("HPBarBG") as Texture2D;
		hpBarHigh = Resources.Load("HPBarHigh") as Texture2D;
		hpBarMedium = Resources.Load("HPBarMedium") as Texture2D;
		hpBarLow = Resources.Load("HPBarLow") as Texture2D;
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
