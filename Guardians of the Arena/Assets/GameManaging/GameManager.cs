using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public bool turn;
	public bool movingPiece;
	public enum gameState {playerMv,playerAtk}
	public bool showReturnButton;
	
	public GUIText uInfo,mana,timerText,combatLog,suInfo;
	//gamestate: player is moving a unit (1), attacking with a unit (2), enemy turn and moving (3), enemy turn and attacking (4);
	public gameState gs;
	public int pMana = 1, maxMana = 1;
	readonly int GAME_MAX_MANA = 8;
	public string buttonOption = "Attack";
	TileManager tm;
	GameProcess gp;
	
	//Selected unit and available move squares
	public Unit selectedUnit = null;
	public HashSet<TileScript> accessibleTiles = new HashSet<TileScript>();
	
	//tile selected x and tile selected y
	public int tsx, tsy;
	
	//Timer variables
	readonly float TIMER_LENGTH = 60f;
	float timer;
	public Dictionary<int,Unit>	units = new Dictionary<int, Unit>();	
	
	void Start () {
		timer = TIMER_LENGTH;
	
		gp = GameObject.Find ("GameProcess").GetComponent<GameProcess>();
		showReturnButton = false;
		suInfo = GameObject.Find("SelectedUnitInfoGUIText").GetComponent<GUIText>();

		if(Application.loadedLevelName.Equals("BoardScene"))
		tm = GameObject.Find("TileManager").GetComponent<TileManager>();
		uInfo = GameObject.Find("UnitInfoGUIText").GetComponent<GUIText>();
		mana = GameObject.Find("ManaGUIText").GetComponent<GUIText>();
		timerText = GameObject.Find("TimerGUIText").GetComponent<GUIText>();
		combatLog = GameObject.Find("CombatLogGUIText").GetComponent<GUIText>();

		if (gp.playerNumber == 1)
			turn = true;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
			clearSelection();
		}
		
		timerText.text = "Time Left: " + (int)timer;

	}
	
	void OnGUI(){

		if (showReturnButton && GUI.Button (new Rect (Screen.width / 2 - 75, Screen.height / 2, 130, 20), "Return to Menu"))
		{
			showReturnButton = false;
			DontDestroyOnLoad(GameObject.Find ("GameProcess"));
			Application.LoadLevel(1);
		}
		
		if (selectedUnit != null){
			Unit script = selectedUnit.GetComponent<Unit>();
			string info = script.name + "\nHP: " + script.hp + "/" + script.maxHP + "\nArmor: " + script.armor;
			info +=  script.atk > 0? "\nDamage: " + script.atk : "";
			
			if (gs ==  gameState.playerMv || gs == gameState.playerMv) {
				info += script.mvCost > 0? "\nMove Cost: " + script.mvCost : "";
			} else {
				info += script.atkCost > 0? "\nAttack Cost: " + script.atkCost : "";
			}
			
			if (script.invincible){
				info+="\nINVINCIBLE";
			}
			if (gs == gameState.playerMv && script.mvd){
				info += "\nAlready moved";
			}
			if (gs ==  gameState.playerAtk && script.atkd){
				info += "\nAlready attacked";
			}
			
			suInfo.text =  info;
		}
		//set display for mana
		mana.text = "Mana: " + pMana + "/" + maxMana;
		
		//Button to toggle between attacking and moving a piece
		if (GUI.Button (new Rect (Screen.width - 260, 0, 130, 20), "Move")) 
		{
			tm.clearAllTiles ();
			accessibleTiles.Clear ();
			gs = gameState.playerMv;

			if (selectedUnit != null) 
				selectedUnit.GetComponent<Unit>().showMvTiles(turn ? Unit.allegiance.ally : Unit.allegiance.enemy);
		}

		if(GUI.Button (new Rect(Screen.width - 130,0,130,20),"Attack")){
			tm.clearAllTiles();
			accessibleTiles.Clear();		
			gs =  gameState.playerAtk;	

			if (selectedUnit != null)
				selectedUnit.GetComponent<Unit>().showAtkTiles();				
		}
		
		//End turn button
		string buttontext = turn ? "End Turn" : "Opponent Turn ";
		if (GUI.Button (new Rect(Screen.width - 130,20,130,20),buttontext)){
			gp.returnSocket().SendTCPPacket("endTurn");
		}
	}
	
	void clearSelection(){
		suInfo.text = "";
		selectedUnit = null;
		accessibleTiles.Clear();
		tm.clearAllTiles();
	}

	void resetPlayerUnits(){
		foreach (int key in units.Keys){
			if (units[key].alleg == Unit.allegiance.ally){
				units[key].atkd = false;
				units[key].mvd = false;
			}
		}
	}
	
	void resetEnemyUnits(){
		foreach (int key in units.Keys){
			if (units[key].alleg == Unit.allegiance.enemy){
				units[key].atkd = false;
				units[key].mvd = false;
			}
		}
	}
	public void nextTurn(){
		//reset game clock, mana, and increase max mana
		timer = TIMER_LENGTH;
		
		if (!turn){
			gs = gameState.playerMv;

			if (maxMana < GAME_MAX_MANA)
				maxMana++;
		}
		turn = !turn;
		pMana = maxMana;
		
		//toggle between players turns and reset units
		tm.clearAllTiles();
		resetPlayerUnits();
		resetEnemyUnits();
		clearSelection();	
	}
	
}
