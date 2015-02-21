using UnityEngine;
using System.Collections;

public class SetupScreenGUI : MonoBehaviour {		

	public bool showGUI;
	AudioManager am;
	public GUIText guiText;
	GameProcess gp;
	PageNumberScript pageNumber;
	PageNameScript pageNameScript;
	public PlayerSetup playerSetup;
	private bool editName;
	private string tempName;
	private bool enterDown;

	bool doWindow5 = true;

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
	
	// This script will only work with the Necromancer skin
	public GUISkin mySkin;
	GUIText boardCapacityGUIText;
	private Rect windowRect5;
	int displayWidth5 = 320;
	int displayHeight5 = 350;

	void Start () {
		showGUI = true;
		boardCapacityGUIText = GameObject.Find("BoardCapacityGUIText").GetComponent<GUIText>();
		pageNumber = GameObject.Find ("PageInfo").GetComponent<PageNumberScript> ();
		pageNameScript = GameObject.Find ("PageInfo").GetComponent<PageNameScript> ();
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		playerSetup = GameObject.Find("PlayerSetup").GetComponent<PlayerSetup>();
		windowRect5 = new Rect (Screen.width - 315, Screen.height - 330, displayWidth5, displayHeight5);
		am = GameObject.Find ("AudioManager").GetComponent<AudioManager> ();
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
	
	
	void DoMyWindow5 (int windowID) 
	{
		
		//FancyTop(windowRect1.width);

		// use the spike function to add the spikes
		// note: were passing the width of the window to the function
		//	AddSpikes(windowRect1.width);
		GUILayout.BeginVertical ();
		GUILayout.Space (8);

		GUILayout.Label ("Other Menu");

//		GUILayout.BeginHorizontal ();
//		GUILayout.FlexibleSpace();
//		if(GUILayout.Button("Clear Board", "ShortButton"))
//		{
//			Application.LoadLevel (2);
//		}
//		GUILayout.FlexibleSpace();
//		GUILayout.EndHorizontal ();

		//GUILayout.Label("", "Divider");//-------------------------------- custom

		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace();		
		if (GUILayout.Button("Return to Game Lobby", "ShortButton"))
		{
			Application.LoadLevel(1);
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();

		GUILayout.Label("", "Divider");//-------------------------------- custom


		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Logout", "ShortButton"))
		{
			//send a disconnect packet
			am.playErrorSFX ();
			gp.returnSocket().SendTCPPacket("logout\\" + gp.playerName);
			
			//keep the gameprocess object intact and return to main menu (level 0)
//			Destroy(GameObject.Find ("ListOfPlayers"));
//			Destroy(GameObject.Find ("ListOfPlayersGUIText"));
//			DontDestroyOnLoad(gp);
//			Application.LoadLevel(0);
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();

		GUILayout.Label("", "Divider");//-------------------------------- custom

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Board Setups", "OutlineText");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.Space (8);

		GUILayout.BeginHorizontal();

		if (!editName) 
		{
			GUILayout.FlexibleSpace();
			GUILayout.Label (getPageName (), "PlainText");
		}
		else 
		{
			tempName = GUILayout.TextField (tempName, 20);
		}
	
		if (!editName) 
		{
			GUILayout.FlexibleSpace();
			if (GUILayout.Button ("Rename", "ShortButton")) {
				editName = true;
				tempName = getPageName();
			}
		}
		else
			if (GUILayout.Button ("Save", "ShortButton")) {
				editName = false;
				pageNameScript.pages[playerSetup.activePage] = tempName;
				gp.returnSocket().SendTCPPacket("updateSetupName" + "\\" + pageNumber.selectedPage + "\\" + tempName);
			}

		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button("1", "ShortButton"))
		{
			editName = false;
			playerSetup.deleteAllUnits();
			gp.returnSocket().SendTCPPacket("getBoardData\\1");
			playerSetup.activePage = 0;
			pageNumber.selectedPage = 1;
		}
		
		if (GUILayout.Button("2", "ShortButton"))
		{
			editName = false;
			playerSetup.deleteAllUnits();
			gp.returnSocket().SendTCPPacket("getBoardData\\2");
			playerSetup.activePage = 1;
			pageNumber.selectedPage = 2;
		}
		
		if (GUILayout.Button("3", "ShortButton"))
		{
			editName = false;
			playerSetup.deleteAllUnits();
			gp.returnSocket().SendTCPPacket("getBoardData\\3");
			playerSetup.activePage = 2;
			pageNumber.selectedPage = 3;
		}
		
		if (GUILayout.Button("4", "ShortButton"))
		{
			editName = false;
			playerSetup.deleteAllUnits();
			gp.returnSocket().SendTCPPacket("getBoardData\\4");
			playerSetup.activePage = 3;
			pageNumber.selectedPage = 4;
		}
		
		if (GUILayout.Button("5", "ShortButton"))
		{
			editName = false;
			playerSetup.deleteAllUnits();
			gp.returnSocket().SendTCPPacket("getBoardData\\5");
			playerSetup.activePage = 4;
			pageNumber.selectedPage = 5;
		}

		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();

	}

	string getPageName()
	{
		return pageNameScript.pages[playerSetup.activePage];
	}


	
	void OnGUI () {
		boardCapacityGUIText.text = "Units on Field: " +playerSetup.pages[playerSetup.activePage].onBoardPieces.Count + "/" + playerSetup.boardCapacity;
		GUI.skin = mySkin;

		if (doWindow5)
			windowRect5 = GUI.Window (5, windowRect5, DoMyWindow5, "");
		//now adjust to the group. (0,0) is the topleft corner of the group.
		GUI.BeginGroup (new Rect (0,0,100,100));
		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();


		if (Event.current.keyCode == KeyCode.Return && !enterDown && editName) 
		{
			enterDown = true;
			editName = false;
			pageNameScript.pages[playerSetup.activePage] = tempName;
		}
		
		if (Input.GetKeyUp (KeyCode.Return)) {
			enterDown = false;
		}


	}	

	
	// Update is called once per frame
	void Update () {

	}
}
