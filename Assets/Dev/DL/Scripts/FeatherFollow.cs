using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FeatherFollow : MonoBehaviour {

	public GameObject follower;
	public float speed;
	List<Vector3> prevRotations;
	// Use this for initialization
	void Start () {
		prevRotations = new List<Vector3> ();
	}
	
	// Update is called once per frame
	void Update () {
		prevRotations.Add (this.transform.eulerAngles);
		follower.transform.position = Vector3.Lerp(follower.transform.position, this.transform.position,speed);
		follower.transform.localEulerAngles = prevRotations [0];
//		follower.transform.localScale = this.transform.localScale;
		if(prevRotations.Count>2){
			prevRotations.RemoveAt(0);
		}
	}
}
