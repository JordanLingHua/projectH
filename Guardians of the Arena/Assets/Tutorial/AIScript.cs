using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AIScript : MonoBehaviour {

	AIManager aiManager = GameObject.Find ("AIManager").GetComponent<AIManager> ();
	GameProcess gp = GameObject.Find ("GameProcess").GetComponent<GameProcess> ();
	public List<Unit> aiUnits;
	public System.Random rand;
	// Use this for initialization
	void Start () {
		aiUnits = new List<Unit>();
		rand = new System.Random ();
		foreach (int key in aiManager.units.Keys) 
		{
			if (aiManager.units [key].alleg == Unit.allegiance.playerTwo) 
			{
				aiUnits.Add (aiManager.units [key]);
			}
		}
	}

	public void makeGameAction()
	{
		if (checkForValidGameAction())
		{
			//select a random unit
			aiUnits[(int)(rand.NextDouble() * aiUnits.Count)].selectUnit();

			//select a random move tile
			TileScript destinationTile = aiManager.accessibleTiles[(int)(rand.NextDouble() * aiManager.accessibleTiles.Count)];

			//send packet
			gp.returnSocket().SendTCPPacket("move\\" + aiManager.selectedUnit.unitID+ "\\" + destinationTile.x + "\\" + destinationTile.y);
			print ("Sent move packet");

			wait(3);
			makeGameAction();

		}

		gp.returnSocket().SendTCPPacket("endTurn");
	}

	public bool checkForValidGameAction()
	{
		if (aiManager.pMana > 0) 
		{
			foreach (Unit u in aiUnits)
			{
				if (u.mvCost <= aiManager.pMana)
				{
					return true;
					break;
				}

			}
			return false;
		}
		else 
		{
			return false;
		}
	}

	IEnumerator wait(int sec){
		yield return new WaitForSeconds(sec);
	}


	// Update is called once per frame
	void Update () {
	
	}
}
