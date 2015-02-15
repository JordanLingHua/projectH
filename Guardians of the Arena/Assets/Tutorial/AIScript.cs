using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AIScript : MonoBehaviour {

	GameManager gameManager;
	public bool serverResponded;
	private float actionDelay;
	bool firstMove;
	GameProcess gp;
	public System.Random rand;
	List<Unit> AIUnits;
	List<Unit> targetUnits;
	List<Unit> obstacles;
	List<Unit> playerUnits;
	// Use this for initialization

	void Start () {
		firstMove = true;
		rand = new System.Random (Guid.NewGuid().GetHashCode());
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		gp = GameObject.Find ("GameProcess").GetComponent<GameProcess> ();
		AIUnits = new List<Unit>();		
		targetUnits = new List<Unit>();	
		obstacles = new List<Unit>();	
		playerUnits = new List<Unit>();	
		actionDelay = 2.0f;
	}

	public IEnumerator makeGameAction(Unit u)
	{
		AIUnits.Clear ();
		foreach (Unit x in gameManager.units.Values)
		{
			if(x.alleg == Unit.allegiance.playerTwo)
				AIUnits.Add(x);
		}

		targetUnits.Clear ();

		playerUnits.Clear ();
		foreach (Unit x in gameManager.units.Values)
		{
			if(x.alleg == Unit.allegiance.playerOne)
				playerUnits.Add(x);
		}

		obstacles.Clear ();
		foreach (Unit x in gameManager.units.Values)
		{
			if(x.alleg == Unit.allegiance.neutral)
				obstacles.Add(x);
		}

		Unit toMove;
		if (firstMove) 
		{			
			firstMove = false;
	
			while (true) {
				toMove = AIUnits [(int)(rand.NextDouble () * AIUnits.Count)];
				if (toMove.unitType != 11)
					break;
			}
		} 

		else 
		{
			toMove = u;
		}

		///////////////CHECK IF SELECTED UNIT CAN ATTACK A TARGET///////////////////
		toMove.selectUnit ();
		gameManager.gs = GameManager.gameState.playerAtk;
		TileScript tileOfTarget = checkForTargetInRange (toMove);
		
		if (toMove.atkCost <= gameManager.pMana && tileOfTarget != null && !toMove.atkd) {
			gp.returnSocket ().SendTCPPacket ("attack\\" + gameManager.selectedUnit.unitID + "\\" + tileOfTarget.x + "\\" + tileOfTarget.y);
			print ("The AI sent an attack packet");
			//print ("attacking: " +tileOfTarget.x + "  " + tileOfTarget.y);
			yield return new WaitForSeconds(actionDelay);
		} 

		//NO TARGETS WERE IN RANGE, MOVING TOWARDS CLOSEST TARGET
		else {
			toMove.selectUnit ();
			HashSet<TileScript> possMvTile = toMove.getMvAccessibleTiles (toMove.alleg);
			List<TileScript> possibleMoveTiles = new List<TileScript> (possMvTile);

			Unit closestTarget = getClosestTarget (toMove);
			if (closestTarget != null)
				Debug.Log("closest target is not null and it is: " + closestTarget.name);
			else
				Debug.Log ("closest target is NULL!");

			if (closestTarget != null)
			{
				TileScript destinationTile = getTileClosestToNearestTarget (possibleMoveTiles, closestTarget);

				//send packet
				gameManager.gs = GameManager.gameState.playerMv;
				gp.returnSocket ().SendTCPPacket ("move\\" + gameManager.selectedUnit.unitID + "\\" + destinationTile.x + "\\" + destinationTile.y);
				print ("The AI sent a move packet");

			
				yield return new WaitForSeconds(actionDelay);

				//RECHECK THE ATTACK SINCE UNIT IS ON NEW TILE
				gameManager.gs = GameManager.gameState.playerAtk;
				tileOfTarget = checkForTargetInRange (toMove);
				
				if (tileOfTarget != null)
					print ("TARGET FOUND AFTER MOVE: " +tileOfTarget.x + "  " + tileOfTarget.y);
				else
					print ("NO TARGET FOUND AFTER MOVE");
				
				if (toMove.atkCost <= gameManager.pMana && tileOfTarget != null && !toMove.atkd) {
					gp.returnSocket ().SendTCPPacket ("attack\\" + gameManager.selectedUnit.unitID + "\\" + tileOfTarget.x + "\\" + tileOfTarget.y);
					print ("The AI sent an attack packet");
					yield return new WaitForSeconds(actionDelay);
				}
			}
		}

		Unit next = checkForValidGameAction ();
		//print ("next unit up1 : " + next.name);
		if (next != null)
			StartCoroutine(makeGameAction (next));
		else 
		{
			yield return new WaitForSeconds (actionDelay);
			firstMove = true;
			gp.returnSocket ().SendTCPPacket ("endTurn");		
		}
	}


	public Unit checkForValidGameAction()
	{
		List<Unit> readyUnits = new List<Unit>();
		//print ("MANA: " + gameManager.pMana);
		if (gameManager.pMana > 0) 
		{
			foreach (Unit u in AIUnits)
			{
				//if (u.atkCost <= gameManager.pMana && !u.atkd && u.unitType != 11)
				//{
				//	readyUnits.Add (u); 
				//}

				if (u.mvCost <= gameManager.pMana && !u.mvd && u.unitType != 11)
				{
					readyUnits.Add (u);
				}
			}
			if (readyUnits.Count == 0)
				return null;
			else
				return readyUnits[(int)(rand.NextDouble () * readyUnits.Count)];
		}
		else 
		{
			return null;
		}
	}

	Unit getClosestTarget(Unit toMove){
		Unit closestTarget = null;
		float tempDistance = 50;
		float minDistance = 50;
		TileScript toMoveTileScript = toMove.GetComponentInParent<TileScript> ();

		switch (toMove.unitType) {
				//Swordsmen attack barrels and enemies
				case 7:
					foreach (Unit u in gameManager.units.Values) {
						if ( u != null && u.GetComponent<Unit> ().alleg != Unit.allegiance.playerTwo && u.GetComponent<Unit>().unitType != 21) {
							//targetUnits.Add(t.objectOccupyingTile.GetComponent<Unit>());
							targetUnits.Add (u);
						}
					}
				break;
				
				//All other combat units only attack enemies
				case 3:
				case 1:
				case 10:
					foreach (Unit u in gameManager.units.Values) {
						if ( u != null && u.GetComponent<Unit> ().alleg == Unit.allegiance.playerOne) {
							//targetUnits.Add(t.objectOccupyingTile.GetComponent<Unit>());
							targetUnits.Add (u);
						}
					}
				break;
						
			
				//healer targets lowest health ally
				case 8:
					foreach (Unit u in gameManager.units.Values) {
						if (u != null && u.GetComponent<Unit> ().alleg == Unit.allegiance.playerTwo && u.GetComponent<Unit>().unitType != 11 
				  			  && u.GetComponent<Unit>().unitType != 8 && u.hp < u.maxHP)
							targetUnits.Add (u);
					}

					//no injured units, just go to nearby unit
					if (targetUnits.Count == 0)
					foreach (Unit u in gameManager.units.Values) {
						if (u != null && u.GetComponent<Unit> ().alleg == Unit.allegiance.playerTwo && u.GetComponent<Unit>().unitType != 11
				   		 && u.GetComponent<Unit>().unitType != 8)
							targetUnits.Add (u);
					}
				break;

				case 2:
					foreach (Unit u in gameManager.units.Values) {
					if (u != null && u.GetComponent<Unit> ().alleg == Unit.allegiance.playerTwo && u.GetComponent<Unit>().unitType != 11
				    && u.GetComponent<Unit>().unitType != 2 && toMove.GetComponent<Mystic>().unitFocused == null)
							targetUnits.Add (u);
					}
				break;
				

				default:
						Debug.Log ("i am rekt m8");
						break;
		
				}
				
				foreach (Unit p in targetUnits) 
				{
					TileScript pTileScript = p.GetComponentInParent<TileScript> ();
					tempDistance = Math.Abs (pTileScript.x - toMoveTileScript.x) + Math.Abs (pTileScript.y - toMoveTileScript.y);

					if (tempDistance < minDistance)
					{
						closestTarget = p;
						minDistance = tempDistance;
					}
				}
				
		
		return closestTarget;
	}
	
	TileScript getTileClosestToNearestTarget(List<TileScript> moveTiles, Unit closestTarget){
		float minDistance = 50;
		float tempDistance2;
		Debug.Log ("closestTarget: " + closestTarget.name);
		//Debug.Log (" tile: " + closestTarget.GetComponentInParent<TileScript>());
		TileScript targetLocation = closestTarget.GetComponentInParent<TileScript> ();
		TileScript destinationTile = null;
		foreach (TileScript t in moveTiles)
		{			
			tempDistance2 = Math.Abs(t.x - targetLocation.x) + Math.Abs(t.y - targetLocation.y);
			
			if (tempDistance2 < minDistance && t.GetComponent<TileScript>().objectOccupyingTile == null)
			{
				destinationTile = t;
				minDistance = tempDistance2;
			}
		}
		return destinationTile;
	}

	TileScript checkForTargetInRange(Unit attacker)
	{
		attacker.selectUnit ();

		HashSet<TileScript> attackTiles2 = attacker.getAtkAccessibleTiles ();
		List<TileScript> attackTiles = new List<TileScript> (attackTiles2);

		switch (attacker.unitType) 
		{
			//Units that attack the players units to deal damage
			case 7:
				foreach (TileScript t in attackTiles) {
					if(t.objectOccupyingTile != null && t.objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerOne)
				   		return t;

				else if(t.objectOccupyingTile != null && t.objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.neutral && t.objectOccupyingTile.GetComponent<Unit>().unitType == 20)
						return t;
				}
			break;

			case 3:
			case 1:
			case 10:
				foreach (TileScript t in attackTiles) {
					if(t.objectOccupyingTile != null && (t.objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerOne))				                                   
						return t;
				}

			break;

			//healer targets lowest health ally
			case 8:
				int mostMissing = 100;
				TileScript toHeal = null;
				foreach (TileScript t in attackTiles) {
					if(t.objectOccupyingTile != null && t.objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerTwo
					   && t.objectOccupyingTile.GetComponent<Unit>().unitType != 11)
					{
						int currentHP = t.GetComponentInChildren<Unit>().hp;
						int maxHP = t.GetComponentInChildren<Unit>().maxHP;
						if (currentHP < maxHP)
						{
							int healthMissing = Math.Abs(currentHP - maxHP);
							if ( healthMissing > mostMissing)
							{
								toHeal = t;
								mostMissing = healthMissing;
							}
						}
					}

				}
				return toHeal;
		
			break;

			//Mystic can target enemy or ally but wont move or attack while focusing
			case 2:
				foreach (TileScript t in attackTiles) {
					if(t.objectOccupyingTile != null && t.objectOccupyingTile.GetComponent<Unit>().alleg != Unit.allegiance.neutral 
					   && attacker.GetComponent<Mystic>().unitFocused == null && t.objectOccupyingTile.GetComponent<Unit>().unitType != 11)
						{
						   if(t.objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerTwo && !t.objectOccupyingTile.GetComponent<Unit>().mvd)
								return t;
							else if (t.objectOccupyingTile.GetComponent<Unit>().alleg == Unit.allegiance.playerOne)
								return t;
						
					}
				}
				return null;
			break;

		}

		return null;
	}

	IEnumerator wait(float sec){
		yield return new WaitForSeconds(sec);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
