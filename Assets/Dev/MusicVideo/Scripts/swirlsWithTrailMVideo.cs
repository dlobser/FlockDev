using UnityEngine;
using System.Collections;

public class swirlsWithTrailMVideo : MonoBehaviour {
	
	public int amount;
	public Material mat;
	public Material trailMat;
	//	public GameObject point;
	public GameObject[] points;
	public GameObject[] trails;
	public float orbitStrengthLow = .02f;
	public float orbitStrengthHigh = .1f;
	private float counter = 0;
	public float noiseAmountLow = 10;
	public float noiseAmountHigh = 10;
	public float speedLow = 1;
	public float speedHigh = 1;
	
	public float startWidthHigh = 1;
	public float startWidthLow = 1;
	public float endWidth = 0;
	public float lengthLow = 10;
	public float lengthHigh = 10;
	public float variationSpeed = .01f;
	
	public GameObject altCtrl;
	public float altOrbitStrength = .2f;
	Vector3 altCtrlPrevPos = Vector3.zero;
	float[] Avg;
	float altCtrlAvg;

	
	// Use this for initialization
	void Start () {
		Avg = new float[300];
		points = new GameObject[amount];
		trails = new GameObject[amount];
		for (int i = 0; i < amount; i++) {
			points[i] = new GameObject();
			trails[i] = new GameObject();
			TrailRenderer TR = points[i].AddComponent<TrailRenderer>();
			TrailRenderer TR2 = trails[i].AddComponent<TrailRenderer>();
			TR.material = mat;
			TR2.material = trailMat;
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 poser = transform.position;
		
		
		
		float orbitStrength = map (Mathf.Sin (counter), -1, 1, orbitStrengthLow, orbitStrengthHigh);
		float speed = map (Mathf.Sin (counter*.9f), -1, 1, speedLow, speedHigh);
		float noiseAmount = map (Mathf.Sin (counter*1.1f), -1, 1, noiseAmountLow, noiseAmountHigh);
		float startWidth = map (Mathf.Sin (counter*1.2f), -1, 1, startWidthHigh, startWidthLow);
		float length = map (Mathf.Sin (counter*.7f), -1, 1, lengthLow, lengthHigh);

		float ctrlDist = Vector3.Distance (altCtrlPrevPos, altCtrl.transform.position);
		altCtrlAvg = average(ctrlDist);
		if (altCtrlAvg>0) {

			orbitStrength = Mathf.Lerp(orbitStrengthHigh, altOrbitStrength,altCtrlAvg*10);
//			noiseAmountHigh*=.25f;
		}
		poser = Vector3.Lerp(transform.position,altCtrl.transform.position,altCtrlAvg*20);

		
		counter += variationSpeed;
		for (int i = 0; i < amount; i++) {
			if (ctrlDist>0) 
				speed = speedHigh*Vector3.Distance(poser,points[i].transform.position);

			points[i].transform.Rotate(noiseAmount*new Vector3(
				.5f-Mathf.PerlinNoise(points[i].transform.position.y*.1f,counter*i*2),
				.5f-Mathf.PerlinNoise(points[i].transform.position.z*.1f,counter*i),
				.5f-Mathf.PerlinNoise(points[i].transform.position.x*.01f,points[i].transform.position.y*.01f*counter*i)
				));
			Quaternion oldRot = points[i].transform.rotation;
			points[i].transform.LookAt(poser);
			Quaternion newRot = points[i].transform.rotation;
			points[i].transform.rotation = Quaternion.Lerp(oldRot,newRot,orbitStrength);
			points[i].transform.Translate(Vector3.forward*speed*Time.deltaTime*60);
			trails[i].transform.position = points[i].transform.position;
			trails[i].transform.rotation = points[i].transform.rotation;
			
			TrailRenderer TR = points[i].GetComponent<TrailRenderer>();
			TR.startWidth = startWidth;
			TR.endWidth = endWidth;
			TR.time = length;
			
			TrailRenderer TR2 = trails[i].GetComponent<TrailRenderer>();
			TR2.startWidth = startWidth*.1f;
			TR2.endWidth = 0;
			TR2.time = length*20;
		}
		
		altCtrlPrevPos = altCtrl.transform.position;
		
		
	}

	float average(float av){
		
		for (int i = 0; i < Avg.Length-1; i++) {
			Avg[i] = Avg[i+1];
		}
		
		int c = Avg.Length - 1;
		Avg [c] = av;
		float t = 0;
		
		for (int i = 0; i < Avg.Length; i++) {
			t += Avg[i];
		}
		
		t /= Avg.Length;
		return t;
	
	}
	
	void animateTrail(){
		
	}
	float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
}
