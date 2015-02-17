using UnityEngine;
using System.Collections;

public class DropDownMenu : MonoBehaviour {
	
	private bool showList = false;
	private int listEntry = 0;
	private GUIContent[] list;
	private GUIStyle listStyle;
	public int tempX, tempY;
	private int clickCount = 0;
	public GUISkin mySkin;
	MainMenuGUI mmg;
	
	public PageNumberScript pageNumber;
	public PageNameScript pageNameScript;
	public Popup popup;
	
	void Start () {
		tempX = 203;
		tempY = 262;
		pageNumber = GameObject.Find("PageInfo").GetComponent<PageNumberScript>();
		pageNameScript = GameObject.Find("PageInfo").GetComponent<PageNameScript>();
		mmg = GameObject.Find ("MainMenuGUI").GetComponent<MainMenuGUI> ();
		// Make some content for the popup list
		list = new GUIContent[5];
		list [0] = new GUIContent ("1");
		list [1] = new GUIContent ("2");
		list [2] = new GUIContent ("3");
		list [3] = new GUIContent ("4");
		list [4] = new GUIContent ("5");

		// Make a GUIStyle that has a solid white hover/onHover background to indicate highlighted items
		listStyle = new GUIStyle();
		listStyle.normal.textColor = Color.black;

		Texture2D bg = new Texture2D(2, 2);
		Color[] colors = new Color[4];
		//Color color;
		for (int i = 0; i < colors.Length; i++) 
		{
			colors[i] = new Color ((210f / 255f), (159f / 255f), (104f / 255f));
		}
		bg.SetPixels(colors);
		bg.Apply();
		listStyle.normal.background = bg;


		Texture2D tex = new Texture2D(2, 2);
		colors = new Color[4];
		for (int i = 0; i < colors.Length; i++) 
		{
			colors[i] = new Color((232f / 255f),(200f/255f),(152f/255f));
		}
		tex.SetPixels(colors);
		tex.Apply();
		listStyle.hover.background = tex;
		listStyle.onHover.background = tex;
		listStyle.padding.left = listStyle.padding.right = listStyle.padding.top = listStyle.padding.bottom = 4;
	}
	
	void OnGUI () {

		GUI.skin = mySkin;

		GUI.depth = 0;
		if (mmg.showGUI && !mmg.challengePending)
		if (Popup.List (new Rect(mmg.windowRect2.x + tempX, mmg.windowRect2.y + tempY, 105, 25), ref showList,ref  listEntry, new GUIContent("Choose Setup!"), list, listStyle)) {
			pageNumber.selectedPage = listEntry + 1;
			//UnityEngine.Debug.Log("listEntry: "+ listEntry);
		}
		
		//GUI.Label (new Rect(50, 70, 400, 20), "Current Setup: " + pageNumber.selectedPage);
		
	}

	void Update()
	{
		updatePageNames ();
	
	}

	void updatePageNames()
	{
		for (int i = 0; i < 5; i++)
			list[i] = new GUIContent(pageNameScript.pages[i]);
	}
}
