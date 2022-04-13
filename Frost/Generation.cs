// Frost Library
//
// A random world generation library for Unity.
// Created by: Jan Fredrik Bråstad & Kristina Nikitina
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
        internal static Dictionary<int, Biome> biomes; 

        // The current biomeId
        private static int biomeId = 0;


        /// <summary>
        /// Biome class used to store all tiles within one biome
        /// </summary>
        public class Biome
        {
            // Something went wrong here
            public
                List<List<int>> area;
            public
                List<List<int>> border;
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
                area = new List<List<int>> { };
                border = new List<List<int>> { };
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
            public void expandBorder(List<int> pos)
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
                    if(pos[0] % (limit - 1) == 0 || pos[1] % (limit - 1) == 0)
                    {
                        this.expandBorder(pos);
                    }
                    // test with limit -1 aswell, maybe fix bug
                }
            }
        }


        /// <summary>
        /// Iterates through all biomes and gives them a border based on the limit which divided them
        /// </summary>
        /// <param name="limit"> Limit that divided biomes, used to create the border for merging </param>
        public static void setBiomeBorder(int limit)
        {
            foreach (Biome biome in biomes.Values)
            {
                biome.setBorder(limit);
            }
        } 


        /* Border fix maybe:
         * 
         * Try with only one tile again. Only one tile per border. can be done by checking similarities with x or y pos
         * but check if new border tile x or y pos  tile matches 
         * 
         * 
        */


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

            checkMap();
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
                }
            }
        }


        /// <summary>
        /// Checks either for what biome is biggest or smaller, depends on usage. 
        /// Main objective is to prevent repeatable code in the biomeMerging function.
        /// </summary>
        /// <param name="biome1"> The first biome </param>
        /// <param name="biome2"> The second biome </param>
        /// <param name="up"> Bool value to decide if we look for the smallest or highest biome id </param>
        /// <returns> A list of two biomeId's </returns>
        private static List<int> biomeCheck(Biome biome1, Biome biome2, bool up)
        {
            // Check for which biome has the lowest id, and merge the other with it
            if (up)
            {                
                if (biome2.biomeId > biome1.biomeId)
                {
                    // biome2 id biggest
                    return new List<int> { biome2.biomeId, biome1.biomeId }; // first merge with second, second get removed
                }
                else
                {
                    // biome1 id biggest
                    return new List<int> { biome1.biomeId, biome2.biomeId };
                }
            }

            if (biome2.biomeId < biome1.biomeId)
            {
                // biome1 id biggest
                return new List<int> { biome2.biomeId, biome1.biomeId };
            }
            else
            {
                // biome2 id biggest
                return new List<int> { biome1.biomeId, biome2.biomeId };
            }
        }


        /// <summary>
        /// Function that merge biomes that border each other in the case of generating a map to big that it had to be split up.
        /// </summary>
        /// <param name="upCheck"> Bool passed on to biomeCheck function </param>
        private static void biomeMerging(bool upCheck)
        {
            List<Biome> temp = new List<Biome> { };
            List<List<int>> revomables = new List<List<int>> { };

            // Add biomes with a border
            foreach (Biome biome in biomes.Values)
            {
                if (biome.border.Count > 0 && biome.area.Count > 0) // remove last check?
                {
                    temp.Add(biome);
                }
            }

            // Remove biomes that border to total map border
            // Maybe fix all the nested for loops? split into function?
            foreach (Biome biome in temp)
            {
                int x = biome.border[0][0];
                int y = biome.border[0][1];


                int up = y - 1;
                int down = y + 1;
                int right = x + 1;
                int left = x - 1;


                List<int> list = new List<int> { x, down };
                List<int> list2 = new List<int> { x, up };
                List<int> list3 = new List<int> { left, y };
                List<int> list4 = new List<int> { right, y };


                // make it only check other border biomes for effeciency
                foreach (Biome biome2 in biomes.Values)
                // for(int i = 0; i < biomes.Count;)
                {
                    //if(biomes.ContainsKey(i))
                    foreach (List<int> l in biome2.area) // Causes bug, because we add to are in biomeCheck()
                    {
                        if (down != 0 && l[0] == list[0] && l[1] == list[1] && biome.biomeId != biome2.biomeId)
                        {
                            revomables.Add(biomeCheck(biome, biome2, upCheck));
                            // biome2.tileId = biome.tileId;
                        }
                        else if (up != 0 && l[0] == list2[0] && l[1] == list2[1] && biome.biomeId != biome2.biomeId)
                        {
                            revomables.Add(biomeCheck(biome, biome2, upCheck));
                        }
                        else if (left != 0 && l[0] == list3[0] && l[1] == list3[1] && biome.biomeId != biome2.biomeId)
                        {
                            revomables.Add(biomeCheck(biome, biome2, upCheck));
                        }
                        else if (right != 0 && l[0] == list4[0] && l[1] == list4[1] && biome.biomeId != biome2.biomeId)
                        {
                            revomables.Add(biomeCheck(biome, biome2, upCheck));
                        }
                    }
                }
            }


            
            foreach(List<int> id in revomables)
            {
                if (biomes.ContainsKey(id[1]) && biomes.ContainsKey(id[0]))
                {
                    biomes[id[0]].merge(biomes[id[1]]); // same id may appear more than once?
                    // biomes[id[1]].deleteArea();   

                    biomes.Remove(id[1]);

                    UnityEngine.Debug.Log(id[1]);
                }                
            }   
            
            // maybe fix for biomes without border:
            // search for tiles which pos is dividable by 501/500/499 and do biomecheck

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
            Setup.assignTiles();

            // Controls general map generation
            controller(listLimit);

            // Creates borders for merging
            setBiomeBorder(listLimit);

            // Merge biomes, performed two times for more precision 
            biomeMerging(true);
            biomeMerging(false);

            // Updates the map
            Map.updateAllTiles();

            return Map.map;
        }

    }
}
