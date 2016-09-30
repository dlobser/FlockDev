using UnityEngine;
using System.Collections;
using Holojam.Tools;

public class SettingsManager :  Synchronizable {

	public SettingsJSON settingsJSON;
	private Synchronizable synchronizable;

	void Start () {
		synchronizable = GetComponent<Synchronizable> ();	
//		settingsJSON = new SettingsJSON ();
		UpdateSettings ();
	}

	private void UpdateSettings() { 
		if (sending) {
			synchronizable.synchronizedString = JsonUtility.ToJson (settingsJSON);
		} else {
			settingsJSON = JsonUtility.FromJson<SettingsJSON> (synchronizable.synchronizedString);
		}
	}

	void Update() { 

		UpdateSettings ();
		Debug.Log (synchronizable.synchronizedString);
	}
}
