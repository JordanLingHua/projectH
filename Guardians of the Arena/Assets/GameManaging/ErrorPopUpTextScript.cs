using UnityEngine;
using System.Collections;

public class ErrorPopUpTextScript : MonoBehaviour {
	
	public IEnumerator showText(string affect, Color newColor){
		GUI.depth = -1;
		this.GetComponent<GUIText>().text = affect;
		this.GetComponent<GUIText>().color = newColor;
		Color temp = this.GetComponent<GUIText>().material.color;
		yield return new WaitForSeconds(2.0f);
		for (int i =0; i < 60; i ++){
			temp.a -= 0.016f;
			this.GetComponent<GUIText>().material.color = temp;
			yield return new WaitForSeconds(0.016666f);
		}
		Destroy (this.gameObject);
	}
}
