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
				s+= tokens[j] + "\\" + "\\";
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
				
				if (clientNumber == Int32.Parse(tokens[1]))
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
				DontDestroyOnLoad(this);
				DontDestroyOnLoad (am);
				DontDestroyOnLoad(pum);
				Application.LoadLevel(3);
				
				
			}
			
			// globalChat\\userName\\chatContent
			else if (tokens[0].Equals("globalChat"))
			{
				GameObject.Find ("globalChat").GetComponent<globalChatScript>().addLineToChat(tokens[1], tokens[2]);
			}
			
			// 
			else if (tokens[0].Equals("challengeRequest"))
			{
				GameObject.Find ("challengeManager").GetComponent<challengeScript>().addChallengeRequest(tokens[1]);
			}
			
			else if (tokens[0].Equals("challengeAccepted"))
			{
				
			}
			
			else if (tokens[0].Equals("challengeDeclined"))
			{
				
			}
			
			else if (tokens[0].Equals("challengeCancelled"))
			{
				GameObject.Find ("challengeManager").GetComponent<challengeScript>().removeChallengeRequest(tokens[1]);
			}
			
			#region GAME PACKETS
			//newpiece\\positionx\\positiony\\type\\playerNumber\\uniqueID
			else if (tokens[0].Equals(""))
			{
				//xtileManager.addUnit(Int32.Parse(tokens[1]),Int32.Parse(tokens[2]),Int32.Parse(tokens[]]);
				
			}
			
			// unitID\\toX\\toY
			else if (tokens[0].Equals("move"))
			{
				movePiece(Int32.Parse(tokens[1]), Int32.Parse(tokens[2]), Int32.Parse(tokens[3]));
			}
			
			// unitID (that attacked) \\number of units affected\\ units
			else if (tokens[0].Equals("attack"))
			{
				gameManager.units[Int32.Parse (tokens[1])].atkd = true;
				gameManager.pMana -= gameManager.units[Int32.Parse (tokens[1])].atkCost;
				for (int i = 0; i < Int32.Parse (tokens[2]); i ++ ){
					gameManager.units[Int32.Parse(tokens[3+i])].attackThisUnit(gameManager.units[Int32.Parse (tokens[1])]);
				}
			}
			
			else if (tokens[0].Equals("switchTurns"))
			{
				gameManager.nextTurn();
			}
			
			//TODO
			else if (tokens[0].Equals("victory"))
			{
				gameManager.showReturnButton = true;
				gameManager.combatLog.text = "You won!";
				
			}
			
			//TODO
			else if (tokens[0].Equals("defeat"))
			{
				gameManager.showReturnButton = true;
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
		gameManager.selectedUnit = gameManager.units[unitID];
		gameManager.selectedUnit.showMvTiles (gameManager.selectedUnit.alleg);

		gameManager.pMana -= gameManager.units[unitID].mvCost;
			
		TileScript from = gameManager.units[unitID].transform.parent.GetComponent<TileScript>();
		TileScript to = tileManager.tiles[toX, toY].GetComponent<TileScript>();
		
		to.pathFinder ();
		
		gameManager.accessibleTiles.Clear();
		to.objectOccupyingTile = from.objectOccupyingTile;
		
		gameManager.selectedUnit.transform.parent = to.gameObject.transform;
		from.transform.GetComponent<TileScript>().objectOccupyingTile = null;
		tileManager.clearAllTiles();
	}
	
	void OnLevelWasLoaded(int sceneNumber)
	{
		if(sceneNumber == 3)
			loadManagers();
	}

	void OnApplicationQuit(){

		socks.Disconnect();
		Console.WriteLine("Application disconnected through application quit");
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