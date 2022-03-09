using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CorruptionCore : MonoBehaviour
{
    [SerializeField]
    public Vector2 Position { get; set; }
    [SerializeField]
    public float CorruptionStrength { get; set; }
    [SerializeField]
    public int Level{ get; set; }

    public int EnemyCount;

    public float Timer;
    public float Delay;


    public CorruptionCore(Vector2 pos, float corStr, int level)
    {
        Position = pos;
        CorruptionStrength = corStr;
        Level = level;

    }

    private void Start()
    {
        CorruptionStrength = 10;
        Level = 1;
        EnemyCount = (int)CorruptionStrength * Level;
    }

    private void Update()
    {
        if (EnemyCount > 0)
        {
            if (Timer > Delay)
            {
                NPCManager.Instance.SpawnEnemies(RoomManager.Instance.GetCurrentRoom(), 1, this.transform.position);
                EnemyCount--;
                Timer = 0;
            }
            Timer += Time.deltaTime;
        }
    }


}

