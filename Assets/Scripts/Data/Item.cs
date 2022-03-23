using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField]
    private int itemId;
    [SerializeField]
    private string itemName;

    [JsonProperty(Order = 1)]
    public int ItemId { get { return itemId; } set { itemId = value; } }

    [JsonProperty(Order = 2)]
    public string ItemName { get { return itemName; } set { itemName = value; } }

    [JsonProperty(Order = 3)]
    public List<ItemType> itemsTypes;

    [JsonProperty(Order = 4)]
    public EquipmentType equipmentType;


    [JsonProperty(Order = 5)]
    public List<ItemProperty> properties;



    public Item(int id, string name, EquipmentType type)
    {
        this.itemId = id;
        this.itemName = name;
        properties = new List<ItemProperty>();
        itemsTypes = new List<ItemType>();
        equipmentType = type;
    }

    /// <summary>
    /// Addds a proptery to an item
    /// </summary>
    /// <param name="iProp"></param>
    public void AddItemProperty(ItemProperty iProp)
    {
        ItemProperty prop = GetItemProperty(iProp.Name);
        if (prop == null)
            properties.Add(iProp);
        else
        {
            prop.Value += iProp.Value;
        }
    }

    /// <summary>
    /// Removes a property from an item
    /// </summary>
    /// <param name="name"></param>
    public void RemoveItemProperty(string name)
    {
        if (HasItemProptery(name))
        {
            properties.Remove(GetItemProperty(name));
        }
    }

    /// <summary>
    /// Adds an item type to the item
    /// </summary>
    /// <param name="type"></param>
    public void AddItemType(ItemType type)
    {
        if (!itemsTypes.Contains(type))
            itemsTypes.Add(type);
    }

    /// <summary>
    /// Removes an itemtype from the item
    /// </summary>
    /// <param name="type"></param>
    public void RemoveItemType(ItemType type)
    {
        if (itemsTypes.Contains(type))
            itemsTypes.Remove(type);
    }

    /// <summary>
    /// Checks if item has a specifc property
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool HasItemProptery(string name)
    {
        return properties.Any(a => a.Name.Equals(name));
    }


    /// <summary>
    /// Returns a Itemproperty by name
    /// </summary>
    /// <param name="name">Name of property</param>
    /// <returns></returns>
    public ItemProperty GetItemProperty(string name)
    {
        return properties.Where(a => a.Name.Equals(name)).FirstOrDefault();
    }

}

