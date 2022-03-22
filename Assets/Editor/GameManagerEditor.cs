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
            SaveStateManager.Instance.SaveGame(0);

            //gameManager.SpawnEnemies();
        }
        if(GUILayout.Button("Random Ceiling Test"))
        {
            float x = UnityEngine.Random.Range(-10f, 10f);
            float y = UnityEngine.Random.Range(-10f, 10f);
            Vector2 test = new Vector2(x, y).normalized;

            Vector2 smallest = new Vector2(float.MaxValue, float.MaxValue);
            if (Mathf.Abs(Vector2.Distance(smallest, test)) > Mathf.Abs(Vector2.Distance(test, Vector2.up)))
                smallest = Vector2.up;
            if (Mathf.Abs(Vector2.Distance(smallest, test)) > Mathf.Abs(Vector2.Distance(test, Vector2.down)))
                smallest = Vector2.down;
            if (Mathf.Abs(Vector2.Distance(smallest, test)) > Mathf.Abs(Vector2.Distance(test, Vector2.left)))
                smallest = Vector2.left;
            if (Mathf.Abs(Vector2.Distance(smallest, test)) > Mathf.Abs(Vector2.Distance(test, Vector2.right)))
                smallest = Vector2.right;


            Debug.Log($"X: {x} Y: {y} Vector2: {test} VectorToOne: {smallest}");
        }

    }

}

