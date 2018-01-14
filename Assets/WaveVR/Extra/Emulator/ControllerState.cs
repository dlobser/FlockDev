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

/// @cond
namespace WaveVREmulator {
    public class ControllerState
    {
        // Orientation
        internal EmuConnectionState connectionState = EmuConnectionState.Disconnected;
        internal Quaternion orientation = Quaternion.identity;

        // Pad key
        internal bool padButtonState = false;   // true = clicked
        internal bool padButtonDown = false;
        internal bool padButtonUp = false;

        // Menu key
        internal bool menuButtonState = false;  // true = clicked
        internal bool menuButtonDown = false;
        internal bool menuButtonUp = false;

        // System key
        internal bool sysButtonState = false;   // true = clicked
        internal bool sysButtonDown = false;
        internal bool sysButtonUp = false;

        public void CopyFrom(ControllerState other)
        {
            // Orientation
            orientation = other.orientation;

            // Pad key
            padButtonState = other.padButtonState;
            padButtonDown = other.padButtonDown;
            padButtonUp = other.padButtonUp;

            // Menu key
            menuButtonState = other.menuButtonState;
            menuButtonDown = other.menuButtonDown;
            menuButtonUp = other.menuButtonUp;

            // System key
            sysButtonState = other.sysButtonState;
            sysButtonDown = other.sysButtonDown;
            sysButtonUp = other.sysButtonUp;
        }

        /// Resets the transient state (the state variables that represent events, and which are true
        /// for only one frame).
        public void ClearTransientState()
        {
            // TBD
        }
    }
}
/// @endcond