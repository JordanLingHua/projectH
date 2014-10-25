using UnityEngine;
using System.Collections;

public class globalChatScript : MonoBehaviour {

	public ArrayList chatLines;
	public GUIText gChat;
	int maxLinesToShow;

	// Use this for initialization
	void Start () {
		chatLines = new ArrayList();
		gChat.text = "";
		maxLinesToShow = 10;

	}
	
	// Update is called once per frame
	void Update () {


	
	}

	public void addLineToChat(string userName, string lineToAdd)
	{
		gChat.text = "";

		chatLines.Add (lineToAdd);

		for (int i = chatLines.Count - maxLinesToShow <= 0 ? 0 : chatLines.Count - maxLinesToShow; i < chatLines.Count; i++)
		{
			gChat.text += userName + ": " + chatLines[i] + "\n";
		}
	}
	
}
