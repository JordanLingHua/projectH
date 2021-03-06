﻿using UnityEngine;
using System.Collections;

public class Page {
	public string name;
	public PlayerSetup playerSetup;
	public ArrayList onBoardPieces;
	public ArrayList offBoardPieces;
	// Use this for initialization
	public Page () {
		playerSetup = GameObject.Find("PlayerSetup").GetComponent<PlayerSetup>();
		onBoardPieces = new ArrayList(playerSetup.boardCapacity);
		offBoardPieces = new ArrayList(playerSetup.maxUnitCount);
	}
}
