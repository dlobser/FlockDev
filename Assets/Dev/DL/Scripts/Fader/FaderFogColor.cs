using UnityEngine;
using System.Collections;

public class FaderFogColor : Fader {

	public Color[] _Color;
	public bool interpolate = true;
	Camera[] cams;
	public string[] camNames;

	void Awake(){
		cams = new Camera[camNames.Length];
//		for (int i = 0; i < camNames.Length; i++) {
//			cams [i] = GameObject.Find (camNames [i]).GetComponent<Camera> ();
//		}
	}

	public override void Fade(){
		float currentLevel =( (Mathf.Clamp( level,min,max)-min) / levels)*_Color.Length;
		Color thisColor = _Color [(int)Mathf.Clamp(Mathf.Floor (currentLevel),0,_Color.Length-1)];
		if (interpolate) {
			Color nextColor = _Color [(int)Mathf.Clamp (Mathf.Ceil (currentLevel), 0, _Color.Length - 1)];
			Color matColor = Color.Lerp (thisColor, nextColor, currentLevel - Mathf.Floor (currentLevel));
			RenderSettings.fogColor = matColor;
			Camera.main.backgroundColor = matColor;
			for (int i = 0; i < camNames.Length; i++) {
				if(cams[i]==null && GameObject.Find (camNames [i])!=null)
					cams[i] = GameObject.Find (camNames [i]).GetComponent<Camera> ();
				if(cams[i]!=null)
					cams [i] .backgroundColor = matColor;
			}
		} else {
			RenderSettings.fogColor=thisColor;
			Camera.main.backgroundColor = thisColor;
			for (int i = 0; i < camNames.Length; i++) {
				if(cams[i]==null  && GameObject.Find (camNames [i])!=null)
					cams[i] = GameObject.Find (camNames [i]).GetComponent<Camera> ();
				if(cams[i]!=null)
					cams [i] .backgroundColor = thisColor;
			}
		}
	}
}
