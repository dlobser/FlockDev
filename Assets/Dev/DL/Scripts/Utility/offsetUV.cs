using UnityEngine;
using System.Collections;

public class offsetUV : MonoBehaviour {

	public Vector2 speed;
	Vector2 off = Vector2.zero;
	Material mat;
	// Use this for initialization
	void Start () {
		mat = GetComponent<Renderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		off += speed * Time.deltaTime;
		mat.SetTextureOffset ("_MainTex", off);
	}
}
