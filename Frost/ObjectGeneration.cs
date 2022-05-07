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
    /// Part of the Library that will handle any type of object generation.
    /// This must be done after there has been generated a map.
    /// </summary>
    public class ObjectGeneration
    {
        /// <summary>
        /// This function will generate objects around a map based on given parameters.
        /// </summary>
        /// <param name="Single_obj"> The Unity game object that will be placed on the map </param>
        /// <param name="target_biome"> Id which indicates which biome this object will be generated in </param>
        /// <param name="amount"> The amount of objects to be generated </param>
        public static void randomObj(UnityEngine.Object Single_obj, int target_biome, int amount)
        {
            GameObject parent_object;
            parent_object = new GameObject("Biome: " + target_biome + "Parent " + Single_obj);

            List<Generation.Biome> a = new List<Generation.Biome> { };
            foreach (Generation.Biome b in Generation.biomes.Values)
            {
                if (b.tileId == target_biome)
                    a.Add(b);
            }           

            for (int i = 0; i < amount;i++)
            {
                int c = UnityEngine.Random.Range(0, a.Count - 1);
                
                int d = UnityEngine.Random.Range(0, a[c].area.Count - 1);

                List<int> cHolder = new List<int> { };
                List<int> dHolder = new List<int> { };                

                bool check = true;

                for(int u = 0; u < cHolder.Count; u++)
                    if(c == cHolder[u] && d == dHolder[u])
                    {
                        check = false;
                    }

                if (check) 
                {
                    cHolder.Add(c);
                    dHolder.Add(d);

                    int x = a[c].area[d][0];
                    int y = a[c].area[d][1];

                    UnityEngine.Object.Instantiate(Single_obj, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity, parent_object.transform); }   
                }

        }


        /// <summary>
        /// This function will generate objects on the map that are grouped together.
        /// </summary>
        /// <param name="Group_obj"> The Unity game object that will be placed on the map </param>
        /// <param name="target_biome"> Id which indicates which biome this object will be generated in </param>
        /// <param name="amount"> The amount of group objects to be generated </param>
        public static void randomMultiObj(UnityEngine.Object Group_obj, int target_biome, int amount)
        {
            GameObject parent_object;

            parent_object = new GameObject("Groups, " + "Biome: " + target_biome + " Parent " + Group_obj);

            // Maybe change to variables
            int areas = 9;
            int square = 2;

            for (int u = 1; u <= amount;u++)
            {
               
                List<Generation.Biome> biomeVal = new List<Generation.Biome> { };
                foreach (Generation.Biome b in Generation.biomes.Values)
                {
                    if (b.tileId == target_biome)
                        biomeVal.Add(b);
                }
                
                int c = UnityEngine.Random.Range(0, biomeVal.Count - 1);

                int d = UnityEngine.Random.Range(0, biomeVal[c].area.Count - 1);

                List<int> cHolder = new List<int> { };
                List<int> dHolder = new List<int> { };

                for (int i = 0; i < areas;i++)
                {
                    int offsetX = UnityEngine.Random.Range(-square, square);
                    int offsetY = UnityEngine.Random.Range(-square, square);

                    bool check = true;

                    for (int z = 0; z < cHolder.Count; z++) 
                    { 
                        if (c + offsetX == cHolder[z] && d + offsetY == dHolder[z])
                        {
                            check = false;
                        }
                    }

                       
                    if (check)
                    {
                        cHolder.Add(c + offsetX);
                        dHolder.Add(d + offsetY);

                        int x = biomeVal[c].area[d][0];
                        int y = biomeVal[c].area[d][1];

                        x += offsetX;
                        y += offsetY;

                        if (Setup.obj[x, y] == target_biome) 
                        {
                        UnityEngine.Object.Instantiate(Group_obj, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity, parent_object.transform);
                                    
                        }                   
                    }

                }
            }

        }
    }
}
