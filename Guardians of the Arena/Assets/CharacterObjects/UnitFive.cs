using UnityEngine;
using System.Collections;

public class UnitFive : Unit {

	//void Start () {
	public UnitFive(){
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		name = "Utility Unit";
		hp = 40;
		maxHP = 40;
		armor = 50;
		atk = 0;//not final
		mvRange = 6;
		mvCost = 2;
		atkRange = 0;//not final
		atkCost = 0;//not final
		unitRole = 503;//Utility
		renderer.material.color = new Color32(0,255,255,1);
	}

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		name = "Utility Unit";
		hp = 40;
		maxHP = 40;
		armor = 50;
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
