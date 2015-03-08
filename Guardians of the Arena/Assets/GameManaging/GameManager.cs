using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public bool allowInput;
	public enum gameState {playerMv,playerAtk}

	//combat log window variables
	public GUISkin mySkin;
	int maxCombatLogMessages = 15;
	string combatLogText;
	string graveyardText;
	ArrayList myUnitsLost = new ArrayList();
	ArrayList enemyUnitsLost = new ArrayList();
	Vector2 combatLogScrollPos;
	private Rect combatLogWindowRect;
	float combatLogWidth = Screen.width*0.44f;
	float combatLogHeight = Screen.height * 0.48f;	
	ArrayList combatLogMessages = new ArrayList();
	bool displayCombatLog;

	//game managing variables
	public bool turn, incMana,gameOver,sentEndTurn;
	public GameObject popUpText;
	public GUIText mana,timerText,suInfo,suLevel1BonusShort,suLevel1BonusLong,suLevel2BonusShort,suLevel2BonusLong,unitNameGUI,unitDescriptionGUI;
	public gameState gs;
	public int pMana,maxMana;
	public string buttonOption = "Attack";
	TileManager tm;
	GameProcess gp;
	AudioManager am;
	PopUpMenuNecro pum;
	//Selected unit and available move squares
	public Unit selectedUnit = null,hoverOverUnit = null;
	public HashSet<TileScript> accessibleTiles = new HashSet<TileScript>();

	//Timer variables
	readonly float TIMER_LENGTH = 60f;
	float timer;
	public Dictionary<int,Unit>	units = new Dictionary<int, Unit>();	

	//mana bar display variables
	float percentMana;
	float manaBarXPos ,manaBarYPos ,manaBarWidth, manaBarHeight;
	public Texture2D manaGUIBorder,manaGUIFill;

	void Start () {
		allowInput = true;

		//mana bar display
		pMana = 2;
		maxMana = 2;
		manaBarXPos = 0.562f;
		manaBarYPos = 0.59f;
		manaBarWidth = 0.05f;
		manaBarHeight = -0.55f;
		manaGUIBorder = Resources.Load("manaGUIBorder") as Texture2D;
		manaGUIFill = Resources.Load("manaGUIFill") as Texture2D;

		//combat log info
		graveyardText = "Your units lost:\nNone\nEnemy Units Lost:\nNone";
		displayCombatLog = true;
		combatLogScrollPos = new Vector2 (0.0f, 0.0f);
		combatLogText = "";
		combatLogWindowRect = new Rect (Screen.width-combatLogWidth,Screen.height-combatLogHeight+20, combatLogWidth, combatLogHeight);

		timer = TIMER_LENGTH;
		am = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		gp = GameObject.Find ("GameProcess").GetComponent<GameProcess>();
		suInfo = GameObject.Find("SelectedUnitInfoGUIText").GetComponent<GUIText>();
		pum = GameObject.Find("PopUpMenu").GetComponent<PopUpMenuNecro>();

		if (Application.loadedLevelName.Equals ("BoardScene") || Application.loadedLevelName.Equals ("AIScene")) {
			unitNameGUI = GameObject.Find ("SelectedUnitNameGUIText").GetComponent<GUIText> ();
			unitDescriptionGUI = GameObject.Find ("SelectedUnitDescriptionGUIText").GetComponent<GUIText> ();
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
		Vector3 textPos = new Vector3(0.28f,0.5f,0);

		//TODO: Error with different error text overlapping
		if (GameObject.Find ("ErrorPopUpText(Clone)")!=null){
			Destroy (GameObject.Find ("ErrorPopUpText(Clone)"));
		}
		GameObject text = (GameObject) Instantiate(popUpText,textPos,Quaternion.identity);
		text.GetComponent<ErrorPopUpTextScript> ().StartCoroutine (text.GetComponent<ErrorPopUpTextScript> ().showText (error, Color.red));
	}

	public void showCenterMessage(string message,Color textColor,bool fade){
		GUI.depth = -1;
		GameObject text = (GameObject) Instantiate(GameObject.Find ("CenterPopUpText"),GameObject.Find ("CenterPopUpText").transform.position,Quaternion.identity);
		if (fade) {
			text.GetComponent<CenterPopUpTextScript> ().StartCoroutine (text.GetComponent<CenterPopUpTextScript> ().showText (message, textColor));
		} else {
			text.GetComponent<GUIText>().text = message;
			text.GetComponent<GUIText>().color = textColor;
		}
	}

	void Update () {
		if (allowInput) {
			
			if (Input.GetKeyDown (KeyCode.Escape)) {
				clearSelection ();
			}

			if (Input.GetKeyDown (KeyCode.V)) {
				changeToMoving ();
			}
			if (Input.GetKeyDown (KeyCode.A)) {
				changeToAttacking ();
			}
			if (Input.GetKeyDown (KeyCode.E)) {
				endTurn ();	
			}
		}

		if (!gameOver && !Application.loadedLevelName.Equals ("AIScene")){
			timerText.text = "Time Left: " + (int)timer;
			timer -= Time.deltaTime;
		    if (timer <= 0){
				if (turn){
					endTurn ();
				}
				timer = 0f;
			}
		}

	}

	public void addUnitToGraveyard(string unitName, Unit.allegiance alleg){
		if ((gp.playerNumber == 1 && alleg == Unit.allegiance.playerOne) || (gp.playerNumber == 2 && alleg == Unit.allegiance.playerTwo)){
			myUnitsLost.Add(unitName);
		}else if ((gp.playerNumber == 2 && alleg == Unit.allegiance.playerOne) || (gp.playerNumber == 1 && alleg == Unit.allegiance.playerTwo)){
			enemyUnitsLost.Add (unitName);
		}
		graveyardText = "Your units lost:\n";
		if (myUnitsLost.Count == 0){
			graveyardText += "None";
		}else{
			for (int i = 0; i < myUnitsLost.Count; i++){
				graveyardText += myUnitsLost[i] + " ";
			}
		}
		graveyardText += "\nEnemy Units Lost:\n";
		if (enemyUnitsLost.Count == 0){
			graveyardText += "None";
		}else{
			for (int i = 0; i < enemyUnitsLost.Count; i++){
				graveyardText += enemyUnitsLost[i] + " ";
			}
		}
	}

	
	public void addLogToCombatLog(string log){
		combatLogMessages.Add (log+"\n");
		if (combatLogMessages.Count > maxCombatLogMessages){
			combatLogMessages.RemoveAt(0);
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

		GUILayout.Label ("Controls");
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Mo(v)e", "ShortButton")) {
			changeToMoving();
		}
		if (GUILayout.Button ("(A)ttack", "ShortButton")) {
			changeToAttacking();
		}
		if (gameOver){
			if (GUILayout.Button ("Lobby", "ShortButton")) {
				am.playButtonSFX();
				DontDestroyOnLoad(GameObject.Find ("GameProcess"));
				Application.LoadLevel(1);
			}
		}else if (!gameOver){
			//End turn button
			if (turn){
				if (GUILayout.Button ("(E)nd Turn", "ShortButton")){
					endTurn ();
				}
				GUILayout.Label("Your Turn");
			}else{
				GUILayout.Label("Opponent's Turn");
			}
		}
		GUILayout.EndHorizontal ();

		if (displayCombatLog){
			GUILayout.Label ("Combat Log");
		}else{
			GUILayout.Label("Graveyard");
		}
		GUILayout.BeginHorizontal ();
		combatLogScrollPos = GUILayout.BeginScrollView (combatLogScrollPos , false, true);
		if (displayCombatLog){
			GUILayout.Label (combatLogText, "PlainText");
		}else{
			GUILayout.Label (graveyardText, "PlainText");
		}
		GUILayout.EndScrollView ();
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace();
		if (displayCombatLog){
			if (GUILayout.Button ("Show Graveyard", "ShortButton")){
				displayCombatLog = false;
			}
		}else{
			if (GUILayout.Button ("Show Combat Log", "ShortButton")){
				displayCombatLog = true;
			}
		}
		if (GUILayout.Button ("Options", "ShortButton")) {
			pum.doWindow1 = !pum.doWindow1;
			am.playButtonSFX ();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();

		GUILayout.EndVertical();
	}


	public void displayUnitInfo(Unit unit){
		if (unit!= null){
			Unit script = unit.GetComponent<Unit>();
			
			string info =script.invincible? "" : "\nHP: " + script.hp + "/" + script.maxHP;
			info += (script.unitLevel == 3 || script.alleg == Unit.allegiance.neutral || script.unitType == 11)? "": "\nXP: " + script.xp + "/" + script.XP_TO_LEVEL[script.unitLevel-1];
			
			if (!script.paralyzed){
				if (script.atk > 0 && script.unitType != 11){
					info +="\nDamage: " + script.atk; 
				}else if (script.atk < 0){
					info += "\nHeals for: " + -1*script.atk;
				}else{
					info += "";
				}

				info += script.mvCost > 0? "\nMove Cost: " + script.mvCost : "";
	
				info += script.atkCost > 0? "\nAttack Cost: " + script.atkCost : "";
			}else{
				info += "\nFocused! Cannot move or attack!";
				info += "\nAttack enemy Mystic to\nbreak his channel";
			}
			
			if (script.invincible){
				info+="\nINVINCIBLE";
			}
//			if (gs == gameState.playerMv && script.mvd){
//				info += "\nAlready moved";
//			}
//			if (gs ==  gameState.playerAtk && script.atkd){
//				info += "\nAlready attacked";
//			}
			
			suInfo.text =  info;
			unitNameGUI.text = (script.alleg != Unit.allegiance.neutral && script.unitType != 11) ? "Level " +script.unitLevel + " " + script.unitName : script.unitName;
			unitDescriptionGUI.text = script.wordWrap (50,script.description);//50
			if (!script.paralyzed){
				if (script.unitLevel >= 2){
					suLevel1BonusShort.color = Color.white;
					suLevel1BonusLong.color = Color.white;
				}
				if (script.unitLevel >= 3){
					suLevel2BonusShort.color = Color.white;
					suLevel2BonusLong.color = Color.white;
				}
				suLevel1BonusShort.text = (script.unitType == 11 || script.alleg == Unit.allegiance.neutral )?"" :"Lvl.2 Bonus: " + script.levelBonusShort[0];
				suLevel1BonusLong.text = script.wordWrap(90,script.levelBonusLong[0]);//28
				suLevel2BonusShort.text = (script.unitType == 11 || script.alleg == Unit.allegiance.neutral )?"" :"Lvl.3 Bonus: " + script.levelBonusShort[1];
				suLevel2BonusLong.text = script.wordWrap(90,script.levelBonusLong[1]);//28
			}
		}
	}
	
	public void clearHoverUnitInfo(){
		suInfo.text = "";
		unitNameGUI.text = "";
		unitDescriptionGUI.text = "";
		suLevel1BonusShort.color = Color.gray;
		suLevel1BonusLong.color = Color.gray;
		suLevel2BonusShort.color = Color.gray;
		suLevel2BonusLong.color = Color.gray;
		
		suLevel1BonusShort.text = "";
		suLevel1BonusLong.text = "";
		suLevel2BonusShort.text ="";
		suLevel2BonusLong.text = "";
	}


	void OnGUI(){
		//for Mana Bar
		percentMana = (float)pMana / maxMana;
		GUI.DrawTexture (new Rect(Screen.width * manaBarXPos,Screen.height *manaBarYPos,Screen.width * manaBarWidth,Screen.height* manaBarHeight*percentMana * ((float)maxMana/8)),manaGUIFill);
		GUI.DrawTexture (new Rect(Screen.width * manaBarXPos,Screen.height *manaBarYPos,Screen.width * manaBarWidth,Screen.height* manaBarHeight),manaGUIBorder);

		GUI.skin = mySkin;
		combatLogWindowRect = GUI.Window (2, combatLogWindowRect, combatLogWindow, "");
		GUI.BeginGroup (new Rect (0,0,100,100));
		GUI.EndGroup();
		displayUnitInfo (hoverOverUnit);
	}


	void endTurn(){
		if (turn) {
			if (!sentEndTurn){
				showCenterMessage("Opponent's Turn!",GameObject.Find("CenterPopUpText").GetComponent<GUIText>().color,true);
				gp.returnSocket ().SendTCPPacket ("endTurn");
				sentEndTurn = true;
				am.playTurnEndSFX();
			}else{
				am.playButtonSFX ();
			}
		} 
	}

	public void changeToAttacking(){
		am.playButtonSFX();
		tm.clearAllTiles();
		accessibleTiles.Clear();		
		gs =  gameState.playerAtk;	
		
		if (selectedUnit != null)
			selectedUnit.GetComponent<Unit>().showAtkTiles();
	}

	public void changeToMoving(){
		am.playButtonSFX();
		tm.clearAllTiles ();
		accessibleTiles.Clear ();
		gs = gameState.playerMv;

		if (selectedUnit != null) 
			selectedUnit.GetComponent<Unit>().showMvTiles(selectedUnit.alleg == Unit.allegiance.playerOne? Unit.allegiance.playerOne : Unit.allegiance.playerTwo);
	}

	public void clearSelection(){
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

		if (turn){
			showCenterMessage("Your Turn!",GameObject.Find("CenterPopUpText").GetComponent<GUIText>().color,true);
			am.playTurnStartSFX();
		}
		//toggle between players turns and reset units
		tm.clearAllTiles();
		accessibleTiles.Clear ();
		//possibly wrong
		if ((gp.playerNumber == 1 && turn) || (gp.playerNumber == 2 && !turn)){
			resetPlayerOneUnits();
		}else{
			resetPlayerTwoUnits();
		}
		clearSelection();	

		gs = gameState.playerMv;
		if (Application.loadedLevelName.Equals ("AIScene") && !turn && !gameOver) {
			StartCoroutine (GameObject.Find ("AI").GetComponent<AIScript> ().makeGameAction (null));
		}

	}
}


