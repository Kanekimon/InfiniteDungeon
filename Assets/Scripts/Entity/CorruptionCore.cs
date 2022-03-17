﻿using UnityEngine;

public class CorruptionCore : MonoBehaviour
{
    [SerializeField]
    public Vector2 Position { get; set; }
    [SerializeField]
    public float CorruptionStrength { get; set; }
    [SerializeField]
    public int Level { get; set; }

    public int EnemyCount;

    public float Timer;
    public float Delay;
    public bool Paused;


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
        if (!Paused)
        {
            if (EnemyCount > 0)
            {
                if (Timer > Delay && !Paused)
                {
                    NPCManager.Instance.SpawnEnemies(RoomManager.Instance.GetCurrentRoom(), 1, this.transform.position);
                    EnemyCount--;
                    Timer = 0;
                }
                Timer += Time.deltaTime;
            }
        }
    }


}

