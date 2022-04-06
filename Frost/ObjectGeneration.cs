using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Frost
{
    public class ObjectGeneration
    {
        public static GameObject Single_obj;
        public static GameObject Group_obj;

        public void randomObj(UnityEngine.Object Single_obj, int amount, int target_biome, int width, int height)
        {

            GameObject parent_object;

            parent_object = new GameObject("parent " + Single_obj);

            for (int i = 0; i < amount;)
            {
                int xVal = UnityEngine.Random.Range(0, width);
                int yVal = UnityEngine.Random.Range(0, height);

                if (Setup.obj[xVal, yVal] != 3 && Setup.obj[xVal, yVal] != 2)
                {
                    UnityEngine.Object.Instantiate(Single_obj, new Vector3(xVal + 0.5f, yVal + 0.5f, 0), Quaternion.identity, parent_object.transform);
                    i++;
                };

            };


        }

        private bool restraincheck(int pos_x, int x, int pos_y, int y)
        {
            if (pos_x + x > 0 & pos_y + y > 0)
            {
                if (pos_x + x < Setup.width & pos_y < Setup.height)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void randomMultiObj(UnityEngine.Object Group_obj, int amount, int target_biome)
        {
            GameObject parent_object;

            parent_object = new GameObject("Biome: " + target_biome + " Parent " + Group_obj);

            int areas = 15;
            int square = 8;

            for (int u = 1; u <= amount;)
            {
                int xVal = UnityEngine.Random.Range(0, Setup.width);
                int yVal = UnityEngine.Random.Range(0, Setup.height);
                List<List<int>> vectors = new List<List<int>> { };
                List<int> pos = new List<int> { xVal, yVal };
                int x, y;

                if (Setup.obj[xVal, yVal] == target_biome)
                {
                    u++;
                    for (int i = 0; i < areas;)
                    {
                        x = UnityEngine.Random.Range(-square, square);
                        y = UnityEngine.Random.Range(-square, square);

                        // vectors.Add(new List<int> { x, y });


                        bool check = true;

                        foreach (List<int> v in vectors)
                        {
                            if (v[0] == (pos[0] + x) && v[1] == (pos[1] + y))
                            {
                                check = false;
                                break;
                            }
                        }

                        if (check)
                        {
                            if (restraincheck(pos[0], x, pos[1], y) == true)

                                if (Setup.obj[pos[0] + x, pos[1] + y] == target_biome)
                                {
                                    vectors.Add(new List<int> { pos[0] + x, pos[1] + y });
                                    UnityEngine.Object.Instantiate(Group_obj, new Vector3(pos[0] + x, pos[1] + y, 0), Quaternion.identity, parent_object.transform);
                                    i++;
                                }
                        }

                    }
                }
                pos.Clear();

            }
        }

    }
}
