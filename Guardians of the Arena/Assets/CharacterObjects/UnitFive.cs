﻿using UnityEngine;
using System.Collections;

public class UnitFive : Unit {
	//NOT IN DEVELOPMENT


	void Start () {
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		hpBarBG = Resources.Load("HPBarBG") as Texture2D;
		hpBarHigh = Resources.Load("HPBarHigh") as Texture2D;
		hpBarMedium = Resources.Load("HPBarMedium") as Texture2D;
		hpBarLow = Resources.Load("HPBarLow") as Texture2D;
		unitType = 5;
		unitName = "Utility Unit";
		hp = 40;
		maxHP = 40;
		atk = 0;//not final
		mvRange = 6;
		mvCost = 2;
		atkRange = 0;//not final
		atkCost = 0;//not final
		unitRole = 503;//Utility
		renderer.material.color = new Color32(0,255,255,1);
	}



	void Update () {
	
	}
}
