using UnityEngine;
using System.Collections;

public class TestText : MonoBehaviour {
	public GameObject levelText;
	public Texture2D level2Symbol,level3Symbol;
	void Start () {
		level2Symbol = Resources.Load("Level2Symbol") as Texture2D;
//			levelText = GameObject.Find("LevelText");
//			Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
//			pos.x = pos.x / Screen.width;
//			pos.y = pos.y / Screen.height;
//			GameObject obj = GameObject.Instantiate(levelText, pos, Quaternion.identity) as GameObject;
//			obj.GetComponent<GUIText>().text = "Hi";
//			obj.transform.parent = this.transform;
	}

	void OnGUI(){
		Camera cam = Camera.main;

		Vector3 symbolPos = cam.WorldToScreenPoint(gameObject.transform.position);
		GUI.DrawTexture (new Rect(symbolPos.x-50, Screen.height - symbolPos.y-40,  25, 30),level2Symbol);
	}

	void Update () {
//		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
//		pos.x = pos.x / Screen.width;
//		pos.y = pos.y / Screen.height;
//		//this.transform.position = pos;
	}
}
