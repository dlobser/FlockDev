using UnityEngine;
using System.Collections;

public class LineParticleStrength : MonoBehaviour {

    Vector3 prevPosition;
    float speed;
    public ParticleSystem parti;
    public TrailRenderer trail;
    public GameObject flare;
    public float partiMult;
    public float trailMult;
    public Vector2 flareScaleMinMax;
    Vector3 scalar = Vector3.zero;

    public Material UIMat;
    public float canvasFadeSpeed;
    float fader = 1;
    bool fadeStarted = false;

    public Color startDial;
    public Color endDial;
    public float startTime;

    public float minDialAngle;
    public float maxDialAngle;

    public bool triggered;


    ScaleEnvironmentWhenReady env;

    public GameObject dial;

    public bool UIisActive = true;
    // Use this for initialization
    void Start () {
        env = GameObject.Find("Forest").GetComponent<ScaleEnvironmentWhenReady>();
        UIMat.SetColor("_Color", new Color(1, 1, 1, fader));
        dial.transform.parent.gameObject.GetComponent<MeshRenderer>().material.color = startDial;
    }

	void OnApplicationQuit(){
		UIMat.SetColor("_Color", new Color(1, 1, 1, 1));
	}
	// Update is called once per frame
	void Update () {
        if(Time.time>startTime && triggered)
            GetSpeed();
        trail.time = speed * trailMult;
        parti.emissionRate = speed * partiMult;
        parti.startSpeed = speed * 3;
        float sc = DLUtility.remap(speed, 0, 1, flareScaleMinMax.x, flareScaleMinMax.y);
        scalar.Set(sc, sc, sc);
        this.transform.localScale = scalar;

        float dialer = speed / canvasFadeSpeed;
        if (!fadeStarted)
        {
            float dialRemapped = DLUtility.remap(dialer, 0, 1, minDialAngle, maxDialAngle);
            dial.transform.localEulerAngles = new Vector3((dialRemapped * 360) - 90, 90, 90);
            dial.transform.parent.gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(startDial, endDial, dialer);

        }
		if (triggered && fader>=1) {
			StartCoroutine(fadeUI());
			dial.transform.parent.gameObject.SetActive(true);
		}
        if (triggered && speed > canvasFadeSpeed && !fadeStarted && Time.time>3f)
        {
//            StartCoroutine(fadeUI());
            fadeStarted = true;
            env.Add();
            dial.transform.parent.gameObject.SetActive(false);
            UIisActive = false;
        }
	}

    void GetSpeed()
    {
        speed = speed * 100;
        speed += Vector3.Distance(prevPosition, this.transform.position);
        speed /= 101;
        prevPosition = this.transform.position;
    }

    IEnumerator fadeUI()
    {
        while (fader > 0)
        {
			fader -= Time.deltaTime * 2f;
            UIMat.SetColor("_Color", new Color(1, 1, 1, fader));
			yield return null;
        }
    }
}
