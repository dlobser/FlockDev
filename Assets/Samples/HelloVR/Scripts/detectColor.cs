using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using wvr;

public class detectColor : MonoBehaviour {
	public GameObject RayObject = null;
	public GameObject leftcontrol;
	public GameObject rightcontrol;
	public Transform point_r;
	public WVR_DeviceType Controller1Index = WVR_DeviceType.WVR_DeviceType_Controller_Right;
	public WVR_DeviceType Controller2Index = WVR_DeviceType.WVR_DeviceType_Controller_Left;
	private  bool leftmove=false;
	private  bool rightmove=false;
	private  bool ballisusing = false;
	// Use this for initialization
	void Start () {
		RayObject.GetComponent<MeshRenderer> ().material.SetColor ("_Color",Color.green);
	}

	// Update is called once per frame
	void Update () {
		// Get GameObject hit by raycast
		Vector3 fwd = rightcontrol.transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
		bool rightTriggerDown = false;
		RayObject.GetComponent<MeshRenderer> ().material.SetColor ("_Color", Color.green);
		if (Physics.Raycast (rightcontrol.transform.position, fwd, out hit)&&(WaveVR_Controller.Input (Controller1Index).connected)) {
			GameObject gobj = hit.collider.gameObject;
			string hittag = gobj.tag;
			if ((hittag == "ball") && (!ballisusing)) {
				ballisusing = true;
				RayObject.GetComponent<MeshRenderer> ().material.SetColor ("_Color", Color.red);
				rightTriggerDown |= WaveVR_Controller.Input (Controller1Index).GetPressDown (WVR_InputId.WVR_InputId_Alias1_Touchpad);
				//rightTriggerUp |= WaveVR_Controller.Input (Controller1Index).GetPressUp (WVR_InputId.WVR_InputId_Alias1_Touchpad);
				if (rightTriggerDown) {
					rightmove = rightTriggerDown ^ rightmove;
					if (rightmove) {
						gobj.transform.RotateAround (point_r.position, Vector3.up, 15.0f);
					} else {
						gobj.transform.RotateAround (point_r.position, Vector3.up, -15.0f);
					}
				}
			} else {
				ballisusing = false;
			}
		}else {
			ballisusing = false;
		}
	}
	void LateUpdate()
	{
		bool leftTriggerDown = false;
		RaycastHit hit;
		Vector3 leftfwd = leftcontrol.transform.TransformDirection (Vector3.forward);
		if (Physics.Raycast (leftcontrol.transform.position, leftfwd, out hit)&&(WaveVR_Controller.Input (Controller2Index).connected)) {
			GameObject gobj = hit.collider.gameObject;
			string hittag = gobj.tag;
			if ((hittag == "ball") && (!ballisusing)) {
				ballisusing = true;
				RayObject.GetComponent<MeshRenderer> ().material.SetColor ("_Color", Color.blue);
				leftTriggerDown |= WaveVR_Controller.Input (Controller2Index).GetPressDown (WVR_InputId.WVR_InputId_Alias1_Touchpad);
				// Left controller touchpad clicked
				if (leftTriggerDown) {
					leftmove = leftTriggerDown ^ leftmove;
					if (leftmove) {
						gobj.transform.RotateAround (point_r.position, Vector3.up, -15.0f);
					} else {
						gobj.transform.RotateAround (point_r.position, Vector3.up, 15.0f);
					}
				}
			} else {
				ballisusing = false;
			}
		}else {
			ballisusing = false;
		}
	}
}

   
	
   
