using UnityEngine;
using System.Collections;

//Our "struct" object that is stored in the array inside PlayerSetup.  

public class PieceStruct : MonoBehaviour {

	public GameObject piece_prefab;
	public Unit unitType;


	public PieceStruct(GameObject g, Unit u)
	{
		piece_prefab = g;
		unitType = u;
	}

	//public GameObject get_piece_prefab
	//Get the position of the piece simply by accessing the piece_prefab's position


	public string unitTypeName()
	{
		//Not implemented yet
		return "";
	}

	public string prefabName()
	{
		//Not implemented yet
		return "";
	}


	/*
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	*/
}
