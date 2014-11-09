using UnityEngine;
using System.Collections;

public class SetupTileScript : MonoBehaviour {
	PlayerSetup ps;
	public int x,y;
	public GameObject up,down,left,right;


	public bool occupied;

	public enum TileType{ONFIELD, OFFFIELD};//values given to them from 0 to 1 by default, since this is an enum

	public TileType tt;


	void Start () {
		ps = GameObject.Find("PlayerSetup").GetComponent<PlayerSetup>();

		occupied = false;

	}





	void Update () {



	
	}



}
