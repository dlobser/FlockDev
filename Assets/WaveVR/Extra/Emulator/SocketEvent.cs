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

using proto;

namespace WaveVREmulator {
    struct EmulatorOrientationEvent {
        public readonly long timestamp;
        public readonly Quaternion orientation;

        public EmulatorOrientationEvent(PhoneEvent.Types.OrientationEvent proto) {
            timestamp = proto.Timestamp;
            // Convert from right-handed coordinates to left-handed.
            orientation = new Quaternion(proto.X, proto.Y, -proto.Z, proto.W);
        }
    }

    struct EmulatorButtonEvent {
        // Codes as reported by the IC app (reuses Android KeyEvent codes).
        public enum ButtonCode {
            kNone = 0,
            kPad = 66,
            kMenu = 82,
            kSys = 3
        }

        public readonly ButtonCode code;
        public readonly bool down;
        public EmulatorButtonEvent(PhoneEvent.Types.KeyEvent proto) {
            code = (ButtonCode) proto.Code;
            down = proto.Action == 0;
        }
    }
}