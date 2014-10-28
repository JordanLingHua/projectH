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
			String s = "";
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
				DontDestroyOnLoad(GameObject.Find ("GameProcess"));
				DontDestroyOnLoad(this);
				Application.LoadLevel(3);

				playerNumber = Int32.Parse(tokens[1]);
			}

			// globalChat\\userName\\chatContent
			else if (tokens[0].Equals("globalChat"))
			{
				GameObject.Find ("GlobalChat").GetComponent<globalChatScript>().addLineToChat(tokens[1], tokens[2]);
			}

			#region GAME PACKETS
			else if (tokens[0].Equals("move"))
			{
				tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
				gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

				TileScript from = tileManager.tiles[Int32.Parse(tokens[2]), Int32.Parse(tokens[3])].GetComponent<TileScript>();
				TileScript to = tileManager.tiles[Int32.Parse(tokens[4]), Int32.Parse(tokens[5])].GetComponent<TileScript>();

				//add
				to.occupied = from.transform.GetChild(0).transform.parent.GetComponent<TileScript>().occupied;
				Vector3 newPos = new Vector3(to.transform.position.x,0,to.transform.position.z);

				from.transform.GetChild(0).transform.position = newPos;
				//gameManager.selectedUnit.transform.position = newPos;
				
				gameManager.accessibleTiles.Clear();
				to.objectOccupyingTile = from.transform.GetChild(0).gameObject;
				from.transform.GetChild(0).GetComponent<Unit>().mvd = true;

				//remove
				from.transform.GetChild(0).transform.parent.GetComponent<TileScript>().occupied = 0;
				from.transform.GetChild(0).transform.parent.GetComponent<TileScript>().objectOccupyingTile = null;
				from.transform.GetChild(0).transform.parent = gameObject.transform;

				tileManager.clearAllTiles();

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

	public Sockets returnSocket ()
	{
		return socks;
	}
}
