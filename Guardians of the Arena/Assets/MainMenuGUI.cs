using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;

public class MainMenuGUI : MonoBehaviour {
	
	public bool showGUI;
	public GUIText guiText;
	GameProcess gp;
	PageNumberScript pageNumber;
	public string chat;
	public string challengedPlayer;
	string challengedPlayerCopy;
	globalChatScript globalChat;
	bool challengePending;
	AudioManager am;


	// Use this for initialization
	void Start () {
		pageNumber = GameObject.Find ("PageNumber").GetComponent<PageNumberScript>();
		guiText.text = string.Empty;
		challengedPlayerCopy = string.Empty;
		showGUI = true;
		am = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		globalChat = GameObject.Find("globalChat").GetComponent<globalChatScript>();
		chat = "Press Enter to Chat";
		challengePending = false;
	}
	
	void OnGUI () {
		
		GUI.SetNextControlName ("chatField");
		
		chat = GUI.TextField (new Rect (25, Screen.height - 30, 500, 20), chat, 50);
		
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			GUI.FocusControl ("chatField");
			chat = string.Empty;
		}
		
		
		
		if (Event.current.keyCode == KeyCode.Return && !chat.Equals(string.Empty)) 
		{
			gp.returnSocket().SendTCPPacket("globalChat\\" + gp.playerName + "\\" + chat);
		//	globalChat.addLineToChat(gp.playerName , chat);
			chat = string.Empty;
		}
		
		
		if(showGUI)
		{
			if(!challengePending)
			{
				if(GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 75, 100, 20), "Play Game"))
				{
					am.playButtonSFX();
					//send search request
					gp.returnSocket().SendTCPPacket("search\\" + pageNumber.selectedPage);
					
					guiText.text = "Searching for Opponent...";
					showGUI = false;
				}

				challengedPlayer = GUI.TextField (new Rect (Screen.width - 650, Screen.height - 240, 150, 20), challengedPlayer, 50);
				if(GUI.Button(new Rect(Screen.width - 660, Screen.height - 270, 180, 20), "Send Challenge!"))
				{
					am.playButtonSFX();
					//TODO: 
					gp.returnSocket().SendTCPPacket("challengeRequest\\" + gp.playerName + "\\" + challengedPlayer);
					challengePending = true;
					challengedPlayerCopy = challengedPlayer;
					challengedPlayer = string.Empty;
				}
			}

			else
				if(GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 75, 120, 20), "Cancel Challenge"))
				{
					am.playErrorSFX();
					//send search request
					gp.returnSocket().SendTCPPacket("cancelChallenge\\" + gp.playerName + "\\" + challengedPlayerCopy );
					
					guiText.text = "Searching for Opponent...";
					showGUI = false;
				}
			
			if ( GUI.Button(new Rect( Screen.width / 2 - 55, Screen.height / 2 - 50, 110, 20), "Setup Boards"))
			{
				am.playButtonSFX();
				DontDestroyOnLoad(gp);
				DontDestroyOnLoad(globalChat);
				DontDestroyOnLoad(GameObject.Find("PageNumber"));
				DontDestroyOnLoad(GameObject.Find("ListOfPlayers"));
				DontDestroyOnLoad(GameObject.Find("ListOfPlayersGUIText"));
				Application.LoadLevel(2);
				
				//gp.returnSocket().SendTCPPacket("getBoardData\\1\\" + gp.playerName);
			}
			
			if ( GUI.Button( new Rect(540, Screen.height - 30, 110, 20), "Send"))
			{
				if (!chat.Equals(string.Empty))
				{
					//gp.returnSocket().sendTCPPacket(gp.clientName + "\\" + chat;
					globalChat.addLineToChat("joey" , chat);
					chat = string.Empty;
				}
			}
			
			if ( GUI.Button( new Rect( Screen.width / 2 - 30, Screen.height / 2 - 25, 80, 20), "Logout"))
			{
				am.playErrorSFX();
				//send a disconnect packet
				gp.returnSocket().SendTCPPacket("logout\\" + gp.playerName);
				
				//keep the gameprocess object intact and return to login screen (level 0)
				//DontDestroyOnLoad(gp);
				Application.LoadLevel(0);
				
				// KILL THREAD AND SERVER CONNECTION
				//gp.returnSocket().t.Abort();
				//gp.returnSocket().endThread();
				//gp.returnSocket().Disconnect();
			}
			
		}
		
		if (!showGUI) 
		{
			if(GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 75, 100, 20), "Cancel Search"))
			{
				am.playErrorSFX();
				gp.returnSocket().SendTCPPacket("cancelSearch");
				
				guiText.text = string.Empty;
				showGUI = true;
			}
		}		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
