using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        GameManager gameManager = (GameManager)target;


        if(GUILayout.Button("Spawn Enemies"))
        {
            gameManager.SpawnEnemies();
        }


    }

}

