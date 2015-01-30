using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public enum gameState {playerMv,playerAtk}

	//combat log window variables
	public GUISkin mySkin;
	int maxCombatLogMessages = 15;
	string combatLogText;
	Vector2 combatLogScrollPos;
	private Rect combatLogWindowRect;
	float combatLogWidth = Screen.width*0.43f;
	float combatLogHeight = Screen.width*0.28f;
	ArrayList combatLogMessages = new ArrayList();

	//game managing variables
	public bool turn, incMana,gameOver;
	public GameObject popUpText;
	public GUIText uInfo,mana,timerText,suInfo,suLevel1BonusShort,suLevel1BonusLong,suLevel2BonusShort,suLevel2BonusLong;
	public gameState gs;
	public int pMana,maxMana;
	public string buttonOption = "Attack";
	TileManager tm;
	GameProcess gp;
	AudioManager am;
	int unitActionOption;
	string[] unitOptionStrings = new string[] {"Mo(v)e","(A)ttack"};
	//Selected unit and available move squares
	public Unit selectedUnit = null;
	public HashSet<TileScript> accessibleTiles = new HashSet<TileScript>();

	public GameObject UnitOne,UnitTwo,UnitThree,UnitFour,UnitFive,UnitSix,UnitSeven,UnitEight,UnitNine,UnitTen,UnitEleven;
//	GameObject selectedUnitDisplay;

	//Timer variables
	readonly float TIMER_LENGTH = 60f;
	float timer;
	public Dictionary<int,Unit>	units = new Dictionary<int, Unit>();	
	
	void Start () {
		combatLogScrollPos = new Vector2 (0.0f, 0.0f);
		combatLogText = "";
		combatLogWindowRect = new Rect (Screen.width-combatLogWidth,Screen.height-combatLogHeight+20, combatLogWidth, combatLogHeight);
		pMana = 2;
		maxMana = 2;
		timer = TIMER_LENGTH;
		am = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		gp = GameObject.Find ("GameProcess").GetComponent<GameProcess>();
		suInfo = GameObject.Find("SelectedUnitInfoGUIText").GetComponent<GUIText>();


		if (Application.loadedLevelName.Equals ("BoardScene")) {
			tm = GameObject.Find ("TileManager").GetComponent<TileManager> ();
			suLevel1BonusLong = GameObject.Find("Level1BonusDescriptionGUIText").GetComponent<GUIText>();
			suLevel1BonusShort = GameObject.Find("Level1BonusNameGUIText").GetComponent<GUIText>();
			suLevel2BonusLong = GameObject.Find("Level2BonusDescriptionGUIText").GetComponent<GUIText>();
			suLevel2BonusShort = GameObject.Find("Level2BonusNameGUIText").GetComponent<GUIText>();
			suLevel1BonusShort.color = Color.gray;
			suLevel1BonusLong.color = Color.gray;
			suLevel2BonusShort.color = Color.gray;
			suLevel2BonusLong.color = Color.gray;
		}
		mana = GameObject.Find("ManaGUIText").GetComponent<GUIText>();
		timerText = GameObject.Find("TimerGUIText").GetComponent<GUIText>();
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

		if (Input.GetKeyDown (KeyCode.V)) {
			changeToMoving();
		}
		if (Input.GetKeyDown (KeyCode.A)){
			changeToAttacking();
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			endTurn();	
		}


		timerText.text = "Time Left: " + (int)timer;

		if (!gameOver){
			timer -= Time.deltaTime;
		    if (timer <= 0  && turn){
				endTurn ();
				timer = 0f;
			}
		}

	}
	
	public void addLogToCombatLog(string log){
		combatLogMessages.Add (log+"\n");
		if (combatLogMessages.Count > maxCombatLogMessages){
			combatLogMessages.RemoveAt(maxCombatLogMessages);
		}
		combatLogText = "";
		for (int i = 0; i < combatLogMessages.Count; i ++){
			combatLogText += combatLogMessages[i];
		}
		combatLogScrollPos.y = Mathf.Infinity;
	}

	void combatLogWindow (int windowID) 
	{
		GUILayout.BeginVertical ();
		GUILayout.Space (8);
		
		GUILayout.Label ("Combat Log");
		GUILayout.Label("", "Divider");
		
		
		GUILayout.BeginHorizontal ();
		combatLogScrollPos = GUILayout.BeginScrollView (combatLogScrollPos , false, true);
		GUILayout.Label (combatLogText, "PlainText");
		GUILayout.EndScrollView ();
		GUILayout.EndHorizontal ();
		//GUILayout.Space (8);
		
		GUILayout.EndVertical();
	}


	void OnGUI(){
		GUI.skin = mySkin;
		combatLogWindowRect = GUI.Window (2, combatLogWindowRect, combatLogWindow, "");
		GUI.BeginGroup (new Rect (0,0,100,100));
		GUI.EndGroup();
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
			info += (script.unitLevel == 3 || script.unitName.Equals("Shrub") )? "": "\nXP: " + script.xp + "/" + script.XP_TO_LEVEL[script.unitLevel-1];

			if (!script.paralyzed){
				info +=  script.atk > 0? "\nDamage: " + script.atk : "";
				info += script.mvCost > 0? "\nMove Cost: " + script.mvCost : "";
				info += script.atkCost > 0? "\nAttack Cost: " + script.atkCost : "";
			}else{
				info += "\nFocused! Cannot move or attack!";
				info += "\nAttack enemy Mystic to\nbreak his channel";
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
			if (script.unitLevel >= 2){
				suLevel1BonusShort.color = Color.white;
				suLevel1BonusLong.color = Color.white;
			}
			if (script.unitLevel >= 3){
				suLevel2BonusShort.color = Color.white;
				suLevel2BonusLong.color = Color.white;
			}
			suLevel1BonusShort.text = script.levelBonusShort[0];
			suLevel1BonusLong.text = script.levelBonusLong[0];
			suLevel2BonusShort.text = script.levelBonusShort[1];
			suLevel2BonusLong.text = script.levelBonusLong[1];
		}
		//set display for mana

		mana.text = turn? "Mana: " + pMana + "/" + maxMana : "Opponent's Mana: " + pMana + "/" + maxMana;


		//Button to toggle between attacking and moving a piece
		//only change options if changed
		int prev = unitActionOption;
		unitActionOption = GUI.SelectionGrid(new Rect(Screen.width*0.03f, Screen.height-((float)Screen.height*0.105f),Screen.width*0.3f, (Screen.height-((float)Screen.height*0.905f))), unitActionOption, unitOptionStrings, 2);
		if (prev != unitActionOption){
			if (unitActionOption == 0){
				changeToMoving();
			}else{
				print ("Unit action option is " + unitActionOption);
				changeToAttacking();
			}
		}


		if (!gameOver){
			//End turn button
			if (turn){
				if (GUI.Button (new Rect(Screen.width*0.335f,Screen.height-((float)Screen.height*0.105f),Screen.width*0.15f,(Screen.height-((float)Screen.height*0.905f))),"(E)nd Turn")){
					endTurn ();
				}
			}else{
				GUI.Label(new Rect(Screen.width*0.335f,Screen.height-((float)Screen.height*0.080f),Screen.width*0.15f,35), "Opponent's Turn");
				//GUI.Label(new Rect(Screen.width*0.335f,Screen.height-((float)Screen.height*0.105f),Screen.width*0.3f,(Screen.height-((float)Screen.height*0.905f))), "Opponent's Turn");
			}
		}

	}

	void endTurn(){
		if (turn) {
			gp.returnSocket ().SendTCPPacket ("endTurn");
			am.playButtonSFX ();
		} else {
			am.playButtonSFX ();
			showErrorMessage("It's not your turn!");
		}
	}

	public void changeToAttacking(){
		unitActionOption = 1;
		am.playButtonSFX();
		tm.clearAllTiles();
		accessibleTiles.Clear();		
		gs =  gameState.playerAtk;	
		
		if (selectedUnit != null)
			selectedUnit.GetComponent<Unit>().showAtkTiles();
	}

	public void changeToMoving(){
		unitActionOption = 0;
		am.playButtonSFX();
		tm.clearAllTiles ();
		accessibleTiles.Clear ();
		gs = gameState.playerMv;

		if (selectedUnit != null) 
			selectedUnit.GetComponent<Unit>().showMvTiles(selectedUnit.alleg == Unit.allegiance.playerOne? Unit.allegiance.playerOne : Unit.allegiance.playerTwo);
	}
	
	void clearSelection(){
		suInfo.text = "";

		suLevel1BonusShort.color = Color.gray;
		suLevel1BonusLong.color = Color.gray;
		suLevel2BonusShort.color = Color.gray;
		suLevel2BonusLong.color = Color.gray;

		suLevel1BonusShort.text = "";
		suLevel1BonusLong.text = "";
		suLevel2BonusShort.text ="";
		suLevel2BonusLong.text = "";
//		Destroy (selectedUnitDisplay);
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
		accessibleTiles.Clear ();
		//possibly wrong
		if ((gp.playerNumber == 1 && turn) || (gp.playerNumber == 2 && !turn)){
			resetPlayerOneUnits();
			print ("reset player ones units");
		}else{
			print ("reset player twos units");
			resetPlayerTwoUnits();
		}
		clearSelection();	

		gs = gameState.playerMv;
		if (Application.loadedLevelName.Equals ("AIScene") && !turn) {

						GameObject.Find ("AI").GetComponent<AIScript> ().makeGameAction (null);
				}
	}
	
}
