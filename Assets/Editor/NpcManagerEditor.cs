using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CustomEditor(typeof(NPCManager))]
public class NpcManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NPCManager manager = (NPCManager)target;

        if (GUILayout.Button("Kill Random"))
        {
            List<GameObject> enemies = manager.roomEnemies[RoomManager.Instance.GetCurrentRoom()];
            if(enemies.Count > 0)
            {
                GameObject g = enemies.ElementAt(Random.Range(0, enemies.Count));
                Destroy(g);
            }
        }

        if (GUILayout.Button("Knockback Random"))
        {
            List<GameObject> enemies = manager.roomEnemies[RoomManager.Instance.GetCurrentRoom()];
            if (enemies.Count > 0)
            {
                GameObject player = GameManager.Instance.GetPlayer();
                GameObject g = enemies.First();//enemies.ElementAt(Random.Range(0, enemies.Count));

                Vector2 knockVec = (g.transform.position - player.transform.position).normalized;

                g.GetComponent<Rigidbody2D>().velocity = knockVec * 2f;

                //Destroy(g);
            }
        }

    }



}

