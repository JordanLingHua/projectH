using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit    : MonoBehaviour {

	//These get set depending on the function call used on this class

	//All these are public, so we can modify them all for now.  
    public int unitID, xpos, ypos, unitType;
	public int hp,maxHP,armor,atk,mvRange,atkRange,mvCost,atkCost;
	public bool atkd, mvd;

	public bool invincible;

	//unit cost will be utilized here or elsewhere
	//public string unitRole;//name called in switch statement here or elsewhere
	//change unitRole to int if we can do defines for each unit role in this code or elsewhere
	public int unitRole;//compare this int to the ints provided inside gameManager or wherever unitRole is compared
	/*
	 * DO NOT DELETE THIS COMMENT!!!!
	 * Jordan Hua
	 * 
	 * Unit Roles:  
	 * 
	 * DO NOT DELETE THIS COMMENT!!!!//Made them 500 in case we need to send as byte to server and back
	 * Rubric-  
	 * Ranged = 500
	 * BuffDebuff = 501
	 * AOE = 502
	 * Utility = 503
	 * BuffDebuff = 504
	 * MeleeTank = 505
	 * Healer = 506
	 * Kingpin = 507
	 * 
	 * Unit Abilities (AKA UNIT ID, and use ID to identify whose abilities are whose):  
	 * Rubric- 
	 * unitID0 = 10
	 * unitID1 = 11
	 * unitID2 = 12
	 * unitID3 = 13
	 * unitID4 = 14
	 * unitID5 = 15
	 * unitID6 = 16
	 * unitID7 = 17
	 * unitID8 = 18
	 * unitID9 = 19
	 * unitID10 = 20
	 * 
	 * DO NOT DELETE THIS COMMENT!!!!//Made them 10 to 20 in case we need to send as byte to server and back
	 * 
	 */

	int ID;

	GameManager gm;
	
	void Start () {

		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}	
	
	void Update () {
	}
	
	void OnMouseEnter(){
		//show unit info when hovering over it
		string info = "HP: " + hp + "/" + maxHP + "\nArmor: " + armor + "\nDamage: " + atk;
		if (invincible){
			info+="\nINVINCIBLE";
		}
		if (gm.gameState == 1 && mvd){
			info += "\nAlready moved";
		}
		if (gm.gameState == 2 && atkd){
			info += "\nAlready attacked";
		}
		gm.uInfo.text = info;
		
	}
	
	void OnMouseExit(){
		//clear unit info when not hovering over it
		gm.uInfo.text  = "";
	}
	
    void OnMouseDown() {
		//Attack this piece if:
		//this unit is not the currently selected unit (no attacking self)
		//the game is in attack mode
		//the unit selected is in range of the selected unit
		if (gm.selectedUnit != this && gm.gameState == 2 && gm.accessibleTiles.Contains(this.transform.parent.gameObject)){
			attackUnit();
		}else{
			selectUnit ();
		}
    }
	
	void playerSSKillable(){
		foreach (Unit x in gm.playerUnits){
			if (x.ID == 20){
				x.invincible = false;
				break;
			}
		}
	}
	
	void enemySSKillable(){
		foreach (Unit x in gm.enemyUnits){
			if (x.ID == 20){
				x.invincible = false;
				break;
			}
		}
	}
	
	void attackUnit(){
		if (gm.pMana >= gm.selectedUnit.GetComponent<Unit>().atkCost){
			gm.pMana -= gm.selectedUnit.GetComponent<Unit>().atkCost;
			gm.selectedUnit.transform.GetComponent<Unit>().atkd = true;
			
			if (!invincible){
				gm.combatLog.text = "Combat Log:\nDealt " + (int)(gm.selectedUnit.transform.GetComponent<Unit>().atk * ((100 - this.armor) * 0.01))+ " damage!";
				this.hp -= (int)(gm.selectedUnit.transform.GetComponent<Unit>().atk * ((100 - this.armor) * 0.01));
				
				//if the unit attacked was killed, remove it from the board and unit list
				if (this.hp <=0){
					
					
					//make the soulstone vulnerable if the player guardian was killed
					if (this.ID == 19){
						if (gm.playerUnits.Contains(this)){
							playerSSKillable();
						}else{
							enemySSKillable();
						}
					}
					
					
					if (this.ID == 20){
						if(gm.playerUnits.Contains(this)){
						
							gm.combatLog.text = "Player 2 has won!";
						}else{
							gm.combatLog.text = "Player 1 has won!";
						}
					} 
						
						
					if (gm.playerUnits.Contains(this)){
						gm.playerUnits.Remove(this);
					}else{
						gm.enemyUnits.Remove(this);
					}
					this.transform.parent.GetComponent<TileScript>().occupied = 0;
					this.transform.parent.GetComponent<TileScript>().objectOccupyingTile = null;
					Destroy(gameObject);
					
				}
			}else{
				gm.combatLog.text = "Combat Log:\nTarget is invincible!";
			}
			//clean up the board colors
			gm.accessibleTiles.Clear();
			this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
			
			
		}else{
			gm.combatLog.text = "Combat Log:\nNot enough mana";
		}
	}
	
	void selectUnit(){
		gm.selectedUnit = this;
		gm.accessibleTiles.Clear();
		
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
		//if player is moving a piece
		if (gm.gameState == 1){
			showMvTiles();
		//if player is attacking with a piece	
		}else if (gm.gameState == 2){
			showAtkTiles();
		}
		
	}
	
	public void showMvTiles(){
		if (!mvd){
			showMvAccessibleTiles(this.transform.parent.gameObject,mvRange);
			//can't move to the tile it's in
			gm.accessibleTiles.Remove(this.transform.parent.gameObject);
			
			//can't move to tiles that it's ally occupies
			List<GameObject> temp = new List<GameObject>();
			foreach (GameObject tile in gm.accessibleTiles){
				if (tile.transform.GetComponent<TileScript>().occupied !=0){
					temp.Add(tile);
				}
			}
			foreach (GameObject tile in temp){
				gm.accessibleTiles.Remove(tile);
			}
		}
	}
	
	public void showAtkTiles(){
		if (!atkd){
			showAtkAccessibleTiles(this.transform.parent.gameObject,atkRange);
			gm.accessibleTiles.Remove(this.transform.parent.gameObject);
		}
	}
	
	void showMvAccessibleTiles(GameObject tile, int num){
		if (tile.transform.GetComponent<TileScript>().occupied == 0){
			tile.renderer.material.color = Color.green;
		}
		if (num == 0){
			
		}else{
			if (tile.transform.GetComponent<TileScript>().up != null && tile.transform.GetComponent<TileScript>().up.transform.GetComponent<TileScript>().occupied != 2){
				showMvAccessibleTiles(tile.transform.GetComponent<TileScript>().up,num-1);
				gm.accessibleTiles.Add(tile.transform.GetComponent<TileScript>().up);
			}
			if (tile.transform.GetComponent<TileScript>().down != null && tile.transform.GetComponent<TileScript>().down.transform.GetComponent<TileScript>().occupied != 2){
				showMvAccessibleTiles(tile.transform.GetComponent<TileScript>().down,num-1);
				gm.accessibleTiles.Add(tile.transform.GetComponent<TileScript>().down);
			}
			if (tile.transform.GetComponent<TileScript>().left != null && tile.transform.GetComponent<TileScript>().left.transform.GetComponent<TileScript>().occupied != 2) {
				showMvAccessibleTiles(tile.transform.GetComponent<TileScript>().left,num-1);
				gm.accessibleTiles.Add(tile.transform.GetComponent<TileScript>().left);
			}
			if (tile.transform.GetComponent<TileScript>().right != null && tile.transform.GetComponent<TileScript>().right.transform.GetComponent<TileScript>().occupied != 2){
				showMvAccessibleTiles(tile.transform.GetComponent<TileScript>().right,num-1);
				gm.accessibleTiles.Add(tile.transform.GetComponent<TileScript>().right);
			}	
		}
	}
	
	void showAtkAccessibleTiles(GameObject tile, int num){
		if (tile.transform.GetComponent<TileScript>().occupied == 0){
			tile.renderer.material.color = Color.green;
		}
		if (num == 0){
			
		}else{
			if (tile.transform.GetComponent<TileScript>().up != null){
				showAtkAccessibleTiles(tile.transform.GetComponent<TileScript>().up,num-1);
				gm.accessibleTiles.Add(tile.transform.GetComponent<TileScript>().up);
			}
			if (tile.transform.GetComponent<TileScript>().down != null){
				showAtkAccessibleTiles(tile.transform.GetComponent<TileScript>().down,num-1);
				gm.accessibleTiles.Add(tile.transform.GetComponent<TileScript>().down);
			}
			if (tile.transform.GetComponent<TileScript>().left != null){
				showAtkAccessibleTiles(tile.transform.GetComponent<TileScript>().left,num-1);
				gm.accessibleTiles.Add(tile.transform.GetComponent<TileScript>().left);
			}
			if (tile.transform.GetComponent<TileScript>().right != null){
				showAtkAccessibleTiles(tile.transform.GetComponent<TileScript>().right,num-1);
				gm.accessibleTiles.Add(tile.transform.GetComponent<TileScript>().right);
			}
		}
	}
	
	///*
	public void setUnitOneType(){
		hp = 18;
		maxHP = 18;
		armor = 8;
		atk = 20;
		mvRange = 4;
		mvCost = 1;
		atkRange = 3;
		atkCost = 2;
		//unitRole = "Ranged";
		unitRole = 500;//ranged
	}
	
	public void setUnitTwoType(){
		hp = 30;
		maxHP = 30;
		armor = 10;
		atk = 0;//change later
		mvRange = 2;
		mvCost = 2;
		atkRange = 3;
		atkCost = 4;	
		//unitRole = "BuffDebuff";
		unitRole = 504;//BuffDebuff
	}

	public void setUnitThreeType()
	{
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

	}

	public void setUnitFourType()
	{
		hp = 28;
		maxHP = 28;
		armor = 10;
		atk = 10;
		mvRange = 4;
		mvCost = 1;
		atkRange = 1;
		atkCost = 2;
		//unitRole = "AOE";
		unitRole = 502;//AOE

	}

	public void setUnitFiveType()
	{
		hp = 40;
		maxHP = 40;
		armor = 50;
		atk = 0;//not final
		mvRange = 6;
		mvCost = 2;
		atkRange = 0;//not final
		atkCost = 0;//not final
		unitRole = 503;//Utility
	}

	public void setUnitSixType()
	{
		hp = 40;
		maxHP = 40;
		armor = 25;
		atk = 0;//not final
		mvRange = 2;
		mvCost = 3;
		atkRange = 0;//not final
		atkCost = 0;//not final

		unitRole = 504;//BuffDebuff
	}

	public void setUnitSevenType()
	{
		hp = 25;
		maxHP = 25;
		armor = 35;
		atk = 8;
		mvRange = 3;
		mvCost = 1;
		atkRange = 1;
		atkCost = 1;

		unitRole = 505;//MeleeTank
	}

	public void setUnitEightType()
	{
		hp = 20;
		maxHP = 20;
		armor = 0;//not final
		atk = 0;//not final
		mvRange = 3;
		mvCost = 3;
		atkRange = 0;//not final
		atkCost = 6;

		unitRole = 506;//Healer
	}

	public void setUnitNineType()
	{
		hp = 22;
		maxHP = 22;
		armor = 10;
		atk = 12;
		mvRange = 3;
		mvCost = 2;
		atkRange = 4;
		atkCost = 3;

		unitRole = 500;//Ranged
	}
	
	//Guardian
	public void setUnitTenType()
	{
		hp = 40;
		maxHP = 40;
		armor = 60;
		atk = 8;
		mvRange = 1;
		mvCost = 3;
		atkRange = 1;
		atkCost = 1;

		unitRole = 505;//MeleeTank
	}

	//Soulstone
	public void setUnitElevenType()
	{
		hp = 1;
		maxHP = 1;
		armor = 0;
		atk = 0;
		mvRange = 0;
		mvCost = 0;
		atkRange = 0;
		atkCost = 500;

		invincible = true;//can be changed later
		unitRole = 507;//Kingpin
	}

	//*/

	public void setUnitType(int unitID)
	{

		ID = unitID;

		if(unitID == 10)
		{
			setUnitOneType();
		}
		else if(unitID == 11)
		{
			setUnitTwoType();
		}
		else if(unitID == 12)
		{
			setUnitThreeType();
		}
		else if(unitID == 13)
		{
			setUnitFourType();
		}
		else if(unitID == 14)
		{
			setUnitFiveType();
		}
		else if(unitID == 15)
		{
			setUnitSixType();
		}
		else if(unitID == 16)
		{
			setUnitSevenType();
		}
		else if(unitID == 17)
		{
			setUnitEightType();
		}
		else if(unitID == 18)
		{
			setUnitNineType();
		}
		else if(unitID == 19)
		{
			setUnitTenType();
		}
		else
		{
			//If unitID == 20
			setUnitElevenType();
		}
	}


}












