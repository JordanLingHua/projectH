using UnityEngine;
using System.Collections;

public class globalChatScript : MonoBehaviour {

	public ArrayList chatLines;
	public string gChat;
	int maxLinesToShow;

	// Use this for initialization
	void Start () {
		chatLines = new ArrayList();
		gChat = string.Empty;
		maxLinesToShow = 50; //before chat begins to scroll up
	}
	
	// Update is called once per frame
	void Update () {	
	}

	//saves the user and their chat content in an arraylist
	public void addLineToChat(string userName, string chatLine)
	{
		gChat = string.Empty;
		chatLines.Add (userName +": " + chatLine);

		for (int i = chatLines.Count - maxLinesToShow <= 0 ? 0 : chatLines.Count - maxLinesToShow; i < chatLines.Count; i++)
			gChat += chatLines[i] + "\n";
	}
	
}
