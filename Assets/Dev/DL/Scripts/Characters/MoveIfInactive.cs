using UnityEngine;
using System.Collections;

public class MoveIfInactive : MonoBehaviour {

	Vector3 maximum = new Vector3(10000,10000,0);
	float[] counters;
	public float missingTrackingTime = 2;
	// Use this for initialization
	void Start () {
		
		counters = new float[this.transform.childCount];
		StartCoroutine (mover ());
	}

	IEnumerator mover(){
		while (true) {
			for (int i = 0; i < this.transform.childCount; i++) {
				bool tracked = this.transform.GetChild (i).gameObject.GetComponent<Holojam.Tools.Actor> ().view.IsTracked;
//				Debug.Log ("tracked" + tracked);
				if (!tracked) {
					counters [i] += Time.deltaTime;
					if (counters [i] > missingTrackingTime)
						this.transform.GetChild (i).transform.position = maximum;
				} else if (tracked) {
					counters [i] = 0;
				}
				yield return new WaitForSeconds (0);
			}
		}
	}
}
