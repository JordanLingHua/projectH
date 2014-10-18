using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public GUIText uInfo;
	
	void Start () {
		uInfo = GameObject.Find("UnitInfo").GetComponent<GUIText>();
	}
	
	void Update () {
	
	}

}
