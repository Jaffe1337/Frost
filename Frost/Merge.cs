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
    /// Handles merging of tiles/biomes after generation so that one biome don't just cut right into another
    /// </summary>
    public class Merge
    {

        // Made public in case user want to directly generate world with more controll
        /// <summary>
        /// Merges biome area, but uses tileId directly instead of biomes.
        /// </summary>
        public static void objectMerging()
        {
            bool checkValue = true;

            while (checkValue)
            {
                checkValue = false;
                for (int x = 0; x < Setup.width; x++)
                {
                    for (int y = 0; y < Setup.height; y++)
                    {

                        int up, down, right, left;

                        if (y - 1 >= 0) { up = y - 1; }
                        else
                        {
                            up = 0;
                        }


                        // Check lower tile
                        if (y + 1 <= Setup.height - 1) { down = y + 1; }
                        else
                        {
                            down = Setup.height - 1;
                        }

                        // Check right tile
                        if (x + 1 <= Setup.width - 1) { right = x + 1; }
                        else
                        {
                            right = Setup.width - 1;
                        }

                        // Check left tile
                        if (x - 1 >= 0) { left = x - 1; }
                        else
                        {
                            left = 0;
                        }


                        if (Setup.obj[x, y] > 0)
                        {
                            if (Setup.obj[x, up] > Setup.obj[x, y])
                            {
                                Setup.obj[x, up] = Setup.obj[x, y];
                                checkValue = true;
                            }


                            if (Setup.obj[x, down] > Setup.obj[x, y])
                            {
                                Setup.obj[x, down] = Setup.obj[x, y];
                                checkValue = true;
                            }


                            if (Setup.obj[right, y] > Setup.obj[x, y])
                            {
                                Setup.obj[right, y] = Setup.obj[x, y];
                                checkValue = true;
                            }


                            if (Setup.obj[left, y] > Setup.obj[x, y])
                            {
                                Setup.obj[left, y] = Setup.obj[x, y];
                                checkValue = true;
                            }
                        }
                    }
                }
            }
        }


    }
}
