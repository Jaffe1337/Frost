// Frost Library
//
// A random world generation library for Unity.
// Created by: Jan Fredrik Bråstad & Kristina Nikitina
// Date: Spring 2022


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
        internal static float[,] obj;

        internal static int width, height;
        public static int seed = 0;
        internal static float noiseModifier = 0.1f;

        internal static float waterPercent = 50f;
        internal static float beachPercent = 10f;


        /// <summary>
        /// Generates a Noise map using SimplexNoise
        /// </summary>
        internal static void Noise()
        {
            // Changes seed if user have assigned one
            SimplexNoise.Noise.Seed = seed;

            noise = new float[width,height];
            noise = SimplexNoise.Noise.Calc2D(width, height, noiseModifier);

            // Create a copy of Noise so we have original if we want to use it later
            obj = new float[width, height];
            obj = noise;
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
        internal static void assignTiles()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // spawncheck
                    float spawnpoint = noise[x, y];

                    if (spawnpoint < (256f * (waterPercent / 100f)))
                    {
                        obj[x, y] = 3;
                    }
                    else if (spawnpoint < (256f * (waterPercent / 100f)) + (256f * (beachPercent / 100f)))
                    {
                        obj[x, y] = 2;
                    }
                    else
                    {
                        obj[x, y] = 1;
                    }
                }
            }
        }
    }
}
