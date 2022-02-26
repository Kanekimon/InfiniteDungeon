using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SaveStateManager : MonoBehaviour
{
    public static string saveUrl = @"D:\Unity Workspace\InfiniteDungeon\Assets\SaveData";
    public static SaveStateManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    /// <summary>
    /// Saves a game inside a json script
    /// A save has an index
    /// </summary>
    /// <param name="index"></param>
    public void SaveGame(int index)
    {
        var settings = new Newtonsoft.Json.JsonSerializerSettings();
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;


        SaveData s = CreateSaveDataObject();


        string json = JsonConvert.SerializeObject(s, settings);

        string fileUrl = Path.Combine(saveUrl, $"saveGame{index}.txt");

        using (StreamWriter sw = new StreamWriter(fileUrl))
        {
            sw.Write(json);
        }
    }

    /// <summary>
    /// Loads a game state with selected index
    /// Index was set by selecting a specific save from the menu
    /// </summary>
    public void LoadGame()
    {
        int saveIndex = StartParameters.saveGame;

        string fileUrl = Path.Combine(saveUrl, $"saveGame{saveIndex}.txt");
        StreamReader sr = new StreamReader(fileUrl);
        SaveData s = JsonConvert.DeserializeObject<SaveData>(sr.ReadToEnd());

        RoomManager.Instance.StartFromSave(s.rooms, s.playerRoomIndex, s.playerPosition);
        GameManager.Instance.GetPlayer().GetComponent<PlayerSystem>().LoadAttributes(s.playerAtts);
    }

    /// <summary>
    /// Create the actuall SaveData object with all 
    /// needed attributes
    /// </summary>
    /// <returns>Complete SaveData</returns>
    public SaveData CreateSaveDataObject()
    {

        Vector2 playerPos = (Vector2)GameManager.Instance.GetPlayer().transform.position;
        Vector2 playerRoomIndex = RoomManager.Instance.currentRoom.index;

        foreach (Room r in RoomManager.Instance.roomMap)
        {
            r.GenerateTileDataString();
        }

        List<Room> rooms = RoomManager.Instance.roomMap;
        float currency = 0;

        SaveData s = new SaveData(playerRoomIndex, playerPos, currency, rooms, GameManager.Instance.GetPlayer().GetComponent<AttributeSystem>().attributes);

        return s;
    }

}

