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

/// @cond
namespace WaveVREmulator {
    public enum EmuConnectionState
    {
        /// Indicates that an error has occurred.
        Error = -1,
        /// Indicates that the controller is disconnected.
        Disconnected = 0,
        /// Indicates that the device is scanning for controllers.
        Scanning = 1,
        /// Indicates that the device is connecting to a controller.
        Connecting = 2,
        /// Indicates that the device is connected to a controller.
        Connected = 3,
    };

    public class ControllerProvider
    {
        private ControllerState state = new ControllerState();

        // ---------------------- Rotation begins -----------------------
        private Quaternion lastRawOrientation = Quaternion.identity;
        private Quaternion yawCorrection = Quaternion.identity;

        private static Quaternion ConvertEmulatorQuaternion(Quaternion emulatorQuat) {
            // Convert from the emulator's coordinate space to Unity's standard coordinate space.
            return new Quaternion(emulatorQuat.x, -emulatorQuat.z, emulatorQuat.y, emulatorQuat.w);
        }

        private void HandleOrientationEvent(EmulatorOrientationEvent orientationEvent) {
            lastRawOrientation = ConvertEmulatorQuaternion(orientationEvent.orientation);
            lock (state) {
                state.orientation = yawCorrection * lastRawOrientation;
            }
        }
        // ---------------------- Rotation ends ---------------------------

        // ---------------------- Button begins ---------------------------
        private void HandleButtonEvent(EmulatorButtonEvent buttonEvent) {
            lock (state) {
                if (buttonEvent.code == EmulatorButtonEvent.ButtonCode.kPad)
                {
                    state.padButtonState = buttonEvent.down;
                    state.padButtonDown = buttonEvent.down;
                    state.padButtonUp = !buttonEvent.down;
                } else if (buttonEvent.code == EmulatorButtonEvent.ButtonCode.kMenu)
                {
                    state.menuButtonState = buttonEvent.down;
                    state.menuButtonDown = buttonEvent.down;
                    state.menuButtonUp = !buttonEvent.down;
                } else if (buttonEvent.code == EmulatorButtonEvent.ButtonCode.kSys)
                {
                    state.sysButtonState = buttonEvent.down;
                    state.sysButtonDown = buttonEvent.down;
                    state.sysButtonUp = !buttonEvent.down;
                }
            }
        }
        // ---------------------- Button ends -----------------------------

        public void ReadState(ControllerState outState) {
            lock (state) {
                state.connectionState = SocketManager.Instance.Connected ? EmuConnectionState.Connected : EmuConnectionState.Connecting;
                outState.CopyFrom(state);
            }
            state.ClearTransientState();
        }

        internal ControllerProvider ()
        {
            Debug.Log ("ControllerProvider, initialize SocketManager");
            SocketManager.Instance.orientationEventListeners += HandleOrientationEvent;
            SocketManager.Instance.buttonEventListeners += HandleButtonEvent;
        }
    }
}
/// @endcond