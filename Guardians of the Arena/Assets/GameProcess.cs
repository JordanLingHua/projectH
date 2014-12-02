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
	PopUpMenu pum;
	public PlayerSetup playerSetup;
	
	//PRIVATE MEMBERS
	private Sockets socks;
	private string stringBuffer;
	private string tempBuffer;
	
	void Start () {
		pum = GameObject.Find ("PopUpMenu").GetComponent<PopUpMenu> ();
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
					Destroy(pum);
					Destroy(am);
					Application.LoadLevel(0);

					// KILL THREAD AND SERVER CONNECTION
					returnSocket().t.Abort();
					returnSocket().endThread();
					returnSocket().Disconnect();
				}
			}
			
			// alreadyLoggedIn\\username
			else if (tokens[0].Equals("alreadyLoggedIn"))
			{
				GameObject.Find("Login_GUI").GetComponent<LoginScreenGUI>().guiText.text =
					tokens[1] + " is already logged in!";
			}
			
			// startGame\\playerNumber
			else if (tokens[0].Equals("startGame"))
			{
				playerNumber = Int32.Parse(tokens[1]);
				DontDestroyOnLoad(GameObject.Find ("GameProcess"));
				DontDestroyOnLoad(GameObject.Find ("PageNumber"));
				DontDestroyOnLoad(this);
				DontDestroyOnLoad (am);
				DontDestroyOnLoad(pum);
				Application.LoadLevel(3);
				
				
			}
			
			// globalChat\\userName\\chatContent
			else if (tokens[0].Equals("globalChat"))
			{
				print (tokens[1] +" " + tokens[2]);
				GameObject.Find ("globalChat").GetComponent<globalChatScript>().addLineToChat(tokens[1], tokens[2]);
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
				for (int i = 0; i < playerSetup.boardCapacity * 4; i += 4)
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
					
				}
			}
			#endregion
			
			#region GAME PACKETS

			//spawnPieces\\...
			else if (tokens[0].Equals("spawnPieces"))
			{
				//    | Repeat for all player 1's units
				//    v                                      
				//unitType\\unitID\\xPos\\yPos
				int i = 1;
				while(!tokens[i].Equals("EndPlayer1"))
				{
					tileManager.addUnit(Int32.Parse(tokens[i+2]), Int32.Parse(tokens[i+3]), Int32.Parse(tokens[i]),1, Int32.Parse(tokens[i+1]));
					i += 4;
				}
				i++;

				//    | Repeat for all player 2's units
				//    v                                      
				//unitType\\unitID\\xPos\\yPos
				while(!tokens[i].Equals("EndPlayer2"))
				{
					tileManager.addUnit(Int32.Parse(tokens[i+2]), Int32.Parse(tokens[i+3]), Int32.Parse(tokens[i]),2, Int32.Parse(tokens[i+1]));
					i += 4;
				}
				tileManager.displayHPBars(pum.selGridInt);
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
			}
			
			// unitID (that attacked) \\number of units affected\\ units
			else if (tokens[0].Equals("attack"))
			{
				gameManager.pMana -= gameManager.units[Int32.Parse (tokens[1])].atkCost;
				for (int i = 0; i < Int32.Parse (tokens[2]); i ++ ){
					gameManager.units[Int32.Parse (tokens[1])].gainXP();
					if (gameManager.units[Int32.Parse (tokens[1])].unitType == 2){
						Mystic x =gameManager.units[Int32.Parse (tokens[1])] as Mystic;
						x.resetUnitAbilities();
					}
					gameManager.units[Int32.Parse (tokens[1])].attackUnit(gameManager.units[Int32.Parse(tokens[3+i])]);
				}


			}

			//switchTurns
			else if (tokens[0].Equals("switchTurns"))
			{
				gameManager.nextTurn(Int32.Parse (tokens[1]));
			}
			
			//TODO mmr
			else if (tokens[0].Equals("victory"))
			{
				gameManager.gameOver = true;
				gameManager.combatLog.text = "You won!";
			}
			
			//TODO mmr
			else if (tokens[0].Equals("defeat"))
			{
				gameManager.gameOver = true;
				gameManager.combatLog.text = "You lost!";
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
		}
	}
	
	public void movePiece(int unitID, int toX, int toY)
	{
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
		if (sceneNumber == 3)
			loadManagers ();
		else if (sceneNumber == 2)
			playerSetup = GameObject.Find ("PlayerSetup").GetComponent<PlayerSetup>();
		
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