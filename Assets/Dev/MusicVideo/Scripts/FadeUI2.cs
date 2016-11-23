using UnityEngine;
using System.Collections;

public class FadeUI2 : MonoBehaviour {

    public float fadeStartTime;
    public float fadeSpeed;
    public Material UIMat;
    public Color color;
    float fader = 1f;
	// Use this for initialization
	void Start () {
        UIMat.SetColor("_Color", new Color(color.r, color.g, color.b, fader));
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > fadeStartTime && fader>0)
        {
            fader -= Time.deltaTime * fadeSpeed;
            UIMat.SetColor("_Color", new Color(color.r,color.g,color.b, fader));
        }
        if(Time.time > fadeStartTime && fader < 0)
        {
            this.gameObject.SetActive(false);
            Debug.Log("falsify");
        }
	}
}
