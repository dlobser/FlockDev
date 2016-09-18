using UnityEngine;
using System.Collections;

public class clickStop : MonoBehaviour {
	UpdateAndYield updateAndYield;

	void Start(){
		updateAndYield = (UpdateAndYield)GetComponentInParent (typeof(UpdateAndYield));

		}
	void Update() {
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Pressed left click.");

			updateAndYield.StopStutter ();
		}
	}

}
