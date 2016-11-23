using UnityEngine;
using System.Collections;

public class ResetPositionAtStart : MonoBehaviour {

    public GameObject target;
    bool init = false;
	// Use this for initialization
	void Start () {
        this.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!init && Time.time >.75f)
        {
            this.transform.position = target.transform.position;
            this.GetComponent<Rigidbody>().isKinematic = false;
            init = true;
        }
    }
}
