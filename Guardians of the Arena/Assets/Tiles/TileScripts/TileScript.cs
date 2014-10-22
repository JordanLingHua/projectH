using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {
	
	//Describes what is occupying the tile, 0 for empty, 1 for a friendly unit, 2 for enemy / shrubbery
	public int occupied = 0;
	public GameObject environmentObject, cp;
		
	public GameObject left, right, down, up;
	public GameObject objectOccupyingTile;
	public int x,y;
	
	GameManager gm;
	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		
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
			
			this.occupied = 2;
			
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
			this.occupied = 1;
			renderer.material.color = Color.blue;
		}
	}
	
	void Update () {
	
	}
	
	void OnMouseDown(){
		gm.tsx = x;
		gm.tsy = y;
		
		//move unit selected to this tile if it can access it
		if (gm.accessibleTiles.Contains(gameObject)){
			gm.selectedUnit.GetComponent<Unit>().mvd = true;
			gm.selectedUnit.transform.parent.GetComponent<TileScript>().occupied = 0;
			gm.selectedUnit.transform.parent = gameObject.transform;
			Vector3 newPos = new Vector3(this.transform.position.x,0,this.transform.position.z);
			gm.selectedUnit.transform.position = newPos;
			gm.accessibleTiles.Clear();
			this.occupied = 1;
			this.transform.parent.GetComponent<TileManager>().clearAllTiles();
			renderer.material.color = Color.blue;
		}
	}




	
}
