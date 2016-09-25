using UnityEngine;
using System.Collections;

public class FaderMaterialTexSwitcher : Fader {

	public Texture[] _Tex;
	public Material mat;
	bool ping;
	float previous=0;


	public override void Fade(){

		float lev = Mathf.Clamp (level, min, max) / levels;
		float currentLevel = lev * _Tex.Length;
		float sinLevel = (1+Mathf.Cos (currentLevel * Mathf.PI))*.5f;
		int index = (int)Mathf.Clamp (Mathf.Floor (currentLevel), 0, _Tex.Length - 1);
		Texture2D thisTex = _Tex [index] as Texture2D;

		if (previous <= level) {
			if (index % 2 == 0)
				mat.SetTexture ("_MainTex", thisTex);
			else
				mat.SetTexture ("_SecondTex", thisTex);
		} else {
			if (index % 2 == 0)
				mat.SetTexture ("_SecondTex", thisTex);
			else
				mat.SetTexture ("_MainTex", thisTex);
			
		}
		previous = level;
		mat.SetFloat ("_TextureFade", sinLevel);
	}
}
