using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class Resource
{
    public Vector2 Position;

    [JsonIgnore]
    public GameObject ResourceObject;

    public string ResourceName;

    public Resource(Vector2 position, string resName)
    {
        Position = position;
        ResourceName = resName;
    }
}

