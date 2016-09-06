using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimerToSceneLoad : MonoBehaviour {
	public Text text;
	public int secondsTillNewLevel = 30;
	public string sceneToLoad;

	int timeLeft;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
//		string currentText = text.text;
//
//		numVal = int.Parse (currentText);
//
//
//
//		if (numVal <= 0) {
//			Debug.Log ("timer is less than one");
//		}
//
//		text.text = (numVal--).ToString ();

		timeLeft = secondsTillNewLevel - (int)Time.timeSinceLevelLoad;

		text.text = timeLeft.ToString ();

		if (timeLeft < 1) {
			SceneManager.LoadScene (sceneToLoad);
		}

	}
}
