using UnityEngine;
using System.Collections;

public class directionHintOpacity : MonoBehaviour {

	public GameObject target;
	public float inMin;
	public float inMax;
	public float min;
	public float max;
	float dist;
	public Material mat;
	Vector4 data;
	// Use this for initialization
	void Start () {
		data = mat.GetVector ("_Data");
	}
	
	// Update is called once per frame
	void Update () {
		dist = Vector3.Distance (this.transform.position, target.transform.position);
		data.y = DLUtility.clamp(DLUtility.remap (dist,inMin,inMax, min, max),max,min);
		mat.SetVector ("_Data", data);
	}
}
