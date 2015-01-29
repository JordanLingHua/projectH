using UnityEngine;
using System.Collections;
#pragma warning disable 0114

public class UtilityUnit : Unit {
	//NOT IN DEVELOPMENT

	void Start () {
		base.Start ();
		unitType = 5;
		unitName = "Utility Unit";
		hp = 40;
		maxHP = 40;
		atk = 0;//not final
		mvRange = 6;
		mvCost = 2;
		atkRange = 0;//not final
		atkCost = 0;//not final
		unitRole = 503;//Utility
		renderer.material.color = new Color32(0,255,255,1);
	}

}
