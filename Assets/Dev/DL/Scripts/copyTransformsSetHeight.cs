using UnityEngine;
using System.Collections;

public class copyTransformsSetHeight :MonoBehaviour {

	public GameObject copyTransformsFrom;
	public float height;
	Vector3 pos;

	public bool getAverageHeight;
	public float avgHeight = 0;
	public int avgAmount = 10;
	public GameObject viewer;

	void Update(){
		if (getAverageHeight)
			height = getAvg ();
		Track (this.gameObject, copyTransformsFrom);
	}

	public float getAvg(){
		float h = viewer.transform.position.y;
		avgHeight = ((avgHeight * avgAmount) + h) / (avgAmount + 1);
		return avgHeight;
	}

	public void Track (GameObject a, GameObject b) {
		pos.x = b.transform.position.x;
		pos.z = b.transform.position.z;
		pos.y = height;
		a.transform.position = pos;
		a.transform.localScale = b.transform.lossyScale;
		a.transform.eulerAngles = b.transform.eulerAngles;
	}

}
