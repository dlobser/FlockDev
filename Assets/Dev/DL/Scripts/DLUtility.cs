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
}
