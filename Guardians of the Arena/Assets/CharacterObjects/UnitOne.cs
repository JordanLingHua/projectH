using UnityEngine;
using System.Collections;

public class UnitOne : Unit {




	void Start () {
		base.Start ();
		unitType = 1;
		unitName = "Knife Juggler";
		hp = 20;
		maxHP = 20;
		atk = 18;
		mvRange = 4;
		mvCost = 2;
		atkRange = 3;
		atkCost = 2;
		//unitRole = "Ranged";
		unitRole = 500;//ranged
		renderer.material.color = new Color32(255,153,204,1);
	}


	 public override void showAtkTiles(){
		if (!atkd){
			showAtkAccessibleTiles(this.transform.parent.GetComponent<TileScript>(),atkRange);
			gm.accessibleTiles.Remove(this.transform.parent.GetComponent<TileScript>());
			
			switch(alleg){
			case allegiance.playerOne:
				this.transform.parent.renderer.material.color = Color.blue;
				break;
			case allegiance.playerTwo:
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
