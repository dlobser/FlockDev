﻿// ViveBuildTogglerEditor.cs
// Created by Holojam Inc. on 01.03.17

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Holojam.Vive {

  [CustomEditor(typeof(ViveBuildToggler))]
  public class ViveBuildTogglerEditor : Editor {

    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      PlayerSettings.virtualRealitySupported =
        !((ViveBuildToggler)serializedObject.targetObject).masterClientBuild;

      EditorGUILayout.LabelField("Virtual Reality Supported: " + PlayerSettings.virtualRealitySupported);
    }
  }
}

