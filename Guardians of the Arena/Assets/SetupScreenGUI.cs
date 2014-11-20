using UnityEngine;
using System.Collections;

public class SetupScreenGUI : MonoBehaviour {		

	public bool showGUI;
	public GUIText guiText;
	GameProcess gp;


	public PlayerSetup playerSetup;


	// Use this for initialization
	void Start () {
		showGUI = true;
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();


		playerSetup = GameObject.Find("PlayerSetup").GetComponent<PlayerSetup>();



	}
	
	void OnGUI () {

		if(showGUI)
		{

//			if(GUI.Button(new Rect(Screen.width - 175, Screen.height / 2 - 125, 120, 20), "Revert Changes"))
//			{
//				switch(playerSetup.activePage)
//				{
//				case 1:
//					gp.returnSocket().SendTCPPacket("getBoardData\\1\\" + gp.playerName);
//					playerSetup.pages[0].modified = false;
//					break;
//				case 2:
//					gp.returnSocket().SendTCPPacket("getBoardData\\2\\" + gp.playerName);
//					playerSetup.pages[1].modified = false;
//					break;
//				case 3:
//					gp.returnSocket().SendTCPPacket("getBoardData\\3\\" + gp.playerName);
//					playerSetup.pages[2].modified = false;
//					break;
//				case 4:
//					gp.returnSocket().SendTCPPacket("getBoardData\\4\\" + gp.playerName);
//					playerSetup.pages[3].modified = false;
//					break;
//				case 5:
//					gp.returnSocket().SendTCPPacket("getBoardData\\5\\" + gp.playerName);
//					playerSetup.pages[4].modified = false;
//					break;
//
//				}


			}
//			if(GUI.Button(new Rect(Screen.width - 175, Screen.height / 2 - 100, 120, 20), "Save Pages"))
//			{
//
//				for(int i = 0; i < 5; i++)
//				{
//					if(playerSetup.pages[i].modified)
//					{
//						string onBoardPieces = "";
//
//						foreach (GameObject piece in playerSetup.pages[i].onBoardPieces)
//						{
//							onBoardPieces += piece.GetComponent<Unit>().unitType + "\\" 
//								          + piece.GetComponentInParent<SetupTileScript>().x + "\\" 
//									+ piece.GetComponentInParent<SetupTileScript>().y;
//						}
//
//						gp.returnSocket().SendTCPPacket("onBoardSetup" + "\\" + gp.playerName + "\\" + i + "\\"
//						                                + onBoardPieces.Length + "\\" + onBoardPieces);
//
//
//						string offBoardPieces = "";
//
//						foreach (GameObject piece in playerSetup.pages[i].offBoardPieces)
//						{
//							offBoardPieces += piece.GetComponent<Unit>().unitType + "\\" 
//								+ piece.GetComponentInParent<SetupTileScript>().x + "\\" 
//									+ piece.GetComponentInParent<SetupTileScript>().y;
//						}
//						
//						gp.returnSocket().SendTCPPacket("offBoardSetup" + "\\" + gp.playerName + "\\" + i + "\\"
//						                                + offBoardPieces.Length + "\\" + offBoardPieces);
//
//						//Reset modified to false
//						playerSetup.pages[i].modified = false;
//					}
//				}
//
//
//			}

			if(GUI.Button(new Rect(Screen.width - 175, Screen.height / 2 - 75, 120, 20), "Clear Board"))
			{


				//save the position where the soulstone and guardian are placed.  
				//move the other units back to their offField positions

				//You need to change the parameters here in providedPieces[] as you change the number of pieces in providedPieces later
				//Vector3 soulStonePosition = playerSetup.providedPieces[9].transform.position;
				//Vector3 guardianPosition = playerSetup.providedPieces[8].transform.position;



				//Application.LoadLevel (Application.loadedLevel);
				Application.LoadLevel (2);



				//playerSetup.providedPieces[8].GetComponent<SetupTileScript>().occupied = false;
				//playerSetup.providedPieces[8].transform.parent = slot;
				//playerSetup.transform.position = new Vector3(slot.transform.position.x, 5.0f, slot.position.z);
				//playerSetup.slot.GetComponent<SetupTileScript>().occupied = true;


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

			

			if (GUI.Button( new Rect( Screen.width - 200, Screen.height / 2 + 75, 25, 20), "1"))
			{
				playerSetup.deleteAllUnits();
				gp.returnSocket().SendTCPPacket("getBoardData\\1\\" + gp.playerName);
				playerSetup.activePage = 0;
			}

			if ( GUI.Button( new Rect( Screen.width - 165, Screen.height / 2 + 75, 25, 20), "2"))
			{
				playerSetup.deleteAllUnits();
				gp.returnSocket().SendTCPPacket("getBoardData\\2\\" + gp.playerName);
				playerSetup.activePage = 1;
			}

			if ( GUI.Button( new Rect( Screen.width - 130, Screen.height / 2 + 75, 25, 20), "3"))
			{
				playerSetup.deleteAllUnits();
				gp.returnSocket().SendTCPPacket("getBoardData\\3\\" + gp.playerName);
				playerSetup.activePage = 2;
			}

			if ( GUI.Button( new Rect( Screen.width - 95, Screen.height / 2 + 75, 25, 20), "4"))
			{
				playerSetup.deleteAllUnits();
				gp.returnSocket().SendTCPPacket("getBoardData\\4\\" + gp.playerName);
				playerSetup.activePage = 3;
			}

			if ( GUI.Button( new Rect( Screen.width - 60, Screen.height / 2 + 75, 25, 20),"5"))
			{
				playerSetup.deleteAllUnits();
				gp.returnSocket().SendTCPPacket("getBoardData\\5\\" + gp.playerName);
				playerSetup.activePage = 4;
			}


		}		

	
	// Update is called once per frame
	void Update () {
		
	}
}
