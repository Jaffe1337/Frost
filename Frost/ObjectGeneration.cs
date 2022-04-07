using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Frost
{
    public class ObjectGeneration
    {

        public static void randomObj(UnityEngine.Object Single_obj, int target_biome, int amount)
        {

            /*int xVal = 0;
            int yVal = 0;
            
            GameObject parent_object;
            parent_object = new GameObject("parent " + Single_obj);

            for (int i = 0; i < amount;)
            {
                generatepoint(xVal,yVal);

                if (Setup.obj[xVal, yVal] == target_biome)
                {
                    UnityEngine.Object.Instantiate(Single_obj, new Vector3(xVal + 0.5f, yVal + 0.5f, 0), Quaternion.identity, parent_object.transform);
                    i++;
                };

            };*/

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


        public static void randomMultiObj(UnityEngine.Object Group_obj, int target_biome, int amount)
        {
            GameObject parent_object;

            parent_object = new GameObject("Groups, " + "Biome: " + target_biome + " Parent " + Group_obj);

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
