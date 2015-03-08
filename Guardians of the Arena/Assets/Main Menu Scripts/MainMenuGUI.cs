using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;

public class MainMenuGUI : MonoBehaviour {
	
	public bool showGUI;
	public string infoText;
	GameProcess gp;
	PageNumberScript pageNumber;
	PageNameScript pageNameScript;
	globalChatScript globalChatScript;
	ListOfPlayersScript listOfPlayers;
	public string chat;
	public string challengedPlayer;
	string challengedPlayerCopy;
	public int space;

	public bool challengePending;
	AudioManager am;

	bool doWindow2 = true;
	bool doWindow3 = true;
	bool doWindow4 = true;
	
	private float leafOffset;
	private float frameOffset;
	private float skullOffset;
	
	private float RibbonOffsetX;
	private float FrameOffsetX;
	private float SkullOffsetX;
	private float RibbonOffsetY;
	private float FrameOffsetY;
	private float SkullOffsetY;
	
	private float WSwaxOffsetX;
	private float WSwaxOffsetY;
	private float WSribbonOffsetX;
	private float WSribbonOffsetY;
	
	private int spikeCount;
	private int windowSeparation;
	public Vector2 scrollPosition3;
	public Vector2 scrollPosition4;
	
	// This script will only work with the Necromancer skin
	public GUISkin mySkin;
	
	//menu window
	public Rect windowRect2;
	int displayWidth2 = 400;
	int displayHeight2 = 500;

	//chat window
	public Rect windowRect3;
	int displayWidth3 = 400;
	int displayHeight3 = 450;

	//players online window
	public Rect windowRect4;
	int displayWidth4 = 275;
	int displayHeight4 = 450;

	// Use this for initialization
	void Start () {
		pageNumber = GameObject.Find ("PageInfo").GetComponent<PageNumberScript>();
		pageNameScript = GameObject.Find ("PageInfo").GetComponent<PageNameScript>();
		infoText = string.Empty;
		challengedPlayerCopy = string.Empty;
		showGUI = true;
		am = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		globalChatScript = GameObject.Find ("GlobalChat").GetComponent<globalChatScript> ();
		listOfPlayers = GameObject.Find ("ListOfPlayers").GetComponent<ListOfPlayersScript> ();
		chat = string.Empty;
		challengePending = false;

		windowSeparation = 25;
		windowRect2 = new Rect (Screen.width / 2 - displayWidth2 / 2, Screen.height / 2 - 250, displayWidth2, displayHeight2);
		windowRect3 = new Rect (Screen.width / 2 - displayWidth2 / 2 - displayWidth2 - windowSeparation, Screen.height / 2 - 200, displayWidth3, displayHeight3);
		windowRect4 = new Rect (Screen.width / 2 + displayWidth2 / 2 + windowSeparation, Screen.height / 2 - 200, displayWidth4, displayHeight4);
		space = 10;

	}


	
	void AddSpikes(float winX)
	{
		spikeCount = (int)Mathf.Floor(winX - 152)/22;
		GUILayout.BeginHorizontal();
		GUILayout.Label ("", "SpikeLeft");//-------------------------------- custom
		for (int i = 0; i < spikeCount; i++)
		{
			GUILayout.Label ("", "SpikeMid");//-------------------------------- custom
		}
		GUILayout.Label ("", "SpikeRight");//-------------------------------- custom
		GUILayout.EndHorizontal();
	}
	
	void FancyTop(float topX)
	{
		leafOffset = (topX/2)-64;
		frameOffset = (topX/2)-27;
		skullOffset = (topX/2)-20;
		GUI.Label(new Rect(leafOffset, 18, 0, 0), "", "GoldLeaf");//-------------------------------- custom	
		GUI.Label(new Rect(frameOffset, 3, 0, 0), "", "IconFrame");//-------------------------------- custom	
		GUI.Label(new Rect(skullOffset, 12, 0, 0), "", "Skull");//-------------------------------- custom	
	}
	
	void WaxSeal(float x, float y)
	{
		WSwaxOffsetX = x - 120;
		WSwaxOffsetY = y - 115;
		WSribbonOffsetX = x - 114;
		WSribbonOffsetY = y - 83;
		
		GUI.Label(new Rect(WSribbonOffsetX, WSribbonOffsetY, 0, 0), "", "RibbonBlue");//-------------------------------- custom	
		GUI.Label(new Rect(WSwaxOffsetX, WSwaxOffsetY, 0, 0), "", "WaxSeal");//-------------------------------- custom	
	}
	
	void DeathBadge(float x, float y)
	{
		RibbonOffsetX = x;
		FrameOffsetX = x+3;
		SkullOffsetX = x+10;
		RibbonOffsetY = y+22;
		FrameOffsetY = y;
		SkullOffsetY = y+9;
		
		GUI.Label(new Rect(RibbonOffsetX, RibbonOffsetY, 0, 0), "", "RibbonRed");//-------------------------------- custom	
		GUI.Label(new Rect(FrameOffsetX, FrameOffsetY, 0, 0), "", "IconFrame");//-------------------------------- custom	
		GUI.Label(new Rect(SkullOffsetX, SkullOffsetY, 0, 0), "", "Skull");//-------------------------------- custom	
	}
	
	
	void DoMyWindow2 (int windowID) 
	{
		AddSpikes(windowRect2.width);
		FancyTop(windowRect2.width);

		// use the spike function to add the spikes
		// note: were passing the width of the window to the function
		//	AddSpikes(windowRect1.width);
		GUILayout.BeginVertical ();
		GUILayout.Space (space);

		GUILayout.Label ("Welcome Guardian!");
		GUILayout.Label("", "Divider");//-------------------------------- custom

		if (showGUI) 
		{
			if (!challengePending) 
			{
				if (gp.enableMatchmaking){
					GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					if (GUILayout.Button ("Start Matchmaking", "ShortButton")) 
					{
						am.playButtonSFX ();
						//send search request
						gp.returnSocket ().SendTCPPacket ("search\\" + pageNumber.selectedPage);

						infoText = "Searching for Opponent...";
						showGUI = false;
					}
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();

				}else{

					GUILayout.Label ("Play vs AI to Enable Matchmaking");
				}

				GUILayout.Label("", "Divider");//-------------------------------- custom
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button ("Play vs AI", "ShortButton")) 
				{
					am.playButtonSFX ();
					//send search request
				    gp.returnSocket ().SendTCPPacket ("playAI\\" + pageNumber.selectedPage);
					showGUI = false;		
				}

				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.Label("", "Divider");//-------------------------------- custom



//				GUILayout.BeginHorizontal();
//				challengedPlayer = GUILayout.TextField(challengedPlayer, 20);
//				if (GUILayout.Button ("Send Challenge", "ShortButton")) {
//					am.playButtonSFX ();
//					//TODO: 
//					gp.returnSocket ().SendTCPPacket ("challengeRequest\\" + gp.playerName + "\\" + challengedPlayer);
//					challengePending = true;
//					challengedPlayerCopy = challengedPlayer;
//					infoText = "Waiting for " + challengedPlayer + " to respond...";
//
//
//				}
//
//
//				GUILayout.EndHorizontal();
//				GUILayout.Label("", "Divider");//-------------------------------- custom

				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label ("Selected Setup: " + pageNameScript.pages[pageNumber.selectedPage - 1], "PlainText");//------------------------------------ custom
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				//GUILayout.FlexibleSpace();
				if (GUILayout.Button ("Setup Boards", "ShortButton")) 
				{
					am.playButtonSFX ();
					DontDestroyOnLoad (gp);
					DontDestroyOnLoad (GameObject.Find ("GlobalChat"));
					DontDestroyOnLoad (GameObject.Find ("gChat"));
					DontDestroyOnLoad (GameObject.Find ("PageInfo"));
					DontDestroyOnLoad (GameObject.Find ("ListOfPlayers"));
					DontDestroyOnLoad (GameObject.Find ("ListOfPlayersGUIText"));
					Application.LoadLevel (2);
				}
				GUILayout.FlexibleSpace();
				GUILayout.Label ("", "PlainText"); // spacing... placeholder for chose setup
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();


				GUILayout.Space(10);
				GUILayout.Label("", "Divider");//-------------------------------- custom
				
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button ("Logout", "ShortButton")) 
				{
					am.playErrorSFX ();
					//send a disconnect packet
					gp.returnSocket ().SendTCPPacket ("logout\\" + gp.playerName);
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.Label("", "Divider");//-------------------------------- custom
			}
		 

			else
			{
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button ("Cancel Challenge", "ShortButton")) {
					am.playErrorSFX ();
					//send search request
					gp.returnSocket ().SendTCPPacket ("cancelChallenge\\" + gp.playerName + "\\" + challengedPlayerCopy);
					challengedPlayer = string.Empty;
					infoText = string.Empty;
					challengePending = false;
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.Label("", "Divider");//-------------------------------- custom
			}

		}

		if (!showGUI) 
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if(GUILayout.Button ("Cancel Search", "ShortButton"))
			{
				am.playErrorSFX();
				gp.returnSocket().SendTCPPacket("cancelSearch");
				
				infoText = string.Empty;
				showGUI = true;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.Label("", "Divider");//-------------------------------- custom
		}	

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical ();

		GUILayout.BeginVertical ();
		//GUILayout.Label("", "Divider");//-------------------------------- custom
		GUILayout.Label (infoText);

		GUILayout.EndVertical ();


		// Make the windows be draggable.
		//GUI.DragWindow (new Rect (0,0,10000,10000));
	}

	void DoMyWindow3 (int windowID) 
	{
		//AddSpikes(windowRect3.width);
		GUILayout.BeginVertical ();
		GUILayout.Space (8);

		GUILayout.Label ("Chat");
		GUILayout.Label("", "Divider");//-------------------------------- custom


		GUILayout.BeginHorizontal ();
		scrollPosition3 = GUILayout.BeginScrollView (scrollPosition3, false, true);
		//scrollPosition = GUILayout.BeginScrollView (scrollPosition, false, true, GUILayout.Width(100), GUILayout.Height(100));
		GUILayout.Label (globalChatScript.gChat, "PlainText");
		GUILayout.EndScrollView ();
		GUILayout.EndHorizontal ();
		GUILayout.Space (8);

		GUILayout.BeginHorizontal();
		chat = GUILayout.TextField (chat, 50);
		if (GUILayout.Button ("Send", "ShortButton")) 
		{
			if (!chat.Equals (string.Empty)) 
			{
				gp.returnSocket ().SendTCPPacket ("globalChat\\" + chat);
				chat = string.Empty;
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical ();

		// Make the windows be draggable.
		//GUI.DragWindow (new Rect (0,0,10000,10000));

	}

	void DoMyWindow4 (int windowID) 
	{
		//AddSpikes(windowRect4.width);
		GUILayout.BeginVertical ();
		GUILayout.Space (8);

		GUILayout.Label ("Players Online");
		GUILayout.Label ("", "Divider");//-------------------------------- custom


		GUILayout.BeginHorizontal ();
		scrollPosition4 = GUILayout.BeginScrollView (scrollPosition4, false, true);
		//scrollPosition = GUILayout.BeginScrollView (scrollPosition, false, true, GUILayout.Width(100), GUILayout.Height(100));
		GUILayout.Label (listOfPlayers.playerList, "PlainText");
		GUILayout.EndScrollView ();
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();

		// Make the windows be draggable.
		//GUI.DragWindow (new Rect (0,0,10000,10000));
	}
	
	
	void OnGUI () {

		GUI.skin = mySkin;
		
		if (doWindow2)
			windowRect2 = GUI.Window (2, windowRect2, DoMyWindow2, "");

		//now adjust to the group. (0,0) is the topleft corner of the group.
		GUI.BeginGroup (new Rect (0,0,100,100));
		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();

		if (doWindow3)
			windowRect3 = GUI.Window (3, windowRect3, DoMyWindow3, "");

		//now adjust to the group. (0,0) is the topleft corner of the group.
		GUI.BeginGroup (new Rect (0,0,100,100));
		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();

		if (doWindow4)
			windowRect4 = GUI.Window (4, windowRect4, DoMyWindow4, "");
		
		//now adjust to the group. (0,0) is the topleft corner of the group.
		GUI.BeginGroup (new Rect (0,0,100,100));
		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();
		
		GUI.SetNextControlName ("chatField");
		
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			GUI.FocusControl ("chatField");
			chat = string.Empty;
		}

		if (Event.current.keyCode == KeyCode.Return && !chat.Equals(string.Empty)) 
		{
			gp.returnSocket().SendTCPPacket("globalChat\\" + chat);
			chat = string.Empty;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
