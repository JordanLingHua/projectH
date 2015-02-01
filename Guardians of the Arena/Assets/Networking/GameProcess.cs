using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;
 
public class GameProcess : MonoBehaviour {
	
	//PUBLIC MEMBERS 
	public int clientNumber;
	public int playerNumber;
	public string playerName;
	public bool play;
	
	public DateTime dT;
	public Stopwatch uniClock;
	public TileManager tileManager;
	public GameManager gameManager;
	AudioManager am;
	PopUpMenuNecro pum;
	public PlayerSetup playerSetup;

	//
	public float targetTileX, targetTileZ;

	
	//PRIVATE MEMBERS
	private Sockets socks;
	private string stringBuffer;
	private string tempBuffer;
	
	void Start () {
		pum = GameObject.Find ("PopUpMenu").GetComponent<PopUpMenuNecro> ();
		am = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		uniClock = new Stopwatch();
		
		socks = new Sockets();
		play = false;
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
				GameObject.Find("Login_GUI").GetComponent<LoginScreenGUI>().loginSucceed();
				playerName = tokens[1];
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

			else if (tokens[0].Equals("spawnObstacles"))
			{
				//    | Repeat for all trees
				//    v                                      
				//treeID\\xPos\\yPos
				int i = 1;
				while(!tokens[i].Equals("endSpawnObstacles"))
				{
					tileManager.addTree(Int32.Parse(tokens[i+1]), Int32.Parse(tokens[i+2]), Int32.Parse(tokens[i]));
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
			}
			
			// unitID (that attacked) \\number of units affected\\ units
			else if (tokens[0].Equals("attack"))
			{
				gameManager.pMana -= gameManager.units[Int32.Parse (tokens[1])].atkCost;
				print ("attacker position x:" + gameManager.units[Int32.Parse (tokens[1])].transform.position.x);
				print ("attacker position z:" + gameManager.units[Int32.Parse (tokens[1])].transform.position.z);
				print ("attacked position x:" + targetTileX);
				print ("attacked position z:" + targetTileZ);
				print ("attacker animation state #:" + gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().GetInteger("mode_and_dir"));
				//Use the values assigned to targetTileX and targetTileZ from TileScript.cs:
				//Attack animation based on the position of the tile that is going to be attacked
				if(gameManager.units[Int32.Parse (tokens[1])].transform.position.z < (targetTileZ*10))
					gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 8);
				else if(gameManager.units[Int32.Parse (tokens[1])].transform.position.z > (targetTileZ*10))
					gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 9);
				else if(gameManager.units[Int32.Parse (tokens[1])].transform.position.x > (targetTileX*10))
					gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 10);
				else if(gameManager.units[Int32.Parse (tokens[1])].transform.position.x < (targetTileX*10))
					gameManager.units[Int32.Parse (tokens[1])].GetComponent<Animator>().SetInteger("mode_and_dir", 11);
				//hmm it seems to always play the attack_front animation



				if (Int32.Parse (tokens[2]) != 0){
	
					for (int i = 0; i < Int32.Parse (tokens[2]); i ++ ){
						if (gameManager.units[Int32.Parse (tokens[1])].unitType == 2){
							(gameManager.units[Int32.Parse (tokens[1])] as Mystic).revertStatsOfFocused();
						}
						gameManager.units[Int32.Parse (tokens[1])].attackUnit(gameManager.units[Int32.Parse(tokens[3+i])]);
						gameManager.units[Int32.Parse (tokens[1])].gainXP();
					}
				}else{
					gameManager.units[Int32.Parse (tokens[1])].showPopUpText("Attacked Nothing!", Color.red);
					string player = ((playerNumber ==  1 && gameManager.units[Int32.Parse (tokens[1])].alleg == Unit.allegiance.playerOne) || (playerNumber ==  2 && gameManager.units[Int32.Parse (tokens[1])].alleg == Unit.allegiance.playerTwo)) ? "Your " : "Opponent's ";
					gameManager.addLogToCombatLog(player + gameManager.units[Int32.Parse (tokens[1])].unitName + " attacked nothing for " + gameManager.units[Int32.Parse (tokens[1])].atkCost + " mana!");
				}
				if (pum.allowAutoMoveAttackToggle){
					gameManager.changeToMoving();
				}
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
				gameManager.addLogToCombatLog("You were: REKT ☑\nREKTangle ☑ \nSHREKT ☑ \nREKT-it Ralph ☑ \nTotal REKTall ☑ \nThe Lord of the REKT ☑ \nThe Usual SusREKTs ☑ North by NorthREKT ☑");
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

		if (sceneNumber == 3)
			loadManagers ();
		else if (sceneNumber == 2)
			playerSetup = GameObject.Find ("PlayerSetup").GetComponent<PlayerSetup> ();
		else if (sceneNumber == 0)
			socks = new Sockets ();
		

	}

	void OnApplicationQuit(){
		try{
			socks.Disconnect();
			Console.WriteLine("Application disconnected through application quit");
		}catch(Exception e){
			print ("Error on disconnect: " + e);
		}		
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