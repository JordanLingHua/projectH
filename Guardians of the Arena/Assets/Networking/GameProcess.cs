using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;
 
public class GameProcess : MonoBehaviour {
	public GUISkin mySkin;
	//PUBLIC MEMBERS 
	public int clientNumber;
	public int playerNumber;
	public string playerName;
	public string[] tempPageNameStorage;
	public bool play;
	private bool loaded;

	public DateTime dT;
	public Stopwatch uniClock;
	public TileManager tileManager;
	public GameManager gameManager;
	AudioManager am;
	PopUpMenuNecro pum;
	public PlayerSetup playerSetup;
	public float targetTileX, targetTileZ;

	//variables for popup windows and tips
	Rect popUpWindowRect;
	bool showPopUpTip,neverShowPopUpWindow;
	ArrayList popUpWindowText;
	string popUpTitle;
	int popUpIndex;


	
	//PRIVATE MEMBERS
	private Sockets socks;
	private string stringBuffer;
	private string tempBuffer;
	
	void Start () {
		showPopUpTip = true;
		popUpWindowText = new ArrayList();
		popUpIndex = 0;
		setupScreenTips();
		popUpWindowRect = new Rect(0,0,400,400);
		pum = GameObject.Find ("PopUpMenu").GetComponent<PopUpMenuNecro> ();
		am = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		uniClock = new Stopwatch();
		
		socks = new Sockets();
		play = false;
		tempPageNameStorage = new string[5];
		tempPageNameStorage [0] = "";
		tempPageNameStorage [1] = "";
		tempPageNameStorage [2] = "";
		tempPageNameStorage [3] = "";
		tempPageNameStorage [4] = "";
		loaded = false;
	}

	void Update () {
		if (socks.recvBuffer.Count > 0)
		{
			//Dequeue the single-line string sent from the server
			stringBuffer = (string)socks.recvBuffer.Dequeue();
			
			//separate the string by its delimiter '\\' to parse the line's content
			string[] tokens = stringBuffer.Split(new string[] {"\\"}, StringSplitOptions.None);
			
			///////////////////// DEBUG - WRITE ALL COMMANDS RECEIVED /////////////////////////
			String s = string.Empty;
			for (int j = 0; j < tokens.Length; j++)
			{
				s+= tokens[j];
				if (j != tokens.Length - 1)
					s += "\\" + "\\";
			}

			UnityEngine.Debug.Log(s);
			///////////////////////////////////////////////////////////////////////////////////

			// Determine the content of the string sent from the server
			
			// client\\clientNumber
			if(tokens[0].Equals("client"))
			{
				clientNumber = Int32.Parse(tokens[1]);
			}
			
			// loginSucceed\\correctUsername
			else if (tokens[0].Equals("loginSucceed"))
			{
				if (tokens[7].Equals("1"))
					GameObject.Find("Login_GUI").GetComponent<LoginScreenGUI>().loginText =
						"Creating Account...";
				GameObject.Find("Login_GUI").GetComponent<LoginScreenGUI>().loginSucceed();
				playerName = tokens[1];


			}

			else if (tokens[0].Equals("pageNames"))
			{
				PageNameScript pns = GameObject.Find ("PageInfo").GetComponent<PageNameScript>();
				for (int i = 0; i < 5; i++)
				{
					pns.pages[i] = tokens[i+1];
				}
			}
			
			// loginFail
			else if (tokens[0].Equals("loginFail"))
			{
				GameObject.Find("Login_GUI").GetComponent<LoginScreenGUI>().loginFail();
			}
			// hasLoggedIn\\playerNameToAdd
			else if (tokens[0].Equals("hasLoggedIn"))
			{
				GameObject.Find("ListOfPlayers").GetComponent<ListOfPlayersScript>().addPlayer(tokens[1]);
			}
			
			// hasLoggedOut\\playerNameToRemove
			else if (tokens[0].Equals("hasLoggedOut"))
			{	
				loaded = false;

				if (GameObject.Find("ListOfPlayers") != null)
					GameObject.Find("ListOfPlayers").GetComponent<ListOfPlayersScript>().removePlayer(tokens[1]);
				
				if (playerName == tokens[1])
				{
					UnityEngine.Debug.Log ("Logging out through playername");
					Destroy(pum);
					Destroy(am);
					Destroy(GameObject.Find ("ListOfPlayers"));

					// KILL THREAD AND SERVER CONNECTION
					returnSocket().t.Abort();
					returnSocket().endThread();
					returnSocket().Disconnect();

					Application.LoadLevel(0);
				}
			}
			
			// alreadyLoggedIn\\username
			else if (tokens[0].Equals("alreadyLoggedIn"))
			{
				GameObject.Find("Login_GUI").GetComponent<LoginScreenGUI>().loginText =
					tokens[1] + " is already logged in!";
			}

		
			
			// startGame\\playerNumber
			else if (tokens[0].Equals("startGame"))
			{
				playerNumber = Int32.Parse(tokens[1]);
				DontDestroyOnLoad(GameObject.Find ("GameProcess"));
				DontDestroyOnLoad(GameObject.Find ("PageNumber"));
				Destroy(GameObject.Find("GlobalChat"));
				Destroy(GameObject.Find("gChat"));
				Destroy(GameObject.Find("ListOfPlayers"));
				Destroy(GameObject.Find("ListOfPlayersGUIText"));
				DontDestroyOnLoad(this);
				DontDestroyOnLoad (am);
				DontDestroyOnLoad(pum);
				Application.LoadLevel(3);				
			}

			// startAI
			else if (tokens[0].Equals("startAI"))
			{
				playerNumber = 1;//Int32.Parse(tokens[1]);
				DontDestroyOnLoad(GameObject.Find ("GameProcess"));
				DontDestroyOnLoad(GameObject.Find ("PageNumber"));
				Destroy(GameObject.Find("GlobalChat"));
				Destroy(GameObject.Find("gChat"));
				Destroy(GameObject.Find("ListOfPlayers"));
				Destroy(GameObject.Find("ListOfPlayersGUIText"));
				DontDestroyOnLoad(this);
				DontDestroyOnLoad (am);
				DontDestroyOnLoad(pum);
				Application.LoadLevel(5);		

				
			}

//			DontDestroyOnLoad(GameObject.Find ("PopUpMenu"));
//			Application.LoadLevel(3);

			
			// globalChat\\userName\\chatContent
			else if (tokens[0].Equals("globalChat"))
			{
				print (tokens[1] +" " + tokens[2]);
				GameObject.Find ("GlobalChat").GetComponent<globalChatScript>().addLineToChat(tokens[1], tokens[2]);
			}
			
			//challengeRequest\\challengerName
			else if (tokens[0].Equals("challengeRequest"))
			{
				GameObject.Find ("challengeManager").GetComponent<challengeScript>().addChallengeRequest(tokens[1]);
			}

			//TODO
			else if (tokens[0].Equals("challengeAccepted"))
			{
				
			}

			//TODO
			else if (tokens[0].Equals("challengeDeclined"))
			{
				
			}

			//challengeCancelled\\challengerName
			else if (tokens[0].Equals("challengeCancelled"))
			{
				GameObject.Find ("challengeManager").GetComponent<challengeScript>().removeChallengeRequest(tokens[1]);
			}

			#region SETTING UP BOARDS
			//boardSetup\\...
			else if (tokens[0].Equals("boardSetup"))
			{
				playerSetup.pages[playerSetup.activePage].offBoardPieces.Clear();
				playerSetup.pages[playerSetup.activePage].onBoardPieces.Clear();
				GameObject unitToAdd;
				                                      
				//    | Repeat boardCapacity number of times | 
				//    v                                      v
				//...{pageNumber\\unitType\\xPos\\yPos\\onField}
				for (int i = 0; i < playerSetup.maxUnitCount * 4; i += 4)
				{
					//if the unit is OnField
					if (tokens[i + 4].Equals("True"))
					{
						unitToAdd = playerSetup.addUnit(PlayerSetup.placement.ONFIELD, Int32.Parse (tokens[i + 2]), 
						                                Int32.Parse (tokens[i + 3]), Int32.Parse (tokens[i + 1]));
						unitToAdd.AddComponent("move");
						playerSetup.pages[playerSetup.activePage].onBoardPieces.Add(unitToAdd);
					}
		
					//the unit is offField
					else
					{
						unitToAdd = playerSetup.addUnit(PlayerSetup.placement.OFFFIELD, Int32.Parse (tokens[i + 2]), 
					                                    Int32.Parse (tokens[i + 3]), Int32.Parse (tokens[i + 1]));
						unitToAdd.AddComponent("move");
						playerSetup.pages[playerSetup.activePage].offBoardPieces.Add(unitToAdd);
					}
					unitToAdd.GetComponent<unitSetupScript>().rotateForSetupScreen();
				}
			}
			#endregion
			
			#region GAME PACKETS

			//spawnPieces\\...
			else if (tokens[0].Equals("spawnPieces"))
			{
				GameObject unitToAdd;
				//    | Repeat for all player 1's units
				//    v                                      
				//unitType\\unitID\\xPos\\yPos
				int i = 1;
				while(!tokens[i].Equals("EndPlayer1"))
				{
					unitToAdd = tileManager.addUnit(Int32.Parse(tokens[i+2]), Int32.Parse(tokens[i+3]), Int32.Parse(tokens[i]),1, Int32.Parse(tokens[i+1]));
					if (playerNumber == 1)
						unitToAdd.GetComponent<unitSetupScript>().rotateForPlayerOne();
					else
						unitToAdd.GetComponent<unitSetupScript>().rotateForPlayerTwo();
					i += 4;
				}
				i++;

				//    | Repeat for all player 2's units
				//    v                                      
				//unitType\\unitID\\xPos\\yPos
				while(!tokens[i].Equals("EndPlayer2"))
				{
					unitToAdd = tileManager.addUnit(Int32.Parse(tokens[i+2]), Int32.Parse(tokens[i+3]), Int32.Parse(tokens[i]),2, Int32.Parse(tokens[i+1]));
					if (playerNumber == 1)
						unitToAdd.GetComponent<unitSetupScript>().rotateForPlayerOne();
					else
						unitToAdd.GetComponent<unitSetupScript>().rotateForPlayerTwo();
					i += 4;
				}

				tileManager.clearAllTiles();
				tileManager.displayHPBars(pum.hpSelGridInt);
				tileManager.displayXPBars(pum.xpSelGridInt);
			}

			else if (tokens[0].Equals("spawnTrees"))
			{
				//    | Repeat for all trees
				//    v                                      
				//treeID\\xPos\\yPos
				int i = 1;
				while(!tokens[i].Equals("endSpawnTrees"))
				{
					tileManager.addTree(Int32.Parse(tokens[i+1]), Int32.Parse(tokens[i+2]), Int32.Parse(tokens[i]));
					i += 3;
				}
			}

			else if (tokens[0].Equals("spawnRocks"))
			{
				//    | Repeat for all rocks
				//    v                                      
				//rockID\\xPos\\yPos
				int i = 1;
				while(!tokens[i].Equals("endSpawnRocks"))
				{
					tileManager.addRock(Int32.Parse(tokens[i+1]), Int32.Parse(tokens[i+2]), Int32.Parse(tokens[i]));
					i += 3;
				}
			}
			
			// unitID\\toX\\toY
			else if (tokens[0].Equals("move"))
			{
				movePiece(Int32.Parse(tokens[1]), Int32.Parse(tokens[2]), Int32.Parse(tokens[3]));
				if (pum.allowAutoMoveAttackToggle){
					gameManager.changeToAttacking();
				}
				if (Application.loadedLevelName.Equals("AIScene"))
					GameObject.Find("AI").GetComponent<AIScript>().serverResponded = true;
			}
			
			// unitID (that attacked) \\number of units affected\\ units
			else if (tokens[0].Equals("attack"))
			{

				gameManager.pMana -= gameManager.units[Int32.Parse (tokens[1])].atkCost;

//				//Use the values assigned to targetTileX and targetTileZ from TileScript.cs:
//				//Attack animation based on the position of the tile that is going to be attacked
//				if(gameManager.units[Int32.Parse (tokens[1])].transform.position.z > targetTileZ)
//					gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 8);
//				else if(gameManager.units[Int32.Parse (tokens[1])].transform.position.z < targetTileZ)
//					gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 9);
//				else if(gameManager.units[Int32.Parse (tokens[1])].transform.position.x > targetTileX)
//					gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 10);
//				else if(gameManager.units[Int32.Parse (tokens[1])].transform.position.x < targetTileX)
//					gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 11);
//				//hmm it seems to always play the attack_front animation
//



				//new
				//Step 1)  Before you do anything, Transition from neutral_states in the post_attack version, to the actual neutral states
				//NOTE:  the post_attack neutral states don't get signified by a mode_and_dir.  occured at exit time of attack.  So mode_and_dir is still 
				//== the attack state it left off at
//				if(gameManager.units[Int32.Parse (tokens[1])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 8 || 
//				   gameManager.units[Int32.Parse (tokens[1])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 12)
//					gameManager.units[Int32.Parse (tokens[1])].GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 0);
//				else if(gameManager.units[Int32.Parse (tokens[1])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 9 || 
//				   gameManager.units[Int32.Parse (tokens[1])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 13)
//					gameManager.units[Int32.Parse (tokens[1])].GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 1);
//				else if(gameManager.units[Int32.Parse (tokens[1])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 10 || 
//				   gameManager.units[Int32.Parse (tokens[1])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 14)
//					gameManager.units[Int32.Parse (tokens[1])].GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 2);
//				else if(gameManager.units[Int32.Parse (tokens[1])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 11 || 
//				   gameManager.units[Int32.Parse (tokens[1])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 15)
//					gameManager.units[Int32.Parse (tokens[1])].GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 3);
//				//NOTE:  I will use the above switch state as the preamble to nearly every switch statement like below
//
//

				gameManager.pMana -= gameManager.units[Int32.Parse (tokens[1])].atkCost;

//
//				//Step 2)  Now you can use the appropriate states for this attack functionality
//				print ("attacker position x:" + gameManager.units[Int32.Parse (tokens[1])].transform.position.x);
//				print ("attacker position z:" + gameManager.units[Int32.Parse (tokens[1])].transform.position.z);
//				print ("attacked position x:" + targetTileX);
//				print ("attacked position z:" + targetTileZ);
//				print ("attacker animation state #:" + gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().GetInteger("mode_and_dir"));
//				//Use the values assigned to targetTileX and targetTileZ from TileScript.cs:
//				//Attack animation based on the position of the tile that is going to be attacked
//				if(gameManager.units[Int32.Parse (tokens[1])].transform.position.z < (targetTileZ*10))
//					gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 8);
//				else if(gameManager.units[Int32.Parse (tokens[1])].transform.position.z > (targetTileZ*10))
//					gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 9);
//				else if(gameManager.units[Int32.Parse (tokens[1])].transform.position.x > (targetTileX*10))
//					gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 10);
//				else if(gameManager.units[Int32.Parse (tokens[1])].transform.position.x < (targetTileX*10))
//					gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 11);
//				//hmm it seems to always play the attack_front animation
//







				if (Int32.Parse (tokens[2]) != 0){
//	
					for (int i = 0; i < Int32.Parse (tokens[2]); i ++ ){
						if (gameManager.units[Int32.Parse (tokens[1])].unitType == 2){
							(gameManager.units[Int32.Parse (tokens[1])] as Mystic).revertStatsOfFocused();
						}
						gameManager.units[Int32.Parse (tokens[1])].attackUnit(gameManager.units[Int32.Parse(tokens[3+i])]);


						//new
						//Step 1)  Before you do anything, Transition from neutral_states in the post_attack version, to the actual neutral states
						//NOTE:  the post_attack neutral states don't get signified by a mode_and_dir.  occured at exit time of attack.  So mode_and_dir is still 
						//== the attack state it left off at
//						if(gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 8 || 
//						   gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 12)
//							gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 0);
//						else if(gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 9 || 
//						   gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 13)
//							gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 1);
//						else if(gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 10 || 
//						   gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 14)
//							gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 2);
//						else if(gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 11 || 
//						   gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().GetInteger("mode_and_dir") == 15)
//							gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 3);
//						//NOTE:  I will use the above switch state as the preamble to nearly every switch statement like below
//
//						//Step 2
//						//Unit gets hit facing the direction of the attacker
//						if(gameManager.units[Int32.Parse(tokens[3+i])].transform.position.z < gameManager.units[Int32.Parse (tokens[1])].transform.position.z)
//							gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 12);
//						else if(gameManager.units[Int32.Parse(tokens[3+i])].transform.position.z > gameManager.units[Int32.Parse (tokens[1])].transform.position.z)
//							gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 13);
//						else if(gameManager.units[Int32.Parse(tokens[3+i])].transform.position.x > gameManager.units[Int32.Parse (tokens[1])].transform.position.x)
//							gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 14);
//						else if(gameManager.units[Int32.Parse(tokens[3+i])].transform.position.x < gameManager.units[Int32.Parse (tokens[1])].transform.position.x)
//							gameManager.units[Int32.Parse(tokens[3+i])].GetComponentInChildren<Animator>().SetInteger("mode_and_dir", 15);


						if (gameManager.units.ContainsKey(Int32.Parse(tokens[3+i])) && !gameManager.units[Int32.Parse(tokens[3+i])].invincible){
							gameManager.units[Int32.Parse (tokens[1])].gainXP();
						}


					}
			}else{
					gameManager.units[Int32.Parse (tokens[1])].showPopUpText("Attacked Nothing!", Color.red);
					string player = ((playerNumber ==  1 && gameManager.units[Int32.Parse (tokens[1])].alleg == Unit.allegiance.playerOne) || (playerNumber ==  2 && gameManager.units[Int32.Parse (tokens[1])].alleg == Unit.allegiance.playerTwo)) ? "Your " : "Opponent's ";
					gameManager.addLogToCombatLog(player + gameManager.units[Int32.Parse (tokens[1])].unitName + " attacked nothing for " + gameManager.units[Int32.Parse (tokens[1])].atkCost + " mana!");
				}


				//
				/*
				AnimatorStateInfo stateInfo = gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
				int x = 0;
				float animPercentage = 0;//stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime);

				while(animPercentage <= 1.0f)
				{
					animPercentage = stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime);
					//while(x < stateInfo.normalizedTime*Time.deltaTime)
					//	x++;
					if(animPercentage >= 1.0f)
					{
						if(gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().GetInteger("mode_and_dir")==8){
							gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 0);
						}
						else if(gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().GetInteger("mode_and_dir")==9){
							gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 1);
						}	
						else if(gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().GetInteger("mode_and_dir")==10){
							gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 2);
						}
						else if(gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().GetInteger("mode_and_dir")==11){
							gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 3);
						}
					}

				}
				*/





				if (pum.allowAutoMoveAttackToggle){
					gameManager.changeToMoving();
				}

				if (Application.loadedLevelName.Equals("AIScene"))
					GameObject.Find("AI").GetComponent<AIScript>().serverResponded = true;
			}

			//switchTurns
			else if (tokens[0].Equals("switchTurns"))
			{
				gameManager.sentEndTurn = false;
				gameManager.nextTurn(Int32.Parse (tokens[1]));
			}
			
			//TODO mmr
			else if (tokens[0].Equals("victory"))
			{
				gameManager.gameOver = true;
				gameManager.addLogToCombatLog("Congratulations! You have won!");
			}
			
			//TODO mmr
			else if (tokens[0].Equals("defeat"))
			{
				gameManager.gameOver = true;
				gameManager.addLogToCombatLog("You have lost!");
			}
			#endregion
			
			else
			{
				string packet = "Unrecognized packet: ";
				foreach (string i in tokens)
				{
					packet += i;
					
					if(i != tokens[tokens.Length - 1])
						packet += "\\" + "\\";
				}
				UnityEngine.Debug.Log(packet);
			}
		}}  

	
	public void movePiece(int unitID, int toX, int toY)
	{
		bool players = ((playerNumber ==  1 && gameManager.units [unitID].alleg == Unit.allegiance.playerOne) || (playerNumber ==  2 && gameManager.units [unitID].alleg == Unit.allegiance.playerTwo));
		if (players){
			gameManager.addLogToCombatLog("Your " + gameManager.units[unitID].unitName + " moved for " + gameManager.units[unitID].mvCost + " mana!");
		}else if (!players){
			gameManager.addLogToCombatLog("Opponent's " + gameManager.units[unitID].unitName + " moved for " + gameManager.units[unitID].mvCost + " mana!");
		}
		if (gameManager.units [unitID].unitType == 2) {
			Mystic x = gameManager.units[unitID] as Mystic;
			x.revertStatsOfFocused();
		}
		gameManager.pMana -= gameManager.units[unitID].mvCost;
		
		TileScript from = gameManager.units[unitID].transform.parent.GetComponent<TileScript>();
		TileScript to = tileManager.tiles[toX, toY].GetComponent<TileScript>();
		
		to.pathFinder (gameManager.units[unitID]);
		gameManager.units[unitID].mvd = true;

		gameManager.accessibleTiles.Clear();
		to.objectOccupyingTile = from.objectOccupyingTile;
		
		gameManager.units[unitID].transform.parent = to.gameObject.transform;
		from.transform.GetComponent<TileScript>().objectOccupyingTile = null;
		tileManager.clearAllTiles();
	}
	
	void OnLevelWasLoaded(int sceneNumber)
	{
		if (sceneNumber == 1) {
//			PageNameScript pns = GameObject.Find("PageInfo").GetComponent<PageNameScript>();
//			pns.pages = new string[5];
//			if (!loaded){
//				loaded = true;
//			
//			for (int i = 0; i < 5; i++)
//			{
//				pns.pages[i] = tempPageNameStorage[i];
//			}
//		}
//
//			else{
//				for (int i = 0; i < 5; i++)
//				{
//				pns.pages[i] = GameObject.Find ("PageInfo").GetComponent<PageNameScript>().pages[i];
//				}
//			}
			returnSocket().SendTCPPacket("pageNames");
				
		}
		if (sceneNumber == 3)//PvP multiplayer
				loadManagers ();
		else if (sceneNumber == 2)
				playerSetup = GameObject.Find ("PlayerSetup").GetComponent<PlayerSetup> ();
		else if (sceneNumber == 0)
				socks = new Sockets ();
		else if (sceneNumber == 5) {
				loadManagers ();
				//loadAI ();
		}
		else if (sceneNumber == 0)
			socks = new Sockets ();
		

	}

	void OnApplicationQuit(){
		try{
			//returnSocket().SendTCPPacket("logout\\" + playerName);
			//returnSocket().t.Abort();
			//returnSocket().endThread();
			returnSocket().Disconnect();
			Console.WriteLine("Application disconnected through application quit");
		}catch(Exception e){
			print ("Error on disconnect: " + e);
		}		
	}

	void popUpWindow(int windowID) 
	{
		GUI.DragWindow(new Rect(0, 30, 10000, 25));
		//GUILayout.BeginVertical ();
		//title

		GUILayout.Label (popUpTitle);
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal ();
		GUILayout.Label ((string)popUpWindowText[popUpIndex], "PlainText");
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		neverShowPopUpWindow = GUILayout.Toggle(neverShowPopUpWindow,"Never display this tip again");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button ("Close Tip", "ShortButton")) 
			showPopUpTip = false;
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		GUILayout.EndVertical();
	}

	void OnGUI(){
		GUI.skin = mySkin;
		if (showPopUpTip){
			popUpWindowRect = GUI.Window (1, popUpWindowRect, popUpWindow,"");
		}

	}

	public void setupScreenTips(){
		popUpTitle = "Tip";
		popUpIndex = 0;
		popUpWindowText.Add("This is the setup screen. Hover over units with your mouse to see more information about it.");
		popUpWindowText.Add("test 2");
		popUpWindowText.Add("Test 3");
	}


	public void loadManagers()
	{
		tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	public Sockets returnSocket ()
	{
		return socks;
	}
}