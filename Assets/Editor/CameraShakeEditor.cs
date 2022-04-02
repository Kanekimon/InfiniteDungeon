using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraShake))]
public class CameraShakeEditor : Editor
{
    float duration;
    float magnitude;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CameraShake shake = (CameraShake)target;

        duration = float.Parse(EditorGUILayout.TextField("" + duration));
        magnitude = float.Parse(EditorGUILayout.TextField("" + magnitude));

        if (GUILayout.Button("Shake"))
        {
            shake.Shake(duration, magnitude);
        }

    }

}
