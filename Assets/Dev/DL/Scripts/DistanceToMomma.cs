using UnityEngine;
using System.Collections;

public class DistanceToMomma : MonoBehaviour {

	public GameObject Momma;
	public GameObject Viewer;
	public float distance;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		PlayerPrefs.SetFloat("distance", Vector3.Distance (Momma.transform.position, Viewer.transform.position));
	}
}
