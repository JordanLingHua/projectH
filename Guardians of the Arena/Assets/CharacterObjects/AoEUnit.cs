using UnityEngine;
using System.Collections;

#pragma warning disable 0114
public class AoEUnit : Unit {
	//NOT IN DEVELOPMENT

	void Start () {
		base.Start ();
		unitType = 4;
		unitName = "AoE Unit";
		hp = 28;
		maxHP = 28;
		atk = 10;
		mvRange = 4;
		mvCost = 1;
		atkRange = 1;
		atkCost = 2;
		//unitRole = "AOE";
		unitRole = 502;//AOE
		renderer.material.color = new Color32(0,255,0,1);
	}
}
