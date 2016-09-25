using UnityEngine;
using System.Collections;

public class MommaBirdAnimator : MonoBehaviour {

	public float speed;
	public float radius;
	Vector3 pos = Vector3.zero;
	Vector3 prevPos = Vector3.zero;
	public float height;
	float counter;

	public bool getAverageHeight;
	public float avgHeight = 0;
	public int avgAmount = 10;
	public GameObject viewer;

	// Use this for initialization
	void Start () {
		pos.y = height;
	}

	public float getAvg(){
		float h = viewer.transform.position.y;
		avgHeight = ((avgHeight * avgAmount) + h) / (avgAmount + 1);
		return avgHeight;
	}

	// Update is called once per frame
	void Update () {
		if (getAverageHeight)
			height = getAvg ();
		counter += Time.deltaTime * speed;
		pos.x = Mathf.Sin (counter)*.35f;
		pos.z = Mathf.Cos (counter);
		pos.y = height;
		this.transform.localPosition = pos*radius;
		this.transform.LookAt (prevPos);
		prevPos = this.transform.position;
	}
}
