using Assets.Scripts.Enum;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Generator
{
    public static class RoomGenerator
    {

        //public static Room CreateHub()
        //{

        //}


        public static Room CreateFromPreset(string name)
        {
            GameObject hub = RoomManager.Instance.Hub;
            if (hub == null)
            {
                hub = new GameObject("Hub_Container");
                RoomManager.Instance.Hub = hub;
                
            }
            string json = Resources.Load<TextAsset>($"Rooms/{name}").text;

            Room hub_room = JsonConvert.DeserializeObject<Room>(json);

            GameObject room = new GameObject(name);
            room.transform.parent = hub.transform;
            int x = 0;
            int y = 0;
            string tD = hub_room.GetTileData();
            for (int i = 0; i < tD.Length; i++)
            {
                if (x >= hub_room.xLength)
                {
                    x = 0;
                    y++;
                }

                int index = (x) + (y * (hub_room.xLength));
                GenerateTile(room, x + hub_room.bounds.startX, y + hub_room.bounds.startY, (TileType)int.Parse(tD[index].ToString()), hub_room);
                x++;
            }

            hub_room.SetParent(hub);

            return hub_room;
        }


        static GameObject GenerateTile(GameObject parentRoom, float x, float y, TileType tileType, Room r)
        {
            GameObject w = GameObject.Instantiate(GetTileFromType(tileType));
            w.GetComponent<SpriteRenderer>().sprite = GetSpriteForBiome(r.biome, tileType);
            w.transform.position = new Vector3(x, y, 2);
            w.transform.parent = parentRoom.transform;

            if (tileType == TileType.door)
            {
                r.SetupDoor(new Vector2(x, y), w);
            }
            if (tileType == TileType.floor)
                w.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Textures/Tiles/{r.biome}/base");
            if (tileType == TileType.path)
                w.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Textures/Tiles/{r.biome}/path");


            return w;
        }

        static Sprite GetSpriteForBiome(Biome b, TileType t)
        {
            Sprite sprite = Resources.Load<Sprite>($"Textures/Tiles/{b}/{t}");

            if (sprite == null)
                sprite = Resources.Load<Sprite>($"Textures/Tiles/{t}");

            return sprite;
        }

        static GameObject GetTileFromType(TileType tileType)
        {
            return Resources.Load<GameObject>($"tiles/{tileType.ToString()}");
        }


    }
}
