using UnityEngine;
using System.Collections;
using Holojam.Tools;

public class SettingsManager : MonoBehaviour {

	public SettingsJSON settingsJSON;
	private Synchronizable synchronizable;

	void Start () {
		synchronizable = GetComponent<Synchronizable> ();	
//		settingsJSON = new SettingsJSON ();
		UpdateSettings ();
	}

	private void UpdateSettings() { 

		synchronizable.synchronizedString = JsonUtility.ToJson (settingsJSON);

	}

	void Update() { 

		UpdateSettings ();
		Debug.Log (synchronizable.synchronizedString);
	}
}
