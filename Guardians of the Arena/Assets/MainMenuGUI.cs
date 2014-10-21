using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {

	public bool showGUI;
	public GUIText guiText;
	GameProcess gp;

	// Use this for initialization
	void Start () {
		guiText.text = "";
		showGUI = true;
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
	}

	void OnGUI () {

		if(showGUI)
		{
			if(GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 75, 100, 20), "Play Game"))
			{
				//send search request
				//gp.returnSocket().SendTCPPacket("search\\clientNumber");

				//this text must be updated once a packet in gameprocess tells it to do so
				guiText.text = "Searching for Opponent...";
				showGUI = false;
			}

			if ( GUI.Button( new Rect( Screen.width / 2 - 45, Screen.height / 2 - 50, 110, 20), "Setup Boards"))
			{
				DontDestroyOnLoad(gp);
				DontDestroyOnLoad(GameObject.Find("ListOfPlayers"));
				DontDestroyOnLoad(GameObject.Find("ListOfPlayersGUIText"));
				Application.LoadLevel(2);

				//gp.returnSocket().SendTCPPacket("getBoardData\\1\\" + gp.playerName);
			}

			if ( GUI.Button( new Rect( Screen.width / 2 - 30, Screen.height / 2 - 25, 80, 20), "Logout"))
			{
				//send a disconnect packet
				//gp.returnSocket().SendTCPPacket("logout\\" + gp.playerName);

				//keep the gameprocess object intact and return to login screen (level 0)
				DontDestroyOnLoad(gp);
				Application.LoadLevel(0);

				// KILL THREAD AND SERVER CONNECTION
				gp.returnSocket().t.Abort();
				gp.returnSocket().endThread();
				gp.returnSocket().Disconnect();
			}

		}

		if (!showGUI) 
		{
			if(GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 75, 100, 20), "Cancel Search"))
			{
				//gp.returnSocket().SendTCPPacket("cancelSearch\\clientNumber");
				
				guiText.text = "";
				showGUI = true;
			}
		}		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
