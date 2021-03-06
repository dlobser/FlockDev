﻿using UnityEngine;
using System.Collections;

public class TapToReset : MonoBehaviour {
	
	float tapCounter = 1.5f;
	int taps;
	LevelHandler lHandler;

	public GameObject[] activateOnReset;
	public GameObject[] deactivateOnReset;

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
//				Debug.Log ("poop");
				for (int i = 0; i < activateOnReset.Length; i++) {
					if (!activateOnReset [i].activeInHierarchy)
						activateOnReset [i].gameObject.SetActive (true);
				}
				for (int i = 0; i < deactivateOnReset.Length; i++) {
					if (deactivateOnReset [i].activeInHierarchy)
						deactivateOnReset [i].gameObject.SetActive (false);
				}
				lHandler.Reset ();
				taps = 0;
			}
		}
	}
}
