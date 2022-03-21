using Assets.Scripts.Enum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Data.StaticData
{

    public class BiomeMatrix
    {
        Dictionary<Biome, List<Biome>> biomeMatrix = new Dictionary<Biome, List<Biome>>();
        Dictionary<Vector2Int, Biome> biomeMatrixDirection = new Dictionary<Vector2Int, Biome>();



        public BiomeMatrix()
        {
            List<Biome> grassComp = new List<Biome>();
            grassComp.Add(Biome.grassland);
            grassComp.Add(Biome.earth);

            biomeMatrix.Add(Biome.grassland, grassComp);

            List<Biome> desertComp = new List<Biome>();
            desertComp.Add(Biome.earth);
            desertComp.Add(Biome.desert);

            biomeMatrix.Add(Biome.desert, desertComp);

            List<Biome> earthComp = new List<Biome>();
            earthComp.Add(Biome.earth);
            earthComp.Add(Biome.desert);
            earthComp.Add(Biome.grassland);

            biomeMatrix.Add(Biome.earth, earthComp);


            Vector2Int northStart = new Vector2Int(1, 0);
            biomeMatrixDirection[northStart] = Biome.snowy;
            Vector2Int eastStart = new Vector2Int(2, 0);
            biomeMatrixDirection[eastStart] = Biome.grassland;
            Vector2Int southStart = new Vector2Int(3, 0);
            biomeMatrixDirection[southStart] = Biome.desert;
            Vector2Int westStart = new Vector2Int(4, 0);
            biomeMatrixDirection[westStart] = Biome.earth;

        }


        public List<Biome> GetPossibleBiomeDirection(Room r, Direction d)
        {
            List<Biome> possible = new List<Biome>();
            int depth = r.depth;
            int dir = (int)d;

            List<KeyValuePair<Vector2Int, Biome>> inDirection = biomeMatrixDirection.Where(a => a.Key.x == dir).ToList();
            foreach (KeyValuePair<Vector2Int, Biome> b in inDirection)
            {
                if (b.Key.y <= depth)
                    possible.Add(b.Value);

            }
            return possible;
        }

        public List<Biome> GetPossibleBiomes(List<Biome> biomes)
        {
            List<Biome> possible = new List<Biome>();

            foreach (Biome biome in biomes)
            {
                if (biomeMatrix.ContainsKey(biome))
                    possible.AddRange(biomeMatrix[biome]);
            }

            return possible;
        }
    }

}
