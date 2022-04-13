// Frost Library
//
// A random world generation library for Unity.
// Created by: Jan Fredrik Bråstad & Kristina Nikitina
// Date: Spring 2022


using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Frost
{
    /// <summary>
    /// Handles the map and setting tiles in the tilemap.
    /// </summary>
    public class Map
    {
        // Internal Tile variables
        private static Tile Coast;
        private static Tile Water;
        private static Tile Biome1;
        private static Tile Biome2;
        private static Tile Biome3;

        // Used for debugging
        private static Tile Border;
        
        // The tilemap
        internal static Tilemap map;


        /// <summary>
        /// Small function that prevents a lot of similar code.
        /// </summary>
        /// <param name="biome"> The biome that will be put on the tilemap </param>
        /// <param name="tile"> The tile we want to place </param>
        private static void assign(Generation.Biome biome, Tile tile)
        {
            foreach (List<int> pos in biome.area)
            {
                map.SetTile(new Vector3Int(pos[0], pos[1], 0), tile);
            }
        }


        /// <summary>
        /// Set all the tiles in the tilemap
        /// </summary>
        internal static void updateAllTiles()
        {
            // Clears the map.
            clearMap();
            
            // Set water and coast tile
            for (int x = 0; x < Setup.width; x++)
            {
                for (int y = 0; y < Setup.height; y++)
                {
                    switch (Setup.obj[x, y])
                    {
                        case 2:
                            map.SetTile(new Vector3Int(x, y, 0), Coast);
                            break;
                        case 3:
                            map.SetTile(new Vector3Int(x, y, 0), Water);
                            break;
                    }
                }
            }

            // Set all biome tiles
            foreach (Generation.Biome biome in Generation.biomes.Values)
            {
                switch (biome.tileId)
                {
                    case 4:
                        assign(biome, Biome1); // base landmass
                        break;
                    case 5:
                        assign(biome, Biome2);
                        break;
                    case 6:
                        assign(biome, Biome3);
                        break;
                }

            }

            // Set bordering tiles.
            // Border tile is set to null and this part is only used for debugging
            foreach (Generation.Biome biome in Generation.biomes.Values)
            {
                foreach (List<int> pos in biome.border)
                {
                    map.SetTile(new Vector3Int(pos[0], pos[1], 0), Border);
                }
            }     
        }


        /// <summary>
        /// Assigns all tiles and the map to their internal variable
        /// </summary>
        public static void setTiles(Tile pCoast, Tile pWater, Tile pBiome1, Tile pBiome2, Tile pBiome3, Tilemap grid)
        {
            Coast = pCoast;
            Water = pWater;
            Biome1 = pBiome1;
            Biome2 = pBiome2;
            Biome3 = pBiome3;

            map = grid;
        }

        /// <summary>
        /// This function is needed to prevent a bug in map generation.
        /// Therefore the generation class needs to use this function.
        /// </summary>
        internal static void clearMap()
        {
            map.ClearAllTiles();
        }
    }
}
