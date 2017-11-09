// "WaveVR SDK 
// © 2017 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the WaveVR SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using UnityEngine;
using wvr;

/// Emulate tracking in Editor mode
#if UNITY_EDITOR
public class WaveVR_EmulatorPoseTracker : MonoBehaviour
{
    private WaveVR_PoseTracker parentTrackedObject;

    public void setParent(WaveVR_PoseTracker obj)
    {
        parentTrackedObject = obj;
    }

    void Update()
    {
        EmulateTransform();
    }

    private float shiftX = 0;
    private float shiftY = 0;
    private float shiftZ = 0;
    private float angleX = 0;
    private float angleY = 0;
    private float angleZ = 0;

    private struct Transform
    {
        public Vector3 pos;
        public Quaternion rot;
    }

    private void EmulateTransform()
    {
        Transform transform = new Transform();
        const float speed = 10;
        switch (parentTrackedObject.type)
        {
            // emulate head movements in Editor mode
            // Ctrl + mouse: tilt
            // Alt + mouse: horizontal or vertical shift
            case WVR_DeviceType.WVR_DeviceType_HMD:
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    angleY += Input.GetAxis("Mouse X") * 5;
                    if (angleY <= -180)
                    {
                        angleY += 360;
                    }
                    else if (angleX > 180)
                    {
                        angleY -= 360;
                    }
                    angleX -= Input.GetAxis("Mouse Y") * 2.4f;
                    angleX = Mathf.Clamp(angleX, -89, 89);
                }
                else if (Input.GetKey(KeyCode.LeftControl))
                {
                    angleZ += Input.GetAxis("Mouse X") * 5;
                    angleZ = Mathf.Clamp(angleZ, -89, 89);
                }

                transform.rot = Quaternion.Euler(angleX, angleY, angleZ);
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    // Assume mouse can move move 150 pixels in a-half second. 
                    // So we map from 150 pixels to 0.3 meter.

                Vector3 shift = transform.rot * new Vector3(Input.GetAxis("Mouse X") / 5, Input.GetAxis("Mouse Y") / 5, Input.GetAxis("Mouse ScrollWheel"));
                    shiftX += shift.x;
                    shiftY += shift.y;
                    shiftZ += shift.z;
                }
                transform.pos = new Vector3(shiftX, shiftY, shiftZ);
                break;
            case WVR_DeviceType.WVR_DeviceType_Controller_Right:

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    shiftY += speed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.DownArrow))
                {
                    shiftY -= speed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    shiftX -= speed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    shiftX += speed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.RightAlt))
                {
                    shiftZ += speed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.RightControl))
                {
                    shiftZ -= speed * Time.deltaTime;
                }
                transform.pos = new Vector3(shiftX, shiftY, shiftZ);
                break;

            default:
                break;

        }
        parentTrackedObject.updatePose(new WaveVR_Utils.RigidTransform(transform.pos, transform.rot));
    }

}
#endif
