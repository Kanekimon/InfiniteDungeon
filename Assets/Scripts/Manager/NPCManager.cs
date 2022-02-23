using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;

    public int spawnCount;

    public Dictionary<Room, List<GameObject>> roomEnemies = new Dictionary<Room, List<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    public void SpawnEnemies(Room r)
    {
        Boundary bounds = r.GetBoundary();

        if (!roomEnemies.ContainsKey(r))
            roomEnemies.Add(r, new List<GameObject>());

        for (int i = 0; i < spawnCount; i++)
        {
            int randX = UnityEngine.Random.Range(bounds.startX, bounds.endX);
            int randY = UnityEngine.Random.Range(bounds.startY, bounds.endY);

            GameObject e = Instantiate(Resources.Load<GameObject>("Prefabs/Enemo"));

            e.transform.position = new Vector3(randX, randY, 0);
            r.AddEnemyToRoom(e.GetComponent<NpcBase>().GetId(),1);

            roomEnemies[r].Add(e);
        }
    }

    public void SpawnRandomPosition()
    {
        GameObject e = Instantiate(Resources.Load<GameObject>("Prefabs/Enemo"));
    }

    public void SpawnFromSave(Dictionary<int, int> enemies, Room r)
    {
        Boundary bounds = r.bounds;
        foreach(KeyValuePair<int, int> keyVal in enemies)
        {
            for (int i = 0; i < keyVal.Value; i++)
            {
                int randX = UnityEngine.Random.Range(bounds.startX, bounds.endX);
                int randY = UnityEngine.Random.Range(bounds.startY, bounds.endY);

                GameObject e = Instantiate(Resources.Load<GameObject>("Prefabs/Enemo"));

                e.transform.position = new Vector3(randX, randY, 0);
            }
        }
    }

    public void RemoveFromRoomList(Room r, GameObject g)
    {
        if (roomEnemies.ContainsKey(r))
        {
            if (roomEnemies[r].Contains(g))
                roomEnemies[r].Remove(g);
        }
    }


}

