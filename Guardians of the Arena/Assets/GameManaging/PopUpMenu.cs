using UnityEngine;
using System.Collections;

public class PopUpMenu : MonoBehaviour {
	Texture2D menuTexture;
	public int hpSelGridInt,xpSelGridInt;
	bool displayMenu;
	GameManager gm;
	AudioManager am;
	GameProcess gp;
	string[] selStrings = new string[] {"All", "Allied", "Enemy","Off"};
	int displayWidth = 500,displayHeight = 400;


	void Start () {
		menuTexture = (Texture2D)Resources.Load ("PopupMenuBG");
		am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
	}
	
	void OnGUI(){	
		if (displayMenu) {
			//displays on top of everything else and sets up background
			GUI.depth = -1;
			GUI.skin.box.normal.background = menuTexture;
			GUI.BeginGroup (new Rect ((Screen.width- displayWidth) / 2  , (Screen.height - displayHeight) / 2 , displayWidth, displayHeight));
			//main box
			GUI.Box (new Rect (0,0,displayWidth,displayHeight), "Options Menu");
			//health bar options
			GUI.Label(new Rect (10,20,100,20),"Health Bars");
			//only change options if changed
			int prev = hpSelGridInt;
			hpSelGridInt = GUI.SelectionGrid(new Rect(10, 40, displayWidth-20, 20), hpSelGridInt, selStrings, 4);
			if (prev != hpSelGridInt){
				am.playButtonSFX();
			    if( Application.loadedLevelName.Equals ("BoardScene")){
					GameObject.Find ("TileManager").GetComponent<TileManager>().displayHPBars(hpSelGridInt);
				}
			}

			prev = xpSelGridInt;
			GUI.Label(new Rect (10,60,100,20),"Experience Bars");
			xpSelGridInt = GUI.SelectionGrid(new Rect(10, 80, displayWidth-20, 20), xpSelGridInt, selStrings, 4);
			if (prev != xpSelGridInt){
				am.playButtonSFX();
				if( Application.loadedLevelName.Equals ("BoardScene")){
					GameObject.Find ("TileManager").GetComponent<TileManager>().displayXPBars(xpSelGridInt);
				}
			}
			
			//sound options
			GUI.Label(new Rect (10,100,100,20),"Sound Controls");
			GUI.Label(new Rect (20,120,100,20),"Master Volume");
			am.masterVolume = GUI.HorizontalSlider(new Rect(20,140,displayWidth-80,20),am.masterVolume,0.0f,1.0f);
			GUI.Label(new Rect(displayWidth - 45,135,displayWidth-80,20),am.masterVolume > 0?(am.masterVolume * 100).ToString("##") + "%": "OFF");

			GUI.Label(new Rect (20,160,100,20),"Music Volume");
			am.musicVolume = GUI.HorizontalSlider(new Rect(20,180,displayWidth-80,20),am.musicVolume,0.0f,1.0f);
			GUI.Label(new Rect(displayWidth - 45,175,displayWidth-80,20),am.musicVolume > 0?(am.musicVolume * 100).ToString("##") + "%": "OFF");
			am.bgMusic.volume = am.musicVolume * am.masterVolume;

			GUI.Label(new Rect (20,200,100,20),"Sound Effects Volume");
			am.sfxVolume = GUI.HorizontalSlider(new Rect(20,220,displayWidth-80,20),am.sfxVolume,0.0f,1.0f);
			GUI.Label(new Rect(displayWidth - 45,215,displayWidth-80,20),am.sfxVolume> 0?(am.sfxVolume * 100).ToString("##") + "%": "OFF");
			am.setSFXVolume(am.sfxVolume);

			if (Application.loadedLevelName.Equals("BoardScene")){
				//Surrender Button
				if (!GameObject.Find("GameManager").GetComponent<GameManager>().gameOver){
					if (GUI.Button (new Rect ((displayWidth - 100)/ 2, displayHeight- 55, 100, 20), "Surrender"))
					{
						am.playButtonSFX();
						gp.returnSocket().SendTCPPacket("surrender");
					}
				}else{
					if (GUI.Button (new Rect ((displayWidth - 100)/ 2, displayHeight- 55, 120, 20), "Return to Menu")){
						displayMenu = false;
						am.playButtonSFX();
						DontDestroyOnLoad(GameObject.Find ("GameProcess"));
						Application.LoadLevel(1);
					}
				}
			}
			
			//Quit Button
			if (GUI.Button (new Rect ((displayWidth - 100)/ 2, displayHeight- 30, 100, 20), "Quit"))
			{
				am.playErrorSFX();
				Application.Quit();
			}
			GUI.EndGroup();
		}
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.F10)){
			displayMenu = !displayMenu;
		}
	}
}