using UnityEngine;
using System.Collections;

public class FadeUI3 : MonoBehaviour {

    public float fadeStartTime;
    public float fadeSpeed;
    public Material UIMat;
    float fader = 1f;
    float counter = 0;
	// Use this for initialization
	void Start () {
        UIMat.SetColor("_Color", new Color(1, 1, 1, fader));
    }
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if (counter > fadeStartTime)
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
