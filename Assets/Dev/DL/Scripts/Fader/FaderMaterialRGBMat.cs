using UnityEngine;
using System.Collections;

public class FaderMaterialRGBMat : Fader {

	public Color[] _ColorR;
	public Color[] _ColorG;
	public Color[] _ColorB;
	public Color[] _ColorR2;
	public Color[] _ColorG2;
	public Color[] _ColorB2;
	public Color[] _ColorMult;
	public Color[] _ColorAdd;
	public Vector4[] _InHueShift;
	public Vector4[] _InSpeed;

	public Material mat;

	Color thisColor;
	Color nextColor;
	Color matColor;

	public override void Fade(){
		float currentLevel =( Mathf.Clamp( level,min,max) / levels)*_ColorR.Length;
		colorSetter (_ColorR, currentLevel, "_ColorR");
		colorSetter (_ColorG, currentLevel, "_ColorG");
		colorSetter (_ColorB, currentLevel, "_ColorB");
		colorSetter (_ColorR2, currentLevel, "_ColorR2");
		colorSetter (_ColorG2, currentLevel, "_ColorG2");
		colorSetter (_ColorB2, currentLevel, "_ColorB2");
		colorSetter (_ColorMult, currentLevel, "_ColorMult");
		colorSetter (_ColorAdd, currentLevel, "_ColorAdd");
		vectorSetter (_InHueShift, currentLevel, "_InHueShift");
		vectorSetter (_InSpeed, currentLevel, "_InSpeed");

	}

	void colorSetter(Color[] col, float currentLevel, string colorName){
		thisColor = col [(int)Mathf.Clamp(Mathf.Floor (currentLevel),0,col.Length-1)];
		nextColor = col [(int)Mathf.Clamp(Mathf.Ceil (currentLevel),0,col.Length-1)];
		matColor = Color.Lerp (thisColor, nextColor, currentLevel - Mathf.Floor (currentLevel));
		mat.SetColor (colorName, matColor);
	}
	void vectorSetter(Vector4[] vec, float currentLevel, string vecName){
		thisColor = vec [(int)Mathf.Clamp(Mathf.Floor (currentLevel),0,vec.Length-1)];
		nextColor = vec [(int)Mathf.Clamp(Mathf.Ceil (currentLevel),0,vec.Length-1)];
		matColor = Color.Lerp (thisColor, nextColor, currentLevel - Mathf.Floor (currentLevel));
		mat.SetColor (vecName, matColor);
	}


}
