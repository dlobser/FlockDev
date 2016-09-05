using UnityEngine;
using System.Collections;

public class unitySpring : MonoBehaviour {

	public Rigidbody follower;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		follower.AddForce ((this.transform.position - follower.transform.position));
//		follower.vel
	}
}
