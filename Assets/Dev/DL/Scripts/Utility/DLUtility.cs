using UnityEngine;
using System.Collections;

public static class DLUtility {


	public static float remap(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}

	public static float clamp(float val, float min, float max){
		return Mathf.Max(min,Mathf.Min (max, val));
	}

	public static void copyTransforms(GameObject b, GameObject a){
		a.transform.localPosition = b.transform.localPosition;
		a.transform.localScale = b.transform.localScale;
		a.transform.localEulerAngles = b.transform.localEulerAngles;
	}

	public static void copyWorldTransforms(GameObject b, GameObject a){
		a.transform.position = b.transform.position;
		a.transform.localScale = b.transform.localScale;
		a.transform.eulerAngles = b.transform.eulerAngles;
	}
}
