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
using wvr;
/// <summary>
/// Create WaveVR_EmulatorControllerProvider Instance to simulate controller Trigger input
/// </summary>
public class WaveVR_EmulatorControllerProvider : MonoBehaviour
{
    public static WaveVR_EmulatorControllerProvider Instance
    {
        get
        {
            if (instance == null)
            {
                var gameObject = new GameObject("PhoneRemote");
                instance = gameObject.AddComponent<WaveVR_EmulatorControllerProvider>();
                // This object should survive all scene transitions.
                GameObject.DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }
    private static WaveVR_EmulatorControllerProvider instance = null;
    WaveVR_Utils.WVR_ButtonState_t In_state_right, In_state_left;
    WVR_PoseState_t In_pose_right, In_pose_left;
    WVR_Axis_t In_axis_right, In_axis_left;

    // Use this for initialization
	void Start () {
	
	}
	
    public static ulong Input_Mask_Menu        = 1UL << (int)WVR_InputId.WVR_InputId_Alias1_Menu;
    public static ulong Input_Mask_Grip        = 1UL << (int)WVR_InputId.WVR_InputId_Alias1_Grip;
    public static ulong Input_Mask_Touchpad    = 1UL << (int)WVR_InputId.WVR_InputId_Alias1_Touchpad;
    public static ulong Input_Mask_Trigger     = 1UL << (int)WVR_InputId.WVR_InputId_Alias1_Trigger;

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))        // left mouse key
        {
			Debug.Log ("WaveVR_EmulatorControllerProvider, mouse left button down");
            //In_state_left.BtnPressed |= Input_Mask_Touchpad;
            //In_state_left.BtnPressed |= Input_Mask_Menu;
            //In_state_left.BtnPressed |= Input_Mask_Grip;
            //In_state_left.BtnPressed |= Input_Mask_Trigger;
            In_state_left.BtnTouched |= Input_Mask_Touchpad;
            In_state_left.BtnTouched |= Input_Mask_Menu;
            In_state_left.BtnTouched |= Input_Mask_Grip;
            In_state_left.BtnTouched |= Input_Mask_Trigger;
            In_pose_left.Velocity.v0 = 0.0f;
            In_pose_left.Velocity.v1 = 0.0f;
            In_pose_left.Velocity.v2 = 0.0f;
            In_pose_left.AngularVelocity.v0 = 0.0f; 
            In_pose_left.AngularVelocity.v1 = 0.0f;
            In_pose_left.AngularVelocity.v2 = 0.0f;
            In_axis_left.x = 0.0f;
            In_axis_left.y = 0.0f;
        }
        else if (Input.GetMouseButtonUp(0))     // left mouse key
        {
            Debug.Log ("WaveVR_EmulatorControllerProvider, mouse left button up");
            In_state_left.BtnPressed = 0x0;
            In_state_left.BtnTouched = 0x0;
        }
		else if (Input.GetMouseButtonDown(1))   // right mouse key
        {
			Debug.Log ("WaveVR_EmulatorControllerProvider, mouse right button down");
            In_state_right.BtnPressed |= Input_Mask_Touchpad;
            In_state_right.BtnPressed |= Input_Mask_Menu;
            In_state_right.BtnPressed |= Input_Mask_Grip;
            In_state_right.BtnPressed |= Input_Mask_Trigger;
            //In_state_right.BtnTouched |= Input_Mask_Touchpad;
            //In_state_right.BtnTouched |= Input_Mask_Menu;
            //In_state_right.BtnTouched |= Input_Mask_Grip;
            //In_state_right.BtnTouched |= Input_Mask_Trigger;
            In_pose_right.Velocity.v0 = 0.0f;
            In_pose_right.Velocity.v1 = 0.0f;
            In_pose_right.Velocity.v2 = 0.1f;
            In_pose_right.AngularVelocity.v0 = 0.0f;
            In_pose_right.AngularVelocity.v1 = 0.0f;
            In_pose_right.AngularVelocity.v2 = 0.1f;
            In_axis_right.x = 0.1f;
            In_axis_right.y = 0.1f;
        }
        else if (Input.GetMouseButtonUp(1))   // right mouse key
        {
            Debug.Log ("WaveVR_EmulatorControllerProvider, mouse right button up");
            //In_state_right.ulButtonPressed &= (0ul << (int)WVR_InputId.WVR_InputId_Alias1_Trigger);
            In_state_right.BtnPressed = 0x0;
            In_state_right.BtnTouched = 0x0;
        }
    }

    public bool WVR_GetInputButtonState(WVR_DeviceType _dt, WVR_InputId _id)
    {
        bool _ret = false;

        switch (_dt)
        {
        case WVR_DeviceType.WVR_DeviceType_Controller_Left:
            switch (_id)
            {
            case WVR_InputId.WVR_InputId_Alias1_Menu:
                if ((In_state_left.BtnPressed & Input_Mask_Menu) != 0)
                    _ret = true;
                break;
            case WVR_InputId.WVR_InputId_Alias1_Grip:
                if ((In_state_left.BtnPressed & Input_Mask_Grip) != 0)
                    _ret = true;
                break;
            case WVR_InputId.WVR_InputId_Alias1_Touchpad:
                if ((In_state_left.BtnPressed & Input_Mask_Touchpad) != 0)
                    _ret = true;
                break;
            case WVR_InputId.WVR_InputId_Alias1_Trigger:
                if ((In_state_left.BtnPressed & Input_Mask_Trigger) != 0)
                    _ret = true;
                break;
            default:
                break;
            }
            break;
        case WVR_DeviceType.WVR_DeviceType_Controller_Right:
            switch (_id)
            {
            case WVR_InputId.WVR_InputId_Alias1_Menu:
                if ((In_state_right.BtnPressed & Input_Mask_Menu) != 0)
                    _ret = true;
                break;
            case WVR_InputId.WVR_InputId_Alias1_Grip:
                if ((In_state_right.BtnPressed & Input_Mask_Grip) != 0)
                    _ret = true;
                break;
            case WVR_InputId.WVR_InputId_Alias1_Touchpad:
                if ((In_state_right.BtnPressed & Input_Mask_Touchpad) != 0)
                    _ret = true;
                break;
            case WVR_InputId.WVR_InputId_Alias1_Trigger:
                if ((In_state_right.BtnPressed & Input_Mask_Trigger) != 0)
                    _ret = true;
                break;
            default:
                break;
            }
            break;
        default:
            break;
        }

        return _ret;
    }

    public bool WVR_GetInputTouchState(WVR_DeviceType _dt, WVR_InputId _id)
    {
        bool _ret = false;

        switch (_dt)
        {
        case WVR_DeviceType.WVR_DeviceType_Controller_Left:
            switch (_id)
            {
            case WVR_InputId.WVR_InputId_Alias1_Menu:
                if ((In_state_left.BtnTouched & Input_Mask_Menu) != 0)
                    _ret = true;
                break;
            case WVR_InputId.WVR_InputId_Alias1_Grip:
                if ((In_state_left.BtnTouched & Input_Mask_Grip) != 0)
                    _ret = true;
                break;
            case WVR_InputId.WVR_InputId_Alias1_Touchpad:
                if ((In_state_left.BtnTouched & Input_Mask_Touchpad) != 0)
                    _ret = true;
                break;
            case WVR_InputId.WVR_InputId_Alias1_Trigger:
                if ((In_state_left.BtnTouched & Input_Mask_Trigger) != 0)
                    _ret = true;
                break;
            default:
                break;
            }
            break;
        case WVR_DeviceType.WVR_DeviceType_Controller_Right:
            switch (_id)
            {
            case WVR_InputId.WVR_InputId_Alias1_Menu:
                if ((In_state_right.BtnTouched & Input_Mask_Menu) != 0)
                    _ret = true;
                break;
            case WVR_InputId.WVR_InputId_Alias1_Grip:
                if ((In_state_right.BtnTouched & Input_Mask_Grip) != 0)
                    _ret = true;
                break;
            case WVR_InputId.WVR_InputId_Alias1_Touchpad:
                if ((In_state_right.BtnTouched & Input_Mask_Touchpad) != 0)
                    _ret = true;
                break;
            case WVR_InputId.WVR_InputId_Alias1_Trigger:
                if ((In_state_right.BtnTouched & Input_Mask_Trigger) != 0)
                    _ret = true;
                break;
            default:
                break;
            }
            break;
        default:
            break;
        }

        return _ret;
    }

    public WVR_Axis_t WVR_GetInputAnalogAxis(WVR_DeviceType _dt, WVR_InputId _id)
    {
        WVR_Axis_t _axis;
        _axis.x = 0;
        _axis.y = 0;

        switch (_dt)
        {
        case WVR_DeviceType.WVR_DeviceType_Controller_Left:
            _axis = In_axis_left;
            break;
        case WVR_DeviceType.WVR_DeviceType_Controller_Right:
            _axis = In_axis_right;
            break;
        default:
            break;
        }

        return _axis;
    }

    public void GetControllerStateWithPose(WVR_DeviceType index, ref WaveVR_Utils.WVR_ButtonState_t state, ref WVR_PoseState_t pose, ref WVR_Axis_t axis)
    {
        if (index == WVR_DeviceType.WVR_DeviceType_Controller_Right)
        {
            state = In_state_right;
            pose = In_pose_right;
            axis = In_axis_right;
        }
        if (index == WVR_DeviceType.WVR_DeviceType_Controller_Left)
        {
            state = In_state_left;
            pose = In_pose_left;
            axis = In_axis_left;
        }
    }

    public void TriggerHapticPulse(WVR_DeviceType device, WVR_InputId id, ushort durationMicroSec)
    {

    }
}