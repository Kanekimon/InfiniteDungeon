using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveGameLoader : MonoBehaviour
{
    public static string saveUrl = @"D:\Unity Workspace\InfiniteDungeon\Assets\SaveData";
    public GameObject saveGameWindow;
    public GameObject savePref;



    /// <summary>
    /// Displays all existing savegames
    /// And Starts the one which the player clicks on
    /// </summary>
    private void Start()
    {
        DirectoryInfo saveDir = new DirectoryInfo(saveUrl);
        FileInfo[] files = saveDir.GetFiles("*.txt");
        for (int i = 0; i < 3; i++)
        {
            GameObject g = saveGameWindow.transform.GetChild(i).gameObject;

            if (i < files.Length)
            {
                g.transform.GetChild(0).GetComponent<Text>().text = files[i].LastWriteTime.ToString();
                EventTrigger et = g.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventdata) =>
                {
                    MenuManager.Instance.LoadGame(int.Parse(g.name.Replace("Save", "")));
                });
                et.triggers.Add(entry);
            }
        }
    }


    public static List<int> GetSaveGames()
    {
        List<int> saveGames = new List<int>();
        DirectoryInfo saveDir = new DirectoryInfo(saveUrl);
        FileInfo[] files = saveDir.GetFiles("*.txt");
        for (int i = 0; i < files.Length; i++)
        {
            saveGames.Add(i);

        }

        return saveGames;
    }

}

