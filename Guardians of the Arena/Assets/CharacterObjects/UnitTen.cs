using UnityEngine;
using System.Collections;

public class UnitTen :Unit {



	void Start(){
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		hpBarBG = Resources.Load("HPBarBG") as Texture2D;
		hpBarHigh = Resources.Load("HPBarHigh") as Texture2D;
		hpBarMedium = Resources.Load("HPBarMedium") as Texture2D;
		hpBarLow = Resources.Load("HPBarLow") as Texture2D;
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
