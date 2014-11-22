using UnityEngine;
using System.Collections;

public class ListOfPlayersScript : MonoBehaviour {

	public GUIText guiText;
	public ArrayList playerNames;


	void Start () {
		guiText.text = "Players Online:";
		playerNames = new ArrayList();
	}

	// Update is called once per frame
	void Update () {

	}

	//whenever the list of players changes, refreshplayerlist updates the guitext to reflect it
	public void refreshPlayerList()
	{
		string s = "Players Online:";
		foreach (string name in playerNames) 
			s += System.Environment.NewLine + name;
		guiText.text = s;
	}

	//add the new player and re-sort the list (for easy visual searches by the user)
	public void addPlayer(string playerName)
	{
		playerNames.Add (playerName);
		playerNames.Sort ();
		refreshPlayerList ();
	}

	//remove a player on disconnect
	public void removePlayer(string playerName)
	{
		playerNames.Remove (playerName);
		refreshPlayerList ();
	}
}
