using UnityEngine;
using System.Collections;
using Holojam.Tools;

public class SettingsManager :  MonoBehaviour {

	public SynchronizedString synchronizable;
	public SettingsJSON settingsJSON;

	void Update() { 
		if (BuildManager.IsMasterClient()) {
			synchronizable.SetText(JsonUtility.ToJson(settingsJSON));
		} else {
			settingsJSON = JsonUtility.FromJson<SettingsJSON>(synchronizable.GetText());
		}
	}
}
