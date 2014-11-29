using UnityEngine;
using System.Collections;

public class PopUpMenu : MonoBehaviour {
	Texture2D menuTexture;
	public int selGridInt;
	bool displayMenu;
	GameManager gm;
	AudioManager am;
	GameProcess gp;
	string[] selStrings = new string[] {"All", "Allied", "Enemy","Off"};

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
			GUI.BeginGroup (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 150, 200, 300));
			//main box
			GUI.Box (new Rect (0,0,200,300), "Main menu");
			//health bar options
			GUI.Label(new Rect (10,17,100,20),"Health Bars");
			//only change options if changed
			int prev = selGridInt;
			selGridInt = GUI.SelectionGrid(new Rect(10, 40, 180, 40), selGridInt, selStrings, 2);
			if (prev != selGridInt){
				am.playButtonSFX();
			    if( Application.loadedLevelName.Equals ("BoardScene")){
					GameObject.Find ("TileManager").GetComponent<TileManager>().displayHPBars(selGridInt);
				}
			}
			
			//sound options
			GUI.Label(new Rect (10,80,100,20),"Sound Controls");
			GUI.Label(new Rect (20,95,100,20),"Master Volume");
			am.masterVolume = GUI.HorizontalSlider(new Rect(20,115,160,20),am.masterVolume,0.0f,1.0f);

			GUI.Label(new Rect (20,125,100,20),"Music Volume");
			am.musicVolume = GUI.HorizontalSlider(new Rect(20,145,160,20),am.musicVolume,0.0f,1.0f);
			am.bgMusic.volume = am.musicVolume * am.masterVolume;

			GUI.Label(new Rect (20,155,100,20),"Sound Effects Volume");
			am.sfxVolume = GUI.HorizontalSlider(new Rect(20,175,160,20),am.sfxVolume,0.0f,1.0f);
			am.setSFXVolume(am.sfxVolume);

			if (Application.loadedLevelName.Equals("BoardScene")){
				//Surrender Button
				if (!GameObject.Find("GameManager").GetComponent<GameManager>().gameOver){
					if (GUI.Button (new Rect (50, 245, 100, 20), "Surrender"))
					{
						am.playButtonSFX();
						gp.returnSocket().SendTCPPacket("surrender");
					}
				}else{
					if (GUI.Button (new Rect (40, 245, 120, 20), "Return to Menu")){
						am.playButtonSFX();
						DontDestroyOnLoad(GameObject.Find ("GameProcess"));
						Application.LoadLevel(1);
					}
				}
			}
			
			//Quit Button
			if (GUI.Button (new Rect (50, 275, 100, 20), "Quit"))
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