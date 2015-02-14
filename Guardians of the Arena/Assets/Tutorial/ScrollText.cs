using UnityEngine;
using System.Collections;

public class ScrollText : MonoBehaviour {
	int maxCombatLogMessages = 15;
	int count;
	string combatLogText;
	public GUISkin mySkin;
	Vector2 combatLogScrollPos;
	private Rect combatLogWindowRect;
	int combatLogWidth = 400;
	int combatLogHeight = 250;
	ArrayList combatLogMessages = new ArrayList();

	void Start () {
		count = 0;
		combatLogText = "";
		combatLogWindowRect = new Rect (Screen.width-combatLogWidth,Screen.height-combatLogHeight+20, combatLogWidth, combatLogHeight);
		combatLogScrollPos.y = Mathf.Infinity;
	}


	void combatLogWindow (int windowID) 
	{
		GUILayout.BeginVertical ();
		GUILayout.Space (8);
		
		GUILayout.Label ("Combat Log");
		GUILayout.Label("", "Divider");
		
		
		GUILayout.BeginHorizontal ();
			combatLogScrollPos = GUILayout.BeginScrollView (combatLogScrollPos , false, true);
			GUILayout.Label (combatLogText, "PlainText");
			GUILayout.EndScrollView ();

		GUILayout.EndHorizontal ();
		//GUILayout.Space (8);

		GUILayout.EndVertical();
	}

	public void addLogToCombatLog(string log){
		combatLogMessages.Add(log+"\n");
		if (combatLogMessages.Count > maxCombatLogMessages){
			combatLogMessages.RemoveAt(maxCombatLogMessages);
		}
		combatLogText = "";
		for (int i = 0; i < combatLogMessages.Count; i ++){
			combatLogText += combatLogMessages[i];
		}
		combatLogScrollPos.y = Mathf.Infinity;
	}

	public void showCenterMessage(string message){
		GUI.depth = -1;
		GameObject text = (GameObject) Instantiate(GameObject.Find ("CenterPopUpText"),GameObject.Find ("CenterPopUpText").transform.position,Quaternion.identity);
		text.GetComponent<CenterPopUpTextScript> ().StartCoroutine (text.GetComponent<CenterPopUpTextScript> ().showText (message));
	}

	void OnGUI(){
		GUI.skin = mySkin;
		combatLogWindowRect = GUI.Window (2, combatLogWindowRect, combatLogWindow, "");
		GUI.BeginGroup (new Rect (0, 0, 100, 100));
		GUI.EndGroup ();
		if (GUI.Button (new Rect (Screen.width * 0.335f, Screen.height - ((float)Screen.height * 0.105f), Screen.width * 0.15f, (Screen.height - ((float)Screen.height * 0.905f))), "(E)nd Turn")) {
			addLogToCombatLog("count: " + count);
			count++;
			showCenterMessage("Your Turn");
		}
	}


	void Update () {
	
	}
}
