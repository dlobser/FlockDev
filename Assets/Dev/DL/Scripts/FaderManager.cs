using UnityEngine;
using System.Collections;

public class FaderManager : MonoBehaviour {

	Fader[] faders;
	public float level;
	public float minLevel;
	public float maxLevel;
	// Use this for initialization
	void Start () {
		faders = GetComponentsInChildren<Fader> ();
		for (int i = 0; i < faders.Length; i++) {
			faders [i].min = minLevel;
			faders [i].max = maxLevel;
			faders [i].Init ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < faders.Length; i++) {
			faders [i].level = level;
			faders [i].Fade ();
		}
	}
}
