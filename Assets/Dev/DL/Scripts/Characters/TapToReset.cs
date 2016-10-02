using UnityEngine;
using System.Collections;

public class TapToReset : MonoBehaviour {
	
	float tapCounter = 1.5f;
	int taps;
	LevelHandler lHandler;

	// Use this for initialization
	void Start () {
		lHandler = GameObject.Find ("LevelHandler").GetComponent<LevelHandler> ();
	}
	
	// Update is called once per frame
	void Update () {
		tapCounter -= Time.deltaTime;
		if (tapCounter <= 0) {
			tapCounter = 1.5f;
			if (taps > 0)
				taps--;
		}
		if (Input.GetMouseButtonDown (0) && tapCounter>0) {
			taps++;
			tapCounter = 1.5f;
			if (taps == 4) {
				lHandler.Reset ();
			}
		}
	}
}
