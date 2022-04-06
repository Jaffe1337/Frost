using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Frost
{
    public class Map
    {
        public static Tile Tile2;
        public static Tile Tile3;
        public static Tile Tile4;
        public static Tile Tile5;
        public static Tile Tile6;

        public static Tile Border;

        public static Tilemap map;

        private static void assign(Generation.Biome biome, Tile tile)
        {
            foreach (List<int> pos in biome.area)
            {
                map.SetTile(new Vector3Int(pos[0], pos[1], 0), tile);
            }
        }


        public static void updateAllTiles()
        {
            clearMap();


            for (int x = 0; x < Setup.width; x++)
            {
                for (int y = 0; y < Setup.height; y++)
                {
                    switch (Setup.obj[x, y])
                    {
                        case 2:
                            map.SetTile(new Vector3Int(x, y, 0), Tile2);
                            break;
                        case 3:
                            map.SetTile(new Vector3Int(x, y, 0), Tile3);
                            break;
                    }
                }
            }


            foreach (Generation.Biome biome in Generation.biomes.Values)
            {
                switch (biome.tileId)
                {
                    case 4:
                        assign(biome, Tile4); // base landmass
                        break;
                    case 5:
                        assign(biome, Tile5);
                        break;
                    case 6:
                        assign(biome, Tile6);
                        break;
                }

            }


            foreach (Generation.Biome biome in Generation.biomes.Values)
            {
                foreach (List<int> pos in biome.border)
                {
                    map.SetTile(new Vector3Int(pos[0], pos[1], 0), Border);
                }
            }


            /*
            foreach (Biome biome in testArea)
            {
                foreach (List<int> pos in biome.area)
                    map.SetTile(new Vector3Int(pos[0], pos[1], 0), Border);
            } */


        }


        public static void setTiles(Tile a, Tile b, Tile c, Tile d, Tile e, Tilemap grid)
        {
            Tile2 = a;
            Tile3 = b;
            Tile4 = c;
            Tile5 = d;
            Tile6 = e;

            //Border = border;

            map = grid;
    }


        internal static void clearMap()
        {
            map.ClearAllTiles();
        }




    }
}
