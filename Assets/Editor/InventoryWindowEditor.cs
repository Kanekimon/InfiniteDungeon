﻿using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryWindow))]
public class InventoryWindowEditor : Editor
{
    string ItemId = "0";
    string ItemName = "rock";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        InventoryWindow window = (InventoryWindow)target;

        ItemId = EditorGUILayout.TextField(ItemId);
        ItemName = EditorGUILayout.TextField(ItemName);

        if (GUILayout.Button("Add Random Item"))
        {
            GameManager.Instance.GetPlayerSystem().InventorySystem.AddItem(new Item(int.Parse(ItemId), ItemName, EquipmentType.none), UnityEngine.Random.Range(1, 100));
        }


    }

}
