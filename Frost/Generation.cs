using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine.Tilemaps;


namespace Frost
{
    public class Generation // mapGeneration
    {
        // A variable that holds all tile ID's in an array of [x, y] where x and y are the position of the tile
        // private static float[,] obj;

        // A smaller version of the obj variable, may be used if we generate larger maps
        private static float[,] altMap;

        // List of all biomes currently in our generated map
        // Public because needed to update
        public static Dictionary<int, Biome> biomes; // = new Dictionary<int, Biome> { };

        // The current biomeId
        private static int biomeId = 0;


        public class Biome
        {
            public
                List<List<int>> area;
            public
                List<List<int>> border;
            public
                int biomeId, tileId;


            public Biome(int biomeId, int tileId)
            {
                this.biomeId = biomeId;
                this.tileId = tileId;
                area = new List<List<int>> { };
                border = new List<List<int>> { };
            }

            public void expandArea(List<int> pos)
            {
                this.area.Add(pos);
            }
            public void expandBorder(List<int> pos)
            {
                // Only one border tile can fail if biome border to multiple biomes
                this.border.Add(pos);
            }

        }


        // arr --> pos som skal sjekkes
        // cId, Id that will be checked
        // nId, new assigned Id
        private static void checkPos(List<List<int>> arr, int nId, int[] modifier)
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
                bool borderTile = false;

                Setup.obj[x + modifier[0], y + modifier[1]] = nId;

                // Add to biome array
                biomes[biomeId].expandArea(new List<int> { x + modifier[0], y + modifier[1] });

                // Tiles to check relevent to current position
                int up, down, right, left;

                // Check upper tile
                if (y - 1 >= 0) { up = y - 1; }
                else
                {
                    up = 0;
                    borderTile = true;
                }


                // Check lower tile
                if (y + 1 <= 500 - 1) { down = y + 1; }
                else
                {
                    down = 500 - 1;
                    borderTile = true;
                }

                // Check right tile
                if (x + 1 <= 500 - 1) { right = x + 1; }
                else
                {
                    right = 500 - 1;
                    borderTile = true;
                }

                // Check left tile
                if (x - 1 >= 0) { left = x - 1; }
                else
                {
                    left = 0;
                    borderTile = true;
                }



                if (altMap[x, up] == cId)
                {
                    // Add to biome, dependent on Border or not
                    if (borderTile) { biomes[biomeId].expandBorder(new List<int> { x + modifier[0], up + modifier[1] }); }

                    // Add to newArr to recursivly check rest of connected tiles
                    newArr.Add(new List<int> { x, up });

                    // Set checked value to 0 to prevent checking it again later
                    altMap[x, up] = 0;
                }


                if (altMap[x, down] == cId)
                {
                    if (borderTile) { biomes[biomeId].expandBorder(new List<int> { x + modifier[0], down + modifier[1] }); }

                    newArr.Add(new List<int> { x, down });
                    altMap[x, down] = 0;
                }


                if (altMap[right, y] == cId)
                {
                    if (borderTile) { biomes[biomeId].expandBorder(new List<int> { right + modifier[0], y + modifier[1] }); }

                    newArr.Add(new List<int> { right, y });
                    altMap[right, y] = 0;
                }
                if (altMap[left, y] == cId)
                {
                    if (borderTile) { biomes[biomeId].expandBorder(new List<int> { left + modifier[0], y + modifier[1] }); }

                    newArr.Add(new List<int> { left, y });
                    altMap[left, y] = 0;
                }
            }

            // Continue function if there is any position in newArr
            if (newArr.Count > 0)
            {
                checkPos(newArr, nId, modifier);
            }

        }


        // modifier and size same?
        public static void checkMap(int[] modifier, int[] size) // take in a map variable 
        {
            int id = 1; // which id should be changed, change to var later
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
                        checkPos(CheckArr, newId, modifier);

                        biomeId++; // only added in this funtion? maybe local variable instead of global


                        newId++;
                        if (newId >= 7) newId = 4;

                    }
                }
            }

        }


        private static void divide(int[] modifier, int[] size) //int xLimit, int yLimit) // Only affects color changes of biomes!
        {
            altMap = new float[size[0], size[1]];

            for (int w = 0; w < size[0]; w++) // maybe put in function to prevent so many nested loops
                for (int h = 0; h < size[1]; h++)
                    altMap[w, h] = Setup.obj[w + modifier[0], h + modifier[1]];

            checkMap(modifier, size);
        }


        public static void controller(int Limit)
        {
            int width = Setup.width;
            int height = Setup.height;

            int[] modifier = new int[] { 0, 0 };
            int[] size; // = new int[] { xLimit, yLimit };


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

                    divide(modifier, size);
                }
            }
        }


        private static void biomeMerging()
        {
            List<Biome> temp = new List<Biome> { };

            // Add biomes with a border
            foreach (Biome biome in biomes.Values)
            {
                if (biome.border.Count > 0)
                {
                    temp.Add(biome);
                }
            }

            // Remove biomes that border to total map border

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


                foreach (Biome biome2 in biomes.Values)
                {
                    foreach (List<int> l in biome2.area)
                    {
                        if (down != 0 && l[0] == list[0] && l[1] == list[1] && biome.biomeId != biome2.biomeId)
                        {
                            // Merge instead of just changing id
                            biome2.tileId = biome.tileId;
                        }
                        else if (up != 0 && l[0] == list2[0] && l[1] == list2[1] && biome.biomeId != biome2.biomeId)
                        {
                            // testArea.Add(biome2);
                            // biome2.biomeId = biome.biomeId;
                            biome2.tileId = biome.tileId;
                        }
                        else if (left != 0 && l[0] == list3[0] && l[1] == list3[1] && biome.biomeId != biome2.biomeId)
                        {
                            // testArea.Add(biome2);
                            // biome2.biomeId = biome.biomeId;
                            biome2.tileId = biome.tileId;
                        }
                        else if (right != 0 && l[0] == list4[0] && l[1] == list4[1] && biome.biomeId != biome2.biomeId)
                        {
                            // testArea.Add(biome2);
                            // biome2.biomeId = biome.biomeId;
                            biome2.tileId = biome.tileId;
                        }
                    }
                }

            }


        }


        public static Tilemap world(int pWidth, int pHeight, int pSeed, float pModifier)
        {
            int listLimit = 500;

            Setup.width = pWidth;
            Setup.height = pHeight;
            Setup.seed = pSeed;
            Setup.noiseModifier = pModifier;

            biomes = new Dictionary<int, Biome> { };

            Map.clearMap();

            if (Setup.seed == 0)
                Setup.randomSeed();

            Setup.Noise();
            Setup.assignTiles();

            controller(listLimit);

            biomeMerging();

            Map.updateAllTiles();

            return Map.map;
        }


    }
}
