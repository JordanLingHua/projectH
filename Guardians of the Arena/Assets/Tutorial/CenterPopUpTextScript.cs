using UnityEngine;
using System.Collections;

public class CenterPopUpTextScript : MonoBehaviour {

	public IEnumerator showText(string affect){
		GUI.depth = -1;
		this.GetComponent<GUIText>().text = affect;
		Color temp = this.GetComponent<GUIText>().material.color;
		yield return new WaitForSeconds(0.5f);
		for (int i =0; i < 25; i ++){
			temp.a -= 0.04f;
			this.GetComponent<GUIText>().material.color = temp;
			yield return new WaitForSeconds(0.04f);
		}
		Destroy (this.gameObject);
	}
}
