using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;

public class LoginScreenGUI : MonoBehaviour {

/*
AddSpikes (not perfect but works well enough if you’re careful with your window widths)
FancyTop (just an example of using the elements to do a centered header graphic)
WaxSeal (adds the waxseal and ribbon to the right of the window)
DeathBadge (adds the iconFrame, skull, and ribbon elements properly aligned)
*/
	public string loginText;
	private bool enterDown;
	public string userName;
	public string password;
	public string reEnteredPassword;
	public string ip;
	public string buttonText;
	
	public GameProcess process;	
	public bool connected;
	public bool showReEnterPassword;

	AudioManager am;
	PopUpMenu pum;

	bool doWindow0 = true;
	char c = (char)169;
	
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
	
	//if you're using the spikes you'll need to find sizes that work well with them these are a few...

	private Rect windowRect0 = new Rect (Screen.width / 2 - 350 / 2, 230, 350, 400);

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
	
	
	void DoMyWindow0 (int windowID) 
	{
		AddSpikes(windowRect0.width);
		FancyTop(windowRect0.width);
		
		// use the spike function to add the spikes
		// note: were passing the width of the window to the function

		
		GUILayout.BeginVertical();
		GUILayout.Space(8);
		GUILayout.Label("", "Divider");//-------------------------------- custom
		GUILayout.Label("Welcome!");
		
		GUILayout.Label("", "Divider");//-------------------------------- custom
		
		GUILayout.BeginHorizontal();
		GUILayout.Label ("Username :", "BoldOutlineText");//----------------- custom
		userName = GUILayout.TextField(userName, 20);
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label ("Password :", "BoldOutlineText");//----------------- custom
		password = GUILayout.PasswordField(password, '*', 20);
		GUILayout.EndHorizontal();

		if (showReEnterPassword) {
			GUILayout.BeginHorizontal();
			GUILayout.Label ("Re-Enter Password :", "BoldOutlineText");//----------------- custom
			reEnteredPassword = GUILayout.PasswordField(reEnteredPassword, '*', 20);
			GUILayout.EndHorizontal();
		}
//
//		GUILayout.BeginHorizontal();
//		GUILayout.Label ("Set IP :", "BoldOutlineText");//----------------- custom
//		ip = GUILayout.TextField(ip, 20);
//		GUILayout.EndHorizontal();
//		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Login", "ShortButton")) { //-------------------------------- custom
			am.playButtonSFX();	
			if (buttonText.Equals("Cancel"))
			{
				if (password.Equals(reEnteredPassword)){
					attemptLogin();
				}

				else
				{
					am.playErrorSFX ();
					loginText = "Passwords do not match";
				}
			}

			else
				attemptLogin();
		}
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button (buttonText, "ShortButton")) { //-------------------------------- custom
			if (buttonText.Equals("Create Account"))
			{
				windowRect0.height += 30;
				buttonText = "Cancel";
				showReEnterPassword = true;
				WaxSeal(windowRect0.width , windowRect0.height);
			}
			else
			{
				windowRect0.height -= 30;
				buttonText = "Create Account";
				showReEnterPassword = false;
				WaxSeal(windowRect0.width , windowRect0.height);
			}
		}
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal();	
		GUILayout.Label("", "Divider");//-------------------------------- custom

		GUILayout.Label(loginText);

		//GUILayout.Label("", "Divider");//-------------------------------- custom

		GUILayout.BeginHorizontal();

		GUILayout.Label ("Play2Win Productions " + c, "ItalicText");//---------------------------------- custom
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
		
		// add a wax seal at the bottom of the window
		WaxSeal(windowRect0.width , windowRect0.height);
		
		// Make the windows be draggable.
		//GUI.DragWindow (new Rect (0,0,10000,10000));
	}

	public void attemptLogin()
	{
		//use the alternative ip provided by the user if the ip field is not blank
		if (!ip.Equals(string.Empty))
			process.returnSocket().setIP(ip);
		
		if(!connected)
		{
			loginText = "Connecting...";
			if ( process.returnSocket().Connect() )
			{						
				loginText = "Logging in ";
				StartCoroutine(loginDots());
				connected = true;
			}
			
			else {
				am.playErrorSFX ();
				loginText = "Connect Failed!";}
		}
		
		string source = password;
		string hash;
		
		//hash an encrypted password for the user
		using (MD5 md5Hash = MD5.Create())
		{
			hash = GetMd5Hash(md5Hash, source);								
			process.returnSocket().SendTCPPacket("userInfo\\" + userName + "\\" + hash);
		}
	}

	IEnumerator loginDots()
	{
		while (true) {
						if (loginText.Equals ("Logging in ..."))
								loginText = ("Logging in ");
						else if (loginText.Equals ("Invalid Login Info. Try Again."))
						{
							//do nothing
						}
							
						else
								loginText += ".";

						yield return new WaitForSeconds (0.3f);
				}
	}
	
	void OnGUI ()
	{

		GUI.skin = mySkin;
		GUI.BeginGroup (new Rect (0,0,100,100));
//		GUILayout.BeginHorizontal();
//		if (GUILayout.Button ("Visit Website", "ShortButton")) {
//			Application.OpenURL("http://skawafuc.wix.com/guardiansofthearena");
//		}
//		GUILayout.EndHorizontal();
		//not in development
//		GUILayout.BeginHorizontal();
//		if (GUILayout.Button ("Tutorial", "ShortButton")) {
//			Application.LoadLevel("Tutorial");
//		}
//		GUILayout.EndHorizontal();
		GUI.EndGroup ();

		if (doWindow0)
			windowRect0 = GUI.Window (0, windowRect0, DoMyWindow0, "");
		//now adjust to the group. (0,0) is the topleft corner of the group.
		GUI.BeginGroup (new Rect (0,0,100,100));
		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();


		if (Event.current.keyCode == KeyCode.Return && !enterDown) 
		{
			enterDown = true;
			am.playButtonSFX();	
			if (buttonText.Equals("Cancel"))
			{
				if (password.Equals(reEnteredPassword)){
					//am.playButtonSFX();		
					attemptLogin();
				}
				
				else
				{
					am.playErrorSFX ();
					loginText = "Passwords do not match";
				}
			}
			
			else
				attemptLogin();
		}

		if (Input.GetKeyUp (KeyCode.Return)) {
			enterDown = false;
				}
	}

	// Use this for initialization
	void Start () {
		loginText = string.Empty;
		enterDown = false;
		connected = false;
		pum = GameObject.Find ("PopUpMenu").GetComponent<PopUpMenu> ();
		am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		process = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		userName = string.Empty;
		password = string.Empty;
		reEnteredPassword = string.Empty;
		ip = string.Empty;	
		showReEnterPassword = false;
		buttonText = "Create Account";
	}

	//called from gameprocess when a user inputs invalid login info
	public void loginFail()
	{
		password = string.Empty;
		am.playErrorSFX ();
		loginText = "Invalid Login Info. Try Again.";
	}
	
	//server verifies info and user is logged in
	public void loginSucceed()
	{
		DontDestroyOnLoad(process);
		DontDestroyOnLoad (am);
		DontDestroyOnLoad (GameObject.Find ("PopUpMenu"));
		Application.LoadLevel(1);
	}
	
	string GetMd5Hash(MD5 md5Hash, string input)
	{		
		// Convert the input string to a byte array and compute the hash. 
		byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
		
		// Create a new Stringbuilder to collect the bytes 
		// and create a string.
		StringBuilder sBuilder = new StringBuilder();
		
		// Loop through each byte of the hashed data  
		// and format each one as a hexadecimal string. 
		for (int i = 0; i < data.Length; i++)
		{
			sBuilder.Append(data[i].ToString("x2"));
		}
		
		// Return the hexadecimal string. 
		return sBuilder.ToString();
	}
	
	
	public void resetGuiText()
	{
		loginText = string.Empty;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}


