using UnityEngine;
using System.Collections;

public class UnitEight : Unit {



	void Start () {
	
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		hp = 20;
		maxHP = 20;
		armor = 0;//not final
		atk = 0;//not final
		mvRange = 3;
		mvCost = 3;
		atkRange = 0;//not final
		atkCost = 6;
		
		unitRole = 506;//Healer
		renderer.material.color = new Color32(204,255,153,1);
	}



	void Update () {
	
	}
}
