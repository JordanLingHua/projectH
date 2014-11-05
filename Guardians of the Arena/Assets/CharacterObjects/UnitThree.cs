using UnityEngine;
using System.Collections;

public class UnitThree : Unit{
	
	//void Start () {
	public UnitThree(){
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		name = "AoE Unit";
		hp = 38;//hp can --//all other stats cannot --//some other stats ++ for growth
		maxHP = 38;//max hp is constant//use as reference to hp
		armor = 20;
		atk = 10;
		mvRange = 2;
		mvCost = 2;
		atkRange = 1;
		atkCost = 3;
		//unitRole = "AOE";//O is NOT a zero.  it is capital O
		unitRole = 502;//AOE
		renderer.material.color = new Color32(255,255,0,1);
	}

	void Start(){
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		name = "AoE Unit";
		hp = 38;//hp can --//all other stats cannot --//some other stats ++ for growth
		maxHP = 38;//max hp is constant//use as reference to hp
		armor = 20;
		atk = 10;
		mvRange = 2;
		mvCost = 2;
		atkRange = 1;
		atkCost = 3;
		//unitRole = "AOE";//O is NOT a zero.  it is capital O
		unitRole = 502;//AOE
		renderer.material.color = new Color32(255,255,0,1);
	}


	void Update () {
	
	}
}
