using UnityEngine;
using System.Collections;

public class BirdAvatarLOD : MonoBehaviour {

	public SkinnedMeshRenderer[] skins;
	public GameObject[] wings;
	public float slider;
	Vector3 scalar = Vector3.one;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Slide ();
	}

	public void Slide(){
		for (int i = 0; i < skins.Length; i++) {
			skins [i].SetBlendShapeWeight (0, slider * 100);
		}
		float newSlide = Mathf.Clamp (1 - slider, 0, 1);
		scalar.Set (newSlide, newSlide, newSlide);
		for (int i = 0; i < wings.Length; i++) {
			wings [i].transform.localScale = scalar;
		}
	}
}
