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

	public void makeGameAction(Unit u)
	{
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

		List<TileScript> forwardMoveTiles  = new List<TileScript>();

		foreach (TileScript t in possibleMoveTiles)
		{
			if (t.y > toMove.GetComponentInParent<TileScript>().x)
				forwardMoveTiles.Add (t);
		}

		List<TileScript> finalMoveTiles = new List<TileScript> ();
		if (forwardMoveTiles.Count == 0) 
			finalMoveTiles = possibleMoveTiles;				
		else
			finalMoveTiles = forwardMoveTiles;

		int tileIndex = (int)(rand.NextDouble() * finalMoveTiles.Count);
		Debug.Log ("tileindex: " + tileIndex);
		//select a random move tile
		TileScript destinationTile = finalMoveTiles[tileIndex + 1];

		Debug.Log ("destX: " + destinationTile.x + "destY: " + destinationTile.y);
		//send packet
		gp.returnSocket().SendTCPPacket("move\\" + gameManager.selectedUnit.unitID+ "\\" + destinationTile.x + "\\" + destinationTile.y);
		print ("The AI sent a move packet");

		wait(3);

		///////////////CHECK IF CAN ATTACK ANY ENEMY///////////////////
		TileScript enemyInRange = checkForEnemyInRange (toMove);
		if (toMove.atkCost <= gameManager.pMana && enemyInRange != null) 
		{
			gp.returnSocket().SendTCPPacket("attack\\" + gameManager.selectedUnit.unitID+ "\\" + enemyInRange.x + "\\" + enemyInRange.y);
			print ("The AI sent an attack packet");
		}

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

	TileScript checkForEnemyInRange(Unit attacker)
	{
		gameManager.gs = gameManager.gameState.playerAtk;
		attacker.selectUnit ();
		List<TileScript> attackTiles = new List<TileScript> ();
		attackTiles = attacker.getAtkAccessibleTiles ();
		foreach (TileScript t in attackTiles) {
			if(t.objectOccupyingTile != null && t.objectOccupyingTile.GetComponent<Unit>() != null && t.objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerOne)
				return t;
		}
		return null;
	}

	IEnumerator wait(int sec){
		yield return new WaitForSeconds(sec);
	}


	// Update is called once per frame
	void Update () {
	
	}
}
