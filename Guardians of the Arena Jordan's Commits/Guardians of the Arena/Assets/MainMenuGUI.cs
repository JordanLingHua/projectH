using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {
	
	public bool showGUI;
	public GUIText guiText;
	GameProcess gp;
	public string chat;
	globalChatScript globalChat;
	
	// Use this for initialization
	void Start () {
		guiText.text = string.Empty;
		showGUI = true;
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		globalChat = GameObject.Find("globalChat").GetComponent<globalChatScript>();
		chat = "Press Enter to Chat";
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
			//gp.returnSocket().sendTCPPacket(gp.clientName + "\\" + chat;
			globalChat.addLineToChat(gp.playerName , chat);
			chat = string.Empty;
		}
		
		
		if(showGUI)
		{
			if(GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 75, 100, 20), "Play Game"))
			{
				//send search request
				gp.returnSocket().SendTCPPacket("search");
				
				guiText.text = "Searching for Opponent...";
				showGUI = false;
			}
			
			if ( GUI.Button( new Rect( Screen.width / 2 - 45, Screen.height / 2 - 50, 110, 20), "Setup Boards"))
			{
				DontDestroyOnLoad(gp);
				DontDestroyOnLoad(globalChat);
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
