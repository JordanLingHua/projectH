using UnityEngine;
using System.Collections;

public class SetupTileScript : MonoBehaviour {
	PlayerSetup ps;
	public int x,y;
	public GameObject up,down,left,right;
	void Start () {
		ps = GameObject.Find("PlayerSetup").GetComponent<PlayerSetup>();
	}





	void Update () {
	
	}



}
