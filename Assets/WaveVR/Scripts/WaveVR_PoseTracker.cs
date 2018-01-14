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

/// <summary>
/// This class is mainly for Device Tracking.
/// Tracking object communicates with HMD device or controller device in order to:
/// update new position and rotation for rendering
/// </summary>
public class WaveVR_PoseTracker : MonoBehaviour
{
    /// <summary>
    /// The type of this controller device, it should be unique.
    /// </summary>
    public WVR_DeviceType type;
    public bool inversePosition = false;
    public bool trackPosition = true;
    public bool inverseRotation = false;
    public bool trackRotation = true;

    public enum TrackingEvent {
        WhenUpdate,  // Pose will delay one frame.
        WhenNewPoses
    };

    public TrackingEvent timing = TrackingEvent.WhenNewPoses;

#if UNITY_EDITOR
    WaveVR_EmulatorPoseTracker emulatorTracker = null;
#endif

    public void Update()
    {
        if (timing == TrackingEvent.WhenNewPoses)
            return;
        if (WaveVR.Instance == null)
            return;

        WaveVR.Device device = WaveVR.Instance.getDeviceByType(type);
        if (device.connected)
        {
            updatePose(device.rigidTransform);
        }
    }


    /// if device connected, get new pose, then update new position and rotation of transform
    private void OnNewPoses(params object[] args)
    {
        var poses = (WVR_DevicePosePair_t[])args [0];
        var rtPoses = (WaveVR_Utils.RigidTransform[])args [1];

        for (int i = 0; i < poses.Length; i++)
        {
            if (poses[i].type == type && poses[i].pose.IsValidPose)
            {
                updatePose(rtPoses [i]);
                break;
            }
        }
    }

    public void updatePose(WaveVR_Utils.RigidTransform pose)
    {
        if (trackPosition)
        {
            if (inversePosition)
                transform.localPosition = -pose.pos;
            else
                transform.localPosition = pose.pos;
        }
        if (trackRotation)
        {
            if (inverseRotation)
                transform.localRotation = Quaternion.Inverse(pose.rot);
            else
                transform.localRotation = pose.rot;
        }
    }

    void OnEnable()
    {
        if (timing == TrackingEvent.WhenNewPoses)
            WaveVR_Utils.Event.Listen(WaveVR_Utils.Event.NEW_POSES, OnNewPoses);

#if UNITY_EDITOR
        emulatorTracker = GetComponent<WaveVR_EmulatorPoseTracker>();
        if (emulatorTracker == null)
        {
            emulatorTracker = gameObject.AddComponent<WaveVR_EmulatorPoseTracker>();
        }
        emulatorTracker.enabled = true;
        emulatorTracker.setParent(this); 
#endif
    }

    void OnDisable()
    {
        if (timing == TrackingEvent.WhenNewPoses)
            WaveVR_Utils.Event.Remove(WaveVR_Utils.Event.NEW_POSES, OnNewPoses);

#if UNITY_EDITOR
        if (emulatorTracker != null)
        {
            emulatorTracker.enabled = false;
        }
#endif
    }
}

