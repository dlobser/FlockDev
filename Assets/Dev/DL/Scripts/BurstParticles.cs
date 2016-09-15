using UnityEngine;
using System.Collections;

public class BurstParticles : MonoBehaviour {

	public ParticleSystem part;

//	public bool burst;
//
//	void Update(){
//		if (burst) {
//			Burst ();
//			burst = false;
//		}
//	}

	// Update is called once per frame
	public void Burst (int amount) {
		part.Emit(amount);
	}
}
