using UnityEngine;
using System.Collections;

public class MoreBugs : MonoBehaviour {
	//	private bool lockStutter=false;
	private IEnumerator stutterTalk;
	public Swarm swarm;

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("SwarmMaker");
		Swarm swarm = (Swarm)go.GetComponent (typeof(Swarm));

		stutterTalk = StutterTalk (10,5,swarm);
		StartCoroutine (stutterTalk);

	}

	// Update is called once per frame
	void Update () {
			if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("Pressed left click.");

				StopStutter ();
			}
		}

	public void StopStutter(){

		StopCoroutine (stutterTalk);
	}

	IEnumerator StutterTalk(int repeats, int delay,Swarm swarm){
		Debug.Log ("entered Stutter Talk");
		for (int i=1; i<repeats; i++){
			
			yield return new WaitForSeconds (delay);

			swarm.FillFly ();

			Debug.Log ("in wait loop Stutter Talk");

		}
		Debug.Log ("continuing Stutter Talk");
	}
}