using UnityEngine;
using System.Collections;

public class FaderMaterialFloat : Fader {

	public float[] _Floats;
	public Material mat;
	public string floatName;

	public override void Fade(){
		float currentLevel =( (Mathf.Clamp( level,min,max)-min) / levels)*_Floats.Length;
		float thisFloat = _Floats [(int)Mathf.Clamp(Mathf.Floor (currentLevel),0,_Floats.Length-1)];
		float nextFloat = _Floats [(int)Mathf.Clamp(Mathf.Ceil (currentLevel),0,_Floats.Length-1)];
		float matFloat = Mathf.Lerp (thisFloat, nextFloat, currentLevel - Mathf.Floor (currentLevel));
		mat.SetFloat ("floatName", matFloat);
	}
}
