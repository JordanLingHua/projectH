﻿using UnityEngine;
using System.Collections;

public class UnitSix : Unit {




	void Start(){
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		name = "Buffing Unit";
		hp = 40;
		maxHP = 40;
		armor = 25;
		atk = 0;//not final
		mvRange = 2;
		mvCost = 3;
		atkRange = 0;//not final
		atkCost = 0;//not final
		
		unitRole = 504;//BuffDebuff
		renderer.material.color = new Color32(255,0,255,1);
	}

	void Update () {
	
	}
}
