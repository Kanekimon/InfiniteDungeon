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
    public float SpeedModifier = 1;

    public Dictionary<Room, List<GameObject>> roomEnemies = new Dictionary<Room, List<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    private void Start()
    {

    }

    /// <summary>
    /// Spawns enemies from a specific point
    /// </summary>
    /// <param name="r"></param>
    /// <param name="position"></param>
    public void SpawnFromPosition(Room r, Vector2 position)
    {
        Boundary bounds = r.bounds;

        roomEnemies[r].Add(InstantiateEnemy(r, (int)position.x, (int)position.y));
    }

    /// <summary>
    /// Spawns enemies at random positions within the room
    /// </summary>
    /// <param name="r"></param>
    public void SpawnRandom(Room r)
    {
        Boundary bounds = r.bounds;
        int randX = UnityEngine.Random.Range(bounds.startX, bounds.endX);
        int randY = UnityEngine.Random.Range(bounds.startY, bounds.endY);
        roomEnemies[r].Add(InstantiateEnemy(r, randX, randY));
    }

    /// <summary>
    /// Instatiates an enemy
    /// </summary>
    /// <param name="r">The room</param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public GameObject InstantiateEnemy(Room r, int x, int y)
    {
        GameObject e = Instantiate(Resources.Load<GameObject>("Prefabs/Enemo" + UnityEngine.Random.Range(0, 2)));
        NpcBase nBase = e.GetComponent<NpcBase>();
        nBase.SetDifficulty(r);
        e.transform.position = new Vector3(x, y, 0);
        return e;
    }


    /// <summary>
    /// Base Method for spawning enemies
    /// </summary>
    /// <param name="r"></param>
    /// <param name="sp"></param>
    /// <param name="startPos">Spawns enemies from this position, if null, spawns at random</param>
    public void SpawnEnemies(Room r, int sp, Vector2? startPos = null)
    {
        Boundary bounds = r.GetBoundary();

        int spC = sp;

        if (!roomEnemies.ContainsKey(r))
            roomEnemies.Add(r, new List<GameObject>());
        else
            spC = roomEnemies[r].Count;



        for (int i = 0; i < sp; i++)
        {
            if (startPos != null)
                SpawnFromPosition(r,startPos ?? new Vector2(0,0));
            else
                SpawnRandom(r);
        }


    }

    /// <summary>
    /// Spawns the saved amount of enemies
    /// </summary>
    /// <param name="enemies"></param>
    /// <param name="r"></param>
    public void SpawnFromSave(Dictionary<int, int> enemies, Room r)
    {
        Boundary bounds = r.bounds;
        foreach (KeyValuePair<int, int> keyVal in enemies)
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

    /// <summary>
    /// Removes an entity from the room list
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    public void RemoveFromRoomList(Room r, GameObject g)
    {
        if (roomEnemies.ContainsKey(r))
        {
            if (roomEnemies[r].Contains(g))
                roomEnemies[r].Remove(g);
        }
    }


}

