using UnityEngine;
using System.Collections;

public class transformAverage : MonoBehaviour {

	public bool translate;
	public bool scale;
	public bool rotate;

	public float avgAmount;

	public GameObject target;

	Vector3 pos;
	Vector3 scl;
	Quaternion quat;

	void Update () {


		float a = avgAmount + 1;
		Vector3 P = target.transform.position;

		if (translate) {
			pos.Set (
				(pos.x * avgAmount + P.x) / a,
				(pos.y * avgAmount + P.y) / a,
				(pos.z * avgAmount + P.z) / a);
			this.transform.position = pos;
		}
		if (rotate) {
			Quaternion Q = target.transform.rotation;
			quat.Set (
				(Mathf.Lerp(quat.x,Q.x,1/avgAmount)),
				(Mathf.Lerp(quat.y,Q.y,1/avgAmount)),
				(Mathf.Lerp(quat.z,Q.z,1/avgAmount)),
				(Mathf.Lerp(quat.w,Q.w,1/avgAmount)));
			this.transform.rotation = quat;
		}
		if (scale) {
			Vector3 S = target.transform.localScale;
			scl.Set (
				(scl.x * avgAmount + S.x) / a,
				(scl.y * avgAmount + S.y) / a,
				(scl.z * avgAmount + S.z) / a);
			this.transform.localScale = scl;
		}
	}
}
