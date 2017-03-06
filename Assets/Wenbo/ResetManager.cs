using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Holojam.Tools;

public class ResetManager : MonoBehaviour {
	
	public SynchronizedString synchronizable;
	public TapToReset tap;
	string[] signal;
	// Use this for initialization
	void Start () {
		signal = new string[7] { "1", "2", "3", "4","5","6","ALL" };
	}

	// Update is called once per frame
	void Update () {
		string text = "";
		if (Input.GetKeyDown(KeyCode.Q))
			text = signal [0];
		if (Input.GetKeyDown(KeyCode.W))
			text = signal [1];
		if (Input.GetKeyDown(KeyCode.E))
			text = signal [2];
		if (Input.GetKeyDown(KeyCode.R))
			text = signal [3];
    if (Input.GetKeyDown(KeyCode.T))
      text = signal[4];
    if (Input.GetKeyDown(KeyCode.Y))
      text = signal[5];
    if (Input.GetKeyDown(KeyCode.A))
			text = signal [6];
		if (BuildManager.IsMasterPC()) {
			synchronizable.SetText(text);
		} else {
			text = synchronizable.GetText();
			int index;
			Int32.TryParse (text,out index);
			if (index == BuildManager.BUILD_INDEX || text == "ALL") {
				tap.ButtonReset ();
			}
		}
		Debug.Log (text);

	}

}
