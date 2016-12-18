using UnityEngine;
using System.Collections;

public class rotateIntoView : MonoBehaviour {

    public GameObject cam;
    public float timeToMove = 5;
    float counter;
    float rotateCounter = 0;
    public float rotateSpeed = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float angle = Quaternion.Angle(this.transform.rotation, cam.transform.rotation);

        if (angle > 45)
            counter += Time.deltaTime;
        else
            counter = 0;

        if(angle>45 && counter > timeToMove)
        {
            counter = 0;
            StartCoroutine(RotateToCam());

        }
	}

    IEnumerator RotateToCam()
    {
        Quaternion init = this.transform.rotation;
        Quaternion rot = cam.transform.rotation;
        while (rotateCounter < rotateSpeed)
        {

            rotateCounter += Time.deltaTime;
            this.transform.rotation = Quaternion.Lerp(init, rot, Mathf.SmoothStep( 0, 1, rotateCounter / rotateSpeed ));
            this.transform.localEulerAngles = new Vector3(0, this.transform.localEulerAngles.y, 0);
            yield return new WaitForSeconds(Time.deltaTime);

        }

        rotateCounter = 0;

    }
}
