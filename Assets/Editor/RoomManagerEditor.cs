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
    string showButton = "Show All Rooms";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RoomManager roomManager = (RoomManager)target;

        if(GUILayout.Button(showButton))
        {
            if (showButton.Contains("Show"))
            {
                showButton = "Hide All Rooms";
                roomManager.ToggleRooms(true);
            }
            else
            {
                showButton = "Show All Rooms";
                roomManager.ToggleRooms(false);
            }
        }

        if (GUILayout.Button("Serialize"))
        {
            roomManager.SerializeRoom();
        }

    }


}

