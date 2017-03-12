using System.Collections;
using UnityEngine;

public class DistanceToCamera : MonoBehaviour {

  public float distance;
  void Update () {
    //distance = Vector3.Distance (Camera.main.transform.position, this.transform.position);

    // Evil!
    GameObject cameraEye = GameObject.Find("Camera (eye)");
    Transform target = null;

    if (!cameraEye) {
      Debug.Log("DistanceToCamera: Camera (eye) not found!)");
      GameObject masterCam = GameObject.Find("Spectator");

      if (!masterCam) {
        Debug.Log("DistanceToCamera: Spectator not found!)");
      } else {
        target = masterCam.transform;
      }
    } else { 
      target = cameraEye.transform;
    }

    if (!target) return;

    distance = Vector3.Distance(
      target.position, this.transform.position
    );
  }
}
