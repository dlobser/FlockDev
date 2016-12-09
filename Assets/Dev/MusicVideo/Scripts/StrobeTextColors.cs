using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StrobeTextColors : MonoBehaviour {

    public Color color1;
    public Color color2;
    public float speed;
    public Text text;
    Color col;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float t = ((Mathf.Sin(Time.time * speed) + 1) * .5f);
        text.color = Color.Lerp(color1, color2, t);
	}
}
