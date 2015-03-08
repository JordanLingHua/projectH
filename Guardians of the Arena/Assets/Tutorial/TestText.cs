using UnityEngine;
using System.Collections;

public class TestText : MonoBehaviour {

	public GUISkin mySkin;
	public Texture2D manaGUIBorder,manaGUIFill,manaGUIMask;
	int pMana, maxMana;
	float percentMana;
	public int manaBarXPos = 200,manaBarYPos = 200,manaBarWidth = 50,manaBarHeight = -100;
	void Start () {	
		manaBarXPos = 200;
		manaBarYPos = 200;
		manaBarWidth = 50;
		manaBarHeight = -100;

		manaGUIBorder = Resources.Load("manaGUIBorder") as Texture2D;
		manaGUIFill = Resources.Load("manaGUIFill") as Texture2D;
		manaGUIMask = Resources.Load("HPBarBG") as Texture2D;
		pMana = 2;
		maxMana = 2;
	}

	void OnGUI(){
		GUI.skin = mySkin; 
		percentMana = ((float)pMana / (float)maxMana);
		GUI.DrawTexture (new Rect(manaBarXPos,manaBarYPos ,manaBarWidth, manaBarHeight*percentMana * ((float)maxMana/8)),manaGUIFill);
		GUI.DrawTexture (new Rect(manaBarXPos,200,manaBarWidth, manaBarHeight),manaGUIBorder);
		float percentMana2 = ((float)8 - maxMana) / 8;
		if (maxMana != 8){
			GUI.DrawTexture (new Rect(manaBarXPos,manaBarYPos+manaBarHeight, manaBarWidth, (-manaBarHeight * percentMana2)),manaGUIMask);
		}



		if (GUI.Button(new Rect(0,0,100,50),"Mana decrease")){
			if (pMana > 0){
				pMana--;
			}

		}
		if (GUI.Button (new Rect (0, 60, 100, 50), "Mana max increase")) {
			maxMana +=2;
			pMana = maxMana;
		}
	}
	
}
