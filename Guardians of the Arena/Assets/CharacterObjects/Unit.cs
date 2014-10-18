using UnityEngine;
using System.Collections;

public class Unit    : MonoBehaviour {
	
    public int unitNum, xpos, ypos, unitType;
	public int hp,maxHP,armor,atk,mvRange,atkRange,mvCost,atkCost;
	public bool atkd, mvd;
	
	GameManager gm;
	
	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}	
	
	// Update is called once per frame
	void Update () {
	}

    void OnMouseDown() {
		gm.uInfo.text = "HP: " + hp + "/" + maxHP + "\nArmor: " + armor + "\nDamage: " + atk;
    }
	
	

	public void setUnitOneType(){
		hp = 18;
		maxHP = 18;
		armor = 8;
		atk = 20;
		mvRange = 4;
		mvCost = 1;
		atkRange = 3;
		atkCost = 2;	
	}
	
	public void setUnitTwoType(){
		hp = 30;
		maxHP = 30;
		armor = 10;
		atk = 0;
		mvRange = 2;
		mvCost = 2;
		atkRange = 3;
		atkCost = 4;	
	}
}
