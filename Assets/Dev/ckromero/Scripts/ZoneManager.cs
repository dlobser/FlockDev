using UnityEngine;
using System.Collections;

public class ZoneManager : MonoBehaviour {

	protected virtual void OnTriggerEnter(Collider other) {
		Debug.Log ("ZoneManager received: " + this.name + " collided with " + other.name + " at: " + Time.time);
	}

}
