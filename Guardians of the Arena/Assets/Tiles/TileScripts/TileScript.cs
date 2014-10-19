using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

	public bool occupied = false;
	public GameObject environmentObject, cp;
		
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
		}else if (random < 20){
			GameObject unit = (GameObject)Instantiate(cp, 
											            new Vector3(this.transform.position.x, 0, this.transform.position.z), 
											            new Quaternion());
			unit.transform.parent = this.transform;
			if (random %2 == 0){
				//unit.GetComponent<Unit>().setUnitOneType();
				unit.GetComponent<Unit>().setUnitType(10);
			}else{
				//unit.GetComponent<Unit>().setUnitTwoType();
				unit.GetComponent<Unit>().setUnitType(11);
			}
			
			objectOccupyingTile = unit;
			this.occupied = true;
			renderer.material.color = Color.blue;
		}
		
		

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}





	
}
