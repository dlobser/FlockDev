using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swarm : MonoBehaviour {

	public GameObject flyPrefab;
	List<Fly> flies;

	public GameObject schoolAnimator;

	Vector3[] targets;
	//	Vector3[] prevPosition;

	//	GameObject flyParent;
	public float flyScale = 1;
	bool activated = true;

	public UVLookup uvLookup;
	//	public FillWithRandomObjects fill;

	private bool initialized = false;

	public float spread;
	public int amount;

	public bool useNoiseBehavior;

	ImageTools.Core.PerlinNoise PNoise;

//	public bool spawnFly = true;

	float noiseCounter = 0;

	public float noiseSpeed = 1f;
	public float noiseScale = 2.2f;
	public float noiseWorldScale = .1f;


	// Use this for initialization
	void Start () {
		PNoise = new ImageTools.Core.PerlinNoise (1);
		flies = new List<Fly> ();
		uvLookup.Init ();
//		if(spawnFly)
			FillFly ();
	}

	void FillFly() {
		for (int i = 0; i < amount; i++) {

			Vector3 flyPosition = Vector3.Scale( Random.insideUnitSphere,spread*this.transform.localScale);
			GameObject flyInstance = Instantiate (flyPrefab,flyPosition,Quaternion.identity) as GameObject;
			flyInstance.transform.SetParent(this.transform);
			flyInstance.name = "fly_" + i;
			flyInstance.transform.SetParent (this.transform);

			Fly flyScript = flyInstance.GetComponent<Fly> ();
			flyScript.target = flyPosition;
			flyScript.origin = flyPosition;
			flyScript.spriteAimer.uvLookup = uvLookup;
			flyScript.spriteAimer.Init ();

			flyInstance.GetComponent<Holojam.Tools.Synchronizable> ().label  += i;

			flies .Add(flyScript);

			//			yield return null;
		}
		initialized = true;
	}

	void Update () {
		if (initialized) {
			if (activated && flyScale > 0) {
				for (int i = 0; i < amount; i++) {
					Vector3 newPos = schoolAnimator.transform.localToWorldMatrix.MultiplyVector (flies [i].origin) + schoolAnimator.transform.position;
					if (useNoiseBehavior) {
						float off = .5f;
						float scale = noiseScale*.01f;
						float wScale = noiseWorldScale;
						noiseCounter += noiseSpeed * Time.deltaTime;
						flies [i].target = newPos + new Vector3 (
							scale * (float)PNoise.Noise (wScale * newPos.x + noiseCounter + off, noiseCounter + newPos.y * wScale, noiseCounter + wScale * newPos.z),
							scale * (float)PNoise.Noise (wScale * newPos.x + noiseCounter, wScale * newPos.y + noiseCounter + off, noiseCounter + wScale * newPos.z),
							scale * (float)PNoise.Noise (wScale * newPos.x + noiseCounter, wScale * newPos.y + noiseCounter, noiseCounter + wScale * newPos.z + off));

					} else
						flies [i].target = newPos;
					float sc = flies [i].gameObject.GetComponent<ScaleToCamDistance> ().scale;
					float deathScale = flies [i].scaleForDeath;
					flies[i].UpdatePosition(flyScale * sc * deathScale);
				}
			} else if (flyScale <= 0 && activated) {
				for (int i = 0; i < this.transform.childCount; i++) {
					flies [i].gameObject.SetActive (false);

				}
				activated = false;
			} else if (!activated && flyScale > 0) {
				for (int i = 0; i < this.transform.childCount; i++) {
					flies [i].gameObject.SetActive (true);

				}
				activated = true;
			}
		}
	}


}
