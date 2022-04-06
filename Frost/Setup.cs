using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using SimplexNoise;

namespace Frost
{
    public class Setup
    {
        public static float[,] noise;
        public static float[,] obj;

        public static int width, height;
        public static int seed = 0;
        public static float noiseModifier = 0.1f;

        public static float waterPercent = 50f;
        public static float beachPercent = 10f;


        // Initiazes our noise map using SimplexNoise
        public static void Noise()
        {
            // Changes seed if user have assigned one
            SimplexNoise.Noise.Seed = seed;
            noise = SimplexNoise.Noise.Calc2D(width, height, noiseModifier);
            
            // Create a copy of Noise so we have original if we want to use it later
            obj = noise;

        }

        public static void randomSeed()
        {
            // Seed up to 100 million
            seed = UnityEngine.Random.Range(0, 100000000);
        }

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
