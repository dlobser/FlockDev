using UnityEngine;
using System.Collections;

public class ScaleEnvironmentWhenReady : MonoBehaviour {

    public int count = 0;
    public Vector3 start;
    public Vector3 end;
    float counter;
    public Color startFog;
    public Color endFog;
    public GameObject fader;

    bool init = false;
    public float scaleTime = 10;
	// Use this for initialization
	void Start () {
      
    }
    void Update()
    {
        if (!init&&Time.time>.01f)
        {
            //Debug.Log("init");
            this.transform.localScale = start;
            RenderSettings.fogColor = startFog;
            Camera.main.backgroundColor = startFog;
            fader.SetActive(false);
            init = true;
        }
    }

    public void Add()
    {
        count++;
        if (count == 2)
        {
            StartCoroutine(scaleEnvironmentDown());
        }
    }

    IEnumerator scaleEnvironmentDown()
    {
        while (counter < scaleTime)
        {
            counter += .1f;
            this.transform.localScale = Vector3.Lerp(start, end, Mathf.SmoothStep(0,1, counter / scaleTime));
            RenderSettings.fogColor = Color.Lerp(startFog, endFog, counter / scaleTime);
            Camera.main.backgroundColor = Color.Lerp(startFog, endFog, counter / scaleTime); ;

            yield return new WaitForSeconds(Time.deltaTime);
        }
        fader.SetActive(true);
    }
}
