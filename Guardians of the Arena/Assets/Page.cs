using UnityEngine;
using System.Collections;

public class Page {

	public PlayerSetup playerSetup;
	public bool modified;
	public ArrayList onBoardPieces;
	public ArrayList offBoardPieces;


	// Use this for initialization
	public Page () {
		playerSetup = GameObject.Find("PlayerSetup").GetComponent<PlayerSetup>();
		modified = false;
		onBoardPieces = new ArrayList(playerSetup.boardCapacity);
		offBoardPieces = new ArrayList(playerSetup.boardCapacity);
	}
	
	// Update is called once per frame
	void Update () {
		//
	
	}
}
