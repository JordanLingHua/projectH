using UnityEngine;
using System.Collections;

public class UnitFour : Unit {
	//NOT IN DEVELOPMENT



	void Start () {
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		hpBarBG = Resources.Load("HPBarBG") as Texture2D;
		hpBarHigh = Resources.Load("HPBarHigh") as Texture2D;
		hpBarMedium = Resources.Load("HPBarMedium") as Texture2D;
		hpBarLow = Resources.Load("HPBarLow") as Texture2D;
		unitType = 4;
		unitName = "AoE Unit";
		hp = 28;
		maxHP = 28;
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
