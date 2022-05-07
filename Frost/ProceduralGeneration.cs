// Frost Library
//
// A random world generation library for Unity.
// Authors: Jan Fredrik Bråstad & Kristina Nikitina
// Date: Spring 2022


using System;
using System.Collections.Generic;
using System.Text;

namespace Frost
{
    /// <summary>
    /// Continues to generate the world map as the player moves around.
    /// This needs more direct implementation from the user due to happening in run-time.
    /// </summary>
    public class ProceduralGeneration
    {
        /// <summary>
        /// Translates the given coordinates into chunk coordinates.
        /// </summary>
        /// <param name="pos"> List of direct coordinates </param>
        /// <param name="lim"> Size of the chunk </param>
        /// <returns> Returns a list of same size as pos with chunk coordinates </returns>
        private static int[] translatePosition(int[] pos, int lim)
        {
            int x, y;

            // X position
            if (pos[0] > 0)
            {
                x = -1;
                while (pos[0] > 0)
                {
                    pos[0] -= lim;
                    x++;
                }
            }
            else
            {
                x = 0;
                while (pos[0] < 0)
                {
                    pos[0] += lim;
                    x--;
                }
            }

            // Y position 
            if (pos[1] > 0)
            {
                y = -1;
                while (pos[1] > 0)
                {
                    pos[1] -= lim;
                    y++;
                }
            }
            else
            {
                y = 0;
                while (pos[1] < 0)
                {
                    pos[1] += lim;
                    y--;
                }
            }

            return new int[] { x, y };
        }


        /// <summary>
        /// Checks if the player is one their way out of the current loaded map
        /// </summary>
        /// <param name="playerPosition"> The position of the player </param>
        /// <param name="limit"> How close the player should be before the generation start </param>
        /// <param name="seed"> The seed that is used (Has to be passed in again, as this happens in run-time) </param>
        public static void proceduralGeneration(int[] playerPosition, int limit, int seed, float scale)
        {
            // Corners
            if (!Map.map.HasTile(new UnityEngine.Vector3Int(playerPosition[0] + limit, playerPosition[1] + limit, 0)))
            {
                int[] modifiers = new int[] { playerPosition[0] + limit, playerPosition[1] + limit };
                modifiers = translatePosition(modifiers, limit);
                prodGen(limit, modifiers, seed, scale);
            }
            else if (!Map.map.HasTile(new UnityEngine.Vector3Int(playerPosition[0] + limit, playerPosition[1] - limit, 0)))
            {
                int[] modifiers = new int[] { playerPosition[0] + limit, playerPosition[1] - limit };
                modifiers = translatePosition(modifiers, limit);
                prodGen(limit, modifiers, seed, scale);
            }
            else if (!Map.map.HasTile(new UnityEngine.Vector3Int(playerPosition[0] - limit, playerPosition[1] + limit, 0)))
            {
                int[] modifiers = new int[] { playerPosition[0] - limit, playerPosition[1] + limit };
                modifiers = translatePosition(modifiers, limit);
                prodGen(limit, modifiers, seed, scale);
            }
            else if (!Map.map.HasTile(new UnityEngine.Vector3Int(playerPosition[0] - limit, playerPosition[1] - limit, 0)))
            {
                int[] modifiers = new int[] { playerPosition[0] - limit, playerPosition[1] - limit };
                modifiers = translatePosition(modifiers, limit);
                prodGen(limit, modifiers, seed, scale);
            }
            // Front, back and sides
            else if (!Map.map.HasTile(new UnityEngine.Vector3Int(playerPosition[0], playerPosition[1] - limit, 0)))
            {
                int[] modifiers = new int[] { playerPosition[0], playerPosition[1] - limit };
                modifiers = translatePosition(modifiers, limit);
                prodGen(limit, modifiers, seed, scale);
            }
            else if (!Map.map.HasTile(new UnityEngine.Vector3Int(playerPosition[0], playerPosition[1] + limit, 0)))
            {
                int[] modifiers = new int[] { playerPosition[0], playerPosition[1] + limit };
                modifiers = translatePosition(modifiers, limit);
                prodGen(limit, modifiers, seed, scale);
            }
            else if (!Map.map.HasTile(new UnityEngine.Vector3Int(playerPosition[0] + limit, playerPosition[1], 0)))
            {
                int[] modifiers = new int[] { playerPosition[0] + limit, playerPosition[1] };
                modifiers = translatePosition(modifiers, limit);
                prodGen(limit, modifiers, seed, scale);
            }
            else if (!Map.map.HasTile(new UnityEngine.Vector3Int(playerPosition[0] - limit, playerPosition[1], 0)))
            {
                int[] modifiers = new int[] { playerPosition[0] - limit, playerPosition[1] };
                modifiers = translatePosition(modifiers, limit);
                prodGen(limit, modifiers, seed, scale);
            }

        }


        /// <summary>
        /// Checks the given area and adds it to the total map
        /// </summary>
        /// <param name="lim"> Size of the list and chunk </param>
        /// <param name="modifiers"> Abstract modifiers so the chunk get placed in right position relative to the map </param>
        /// <param name="seed"> Seed used for generation </param>
        /// <param name="scale"> Scale value for the noise generation algorithm </param>
        private static void prodGen(int lim, int[] modifiers, int seed, float scale)
        {
            //float scale = 0.01f; // fix this

            float[,] noise = new float[lim, lim];
            int[,] biomeValues;

            SimplexNoise.Noise.Seed = seed;

            modifiers[0] *= lim; // might be off by -1
            modifiers[1] *= lim;

            for (int x = 0; x < lim; x++) // 0 - 20
            {
                for (int y = 0; y < lim; y++) // 0 - 20
                {
                    noise[x, y] = SimplexNoise.Noise.CalcPixel2D(x + Setup.abstractLimit + modifiers[0], y + Setup.abstractLimit + modifiers[1], scale); // x, y, scale
                }
            }

            biomeValues = Setup.assignTiles(noise, lim, lim);

            Map.updatePartialTiles(biomeValues, modifiers, new int[] { lim, lim });
        }

    }
}
