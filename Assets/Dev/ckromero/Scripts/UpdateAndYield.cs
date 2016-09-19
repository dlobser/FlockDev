using UnityEngine;
using System.Collections;

public class UpdateAndYield : MonoBehaviour {
//	private bool lockStutter=false;
	public IEnumerator stutterTalk;

	// Use this for initialization
	void Start () {
		stutterTalk = StutterTalk (10,5);
			
		StartCoroutine (stutterTalk);

	}
	
	// Update is called once per frame
	void Update () {
	}
	public void StopStutter(){

		StopCoroutine (stutterTalk);
	}

	IEnumerator StutterTalk(int repeats, int delay){
		Debug.Log ("entered Stutter Talk");
		for (int i=1; i<repeats; i++){
			
			yield return new WaitForSeconds (delay);
		
			Debug.Log ("in wait loop Stutter Talk");

		}
		Debug.Log ("continuing Stutter Talk");
	}
}
