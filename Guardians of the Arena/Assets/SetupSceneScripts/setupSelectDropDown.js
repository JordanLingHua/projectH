

private var showList = false;
private var listEntry = 0;
private var list : GUIContent[];
private var listStyle : GUIStyle;
private var clickCount = 0;

public var pageNumber;
public var popup;
 
function Start () {
	pageNumber = GameObject.Find("PageNumber").GetComponent("PageNumberScript");
	
	// Make some content for the popup list
	list = new GUIContent[5];
	list[0] = new GUIContent("1");
	list[1] = new GUIContent("2");
	list[2] = new GUIContent("3");
	list[3] = new GUIContent("4");
	list[4] = new GUIContent("5");
 
	// Make a GUIStyle that has a solid white hover/onHover background to indicate highlighted items
	listStyle = new GUIStyle();
	listStyle.normal.textColor = Color.white;
	var tex = new Texture2D(2, 2);
	var colors = new Color[4];
	for (color in colors) color = Color.white;
	tex.SetPixels(colors);
	tex.Apply();
	listStyle.hover.background = tex;
	listStyle.onHover.background = tex;
	listStyle.padding.left = listStyle.padding.right = listStyle.padding.top = listStyle.padding.bottom = 4;
}
 
function OnGUI () {
	if (Popup.List (Rect(50, 100, 100, 20), showList, listEntry, GUIContent("Choose Setup!"), list, listStyle)) {
		pageNumber.selectedPage = System.Int32.Parse(list[listEntry].text);
	}
	
	GUI.Label (Rect(50, 70, 400, 20), "Current Setup: " + pageNumber.selectedPage);
	
}