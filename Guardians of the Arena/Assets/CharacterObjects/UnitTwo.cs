using UnityEngine;
using System.Collections;

public class UnitTwo: Unit {
	


	void Start () {
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		hpBarBG = Resources.Load("HPBarBG") as Texture2D;
		hpBarHigh = Resources.Load("HPBarHigh") as Texture2D;
		hpBarMedium = Resources.Load("HPBarMedium") as Texture2D;
		hpBarLow = Resources.Load("HPBarLow") as Texture2D;
		unitType = 2;
		unitName = "Focus Mage";
		hp = 30;
		maxHP = 30;
		atk = 0;//change later
		mvRange = 2;
		mvCost = 2;
		atkRange = 4;
		atkCost = 4;	
		//unitRole = "BuffDebuff";
		unitRole = 504;//BuffDebuff
		renderer.material.color = new Color32(255,128,0,1);
	}
	
	public override void showAtkTiles(){
		if (!atkd){
			showAtkAccessibleTiles(this.transform.parent.GetComponent<TileScript>(),atkRange);
			gm.accessibleTiles.Remove(this.transform.parent.GetComponent<TileScript>());
			
			switch(alleg){
			case allegiance.ally:
				this.transform.parent.renderer.material.color = Color.blue;
				break;
			case allegiance.enemy:
				this.transform.parent.renderer.material.color = Color.red;
				break;
			case allegiance.neutral:
				this.transform.parent.renderer.material.color = Color.gray;
				break;
			}
		}
	}
	
	
	new void showAtkAccessibleTiles(TileScript tile, int num){
		
		TileScript tileS = tile.transform.GetComponent<TileScript>();
		
		TileScript temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.up != null){
				gm.accessibleTiles.Add(temp.up.GetComponent<TileScript>());
				temp = temp.up.GetComponent<TileScript>();
				temp.gameObject.renderer.material.color = new Color(1f,0.4f,0f, 0f);
			}
		}
		temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.down != null){
				gm.accessibleTiles.Add(temp.down.GetComponent<TileScript>());
				temp = temp.down.GetComponent<TileScript>();
				temp.gameObject.renderer.material.color = new Color(1f,0.4f,0f, 0f);
			}
		}
		temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.left != null){
				gm.accessibleTiles.Add(temp.left.GetComponent<TileScript>());
				temp = temp.left.GetComponent<TileScript>();
				temp.gameObject.renderer.material.color = new Color(1f,0.4f,0f, 0f);
			}
		}
		temp = tileS;
		for (int i = 0; i < atkRange; i ++){
			if (temp.right != null){
				gm.accessibleTiles.Add(temp.right.GetComponent<TileScript>());
				temp = temp.right.GetComponent<TileScript>();
				temp.gameObject.renderer.material.color = new Color(1f,0.4f,0f, 0f);
			}
		}
	}

	void Update () {
		
	}
}
