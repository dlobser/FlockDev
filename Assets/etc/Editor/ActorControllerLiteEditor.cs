using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActorAvatarLite)), CanEditMultipleObjects]
public class ActorAvatarLiteEditor : Holojam.Tools.ActorEditor{
   SerializedProperty mask, motif;
   protected override void EnableDerived(){
      motif = serializedObject.FindProperty("motif");
      mask = serializedObject.FindProperty("mask");
   }
   protected override void DrawDerived(){
      EditorGUILayout.PropertyField(motif);
      EditorGUILayout.PropertyField(mask);
   }
}
