using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Vector2 playerRoomIndex;
    public Vector2 playerPosition;
    public List<Attribute> playerAtts;
    public float currency;

    public List<Room> rooms;

    /// <summary>
    /// Object that holds all information that should
    /// be saved
    /// </summary>
    /// <param name="pIndex">Index of the room in which the player is</param>
    /// <param name="pPos">Actual position of Player</param>
    /// <param name="cur">The amount of currency</param>
    /// <param name="r">List of all generated rooms</param>
    public SaveData(Vector2 pIndex, Vector2 pPos, float cur, List<Room> r, List<Attribute> pAtt)
    {
        this.playerRoomIndex = pIndex;
        this.playerPosition = pPos;
        this.currency = cur;
        this.rooms = r;
        this.playerAtts = pAtt;
    }
}

