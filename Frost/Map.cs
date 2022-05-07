// Frost Library
//
// A random world generation library for Unity.
// Authors: Jan Fredrik Bråstad & Kristina Nikitina
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
        // private static Tile Border;

        // The tilemap
        // Made public in case user want to directly generate world with more controll
        public static Tilemap map;


       
        /// <summary>
        /// Set all the tiles in a given area
        /// </summary>
        /// <param name="partialMap"> List containing the tileId's </param>
        /// <param name="modifiers"> Modifiers refering to these cordinates relative to the rest of the map </param>
        /// <param name="size"> Size of the array </param>
        internal static void updatePartialTiles(int[,] partialMap, int[] modifiers, int[] size)
        {
            for (int x = 0; x < size[0]; x++)
            {
                for (int y = 0; y < size[1]; y++)
                {
                    switch (partialMap[x, y])
                    {
                        // Changed to negative values to not be mistaken for biomes
                        case -2:
                            map.SetTile(new Vector3Int(x + modifiers[0], y + modifiers[1], 0), Coast);
                            break;
                        case -1:
                            map.SetTile(new Vector3Int(x + modifiers[0], y + modifiers[1], 0), Water);
                            break;
                        case 1:
                            map.SetTile(new Vector3Int(x + modifiers[0], y + modifiers[1], 0), Biome1);
                            break;
                        case 4:
                            map.SetTile(new Vector3Int(x + modifiers[0], y + modifiers[1], 0), Biome1);
                            break;
                        case 5:
                            map.SetTile(new Vector3Int(x + modifiers[0], y + modifiers[1], 0), Biome2);
                            break;
                        case 6:
                            map.SetTile(new Vector3Int(x + modifiers[0], y + modifiers[1], 0), Biome3);
                            break;
                    }
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
