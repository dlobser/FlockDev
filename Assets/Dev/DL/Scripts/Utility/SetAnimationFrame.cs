using UnityEngine;
using System.Collections;

public class SetAnimationFrame : MonoBehaviour {

	public Animator anim;
	public float time;
	public string state;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		anim.Play(state, -1, time);
		anim.speed = 0;
	}
}
