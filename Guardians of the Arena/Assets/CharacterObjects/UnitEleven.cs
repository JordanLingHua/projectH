using UnityEngine;
using System.Collections;

public class UnitEleven : Unit {
	


	void Start () {
		hpBarBG = Resources.Load("HPBarBG") as Texture2D;
		hpBarHigh = Resources.Load("HPBarHigh") as Texture2D;
		hpBarMedium = Resources.Load("HPBarMedium") as Texture2D;
		hpBarLow = Resources.Load("HPBarLow") as Texture2D;
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		if(Application.loadedLevelName == "BoardScene")
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		unitType = 11;
		unitName = "Soulstone";
		hp = 1;
		maxHP = 1;
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
