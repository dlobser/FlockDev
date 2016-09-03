using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holojam.Tools;

public class SynchronizablePoop : MonoBehaviour {

	public int amount;
	List<Poop> poops;
	public Poop poop;
	public GameObject Momma;
	public float interval;
	float counter;
	int which;
	// Use this for initialization
	void Start () {
		poops = new List<Poop> ();
		for (int i = 0; i < amount; i++) {
			poops.Add (Instantiate (poop, Vector3.one *( 1000f + Random.value*1000),Quaternion.identity,this.transform) as Poop);
			poops [i].GetComponent<Synchronizable> ().label = "Poop_" + i;
			poops[i].gameObject.name = "Poop_" + i;
		}
	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;
		if (counter > interval) {
			counter = 0;
			which++;
			if (which > poops.Count-1)
				which = 0;
			poops [which].makeAlive (Momma.transform.position + Random.insideUnitSphere*.2f);
			Debug.Log (which);
		}
		for (int i = 0; i < poops.Count; i++) {
			if (poops [i].enabled)
				poops [i].Animate ();
		}
	}
}
