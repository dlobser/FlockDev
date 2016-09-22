using UnityEngine;
using System.Collections;


public class ChosenHeadset : MonoBehaviour {

	public int whichHeadset;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

}
