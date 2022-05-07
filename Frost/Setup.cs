// Frost Library
//
// A random world generation library for Unity.
// Authors: Jan Fredrik Bråstad & Kristina Nikitina
// Date: Spring 2022

// Copyright (c) 2019, Benjamin Ward


using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using SimplexNoise;

namespace Frost
{
    /// <summary>
    /// Sets up the noise map and the seed.
    /// </summary>
    public class Setup
    {
        internal static float[,] noise;
        internal static int[,] obj;

        internal static int width, height;
        public static int seed = 0;
        internal static float noiseModifier = 0.1f;

        internal static float waterPercent = 50f;
        internal static float beachPercent = 10f;

        internal static int abstractLimit = 1000000;


        /// <summary>
        /// Generates a Noise map using SimplexNoise
        /// </summary>
        internal static void Noise()
        {
            // Changes seed if user have assigned one
            SimplexNoise.Noise.Seed = seed;

            noise = new float[width,height];
            // noise = SimplexNoise.Noise.Calc2D(width, height, noiseModifier);

            // Abstract position system:
            // 1 million

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)                
                    noise[x,y] = SimplexNoise.Noise.CalcPixel2D(x + abstractLimit, y + abstractLimit, noiseModifier);               
                    


            // Create a copy of Noise so we have original if we want to use it later
            obj = new int[width, height];
        }


        /// <summary>
        /// A setter function to change the value of the private abstractLimit value
        /// </summary>
        /// <param name="newLimit"> The new value </param>
        public static void setMapLimit(int newLimit)
        {
            abstractLimit = newLimit;
        }


        /// <summary>
        /// Used to generate a random seed using Unity's randomizer if there is no seed given
        /// </summary>
        internal static void randomSeed()
        {
            // Seed up to 100 million
            seed = UnityEngine.Random.Range(0, 100000000);
        }


        /// <summary>
        /// Assign an id to each position in the noise map based on its value
        /// </summary>
        internal static int[,] assignTiles(float[,] area, int width, int height)
        {
            int[,] map = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float spawnpoint = area[x, y]; 

                    if (spawnpoint < (256f * (waterPercent / 100f)))
                    {
                        map[x, y] = -1;
                    }
                    else if (spawnpoint < (256f * (waterPercent / 100f)) + (256f * (beachPercent / 100f)))
                    {
                        map[x, y] = -2;
                    }
                    else
                    {
                        map[x, y] = 1;
                    }
                }
            }

            return map;
        }


        /// <summary>
        /// Getter function that allows the user access to the map matrix
        /// </summary>
        /// <returns> The matrix containing the tile Ids </returns>
        public static int[,] getMapMatrix() { return obj; }
    }
}
