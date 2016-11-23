using UnityEngine;
using System.Collections;

public class AimAndScaleToTarget : MonoBehaviour
{

    public GameObject target;
    public GameObject origin;
    Vector3 scalar;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = origin.transform.position;
        this.transform.LookAt(target.transform.position);
        float dist = Vector3.Distance(this.transform.position, target.transform.position);
        scalar.Set(this.transform.localScale.x, this.transform.localScale.y, dist);
        this.transform.localScale = scalar;
    }
}