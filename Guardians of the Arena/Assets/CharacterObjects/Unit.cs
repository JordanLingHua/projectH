using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit    : MonoBehaviour {
	
	//These get set depending on the function call used on this class
	public enum allegiance{ally,neutral,enemy};
	public allegiance alleg;
	//All these are public, so we can modify them all for now.  
	public int unitID, xpos, ypos;//, unitType;
	public int hp,maxHP,atk,mvRange,atkRange,mvCost,atkCost;
	public bool atkd, mvd;
	public string unitName = string.Empty;
	public string info = string.Empty;
	public bool invincible,displayHPBar;
	
	//unit cost will be utilized here or elsewhere
	//public string unitRole;//name called in switch statement here or elsewhere
	//change unitRole to int if we can do defines for each unit role in this code or elsewhere
	public int unitType;
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
	 * 
	 * Temporary unit identification.  For now, it's just colors.  We have to assign them
	 * different models later
	 * 
	 * 
	 * unit1: pink //255,153,204
	 * unit2: orange//255,128,0
	 * unit3: yellow//255,255,0
	 * unit4: green//0,255,0
	 * unit5: cyan//0,255,255
	 * unit6: magenta//255,0,255
	 * unit7: brown//102,51,0
	 * unit8: pear //204,255,153
	 * unit9: grey//96,96,96
	 * unit10: black//0,0,0
	 * unit11: white//255,255,255
	 * 
	 * 
	 * 
	 * DO NOT DELETE THIS COMMENT!!!!//Made them 10 to 20 in case we need to send as byte to server and back
	 * 
	 */
	
	int ID;
	public Texture2D hpBarBG,hpBarHigh,hpBarMedium,hpBarLow;
	public GameManager gm;
	public GameProcess gp;
	public AudioManager am;
	public virtual void Start () {

		hpBarBG = Resources.Load("HPBarBG") as Texture2D;
		hpBarHigh = Resources.Load("HPBarHigh") as Texture2D;
		hpBarMedium = Resources.Load("HPBarMedium") as Texture2D;
		hpBarLow = Resources.Load("HPBarLow") as Texture2D;
		am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		info = string.Empty;
	}	
	
	void Update () {
	}

	//taken from http://answers.unity3d.com/questions/37752/how-to-render-a-colored-2d-rectangle.html
	void DrawQuad(Rect position, Color color) {
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}

	void OnGUI(){
		if (displayHPBar){
			Camera cam = Camera.main;
			Vector3 HPBarPos = cam.WorldToScreenPoint(gameObject.transform.position);
			//GUI.color = Color.red;
			GUI.DrawTexture (new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-10 < 0?Screen.height:Screen.height - HPBarPos.y-10,  25, 5),hpBarBG);
			//DrawQuad (new Rect(0,0,50,50),Color.red);
			//DrawQuad (new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-10 < 0?Screen.height:Screen.height - HPBarPos.y-10,  25, 5),Color.black);
			float barColorSwitch = (float)hp/maxHP;
			if (barColorSwitch > .6){
				GUI.DrawTexture(new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-10, barColorSwitch * 25, 5),hpBarHigh);
			}else if (barColorSwitch > 0.3){
				GUI.DrawTexture(new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-10, barColorSwitch * 25, 5),hpBarMedium);
			}else{
				GUI.DrawTexture(new Rect(HPBarPos.x-15, Screen.height - HPBarPos.y-10, barColorSwitch * 25, 5),hpBarLow);
			}
		}
	}

	void OnMouseEnter(){
		//show unit info when hovering over it

		if (Application.loadedLevelName.Equals("BoardScene")){
			this.transform.parent.renderer.material.shader = Shader.Find ("Toon/Lighted");
			refreshUnitText ();}

		transform.parent.GetComponent<TileScript> ().OnMouseEnter ();
	}


	public virtual List<GameObject> showAoEAffectedTiles(TileScript tile){
		List<GameObject> nothing = new List<GameObject> ();
		return nothing;

	}

	void refreshUnitText()
	{
		info = unitName + "\nHP: " + hp + "/" + maxHP;
		info += mvCost > 0? "\nMove Cost: " + mvCost : "";
		info += atkCost > 0? "\nAttack Cost: " + atkCost : "";
		info += atk > 0? "\nDamage: " + atk : "";

		if (invincible){
			info+="\nINVINCIBLE";
		}
		if (gm.gs == GameManager.gameState.playerMv && mvd){
			info += "\nAlready moved";
		}
		if (gm.gs == GameManager.gameState.playerAtk && atkd){
			info += "\nAlready attacked";
		}
		gm.uInfo.text = info;
	}
	
	void OnMouseExit(){
		//clear unit info when not hovering over it
		gm.uInfo.text  = "";

		if (Application.loadedLevelName.Equals("BoardScene"))
			this.transform.parent.renderer.material.shader = Shader.Find ("Toon/Basic");
		  

	}
	
	void OnMouseDown() {
		//Attack this piece if:
		//the game is in attack mode
		//the unit selected is in range of the selected unit
		if (gm.turn) {
			if (gm.gs == GameManager.gameState.playerAtk 
					&& gm.accessibleTiles.Contains (this.transform.parent.GetComponent<TileScript> ())) {
				transform.parent.GetComponent<TileScript>().attackTile ();
			} else {
				selectUnit ();
				am.playButtonSFX();
			}
		}
	}

	//TODO: move this logic to the server
	public void attackThisUnit(Unit unitThatAttacked){
			
		if (!invincible){
			gm.combatLog.text = "Combat Log:\nDealt " + unitThatAttacked.atk + " damage!";
			this.hp -= unitThatAttacked.atk;
			
			//if the unit attacked was killed, remove it from the board and unit list
			if (this.hp <=0){				

				gm.units.Remove(unitID);

				this.transform.parent.GetComponent<TileScript>().objectOccupyingTile = null;
				Destroy(gameObject);
				
			}
		}else{
			gm.combatLog.text = "Combat Log:\nTarget is invincible!";
		}
		//clean up the board colors
		gm.accessibleTiles.Clear();
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
		refreshUnitText();

	}

	void selectUnit(){
		gm.selectedUnit = this;
		gm.accessibleTiles.Clear();
		
		this.transform.parent.gameObject.transform.parent.GetComponent<TileManager>().clearAllTiles();
		//if player is moving a piece
		if (gm.gs == GameManager.gameState.playerMv){
			showMvTiles(alleg == allegiance.ally? allegiance.ally : allegiance.enemy);
				          
		//if player is attacking with a piece	
		}else if (gm.gs == GameManager.gameState.playerAtk){
			showAtkTiles();
		}
	}
	
	public void showMvTiles(allegiance ally){
		if (!mvd){
			showMvAccessibleTiles(this.transform.parent.GetComponent<TileScript>(),mvRange,ally);
			//can't move to the tile it's in
			gm.accessibleTiles.Remove(this.transform.parent.GetComponent<TileScript>());	
		}
	}
	
	public virtual void showAtkTiles(){
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
	
	void showMvAccessibleTiles(TileScript tile, int num,allegiance ally){
		TileScript tileS = tile.transform.GetComponent<TileScript>();
		
		if (tileS.objectOccupyingTile == null){
			tile.renderer.material.color = Color.green;
		}
		
		if (num!=0){
			if (tileS.up != null && (tileS.up.GetComponent<TileScript>().objectOccupyingTile ==null  || tileS.up.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == ((ally == allegiance.ally) ? allegiance.ally: allegiance.enemy))){
				showMvAccessibleTiles(tileS.up.GetComponent<TileScript>(),num-1,ally);
				gm.accessibleTiles.Add(tileS.up.GetComponent<TileScript>());
			}
			if (tileS.down != null && (tileS.down.GetComponent<TileScript>().objectOccupyingTile ==null  || tileS.down.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == ((ally == allegiance.ally) ? allegiance.ally: allegiance.enemy))){
				showMvAccessibleTiles(tileS.down.GetComponent<TileScript>(),num-1,ally);
				gm.accessibleTiles.Add(tileS.down.GetComponent<TileScript>());
			}
			if (tileS.left != null && (tileS.left.GetComponent<TileScript>().objectOccupyingTile ==null || tileS.left.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == ((ally == allegiance.ally) ? allegiance.ally: allegiance.enemy))){
				showMvAccessibleTiles(tileS.left.GetComponent<TileScript>(),num-1,ally);
				gm.accessibleTiles.Add(tileS.left.GetComponent<TileScript>());
			}
			if (tileS.right != null && (tileS.right.GetComponent<TileScript>().objectOccupyingTile ==null || tileS.right.GetComponent<TileScript>().objectOccupyingTile.GetComponent<Unit>().alleg == ((ally == allegiance.ally) ? allegiance.ally: allegiance.enemy))){
				showMvAccessibleTiles(tileS.right.GetComponent<TileScript>(),num-1,ally);
				gm.accessibleTiles.Add(tileS.right.GetComponent<TileScript>());
			}	
		}
	}
	
	public void showAtkAccessibleTiles(TileScript tile, int num){
		tile.renderer.material.color = new Color(1f,0.4f,0f, 0f);
		TileScript tileS = tile.transform.GetComponent<TileScript>();
		if (num != 0){
			if (tileS.up != null){
				showAtkAccessibleTiles(tileS.up.GetComponent<TileScript>(),num-1);
				gm.accessibleTiles.Add(tileS.up.GetComponent<TileScript>());
			}
			if (tileS.down != null){
				showAtkAccessibleTiles(tileS.down.GetComponent<TileScript>(),num-1);
				gm.accessibleTiles.Add(tileS.down.GetComponent<TileScript>());
			}
			if (tileS.left != null){
				showAtkAccessibleTiles(tileS.left.GetComponent<TileScript>(),num-1);
				gm.accessibleTiles.Add(tileS.left.GetComponent<TileScript>());
			}
			if (tileS.right != null){
				showAtkAccessibleTiles(tileS.right.GetComponent<TileScript>(),num-1);
				gm.accessibleTiles.Add(tileS.right.GetComponent<TileScript>());
			}
		}
	}
	
	public void makeTree(){
		alleg = allegiance.neutral;
		unitName = "Shrub";
		hp = 1;
		maxHP = 1;
	}
	
}











 