using UnityEngine;
using System.Collections;

#pragma warning disable 0114
public class Barrel : Unit {
	void Start(){
		base.Start ();
		unitPortrait = Resources.Load("BarrelPortrait") as Texture2D;
		barXOffset = 16;
		barYOffset = 18;
		unitType = 20;
		unitName = "Barrel";
		description = "Destructable environment. Kill it to open paths.";
		alleg = allegiance.neutral;
		hp = 1;
		maxHP = 1;
	}
	
}