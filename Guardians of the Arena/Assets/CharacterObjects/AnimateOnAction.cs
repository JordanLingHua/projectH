using UnityEngine;
using System.Collections;


public class AnimateOnAction : MonoBehaviour {

	public string sheetname;
	private Sprite[] sprites;
	private SpriteRenderer sr;
	private string[] names;

	// Use this for initialization
	void Start () {

		sprites = (UnityEngine.Sprite[])Resources.LoadAll(sheetname); 
		sr = GetComponent<SpriteRenderer> ();
		names = new string[sprites.Length];
		
		for(int i = 0; i < names.Length; i++) 
		{
			names[i] = sprites[i].name;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	void ChangeSprite( int index )
	{
		Sprite sprite = sprites[index];
		sr.sprite = sprite;
	}
	
	void ChangeSpriteByName( string name )
	{
		Sprite sprite = sprites[getIndexOf(names, name)];
		sr.sprite = sprite;
	}


	int getIndexOf(string[] names, string value)
	{

		for(int i = 0; i < names.Length; i++)
		{
			if(value == names[i])
			{
				return i;
			}
		}

		//If no match found
		return 0;

	}

}
