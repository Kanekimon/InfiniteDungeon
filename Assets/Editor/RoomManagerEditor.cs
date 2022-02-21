using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RoomManager roomManager = (RoomManager)target;

        if (GUILayout.Button("Serialize"))
        {
            roomManager.SerializeRoom();

        }

    }


}

