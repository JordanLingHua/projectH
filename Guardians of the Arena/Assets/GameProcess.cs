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
	
	//PRIVATE MEMBERS
	private Sockets socks;
	private string stringBuffer;
	private string tempBuffer;
	
	// Use this for initialization
	void Start () {
		uniClock = new Stopwatch();
		
		socks = new Sockets();
		
		play = false;
		
		
	}
	
	// Update is called once per frame
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
				GameObject.Find("ListOfPlayers").GetComponent<ListOfPlayersScript>().removePlayer(tokens[1]);
				
				if (playerName.Equals(tokens[1]))
				{
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
				Application.LoadLevel(3);
				

			}
			
			// globalChat\\userName\\chatContent
			else if (tokens[0].Equals("globalChat"))
			{
				GameObject.Find ("GlobalChat").GetComponent<globalChatScript>().addLineToChat(tokens[1], tokens[2]);
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
			else if (tokens[0].Equals("move"))
			{
				movePiece(Int32.Parse(tokens[2]), Int32.Parse(tokens[3]), Int32.Parse(tokens[4]), Int32.Parse(tokens[5]), Int32.Parse(tokens[6]));
			}

			else if (tokens[0].Equals("attack"))
			{
				//TODO
			}

			else if (tokens[0].Equals("switchTurns"))
			{
				gameManager.nextTurn();
			}

			//TODO
			else if (tokens[0].Equals("victory"))
			{
				gameManager.combatLog.text = "You won!";

			}

			//TODO
			else if (tokens[0].Equals("defeat"))
			{
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
	
	public void movePiece(int fromX, int fromY, int toX, int toY, int manaToMove)
	{
		gameManager.pMana -= manaToMove;

		TileScript from = tileManager.tiles[fromX, fromY].GetComponent<TileScript>();
		TileScript to = tileManager.tiles[toX, toY].GetComponent<TileScript>();

		gameManager.selectedUnit = from.objectOccupyingTile.GetComponent<Unit>();

		to.pathFinder ();
		
		gameManager.accessibleTiles.Clear();
		to.objectOccupyingTile = from.transform.GetChild(0).gameObject;
		from.transform.GetChild(0).GetComponent<Unit>().mvd = true;
		
		//remove
		from.transform.GetChild(0).transform.parent.GetComponent<TileScript>().objectOccupyingTile = null;
		from.transform.GetChild(0).transform.parent = gameObject.transform;
		
		tileManager.clearAllTiles();

		//show attack range
		//to.transform.GetChild (0).GetComponent<Unit> ().showAtkAccessibleTiles (to, to.transform.GetChild (0).GetComponent<Unit> ().atkRange);
	}
	
	void OnLevelWasLoaded(int sceneNumber)
	{
		if(sceneNumber == 3)
			loadManagers();
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
