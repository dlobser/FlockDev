using UnityEngine;
using System.Collections;

public class copyTransforms : MonoBehaviour {

	public GameObject target;

	void Update () {
		DLUtility.copyWorldTransforms (target, transform.gameObject);
	}
}
