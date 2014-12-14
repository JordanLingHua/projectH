using UnityEngine;
using System.Collections;

public class popUpTextScript : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	public IEnumerator showText(Unit unitAffected,string affect, Color newColor){
		GUI.depth = -1;
		Vector3 pos = this.transform.position;

		this.GetComponent<GUIText>().text = affect;
		this.GetComponent<GUIText>().color = newColor;
		Color temp = this.GetComponent<GUIText>().material.color;
		for (int i =0; i < 40; i ++) {
			pos.y += 0.0005f;
			this.transform.position = pos;
			yield return new WaitForSeconds (0.016666f);
		}
		for (int i =0; i < 60; i ++){
			pos.y += 0.0005f;
			this.transform.position = pos;
			temp.a -= 0.016f;
			this.GetComponent<GUIText>().material.color = temp;
			yield return new WaitForSeconds(0.016666f);
		}
		Destroy (this.gameObject);
		unitAffected.popUpTextNum--;
	}
	
	
}
