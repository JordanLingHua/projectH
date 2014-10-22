using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public GUIText uInfo;
	public GUIText mana;
	//int used for whether the player is moving a unit (1), attacking with a unit (2)
	public int gameState = 1, pMana = 1, maxMana = 1;
	string buttonOption = "Attack";
	TileManager tm;
	
	public Unit selectedUnit = null;
	public HashSet<GameObject> accessibleTiles = new HashSet<GameObject>();
	
	//tile selected x and tile selected y
	public int tsx, tsy;
	
	void Start () {
		tm = GameObject.Find("TileManager").GetComponent<TileManager>();
		uInfo = GameObject.Find("UnitInfoGUIText").GetComponent<GUIText>();
		mana = GameObject.Find("ManaGUIText").GetComponent<GUIText>();
	}
	
	void Update () {
	
	}
	
	void OnGUI(){
		//set display for mana
		mana.text = "Mana: " + pMana + "/" + maxMana;
		
		//Button to toggle between attacking and moving a piece
		if(GUI.Button (new Rect(Screen.width - 100,0,100,20),buttonOption)){
			tm.clearAllTiles();
			accessibleTiles.Clear();
			
			uInfo.text = "";
			if (gameState == 1){
				gameState = 2;
				buttonOption = "Move";
			}else if (gameState == 2){
				gameState = 1;
				buttonOption = "Attack";
			}
		}
		
		//End turn button
		if (GUI.Button (new Rect(Screen.width - 100,20,100,20),"End Turn")){
			
		}
	}
}
