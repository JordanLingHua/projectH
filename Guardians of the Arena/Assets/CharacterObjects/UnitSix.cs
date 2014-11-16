using UnityEngine;
using System.Collections;

public class UnitSix : Unit {
	//NOT IN DEVELOPMENT



	void Start(){
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		hpBarBG = Resources.Load("HPBarBG") as Texture2D;
		hpBarHigh = Resources.Load("HPBarHigh") as Texture2D;
		hpBarMedium = Resources.Load("HPBarMedium") as Texture2D;
		hpBarLow = Resources.Load("HPBarLow") as Texture2D;
		unitType = 6;
		unitName = "Buffing Unit";
		hp = 40;
		maxHP = 40;
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
