using UnityEngine;
using System.Collections;

public class PopUpMenu : MonoBehaviour {
	Texture2D menuTexture;
	public int selGridInt;
	bool displayMenu;
	GameManager gm;
	AudioManager am;
	GameProcess gp;
	
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
			GUI.BeginGroup (new Rect (Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 200));
			//main box
			GUI.Box (new Rect (0,0,200,200), "Main menu");
			//health bar options
			GUI.Label(new Rect (10,17,100,20),"Health Bars");
			string[] selStrings = new string[] {"All", "Allied", "Enemy","Off"};
			//only change options if changed
			int prev = selGridInt;
			selGridInt = GUI.SelectionGrid(new Rect(10, 40, 180, 40), selGridInt, selStrings, 2);
			if (prev != selGridInt && Application.loadedLevelName.Equals ("BoardScene")){
				GameObject.Find ("TileManager").GetComponent<TileManager>().displayHPBars(selGridInt);
			}
			
			//sound options
			GUI.Label(new Rect (10,80,100,20),"Sound");
			am.volume = GUI.HorizontalSlider(new Rect(10,100,180,20),am.volume,0.0f,1.0f);
			am.bgMusic.volume = am.volume;

			if (Application.loadedLevelName.Equals("BoardScene")){
			//Surrender Button
				if (GUI.Button (new Rect (50, 150, 100, 20), "Surrender"))
				{
					gp.returnSocket().SendTCPPacket("surrender\\" + gp.clientNumber);
				}
			}
			
			//Quit Button
			if (GUI.Button (new Rect (50, 175, 100, 20), "Quit"))
			{
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