using UnityEngine;
using System.Collections;

#pragma warning disable 0114
public class Rock : Unit {

	void Start(){
		base.Start ();
		alleg = allegiance.neutral;
		displayHPBar = false;
		unitName = "Rock";
		hp = 1;
		maxHP = 1;
		invincible = true;
		unitType = 21;
		description = "Indestructable environment";
	}
}