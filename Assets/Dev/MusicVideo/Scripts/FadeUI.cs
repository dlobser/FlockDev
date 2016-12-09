using UnityEngine;
using System.Collections;

public class FadeUI : MonoBehaviour {

    public float fadeStartTime;
    public float fadeSpeed;
    public Material UIMat;
    float fader = 1f;
	// Use this for initialization
	void Start () {
        UIMat.SetColor("_Color", new Color(1, 1, 1, fader));
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > fadeStartTime)
        {
            fader -= Time.deltaTime * fadeSpeed;
            UIMat.SetColor("_Color", new Color(1,1,1, fader));
        }
        else if(Time.time > fadeStartTime && fader < 0)
        {
            this.gameObject.SetActive(false);
        }
	}

    private void OnApplicationQuit()
    {
        UIMat.SetColor("_Color", new Color(1, 1, 1, 1));
    }
}
