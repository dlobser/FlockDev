using UnityEngine;
using System.Collections;

public class Painter : MonoBehaviour {

	public Texture2D tex;
	public GameObject spriteParent;
	public bool building;
	public int amount = 100;
	public float minSize = 0;
	public float maxSize = .1f;
	public float scale;

	// Use this for initialization
	void Start () {

	}

	void SetupTex(){
		for (int i = 0; i < amount; i++) {
			Vector2 coord = new Vector2 (Random.Range (0, tex.width), (int)Random.Range (0, tex.height));

			Color col = tex.GetPixel ((int)coord.x, (int)coord.y);
			GameObject thisSprite = spriteParent.transform.GetChild ((int)Random.Range (0, spriteParent.transform.childCount-1)).gameObject;
			GameObject sp = Instantiate (thisSprite);
			sp.transform.localPosition = new Vector3 (coord.x * scale, coord.y*scale , i * .0001f);
			float realScale = Random.Range (minSize, maxSize) * ((col.r)*1f);
			sp.transform.localScale = new Vector3 (realScale, realScale, realScale);
			sp.transform.localEulerAngles = new Vector3 (0, 0,( col.g* -360));
			sp.GetComponent<SpriteRenderer> ().color = new Color(col.r*Random.Range(.9f,1.0f),col.r*Random.Range(.8f,1.0f),col.b*.4f,1f);//.2f+((col.r)*.5f));
			sp.GetComponent<SpriteRenderer>().sortingOrder = i;

		}
	}

	// Update is called once per frame
	void Update () {
		if (building) {
			SetupTex ();
			building = false;
		}
	}
}
