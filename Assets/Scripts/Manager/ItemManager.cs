using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ItemManager : MonoBehaviour
{


    public static ItemManager Instance;
    public List<Item> items = new List<Item>(); 


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);


        LoadItemsFromResources();
        SaveItems();
    }


    void LoadItemsFromResources()
    {
        string url = @"D:\Unity Workspace\InfiniteDungeon\Assets\Resources\items\allitems.json";
        StreamReader streamReader = new StreamReader(url);

        items = JsonConvert.DeserializeObject<List<Item>>(streamReader.ReadToEnd());
        streamReader.Close();
    }

    void SaveItems()
    {
        string json = JsonConvert.SerializeObject(items, Formatting.Indented);


        string url = @"D:\Unity Workspace\InfiniteDungeon\Assets\Resources\items\allitems.json";

        using (StreamWriter sw = new StreamWriter(url))
        {
            sw.Write(json);
        }

    }

    public Item GetItem(int id)
    {
        return ItemFactory.CreateItem(items.Where(a => a.ItemId == id).FirstOrDefault());
    }

}

