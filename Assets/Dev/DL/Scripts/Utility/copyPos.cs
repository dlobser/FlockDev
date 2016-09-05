using UnityEngine;
using System.Collections;

public class copyPos : MonoBehaviour {

	public GameObject poser;
	public Material mat;
	Vector2 off = Vector2.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		off.y = Time.time*.01f;
		mat.SetVector ("_Pos", poser.transform.position);
		mat.SetTextureOffset ("_MainTex", off);
	}
}
