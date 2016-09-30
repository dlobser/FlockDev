using UnityEngine;
using System.Collections;

public class ShadowPositioner : MonoBehaviour {

	public float height;
	public Transform target;
	Vector3 pos;
	SettingsManager settings;
	Color col;
	Vector3 scale;

	void Start(){
		settings = GameObject.Find ("SettingsManager").GetComponent<SettingsManager> ();
	}
	void Update(){
		SetPosition ();

	}
	// Update is called once per frame
	void SetPosition () {
		pos.Set (target.position.x, height, target.position.z);
		this.transform.position = pos;
		float size = settings.settingsJSON.shadowSize;
		scale.Set (size, size, size);
		this.transform.localScale = scale;
		Vector4 colVec = settings.settingsJSON.shadowColor;
		col = new Color (colVec.x, colVec.y, colVec.z, colVec.w);
		this.GetComponent<SpriteRenderer> ().color = col;
	}
}
