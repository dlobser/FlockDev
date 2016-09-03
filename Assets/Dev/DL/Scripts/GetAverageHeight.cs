using UnityEngine;
using System.Collections;

public class GetAverageHeight : MonoBehaviour {

	public float avgHeight = 0;
	public int avgAmount = 10;
	public GameObject viewer;

	public float getAvg(){
		float h = viewer.transform.position.y;
		avgHeight = ((avgHeight * avgAmount) + h) / (avgAmount + 1);
		return avgHeight;
	}
}
