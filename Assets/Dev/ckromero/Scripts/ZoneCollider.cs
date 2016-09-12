using UnityEngine;
using System.Collections;

public class ZoneCollider : ZoneManager {

	protected override void OnTriggerEnter(Collider other) {

		if (Time.time > 1) {
			base.OnTriggerEnter (other);

			Debug.Log (this.name + " collided with " + other.name + " at: " + Time.time);
		}
	}
}
