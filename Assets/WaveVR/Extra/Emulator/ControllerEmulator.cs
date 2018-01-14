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
using System.Collections;

using WaveVREmulator;

public class ControllerEmulator : MonoBehaviour
{
    public string WIFI_IP = "192.168.0.2";

    private static ControllerEmulator instance;
    private static ControllerProvider controllerProvider;
    
    static internal ControllerProvider CreateControllerProvider(ControllerEmulator owner)
    {
        return new ControllerProvider ();
    }

    void Awake()
    {
        instance = this;

        if (controllerProvider == null)
        {
            Debug.Log ("ControllerEmulator::Awake, create controller provider.");
            controllerProvider = CreateControllerProvider (this);
        }
    }

    void OnDestroy()
    {
        instance = null;
    }

    private ControllerState controllerState = new ControllerState();

    private void UpdateController()
    {
        controllerProvider.ReadState(controllerState);
        //Debug.Log ("ControllerEmulator::UpdateController, orientation: " + controllerState.orientation);
        transform.localRotation = controllerState.orientation;
        if (controllerState.padButtonDown)
            Debug.Log ("ControllerEmulator::UpdateController, clicked pad!");
        if (controllerState.menuButtonDown)
            Debug.Log ("ControllerEmulator::UpdateController, clicked menu!");
        if (controllerState.sysButtonDown)
            Debug.Log ("ControllerEmulator::UpdateController, clicked system!");
    }

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    IEnumerator EndOfFrame()
    {
        while (true) {
            // This must be done at the end of the frame to ensure that all GameObjects had a chance
            // to read transient controller state (e.g. events, etc) for the current frame before
            // it gets reset.
            yield return waitForEndOfFrame;
            UpdateController();
        }
    }

    private IEnumerator controllerUpdate;

    void OnEnable()
    {
        controllerUpdate = EndOfFrame();
        StartCoroutine(controllerUpdate);
    }

    void OnDisable()
    {
        StopCoroutine(controllerUpdate);
    }

    // --------------------- Get Controller Information begins --------------------

    /// Returns the controller's current orientation in space, as a quaternion.
    /// The space in which the orientation is represented is the usual Unity space, with
    /// X pointing to the right, Y pointing up and Z pointing forward. Therefore, to make an
    /// object in your scene have the same orientation as the controller, simply assign this
    /// quaternion to the GameObject's transform.rotation.
    public static Quaternion Orientation {
        get {
            return instance != null ? instance.controllerState.orientation : Quaternion.identity;
        }
    }

    // --------------------- Get Controller Information ends ----------------------

}