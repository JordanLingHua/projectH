using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

	public bool occupied = false;
	public GameObject environmentObject;
		
	public GameObject left, right, down, up;
	public GameObject objectOccupyingTile;

	// Use this for initialization
	void Start () {

		this.GetComponent<BaseObject>().type = BaseObject.ObjectType.Tile;
		int random = Random.Range (0, 100);

		if (random < 10) 
		{
			GameObject tree = (GameObject)Instantiate(environmentObject, 
											            new Vector3(this.transform.position.x, 0, this.transform.position.z), 
											            new Quaternion());
			tree.transform.parent = this.transform;
			objectOccupyingTile = tree;
			renderer.material.color = Color.red;
			this.occupied = true;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
