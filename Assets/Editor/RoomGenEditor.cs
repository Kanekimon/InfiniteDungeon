using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomGeneratorManager))]
public class RoomGenEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RoomGeneratorManager roomGeneratorManager = (RoomGeneratorManager)target;

        if(GUILayout.Button("Generate Room"))
        {
            //roomGeneratorManager.GenerateRoom();
        }
    }

}
