using UnityEngine;
using System.Collections;

public class SetupScreenGUI : MonoBehaviour {		

	public bool showGUI;
	public GUIText guiText;
	GameProcess gp;
	PageNumberScript pageNumber;
	public PlayerSetup playerSetup;

	void Start () {
		showGUI = true;
		pageNumber = GameObject.Find ("PageNumber").GetComponent<PageNumberScript> ();
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		playerSetup = GameObject.Find("PlayerSetup").GetComponent<PlayerSetup>();
	}
	
	void OnGUI () {

		if(GUI.Button(new Rect(Screen.width - 175, Screen.height / 2 - 75, 120, 20), "Clear Board"))
		{
			Application.LoadLevel (2);
		}
		
		if ( GUI.Button( new Rect( Screen.width - 160, Screen.height / 2 - 25, 80, 20), "Back"))
		{
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

		if (GUI.Button( new Rect( Screen.width - 200, Screen.height / 2 + 75, 25, 20), "1"))
		{
			playerSetup.deleteAllUnits();
			gp.returnSocket().SendTCPPacket("getBoardData\\1");
			playerSetup.activePage = 0;
			pageNumber.selectedPage = 1;
		}

		if ( GUI.Button( new Rect( Screen.width - 165, Screen.height / 2 + 75, 25, 20), "2"))
		{
			playerSetup.deleteAllUnits();
			gp.returnSocket().SendTCPPacket("getBoardData\\2");
			playerSetup.activePage = 1;
			pageNumber.selectedPage = 2;
		}

		if ( GUI.Button( new Rect( Screen.width - 130, Screen.height / 2 + 75, 25, 20), "3"))
		{
			playerSetup.deleteAllUnits();
			gp.returnSocket().SendTCPPacket("getBoardData\\3");
			playerSetup.activePage = 2;
			pageNumber.selectedPage = 4;
		}

		if ( GUI.Button( new Rect( Screen.width - 95, Screen.height / 2 + 75, 25, 20), "4"))
		{
			playerSetup.deleteAllUnits();
			gp.returnSocket().SendTCPPacket("getBoardData\\4");
			playerSetup.activePage = 3;
			pageNumber.selectedPage = 4;
		}

		if ( GUI.Button( new Rect( Screen.width - 60, Screen.height / 2 + 75, 25, 20),"5"))
		{
			playerSetup.deleteAllUnits();
			gp.returnSocket().SendTCPPacket("getBoardData\\5");
			playerSetup.activePage = 4;
			pageNumber.selectedPage = 5;
		}
	}	

	
	// Update is called once per frame
	void Update () {
		
	}
}
