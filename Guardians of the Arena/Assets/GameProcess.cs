using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;

public class GameProcess : MonoBehaviour {

	//PUBLIC MEMBERS 
	public int clientNumber;
	public string playerName;
	public bool play;

	public DateTime dT;
	public Stopwatch uniClock;

	//PRIVATE MEMBERS
	private Sockets socks;
	private string stringBuffer;
	private string tempBuffer;

	// Use this for initialization
	void Start () {

		uniClock = new Stopwatch();

		//play = false;
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


			//Determine the content of the string sent from the server
			
			//client\\clientNumber
			if(tokens[0].Equals("client"))
			{
				clientNumber = Int32.Parse(tokens[1]);
			}

			//loginSucceed\\correctUsername
			else if (tokens[0].Equals("loginSucceed"))
			{
				GameObject.Find("Login_GUI").GetComponent<LoginScreenGUI>().loginSucceed();
				playerName = tokens[1];
			}

			//loginFail
			else if (tokens[0].Equals("loginFail"))
			{
				GameObject.Find("Login_GUI").GetComponent<LoginScreenGUI>().loginFail();
			}

			//hasLoggedIn\\playerNameToAdd
			else if (tokens[0].Equals("hasLoggedIn"))
			{
				GameObject.Find("ListOfPlayers").GetComponent<ListOfPlayersScript>().addPlayer(tokens[1]);
			}

			//hasLoggedOut\\playerNameToRemove
			else if (tokens[0].Equals("hasLoggedOut"))
			{
				GameObject.Find("ListOfPlayers").GetComponent<ListOfPlayersScript>().removePlayer(tokens[1]);
			}

			//alreadyLoggedIn\\username
			else if (tokens[0].Equals("alreadyLoggedIn"))
			{
				GameObject.Find("Login_GUI").GetComponent<LoginScreenGUI>().guiText.text =
					tokens[1] + " is already logged in!";
			}

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
