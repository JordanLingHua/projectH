﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	bool turn = true;
	public bool movingPiece;
	public enum gameState {playerMv,playerAtk,opponentMv,opponentAtk}
	
	public GUIText uInfo,mana,timerText,combatLog,suInfo;
	//gamestate: player is moving a unit (1), attacking with a unit (2), enemy turn and moving (3), enemy turn and attakcking (4);
	public gameState gs;
	public int pMana = 1, maxMana = 1;
	readonly int GAME_MAX_MANA = 8;
	public string buttonOption = "Attack";
	TileManager tm;
	
	//Selected unit and available move squares
	public Unit selectedUnit = null;
	public HashSet<TileScript> accessibleTiles = new HashSet<TileScript>();
	
	//tile selected x and tile selected y
	public int tsx, tsy;
	
	//Timer variables
	readonly float TIMER_LENGTH = 60f;
	float timer;
	
	//set
	public HashSet<Unit> playerUnits = new HashSet<Unit>();
	public HashSet<Unit> enemyUnits = new HashSet<Unit>();
	
	
	void Start () {
		timer = TIMER_LENGTH;
		
		suInfo = GameObject.Find("SelectedUnitInfoGUIText").GetComponent<GUIText>();
		tm = GameObject.Find("TileManager").GetComponent<TileManager>();
		uInfo = GameObject.Find("UnitInfoGUIText").GetComponent<GUIText>();
		mana = GameObject.Find("ManaGUIText").GetComponent<GUIText>();
		timerText = GameObject.Find("TimerGUIText").GetComponent<GUIText>();
		combatLog = GameObject.Find("CombatLogGUIText").GetComponent<GUIText>();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
			clearSelection();
		}
		
		
		timerText.text = "Time Left: " + (int)timer;
		//		timer -= Time.deltaTime;
		//	    if ( timer < 0 ){
		//			
		//			nextTurn ();
		//	    }
	}
	
	void OnGUI(){
		
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
		if(GUI.Button (new Rect(Screen.width - 130,0,130,20),buttonOption)){
			tm.clearAllTiles();
			accessibleTiles.Clear();
			if (gs ==  gameState.playerMv){
				gs =  gameState.playerAtk;
				buttonOption = "Move";
				if (selectedUnit != null){
					selectedUnit.GetComponent<Unit>().showAtkTiles();
				}
			}else if (gs == gameState.playerAtk){
				gs =  gameState.playerMv;
				buttonOption = "Attack";
				if (selectedUnit != null){
					selectedUnit.GetComponent<Unit>().showMvTiles( (gs ==  GameManager.gameState.playerAtk  ||  gs ==  GameManager.gameState.playerMv ) ? Unit.allegiance.ally : Unit.allegiance.enemy);
				}
			}
		}
		
		//End turn button
		string buttontext = turn? "(P1 Turn)" : "(P2 Turn)";
		buttontext += "End Turn";
		if (GUI.Button (new Rect(Screen.width - 130,20,130,20),buttontext)){
			nextTurn ();
		}
	}
	
	void clearSelection(){
		suInfo.text = "";
		selectedUnit = null;
		accessibleTiles.Clear();
		tm.clearAllTiles();
	}
	
	void resetPlayerUnits(){
		foreach(Unit x in playerUnits){
			x.atkd = false;
			x.mvd = false;
		}
		
	}
	
	void resetEnemyUnits(){
		foreach(Unit x in enemyUnits){
			x.atkd = false;
			x.mvd = false;
		}
		
	}
	
	void nextTurn(){
		if (!turn) {
			gs = gameState.playerMv;
		}else {
			//gs = gameState.playerMv;
			gs = gameState.opponentMv;
		}

		//reset game clock, mana, and increase max mana
		timer = TIMER_LENGTH;
		
		if (!turn){
			if (maxMana < GAME_MAX_MANA)
				maxMana++;
		}
		turn = !turn;
		pMana = maxMana;
		
		//toggle between players turns and reset units
		tm.clearAllTiles();
		resetPlayerUnits();
		resetEnemyUnits();
		clearSelection();	}
	
}