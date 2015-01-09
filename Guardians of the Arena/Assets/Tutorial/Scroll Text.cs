using UnityEngine;
using System.Collections;

public class ScrollText : MonoBehaviour {
	Vector2 scrollPosition;
	void Start () {
		//code found from http://answers.unity3d.com/questions/278468/vertucally-scrolling-non-editable-text-area.html
		GUILayout.BeginArea (new Rect(50, 50, Screen.width-100, Screen.height-50));
		scrollPosition = GUILayout.BeginScrollView (scrollPosition, GUILayout.Width (Screen.width-100), GUILayout.Height (Screen.height-100));
		/*changes made in the below 2 lines */
		GUI.skin.box.wordWrap = true;     // set the wordwrap on for box only.
//		GUILayout.Box(rulesText);        // just your message as parameter.
		
		GUILayout.EndScrollView ();
		
		GUILayout.EndArea();
	}

	void Update () {
	
	}
}
