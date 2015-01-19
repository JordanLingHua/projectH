using UnityEngine;
using System.Collections;

public class challengeScript : MonoBehaviour {

	public string challengeText;
	bool showChallengeOptions;
	GameProcess gp;
	ArrayList challengeRequests;
	string challenger;
	// Use this for initialization
	void Start () {
		showChallengeOptions = false;
		gp = GameObject.Find("GameProcess").GetComponent<GameProcess>();
		challengeRequests = new ArrayList ();
		challenger = string.Empty;
	}

	void OnGUI()	
	{
		if (showChallengeOptions) 
		{
			if(GUI.Button(new Rect(Screen.width / 2 - 40, 150, 80, 20), "Accept"))
			{
				gp.returnSocket().SendTCPPacket("acceptChallenge");
				showChallengeOptions = false;
			}

			if(GUI.Button(new Rect(Screen.width / 2 - 40, 150, 80, 20), "Decline"))
			{
				gp.returnSocket().SendTCPPacket("declineChallenge");
				showChallengeOptions = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!showChallengeOptions) 
		{
			if (challengeRequests.Count > 0)
			{
				challenger = (string)challengeRequests.GetEnumerator().Current;
				challengeText = challenger + " has challenged you! Do you accept?";
				showChallengeOptions = true;
			}
			else
				challengeText = string.Empty;
		}	
	}

	public void addChallengeRequest(string challengeSender)
	{
		if(challengeRequests.Contains(challengeSender))
			challengeRequests.Add(challengeSender);
	}

	public void removeChallengeRequest(string challengeCanceller)
	{
		if (!challenger.Equals (challengeCanceller))
			challengeRequests.Remove (challengeCanceller);
		else 
		{
			challengeText = challengeCanceller + " has cancelled the challenge";
			showChallengeOptions = false;
		}

	}
}
