using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AIScript : MonoBehaviour {

	GameManager gameManager;
	bool firstMove;
	//GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	GameProcess gp;
	//public List<Unit> gameManager.units;
	public System.Random rand;
	// Use this for initialization
	void Start () {
	//	gameManager.units = new List<Unit>();
		rand = new System.Random ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		gp = GameObject.Find ("GameProcess").GetComponent<GameProcess> ();
	}

//	void Awake()
//	{
//		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
//		foreach (int key in gameManager.units.Keys) 
//		{
//			if (gameManager.units [key].alleg == Unit.allegiance.playerTwo) 
//			{
//				gameManager.units.Add (gameManager.units [key]);
//			}
//		}
//	}

	public void makeGameAction(Unit u)
	{
		//Unit toMove = checkForValidGameAction ();
		//if (toMove != null)
		//{
			//select a random unit
		Unit toMove;
		if (firstMove) 
		{
			int index = (int)(rand.NextDouble () * 10 + 1);
			firstMove = false;
			Debug.Log ("index is: " + index);
			Debug.Log ("indexModified is: " + (index + 10));
			toMove = gameManager.units [(index + 10)];
		} 
		else 
		{
			toMove = u;
		}

		toMove.selectUnit ();
		HashSet<TileScript> possMvTile = toMove.getMvAccessibleTiles (toMove.alleg);
		List<TileScript> possibleMoveTiles = new List<TileScript>(possMvTile);
		

		//int tileIndex = (int)(rand.NextDouble() * gameManager.accessibleTiles.Count);
		int tileIndex = (int)(rand.NextDouble() * possibleMoveTiles.Count);
		Debug.Log ("tileindex: " + tileIndex);
		//select a random move tile
		TileScript destinationTile = possibleMoveTiles[tileIndex + 1];

		Debug.Log ("destX: " + destinationTile.x + "destY: " + destinationTile.y);
		//send packet
		gp.returnSocket().SendTCPPacket("move\\" + gameManager.selectedUnit.unitID+ "\\" + destinationTile.x + "\\" + destinationTile.y);
		print ("The AI sent a move packet");

		wait(3);

		Unit next = checkForValidGameAction ();
		if (next != null)
				makeGameAction (next);
		else 
		{
			firstMove = true;
			gp.returnSocket ().SendTCPPacket ("endTurn");
		}
	}

	public Unit checkForValidGameAction()
	{
		if (gameManager.pMana > 0) 
		{
			foreach (int key in gameManager.units.Keys)
			{
				Unit u = gameManager.units[key];
				if (u.mvCost <= gameManager.pMana && !u.mvd && u.alleg == Unit.allegiance.playerTwo)
				{
					return u; 
					break;
				}

			}
			return null;
		}
		else 
		{
			return null;
		}
	}

	IEnumerator wait(int sec){
		yield return new WaitForSeconds(sec);
	}



//	01 function alphabeta(node, depth, α, β, maximizingPlayer)
//	02      if depth = 0 or node is a terminal node
//	03          return the heuristic value of node
//	04      if maximizingPlayer
//	05          v := -∞
//	06          for each child of node
//	07              v := max(v, alphabeta(child, depth - 1, α, β, FALSE))
//	08              α := max(α, v)
//	09              if β ≤ α
//	10                  break (* β cut-off *)
//	11          return v
//	12      else
//	13          v := ∞
//	14          for each child of node
//	15              v := min(v, alphabeta(child, depth - 1, α, β, TRUE))
//	16              β := min(β, v)
//	17              if β ≤ α
//	18                  break (* α cut-off *)
//	19          return v

	//int abMiniMax (


	// Update is called once per frame
	void Update () {
	
	}
}
