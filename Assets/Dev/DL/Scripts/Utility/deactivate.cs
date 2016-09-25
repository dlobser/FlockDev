using UnityEngine;
using System.Collections;

public class deactivate : MonoBehaviour {

	public GameObject[] parts;
	public GameObject Mask;
	bool hasBeenSet = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!Mask.activeInHierarchy && !hasBeenSet) {
			for (int i = 0; i < parts.Length; i++) {
				parts [i].SetActive (false);
			}
			hasBeenSet = true;
		}
	}
}
