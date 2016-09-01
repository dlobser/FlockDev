using UnityEngine;
using System.Collections;

public class copyPos : MonoBehaviour {

	public GameObject poser;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<TrailRenderer> ().materials[0].SetVector ("_Pos", poser.transform.position);
	}
}
