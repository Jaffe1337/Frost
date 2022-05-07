// Frost Library
//
// A random world generation library for Unity.
// Authors: Jan Fredrik Bråstad & Kristina Nikitina
// Date: Spring 2022


using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Tilemaps;


namespace Frost
{
    /// <summary>
    /// Handles the map generation and biome creation.
    /// </summary>
    public class Generation    {

        // prevents passing same variable through a bunch of functions
        // A smaller version of the obj variable, may be used if we generate larger maps
        private static float[,] altMap;

        private static int[] modifier = new int[] { 0, 0 };
        private static int[] size; 


        // List of all biomes currently in our generated map
        // set to public for user to use in procedural generation
        public static Dictionary<int, Biome> biomes; 

        // The current biomeId
        private static int biomeId = 0;


        /// <summary>
        /// Biome class used to store all tiles within one biome
        /// </summary>
        public class Biome
        {
            // Something went wrong here
            public
                List<List<int>> area, border;
            public
                int biomeId, tileId;


            /// <summary>
            /// Biome initializer  
            /// </summary>
            /// <param name="biomeId"> Id for this specific biome </param>
            /// <param name="tileId"> Id for the tile that should represent this biome </param>
            public Biome(int biomeId, int tileId)
            {
                this.biomeId = biomeId;
                this.tileId = tileId;
                this.area = new List<List<int>> { };
                this.border = new List<List<int>> { };
            }


            /// <summary>
            /// Expands biome area with new tile position
            /// </summary>
            /// <param name="pos"> List of 2 ints representing the x and y position of a tile </param>
            public void expandArea(List<int> pos)
            {
                this.area.Add(pos);
            }


            /// <summary>
            /// Expands biome border with new tile position
            /// </summary>
            /// <param name="pos"> List of 2 ints representing the x and y position of a tile </param>
            private void expandBorder(List<int> pos)
            {
                // Only one border tile can fail if biome border to multiple biomes
                this.border.Add(pos);
            }


            /// <summary>
            /// Take in another object and merge them together
            /// </summary>
            /// <param name="biome"> Biome which will be merged with this biome </param>
            public void merge(Biome biome)
            {
                foreach (List<int> pos in biome.area)
                    this.expandArea(pos);
                foreach (List<int> pos in biome.border)
                    this.expandBorder(pos);
            }


            /// <summary>
            /// Set biome border based on the divider limit 
            /// </summary>
            /// <param name="limit">  Limit that divided biomes, used to create the border for merging </param>
            public void setBorder(int limit) // bug
            {
                // bug with limit = 500
                foreach (List<int> pos in this.area)
                {
                    if(pos[0] % (limit) == 0 || pos[1] % (limit) == 0)
                    {
                        this.expandBorder(pos);
                    }
                }
            }

        }


        /// <summary>
        /// Iterates through all biomes and gives them a border based on the limit which divided them
        /// </summary>
        /// <param name="limit"> Limit that divided biomes, used to create the border for merging </param>
        private static void setBiomeBorder(int limit)
        {
            foreach (Biome biome in biomes.Values)
            {
                biome.setBorder(limit);
            }
        } 


        /// <summary>
        /// A recursive function that checks a given position with a certain id and find all connected positions
        /// </summary>
        /// <param name="arr"> A list containing lists with positions x and y that are going to be checked </param>
        /// <param name="nId"> The new id of this current biome</param>
        private static void checkPos(List<List<int>> arr, int nId) 
        {
            // Array used to store position that need to be checked
            List<List<int>> newArr = new List<List<int>> { };

            int cId = 1;

            foreach (List<int> subArray in arr)
            {
                // Current position
                int x = subArray[0];
                int y = subArray[1];

                // Value to check for tiles on outline of map area
                // bool borderTile = false;

                Setup.obj[x + modifier[0], y + modifier[1]] = nId; // Useless after new biome system?

                // Add to biome array
                biomes[biomeId].expandArea(new List<int> { x + modifier[0], y + modifier[1] });

                // Tiles to check relevent to current position
                int up = y - 1 >= 0 ? y - 1 : 0;
                int down = y + 1 <= size[1] - 1 ? y + 1 : size[1] - 1;
                int right = x + 1 <= size[0] - 1 ? x + 1 : size[0] - 1;
                int left = x - 1 >= 0 ? x - 1 : 0;


                if (altMap[x, up] == cId)
                {
                    // Add to newArr to recursivly check rest of connected tiles
                    newArr.Add(new List<int> { x, up });

                    // Set checked value to 0 to prevent checking it again later
                    altMap[x, up] = 0;
                }
                if (altMap[x, down] == cId)
                {
                    newArr.Add(new List<int> { x, down });
                    altMap[x, down] = 0;
                }
                if (altMap[right, y] == cId)
                {
                    newArr.Add(new List<int> { right, y });
                    altMap[right, y] = 0;
                }
                if (altMap[left, y] == cId)
                {
                    newArr.Add(new List<int> { left, y });
                    altMap[left, y] = 0;
                }
            }

            // Continue function if there is any position in newArr
            if (newArr.Count > 0)
            {
                // checkPos(newArr, nId, modifier, size);
                checkPos(newArr, nId);
            }

        }


        /// <summary>
        /// Generates the current altMap by assigning biomes its area and ids
        /// </summary>
        private static void checkMap() 
        {
            int id = 1; // which id should be checked, change to var later
            int newId = 5;

            for (int x = 0; x < size[0]; x++)
            {
                for (int y = 0; y < size[1]; y++)
                {
                    if (Setup.obj[x + modifier[0], y + modifier[1]] == id)
                    {

                        List<List<int>> CheckArr = new List<List<int>> { };

                        // Found a new biome
                        biomes[biomeId] = new Biome(biomeId, newId);

                        // Start pos
                        CheckArr.Add(new List<int> { x, y });
                        //checkPos(CheckArr, newId, modifier, size);
                        checkPos(CheckArr, newId);

                        biomeId++; // only added in this funtion? maybe local variable instead of global

                        newId++;
                        if (newId >= 7) newId = 4;

                    }
                }
            }

        }


        /// <summary>
        /// Creates an abstract map array which can be generated.
        /// </summary>
        private static void divide() 
        {
            altMap = new float[size[0], size[1]];

            for (int w = 0; w < size[0]; w++) 
                for (int h = 0; h < size[1]; h++)
                    altMap[w, h] = Setup.obj[w + modifier[0], h + modifier[1]];

        }


        /// <summary>
        /// Splits up map generation in smaller portions to prevents crashes.
        /// </summary>
        /// <param name="Limit"> This limit represent how big of a map can be generated without having to be split up </param>
        private static void controller(int Limit)
        {
            int width = Setup.width;
            int height = Setup.height;

            for (int a = 0; (a * Limit) < width; a++)
            {
                for (int b = 0; (b * Limit) < height; b++)
                {
                    // Refresh size for each iteration
                    size = new int[] { Limit, Limit };


                    modifier[0] = a * Limit;
                    modifier[1] = b * Limit;

                    // Check for width not matching limit
                    if ((modifier[0] + Limit) > width)
                    {
                        size[0] = width - modifier[0];
                    }

                    // Check for height not matching limit
                    if ((modifier[1] + Limit) > height)
                    {
                        size[1] = height - modifier[1];
                    }

                    divide();
                    checkMap();
                }
            }
        }


        /// <summary>
        /// Public function that will be called to generate the map given the parameters
        /// </summary>
        /// <param name="pWidth"> The width of the map </param>
        /// <param name="pHeight"> The height of the map</param>
        /// <param name="pSeed"> Seed to the map </param>
        /// <param name="pModifier"> Modifier that decides how frequently the noise algorithm pulls values from its abstract map. 
        ///                          With bigger maps it is recommended with lower value and vice versa.
        ///                          Important value for generation and should be lower then 1, highest value we recommend is 0.5 </param>
        /// <param name="pWaterPer"> How big percent of the map will be water </param>
        /// <param name="pBeachPer"> How big percent of the map will be coast </param>
        /// 
        /// <returns> A unity tilemap object representing the map </returns>
        public static Tilemap world(int pWidth, int pHeight, int pSeed, float pModifier, float pWaterPer, float pBeachPer)
        {
            // This decides the size of map chunks that can be loaded at a time
            int listLimit = 500;

            // Set internal values
            Setup.width = pWidth;
            Setup.height = pHeight;
            Setup.seed = pSeed;
            Setup.noiseModifier = pModifier;
            Setup.waterPercent = pWaterPer;
            Setup.beachPercent = pBeachPer;            

            // Makes sure biomes are empty
            biomes = new Dictionary<int, Biome> { };

            // Makes sure map is empty
            Map.clearMap();

            // Sets a random seed if there is not given a seed
            if (Setup.seed == 0)
                Setup.randomSeed();

            // Set up noise map
            Setup.Noise();
            Setup.obj = Setup.assignTiles(Setup.noise, pWidth, pHeight);

            // Controls general map generation
            controller(listLimit);

            // biome border fix can be done as early as here?

            // Creates borders for merging
            setBiomeBorder(listLimit);

            // Merge biomes, performed two times for more precision 
            // biomeMerging();
            // biomeMerging(false);

            // Object merging:
            Merge.objectMerging();

            // Updates the map
            // Map.updateAllTiles();
            Map.updatePartialTiles(Setup.obj, new int[] { 0, 0 }, new int[] { Setup.width, Setup.height });

            return Map.map; // not necessary
        }

    }
}
