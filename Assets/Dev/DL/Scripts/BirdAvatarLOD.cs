using UnityEngine;
using System.Collections;

public class BirdAvatarLOD : MonoBehaviour {

	public SkinnedMeshRenderer[] skins;
	public GameObject[] wings;
	public float slider;
	public float flapSpeed = 0;
	public float flapAmount = 0;
	float flapCount = 0;
	Vector3 scalar = Vector3.one;
	Vector3 leftWingRotation = Vector3.zero;
	Vector3 rightWingRotation = Vector3.zero;
	bool wingsActive = true;
	DistanceToCamera dist;
	float distToCam = 0;
	public Vector4 distances;
	public bool manualSet = false;
	// Use this for initialization
	void Start () {
		dist = GetComponent<DistanceToCamera> ();
	}
	
	// Update is called once per frame
	void Update () {
		Slide ();
	}

	public void DeactivateWings(){
		for (int i = 0; i < wings.Length; i++) {
			wings [i].SetActive (false);
		}
		wingsActive = false;
	}
	public void ActivateWings(){
		for (int i = 0; i < wings.Length; i++) {
			wings [i].SetActive (true);
		}
		wingsActive = true;
	}

	public void Slide(){
		if (!manualSet) {
			distToCam = dist.distance;
			slider = DLUtility.remap (distToCam, distances.x, distances.y, distances.z, distances.w);
		}
		for (int i = 0; i < skins.Length; i++) {
			skins [i].SetBlendShapeWeight (0, slider * 100);
		}
		if(slider>=1&&wingsActive)
			DeactivateWings ();
		else if(slider<1&&!wingsActive)
			ActivateWings();

		if (wingsActive) {
			float newSlide = Mathf.Clamp (1 - slider, 0, 1);
			flapCount += Time.deltaTime * flapSpeed;
			scalar.Set (newSlide, newSlide, newSlide);
			leftWingRotation.Set (0, 0, Mathf.Sin (flapCount) * flapAmount);
			rightWingRotation.Set (0, 0, Mathf.Sin (-flapCount) * flapAmount);
			for (int i = 0; i < wings.Length; i++) {
				wings [i].transform.localScale = scalar;
			}
			wings [0].transform.localEulerAngles = leftWingRotation;
			wings [1].transform.localEulerAngles = rightWingRotation;
		}
	}
}
