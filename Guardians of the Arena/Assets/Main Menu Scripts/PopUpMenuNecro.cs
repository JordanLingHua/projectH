using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;

public class PopUpMenuNecro : MonoBehaviour {
	
	/*
AddSpikes (not perfect but works well enough if you’re careful with your window widths)
FancyTop (just an example of using the elements to do a centered header graphic)
WaxSeal (adds the waxseal and ribbon to the right of the window)
DeathBadge (adds the iconFrame, skull, and ribbon elements properly aligned)
*/
	public bool allowEnemyUnitSelection, allowAutoMoveAttackToggle;
	public int hpSelGridInt, xpSelGridInt;
	GameManager gm;
	AudioManager am;
	GameProcess gp;
	int displayWidth = 460;
	int displayHeight = 620;
	public bool doWindow1 = false;

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
	private Rect windowRect1;
	
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
	
	
	void DoMyWindow1 (int windowID) 
	{

		//FancyTop(windowRect1.width);
		
		// use the spike function to add the spikes
		// note: were passing the width of the window to the function
	//	AddSpikes(windowRect1.width);
		GUILayout.BeginVertical();
		GUILayout.Space(8);
	
		GUILayout.Label("Options");

		GUILayout.Space(8);
		//Centers the following text
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Visual Controls", "OutlineText");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();


		GUILayout.Label("Health Bars", "PlainText");

		GUILayout.BeginHorizontal();
		if (GUILayout.Button ("All", "ShortButton"))
		{
			am.playButtonSFX();
			if( Application.loadedLevelName.Equals ("BoardScene")|| Application.loadedLevelName.Equals ("AIScene")){
				hpSelGridInt = 0;
				GameObject.Find ("TileManager").GetComponent<TileManager>().displayHPBars(0);
			}
		}
		if (GUILayout.Button ("Allied", "ShortButton"))
		{
			am.playButtonSFX();
			if( Application.loadedLevelName.Equals ("BoardScene")|| Application.loadedLevelName.Equals ("AIScene")){
				hpSelGridInt = 1;
				GameObject.Find ("TileManager").GetComponent<TileManager>().displayHPBars(1);
			}
		}
		if (GUILayout.Button ("Enemy", "ShortButton"))
		{
			am.playButtonSFX();
			if( Application.loadedLevelName.Equals ("BoardScene")|| Application.loadedLevelName.Equals ("AIScene")){
				hpSelGridInt = 2;
				GameObject.Find ("TileManager").GetComponent<TileManager>().displayHPBars(2);
			}
		}
		if (GUILayout.Button ("Off", "ShortButton"))
		{
			am.playButtonSFX();
			if( Application.loadedLevelName.Equals ("BoardScene")|| Application.loadedLevelName.Equals ("AIScene")){
				hpSelGridInt = 3;
				GameObject.Find ("TileManager").GetComponent<TileManager>().displayHPBars(3);
			}
		}
		GUILayout.EndHorizontal();
	//	GUILayout.Label("", "Divider");//-------------------------------- custom


		GUILayout.Label("EXP Bars", "PlainText");
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button ("All", "ShortButton"))
		{
			am.playButtonSFX();
			if( Application.loadedLevelName.Equals ("BoardScene")|| Application.loadedLevelName.Equals ("AIScene")){
				xpSelGridInt = 0;
				GameObject.Find ("TileManager").GetComponent<TileManager>().displayXPBars(0);
			}
		}
		if (GUILayout.Button ("Allied", "ShortButton"))
		{
			am.playButtonSFX();
			if( Application.loadedLevelName.Equals ("BoardScene") || Application.loadedLevelName.Equals ("AIScene") ){
				xpSelGridInt = 1;
				GameObject.Find ("TileManager").GetComponent<TileManager>().displayXPBars(1);
			}
		}
		if (GUILayout.Button ("Enemy", "ShortButton"))
		{
			am.playButtonSFX();
			if( Application.loadedLevelName.Equals ("BoardScene")|| Application.loadedLevelName.Equals ("AIScene")){
				xpSelGridInt = 2;
				GameObject.Find ("TileManager").GetComponent<TileManager>().displayXPBars(2);
			}
		}
		if (GUILayout.Button ("Off", "ShortButton"))
		{
			am.playButtonSFX();
			if( Application.loadedLevelName.Equals ("BoardScene")|| Application.loadedLevelName.Equals ("AIScene")){
				xpSelGridInt = 3;
				GameObject.Find ("TileManager").GetComponent<TileManager>().displayXPBars(3);
			}
		}
		GUILayout.EndHorizontal();

		GUILayout.Label("", "Divider");//-------------------------------- custom

		GUILayout.Space(12);

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Sound Controls", "OutlineText");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.Label("Master Volume", "PlainText");
		GUILayout.BeginHorizontal();
		am.masterVolume = GUILayout.HorizontalSlider(am.masterVolume, 0.0f, 1.0f);
		GUILayout.Label(am.masterVolume > 0 ? (am.masterVolume * 100).ToString("##") + "%": "OFF", "PlainText");
		GUILayout.EndHorizontal();

		GUILayout.Label("Music Volume", "PlainText");
		GUILayout.BeginHorizontal();
		am.musicVolume = GUILayout.HorizontalSlider(am.musicVolume, 0.0f, 1.0f);
		GUILayout.Label(am.musicVolume > 0 ? (am.musicVolume * 100).ToString("##") + "%": "OFF", "PlainText");
		am.bgMusic.volume = am.musicVolume * am.masterVolume;
		GUILayout.EndHorizontal();

		GUILayout.Label("Sound FX Volume", "PlainText");
		GUILayout.BeginHorizontal();
		am.sfxVolume = GUILayout.HorizontalSlider(am.sfxVolume, 0.0f, 1.0f);
		GUILayout.Label(am.sfxVolume > 0 ? (am.sfxVolume * 100).ToString("##") + "%": "OFF", "PlainText");
		am.setSFXVolume(am.sfxVolume);
		GUILayout.EndHorizontal();

		//GUILayout.Label("", "Divider");//-------------------------------- custom
		GUILayout.Space(8);

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Game Controls", "OutlineText");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		allowAutoMoveAttackToggle = GUILayout.Toggle(allowAutoMoveAttackToggle,"Automatically Switch to Attack Phase After Moving");
		allowEnemyUnitSelection = GUILayout.Toggle (allowEnemyUnitSelection,"Allow Enemy Unit Selection");

		GUILayout.Label("", "Divider");//-------------------------------- custom

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (Application.loadedLevelName.Equals("BoardScene") || Application.loadedLevelName.Equals("AIScene")){
			//Surrender Button
			if (!GameObject.Find("GameManager").GetComponent<GameManager>().gameOver){
				if (GUILayout.Button ("Surrender", "ShortButton"))
				{
					am.playButtonSFX();
					gp.returnSocket().SendTCPPacket("surrender");
				}
			}else{
				if (GUILayout.Button ("Return to Game Lobby", "ShortButton")){
					doWindow1 = false;
					am.playButtonSFX();
					DontDestroyOnLoad(GameObject.Find ("GameProcess"));
					Application.LoadLevel(1);
				}
			}

		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.Label("", "Divider");//-------------------------------- custom


		GUILayout.BeginHorizontal();
		//Quit Button
		GUILayout.FlexibleSpace();
		if (GUILayout.Button ("Quit Game", "ShortButton"))
		{
			am.playErrorSFX();
			Application.Quit();
		}

		//Close Button
		if (GUILayout.Button ("Close Menu", "ShortButton"))
		{
			doWindow1 = !doWindow1;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

		
		// Make the windows be draggable.
		GUI.DragWindow (new Rect (0,0,10000,10000));
	}
	

	
	void OnGUI ()
	{
		GUI.skin = mySkin;

		GUI.BeginGroup (new Rect(Screen.width - (Screen.width*0.065f),0,(Screen.width*0.09f),30));
		GUILayout.BeginHorizontal();
		if (!Application.loadedLevelName.Equals ("BoardScene") && !Application.loadedLevelName.Equals ("AIScene")) {
			if (GUILayout.Button ("Options", "ShortButton")) {
					doWindow1 = !doWindow1;
					am.playButtonSFX ();
			}
		}
		GUILayout.EndHorizontal();
		GUI.EndGroup ();

		if (doWindow1)
			windowRect1 = GUI.Window (1, windowRect1, DoMyWindow1, "");
		//now adjust to the group. (0,0) is the topleft corner of the group.
		GUI.BeginGroup (new Rect (0,0,100,100));
		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();
	}
	
	// Use this for initialization
	void Start () {
		allowAutoMoveAttackToggle = false;
		am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		windowRect1 = new Rect (Screen.width / 2 - 350 / 2, 0, displayWidth, displayHeight);
	}
	

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F10)){
			doWindow1 = !doWindow1;
		}
	}
}


