using UnityEngine;
using System.Collections;

public class ZoneCollider : ZoneManager {

	protected override void OnTriggerEnter(Collider other) {
		
		base.OnTriggerEnter (other);

		Debug.Log (this.name + " collided with " + other.name + " at: " + Time.time);

	}
}
