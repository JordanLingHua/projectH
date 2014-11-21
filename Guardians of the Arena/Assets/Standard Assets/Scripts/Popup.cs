using UnityEngine;

public class Popup {
	static bool almostDone = false;
	static int popupListHash = "PopupList".GetHashCode();
	
	public static bool List (Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent, GUIContent[] listContent,
	                         GUIStyle listStyle) {
		return List(position, ref showList, ref listEntry, buttonContent, listContent, "button", "box", listStyle);
	}
	
	public static bool List (Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent, GUIContent[] listContent,
	                         GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle) {
		int controlID = GUIUtility.GetControlID(popupListHash, FocusType.Passive);
		bool done = false;

		switch (Event.current.GetTypeForControl(controlID)) {
		case EventType.mouseDown:
			if (position.Contains(Event.current.mousePosition)) {
				GUIUtility.hotControl = controlID;
				if(!almostDone)
					showList = true;
			}
			break;
		case EventType.mouseUp:
			if (showList) {
				if (!almostDone)
				almostDone = true;
				else{
					done = true;
					almostDone = false;}
			}
			break;
		}
		
		GUI.Label(position, buttonContent, buttonStyle);
		if (showList) {
			Rect listRect = new Rect(position.x, position.y, position.width, listStyle.CalcHeight(listContent[0], 1.0f)*listContent.Length);
			GUI.Box(listRect, "", boxStyle);
			listEntry = GUI.SelectionGrid(listRect, listEntry, listContent, 1, listStyle);
		}
		if (done) {
			showList = false;

		}
		return done;
	}
}