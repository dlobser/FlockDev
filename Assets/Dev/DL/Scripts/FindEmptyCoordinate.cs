using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FindEmptyCoordinate : MonoBehaviour {

	public Vector2 highBound;
	public Vector2 lowBound;
	public Vector2 divisions;
	public float newHeight;
	int amount;
	public GameObject obj;
	public GameObject actorParent;
	List<GameObject> boxes;
	public GameObject indicator;

	public Vector3 emptyCoordinate;

	// Use this for initialization
	void Start () {
		float X = highBound.x - lowBound.x;
		float Y = highBound.y - lowBound.y;
		amount = (int)( divisions.x * divisions.y);
		boxes = new List<GameObject> ();
		for (int i = 0; i < divisions.x; i++) {
			for (int j = 0; j < divisions.y; j++) {
				GameObject g = Instantiate (obj);
				g.transform.parent = this.transform;
				g.transform.localScale =  new Vector3 (X/divisions.x, 50, Y/divisions.y);
				g.transform.localPosition = new Vector3 (
					(i * (X/divisions.x)) + lowBound.x + ((X/divisions.x)*.5f), 0, 
					(j * (Y/divisions.y)) + lowBound.y + ((X/divisions.x)*.5f));
				boxes.Add (g);
			}
		}
		StartCoroutine (Finder ());
	}


	public IEnumerator Finder(){

		while (true) {
			int allEmpty = 0;
			for (int i = 0; i < amount; i++) {
				int randI = (int)(Random.value * amount);
				bool empty = true;

				for (int j = 0; j < actorParent.transform.childCount; j++) {
					int randJ = j;
					if (checkBounds( actorParent.transform.GetChild (randJ).transform.position,boxes [randI]))	 {
						empty = false;
					
					}
				}

				if (empty) {
					Vector3 pos = boxes [randI].transform.position;
					Vector3 size = boxes [randI].transform.localScale * .5f;
					emptyCoordinate.Set (
						Random.Range (pos.x - size.x, pos.x + size.x), newHeight,
						Random.Range (pos.z - size.z, pos.z + size.z));
					indicator.transform.position = emptyCoordinate;
				} 
				else
					allEmpty++;
				yield return new WaitForSeconds (0);
			}
			if (allEmpty == amount) {
				int randI = (int)(Random.value * amount);
				Vector3 pos = boxes [randI].transform.position;
				Vector3 size = boxes [randI].transform.localScale * .5f;
				emptyCoordinate.Set (
					Random.Range (pos.x - size.x, pos.x + size.x), newHeight,
					Random.Range (pos.z - size.z, pos.z + size.z));
				indicator.transform.position = emptyCoordinate;

			}
		}
	}

	bool checkBounds(Vector3 check, GameObject box){
		Vector3 pos = box.transform.localPosition;
		Vector3 size = box.transform.localScale * .5f;
		if (check.x > pos.x - size.x && check.x < pos.x + size.x &&
		    check.z > pos.z - size.z && check.z < pos.z + size.z) {
			return true;
		} else
			return false;
	}

}
