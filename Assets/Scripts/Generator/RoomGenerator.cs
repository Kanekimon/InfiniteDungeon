using Assets.Scripts.Data.StaticData;
using Assets.Scripts.Enum;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Generator
{
    public static class RoomGenerator
    {
        /// <summary>
        /// Creates a room from a preset
        /// Mainly used for the hub, but could be altered for specific run-related 
        /// rooms such as boss or treasure rooms
        /// </summary>
        /// <param name="name">Name of the preset</param>
        /// <returns></returns>
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


        /// <summary>
        /// Generates a Tile at a given position and adds the relevant sprite to it
        /// </summary>
        /// <param name="parentRoom"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tileType"></param>
        /// <param name="r"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Returns a sprite for a tiletype from a biome
        /// If no tile can be found for the dungeon, the default one are used
        /// </summary>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        static Sprite GetSpriteForBiome(Biome b, TileType t)
        {
            Sprite sprite = Resources.Load<Sprite>($"Textures/Tiles/{b}/{t}");

            if (sprite == null)
                sprite = Resources.Load<Sprite>($"Textures/Tiles/{t}");

            return sprite;
        }

        /// <summary>
        /// Returns the prefab of a tile given by the resourceType
        /// </summary>
        /// <param name="tileType"></param>
        /// <returns></returns>
        static GameObject GetTileFromType(TileType tileType)
        {
            return Resources.Load<GameObject>($"tiles/{tileType.ToString()}");
        }

        /// <summary>
        /// Returns a Biome based on the direction the player is going
        /// TODO: Better evaluation of biomes
        /// </summary>
        /// <param name="r">The next room</param>
        /// <param name="d">Direction relative to last room</param>
        /// <returns></returns>
        static Biome GetBiome(Room r, Direction d)
        {

            BiomeMatrix bMatrix = new BiomeMatrix();

            //Debug.Log($"Get biome for room ({index.x} | {index.y})");

            //Biome biome = Biome.grassland;
            //List<Biome> nBiomes = new List<Biome>();
            //for (int i = 0; i < 8; i++)
            //{
            //    Vector2 nIndex = index + new Vector2(maskX[i], maskY[i]);
            //    Room nR = RoomManager.Instance.GetRoomFromIndex(nIndex);
            //    if (nR != null)
            //    {
            //        nBiomes.Add(nR.biome);
            //    }

            //}

            List<Biome> pos = bMatrix.GetPossibleBiomeDirection(r, d);//bMatrix.GetPossibleBiomes(nBiomes);

            if (pos.Count == 0)
                return Biome.grassland;

            foreach (Biome b in pos)
            {
                Debug.Log($"Possible biome: {b}");
            }


            return pos.ElementAt(Random.Range(0, pos.Count));
        }


        static Biome GetBiomeBasedOnIndex(Room r)
        {
            int x = (int)r.index.x;
            int y = (int)r.index.y;

            if (x == 0 && y == 0)
                return Biome.grassland;

            float value = (float)((Mathf.Atan2(x, y) / Mathf.PI) * 180f);
            if (value < 0) value += 360f;

            Debug.Log($"Angle: {value}");

            if ((value >= 315) || (value < 45))
                return Biome.snowy;
            if (value >= 45 && value < 135)
                return Biome.earth;
            if (value >= 135 && value < 225)
                return Biome.desert;
            if (value >= 225 && value < 315)
                return Biome.grassland;




            return Biome.grassland;
        }

        /// <summary>
        /// Gets the new startpoint for the room, relative to the 
        /// direction the player came from
        /// </summary>
        /// <param name="d">Direction of this room, relative to the last one</param>
        /// <param name="r">The new Room</param>
        /// <returns></returns>
        public static Vector2 GetRoomStartPoint(Direction d, Room r)
        {
            Vector2 startPointPath = new Vector2();
            Boundary b = r.GetBoundary();
            if (d == Direction.north)
            {
                startPointPath = new Vector2(r.center.x, b.startY + 1);
            }
            else if (d == Direction.east)
            {
                startPointPath = new Vector2(b.startX + 1, r.center.y);
            }
            else if (d == Direction.south)
            {
                startPointPath = new Vector2(r.center.x, b.startY + r.yLength - 2);
            }
            else if (d == Direction.west)
            {
                startPointPath = new Vector2(b.startX + r.xLength - 2, r.center.y);
            }

            return startPointPath;
        }


        /// <summary>
        /// Is the given TileType an obstacle or not
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        static bool IsObstacle(TileType t)
        {
            return (t != TileType.floor && t != TileType.path);
        }

        /// <summary>
        /// Gets the correct tiletype for a given position
        /// </summary>
        /// <param name="x">x coordinate of tile</param>
        /// <param name="y">y coordinate of tile</param>
        /// <param name="xSize">width of the room</param>
        /// <param name="ySize">length of the room</param>
        /// <param name="xCord">the world x coordinate</param>
        /// <param name="yCord">the world y coordinate</param>
        /// <param name="path">the created path</param>
        /// <returns></returns>
        static TileType GetTileTypeForPosition(int x, int y, int xSize, int ySize, int xCord, int yCord, List<Vector2> path)
        {
            if ((x == (xSize / 2) && y == 0) || (x == (xSize / 2) && y == ySize - 1) || (x == 0 && y == (ySize / 2)) || (x == xSize - 1 && y == (ySize / 2)))
                return TileType.door;

            if ((y == 0 || y == ySize - 1) || x == 0 || x == xSize - 1)
                return TileType.wall;

            if (path.Count > 0 && path.Contains(new Vector2(xCord, yCord)))
                return TileType.path;

            return TileType.floor;
        }



        #region Generation Methods
        #region Initial Generation
        /// <summary>
        /// Generate the room with a given size
        /// </summary>
        /// <param name="index">index relative to the other rooms</param>
        /// <param name="dir"></param>
        /// <param name="xSize"></param>
        /// <param name="ySize"></param>
        /// <returns></returns>
        public static Room GenerateRoom(Vector2 index, Direction dir, int xSize, int ySize)
        {
            Vector2 startPoint = new Vector2(index.x * xSize, index.y * ySize);

            GameObject runParent = RoomManager.Instance.GetMapParent();
            if (runParent == null)
            {
                runParent = new GameObject("Run");
                RoomManager.Instance.Run = runParent;
            }

            GameObject roomContainer = new GameObject();

            Room room = new Room(System.Guid.NewGuid().ToString(), index, xSize, ySize, (int)startPoint.x, (int)startPoint.y, roomContainer);
            room.SetBiome(GetBiomeBasedOnIndex(room));
            //room.SetBiome(GetBiome(room, dir));
            roomContainer.name = $"Room: ({index.x} | {index.y})";

            Vector2 startPointPath = GetRoomStartPoint(dir, room);
            room.playerSpawnPoint = startPointPath;

            List<Vector2> path = new List<Vector2>();
            List<Resource> resources = RandomResourceGenerator.GenerateStone(room, path, 10);

            if (startPoint != startPointPath)
            {
                path = RandomPathGenerator.GenerateRandomPath(room, startPointPath, Vector2.zero, PathMode.random);
            }

            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    int xCord = x + (int)startPoint.x;
                    int yCord = y + (int)startPoint.y;

                    Vector2 tileCoords = new Vector2(xCord, yCord);

                    TileType tileType = GetTileTypeForPosition(x, y, xSize, ySize, xCord, yCord, path);

                    GameObject tile = GenerateTile(roomContainer, xCord, yCord, tileType, room);
                    room.AddTileToRoom(tileCoords, tile, tileType);
                    room.aMap[x, y] = IsObstacle(tileType);

                    if (resources.Any(a => a.Position.Equals(tileCoords)))
                    {
                        Resource resource = resources.Where(a => a.Position.Equals(tileCoords)).FirstOrDefault();
                        RandomResourceGenerator.GenerateResource(room, tile, resource);
                        room.AddResource(resource);
                        room.aMap[x, y] = true;
                    }

                }
            }

            if (RoomManager.Instance.SpawnCorruptionCore)
                room.CreateCorruptionCore();

            roomContainer.transform.parent = runParent.transform;

            return room;
        }
        #endregion

        /// <summary>
        /// Regenerates the room from the basic data
        /// </summary>
        /// <param name="r">Room to be regenerated</param>
        /// <returns></returns>
        public static Room ReGenerateRoom(Room r)
        {

            GameObject roomParent = new GameObject($"Room: ({r.index.x} | {r.index.y})");
            foreach (Tile t in r.GetTiles())
            {
                GenerateTile(roomParent, t.x, t.y, t.type, r);
            }
            r.SetParent(roomParent);

            return r;
        }

        public static Room ReGenerateRoomFromSave(Room r)
        {
            GameObject g = new GameObject(r.Id);

            int x = 0;
            int y = 0;
            string tD = r.GetTileData();
            Debug.Log($"TD Length: {tD.Length}");
            for (int i = 0; i < tD.Length; i++)
            {
                if (x >= r.xLength)
                {
                    x = 0;
                    y++;
                }

                int index = (x) + (y * (r.xLength));
                GenerateTile(g, x + r.bounds.startX, y + r.bounds.startY, (TileType)int.Parse(tD[index].ToString()), r);
                x++;
            }

            r.SetParent(g);
            return r;
        }


        #endregion




    }
}
