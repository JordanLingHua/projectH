using UnityEngine;
using System.Collections;

public class UnitEleven : Unit {
	


	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		name = "Soulstone";
		hp = 1;
		maxHP = 1;
		armor = 0;
		atk = 0;
		mvRange = 0;
		mvCost = 0;
		atkRange = 0;
		atkCost = 500;
		
		invincible = true;//can be changed later
		unitRole = 507;//Kingpin
		renderer.material.color = new Color32(255,255,255,1);
	}



	void Update () {
	
	}
}
