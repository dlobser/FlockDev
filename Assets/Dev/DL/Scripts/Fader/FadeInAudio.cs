using UnityEngine;
using System.Collections;

public class FadeInAudio : MonoBehaviour {

	public AudioSource[] audios;
	float dist;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		dist = 1-PlayerPrefs.GetFloat ("distance");
		for (int i = 0; i < audios.Length; i++) {
			float div = (float)i / (float)audios.Length;
			audios [i].volume = DLUtility.clamp (dist - div, 0, 1);
		}
	}
}
