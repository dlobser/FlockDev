using UnityEngine;
using System.Collections;

public class FadeTrailToSpeed : MonoBehaviour {

    public float speedMult = 1;
    public float speedMinus;
    public float maxAlpha;
    public TrailRenderer[] trail;
    float fade;
    GetSpeed getSpeed;
    public Material mat;
	// Use this for initialization
	void Start () {
        getSpeed = GetComponent<GetSpeed>();
	}
	
	// Update is called once per frame
	void Update () {
        fade = (getSpeed.averageSpeedOverThousand * speedMult) - speedMinus;
        for (int i = 0; i < trail.Length; i++)
        {
            Color col = mat.color;
            trail[i].material.color = new Color(col.r, col.g, col.b, Mathf.Min(maxAlpha,fade));
        }
	}
}
