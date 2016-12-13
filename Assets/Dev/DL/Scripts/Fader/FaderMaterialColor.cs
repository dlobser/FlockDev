using UnityEngine;
using System.Collections;

public class FaderMaterialColor : Fader {

	public Color[] _Color;
	public Material mat;
	public string colorName;
	public bool interpolate = true;
    public bool affectAlpha = true;

	public override void Fade(){
		float currentLevel =( (Mathf.Clamp( level,min,max)-min) / levels)*_Color.Length;
		Color thisColor = _Color [(int)Mathf.Clamp(Mathf.Floor (currentLevel),0,_Color.Length-1)];
		if (interpolate) {
			Color nextColor = _Color [(int)Mathf.Clamp (Mathf.Ceil (currentLevel), 0, _Color.Length - 1)];
			Color matColor = Color.Lerp (thisColor, nextColor, currentLevel - Mathf.Floor (currentLevel));
            if (affectAlpha)
                mat.SetColor(colorName, matColor);
            else
                mat.SetColor(colorName, new Color(matColor.r, matColor.g, matColor.b, mat.color.a));
		} else {
			  if (affectAlpha)
                mat.SetColor(colorName, thisColor);
            else
                mat.SetColor(colorName, new Color(thisColor.r, thisColor.g, thisColor.b, mat.color.a));
		}
	}
}
