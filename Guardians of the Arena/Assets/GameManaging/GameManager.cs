using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public bool turn, incMana;
	public bool movingPiece,gameOver;
	public enum gameState {playerMv,playerAtk}
	public GameObject popUpText;
	public GUIText uInfo,mana,timerText,combatLog,suInfo;
	//gamestate: player is moving a unit (1), attacking with a unit (2), enemy turn and moving (3), enemy turn and attacking (4);
	public gameState gs;
	public int pMana,maxMana;
	readonly int GAME_MAX_MANA = 12;
	public string buttonOption = "Attack";
	TileManager tm;
	GameProcess gp;
	AudioManager am;
	int unitActionOption;
	string[] unitOptionStrings = new string[] {"Move","Attack"};
	//Selected unit and available move squares
	public Unit selectedUnit = null;
	public HashSet<TileScript> accessibleTiles = new HashSet<TileScript>();

	public GameObject UnitOne,UnitTwo,UnitThree,UnitFour,UnitFive,UnitSix,UnitSeven,UnitEight,UnitNine,UnitTen,UnitEleven;
	GameObject selectedUnitDisplay;

	//Timer variables
	readonly float TIMER_LENGTH = 60f;
	float timer;
	public Dictionary<int,Unit>	units = new Dictionary<int, Unit>();	
	
	void Start () {
		pMana = 2;
		maxMana = 2;
		timer = TIMER_LENGTH;
		am = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		gp = GameObject.Find ("GameProcess").GetComponent<GameProcess>();
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

	public void showErrorMessage(string error){
		GUI.depth = -1;
		Vector3 textPos = new Vector3((Screen.width*0.03f)/Screen.width,(Screen.height-((float)Screen.height*0.02f))/Screen.height,0);
		//TODO: Error with different error text overlapping
		if (GameObject.Find ("ErrorPopUpText(Clone)")!=null){
			Destroy (GameObject.Find ("ErrorPopUpText(Clone)"));
		}
		GameObject text = (GameObject) Instantiate(popUpText,textPos,Quaternion.identity);
		text.GetComponent<ErrorPopUpTextScript> ().StartCoroutine (text.GetComponent<ErrorPopUpTextScript> ().showText (error, Color.red));
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
			clearSelection();
		}

	
		timerText.text = "Time Left: " + (int)timer;
		if (!gameOver){
			timer -= Time.deltaTime;
		    if (timer <= 0  && turn){
				print("timer ended");
				gp.returnSocket().SendTCPPacket("endTurn");
				timer = 0f;
			}
		}

	}
	
	void OnGUI(){
		//extra unit on screen
		//notworking so removed for now
		if (selectedUnit != null){
//			Vector3 worldPoint = gp.playerNumber == 1? new Vector3(122,20,75) : new Vector3(-23,20,30);
//			if (selectedUnitDisplay == null  || selectedUnitDisplay.name != (selectedUnit.unitName + "(Clone)")){
//				Destroy(selectedUnitDisplay);
//				switch(selectedUnit.unitType){
//				case 1:
//					selectedUnitDisplay = (GameObject)Instantiate(UnitOne, worldPoint, new Quaternion());
//					Destroy(selectedUnitDisplay.GetComponent<KnifeThrower>());
//					break;
//				case 2:
//					selectedUnitDisplay = (GameObject)Instantiate(UnitTwo, worldPoint, new Quaternion());
//					Destroy(selectedUnitDisplay.GetComponent<Mystic>());
//					break;
//				case 3:
//					selectedUnitDisplay = (GameObject)Instantiate(UnitThree, worldPoint, new Quaternion());
//					Destroy(selectedUnitDisplay.GetComponent<Templar>());
//					break;
//				case 4:
//					selectedUnitDisplay = (GameObject)Instantiate(UnitFour,worldPoint, new Quaternion());
//					Destroy(selectedUnitDisplay.GetComponent<AoEUnit>());
//					break;
//				case 5:
//					selectedUnitDisplay = (GameObject)Instantiate(UnitFive,worldPoint, new Quaternion());
//					Destroy(selectedUnitDisplay.GetComponent<UtilityUnit>());
//					break;
//				case 6:
//					selectedUnitDisplay = (GameObject)Instantiate(UnitSix, worldPoint, new Quaternion());
//					Destroy(selectedUnitDisplay.GetComponent<BuffingUnit>());
//					break;
//				case 7:
//					selectedUnitDisplay = (GameObject)Instantiate(UnitSeven, worldPoint,new Quaternion());
//					Destroy(selectedUnitDisplay.GetComponent<Swordsman>());
//					break;
//				case 8:
//					selectedUnitDisplay = (GameObject)Instantiate(UnitEight, worldPoint,new Quaternion());
//					Destroy(selectedUnitDisplay.GetComponent<Priest>());
//					break;
//				case 9:
//					selectedUnitDisplay = (GameObject)Instantiate(UnitNine, worldPoint, new Quaternion());
//					Destroy(selectedUnitDisplay.GetComponent<RangedUnit>());
//					break;
//				case 10:
//					selectedUnitDisplay = (GameObject)Instantiate(UnitTen, worldPoint, new Quaternion());
//					Destroy(selectedUnitDisplay.GetComponent<Guardian>());
//					break;
//				case 11:
//					selectedUnitDisplay = (GameObject)Instantiate(UnitEleven,worldPoint,new Quaternion());
//					Destroy(selectedUnitDisplay.GetComponent<SoulStone>());
//					break;
//				}
//				selectedUnitDisplay.transform.localScale = new Vector3(20,20,20);
//
//			}

			Unit script = selectedUnit.GetComponent<Unit>();
			string info ="Level " + script.unitLevel + " " + script.unitName;
			info += "\nHP: " + script.hp + "/" + script.maxHP;
			info += script.unitLevel == 3? "": "\nXP: " + script.xp + "/" + script.XP_TO_LEVEL[script.unitLevel-1];
			info +=  script.atk > 0? "\nDamage: " + script.atk : "";
			info += script.mvCost > 0? "\nMove Cost: " + script.mvCost : "";
			info += script.atkCost > 0? "\nAttack Cost: " + script.atkCost : "";

			
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
		//only change options if changed
		int prev = unitActionOption;
		unitActionOption = GUI.SelectionGrid(new Rect(Screen.width*0.03f, Screen.height-((float)Screen.height*0.105f),Screen.width*0.3f, (Screen.height-((float)Screen.height*0.905f))), unitActionOption, unitOptionStrings, 2);
		if (prev != unitActionOption){
			am.playButtonSFX();
			if (unitActionOption == 0){
				tm.clearAllTiles ();
				accessibleTiles.Clear ();
				gs = gameState.playerMv;
	
				if (selectedUnit != null) 
					selectedUnit.GetComponent<Unit>().showMvTiles(turn ? Unit.allegiance.playerOne : Unit.allegiance.playerTwo);
			}else{
				tm.clearAllTiles();
				accessibleTiles.Clear();		
				gs =  gameState.playerAtk;	
	
				if (selectedUnit != null)
					selectedUnit.GetComponent<Unit>().showAtkTiles();
			}
		}


		if (!gameOver){
			//End turn button
			if (turn){
				if (GUI.Button (new Rect(Screen.width*0.335f,Screen.height-((float)Screen.height*0.105f),Screen.width*0.15f,(Screen.height-((float)Screen.height*0.905f))),"End Turn")){
					if (turn){
						gp.returnSocket().SendTCPPacket("endTurn");
						am.playButtonSFX();
					}else{
						am.playErrorSFX();
					}
					
				}
			}else{
				GUI.Label(new Rect(Screen.width*0.335f,Screen.height-((float)Screen.height*0.105f),Screen.width*0.3f,(Screen.height-((float)Screen.height*0.905f))), "Opponent's Turn");
			}
		}

	}
	
	void clearSelection(){
		suInfo.text = "";
		Destroy (selectedUnitDisplay);
		selectedUnit = null;
		accessibleTiles.Clear();
		tm.clearAllTiles();
	}

	void resetPlayerOneUnits(){
		foreach (int key in units.Keys){
			if (units[key].alleg == Unit.allegiance.playerOne){
				units[key].resetUnitAbilities();
			}
		}
	}
	
	void resetPlayerTwoUnits(){
		foreach (int key in units.Keys){
			if (units[key].alleg == Unit.allegiance.playerTwo){
				units[key].resetUnitAbilities();
			}
		}
	}

	public void nextTurn(int mana){
		//reset game clock, mana, and increase max mana
		timer = TIMER_LENGTH;
		turn = !turn;
		pMana = maxMana = mana;
		
		//toggle between players turns and reset units
		tm.clearAllTiles();

		//possibly wrong
		if ((gp.playerNumber == 1 && turn) || (gp.playerNumber == 2 && !turn)){
			resetPlayerOneUnits();
			print ("reset player ones units");
		}else{
			print ("reset player twos units");
			resetPlayerTwoUnits();
		}
		clearSelection();	
	}
	
}
