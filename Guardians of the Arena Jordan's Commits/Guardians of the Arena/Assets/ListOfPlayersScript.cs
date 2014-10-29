using UnityEngine;
using System.Collections;

public class ListOfPlayersScript : MonoBehaviour {

	public GUIText guiText;
	public ArrayList playerNames;


	// Use this for initialization
	void Start () {
		guiText.text = "Players Online:";
		playerNames = new ArrayList();
	}

	// Update is called once per frame
	void Update () {

	}

	public void refreshPlayerList()
	{
		string s = "Players Online:";
		foreach (string name in playerNames) 
			s += System.Environment.NewLine + name;
		guiText.text = s;
	}

	public void addPlayer(string playerName)
	{
		playerNames.Add (playerName);
		playerNames.Sort ();
		refreshPlayerList ();
	}

	public void removePlayer(string playerName)
	{
		playerNames.Remove (playerName);
		refreshPlayerList ();
	}
}
