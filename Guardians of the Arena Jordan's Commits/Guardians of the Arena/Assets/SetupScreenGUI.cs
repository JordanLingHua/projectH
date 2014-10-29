using UnityEngine;
using System.Collections;

public class SetupScreenGUI : MonoBehaviour {		

	public bool showGUI;
	public GUIText guiText;
	GameProcess gp;
	
	// Use this for initialization
	void Start () {
		showGUI = true;
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
	}
	
	void OnGUI () {

		if(showGUI)
		{
			if(GUI.Button(new Rect(Screen.width - 175, Screen.height / 2 - 100, 120, 20), "Save Pages"))
			{

			}

			if(GUI.Button(new Rect(Screen.width - 175, Screen.height / 2 - 75, 120, 20), "Clear Board"))
			{
				
			}
			
			if ( GUI.Button( new Rect( Screen.width - 160, Screen.height / 2 - 25, 80, 20), "Back"))
			{
				//DontDestroyOnLoad(gp);
				Application.LoadLevel(1);
			}
			
			if ( GUI.Button( new Rect( Screen.width - 160, Screen.height / 2 - 0, 80, 20), "Logout"))
			{
				//send a disconnect packet
				gp.returnSocket().SendTCPPacket("logout\\" + gp.playerName);
				
				//keep the gameprocess object intact and return to main menu (level 0)
				Destroy(GameObject.Find ("ListOfPlayers"));
				Destroy(GameObject.Find ("ListOfPlayersGUIText"));
				DontDestroyOnLoad(gp);
				Application.LoadLevel(0);
				
				// KILL THREAD AND SERVER CONNECTION
				gp.returnSocket().t.Abort();
				gp.returnSocket().endThread();
				gp.returnSocket().Disconnect();
			}

			if ( GUI.Button( new Rect( Screen.width - 200, Screen.height / 2 + 75, 25, 20), "1"))
			{
				//gp.returnSocket().SendTCPPacket("getBoardData\\1\\" + gp.playerName);
			}

			if ( GUI.Button( new Rect( Screen.width - 165, Screen.height / 2 + 75, 25, 20), "2"))
			{
				//gp.returnSocket().SendTCPPacket("getBoardData\\2\\" + gp.playerName);
			}

			if ( GUI.Button( new Rect( Screen.width - 130, Screen.height / 2 + 75, 25, 20), "3"))
			{
				//gp.returnSocket().SendTCPPacket("getBoardData\\3\\" + gp.playerName);
			}

			if ( GUI.Button( new Rect( Screen.width - 95, Screen.height / 2 + 75, 25, 20), "4"))
			{
				//gp.returnSocket().SendTCPPacket("getBoardData\\4\\" + gp.playerName);
			}

			if ( GUI.Button( new Rect( Screen.width - 60, Screen.height / 2 + 75, 25, 20), "5"))
			{
				//gp.returnSocket().SendTCPPacket("getBoardData\\5\\" + gp.playerName);
			}

			if ( GUI.Button( new Rect( Screen.width - 160, Screen.height / 2 + 140, 25, 20), "A"))
			{
			}
			
			if ( GUI.Button( new Rect( Screen.width - 120, Screen.height / 2 + 140, 25, 20), "B"))
			{
			}			
		}		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
