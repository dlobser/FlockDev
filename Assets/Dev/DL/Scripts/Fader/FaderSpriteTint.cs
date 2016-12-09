using UnityEngine;
using System.Collections;

public class FaderSpriteTint : Fader {

	public Color[] _Color;
	public SpriteRenderer sprite;
	public bool interpolate = true;
    public Color tintMult;

	public override void Fade(){
		float currentLevel =( (Mathf.Clamp( level,min,max)-min) / levels)*_Color.Length;
		Color thisColor = _Color [(int)Mathf.Clamp(Mathf.Floor (currentLevel),0,_Color.Length-1)];
		if (interpolate) {
			Color nextColor = _Color [(int)Mathf.Clamp (Mathf.Ceil (currentLevel), 0, _Color.Length - 1)];
			Color matColor = Color.Lerp (thisColor, nextColor, currentLevel - Mathf.Floor (currentLevel));
			sprite.color= matColor*tintMult;
		} else {
            sprite.color = thisColor*tintMult;
        }
	}
}
