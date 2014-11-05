using UnityEngine;
using System.Collections;

public class UnitNine : Unit {

	//void Start () {
	public UnitNine(){
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		name = "Ranged Unit";
		hp = 22;
		maxHP = 22;
		armor = 10;
		atk = 12;
		mvRange = 3;
		mvCost = 2;
		atkRange = 4;
		atkCost = 3;
		
		unitRole = 500;//Ranged
		renderer.material.color = new Color32(96,96,96,1);
	}


	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		name = "Ranged Unit";
		hp = 22;
		maxHP = 22;
		armor = 10;
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
