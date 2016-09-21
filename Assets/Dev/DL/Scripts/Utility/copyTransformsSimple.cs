using UnityEngine;
using System.Collections;

public class copyTransformsSimple :MonoBehaviour {

	public GameObject copyTransformsFrom;

	void Update(){
		Track (this.gameObject, copyTransformsFrom);
	}

	public void Track (GameObject a, GameObject b) {
		a.transform.position = b.transform.position;
		a.transform.localScale = b.transform.lossyScale;
		a.transform.eulerAngles = b.transform.eulerAngles;
	}

}
