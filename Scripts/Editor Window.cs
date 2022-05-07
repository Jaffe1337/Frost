using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.Tilemaps;
using SimplexNoise;
using Frost;

class FrostEditor : EditorWindow
{
    public int width, height;
    public float modifier = 0.01f;
    public int seed = 0;
    public Tile Coast;
    public Tile Water;
    public Tile Main_land;
    public Tile Biome_1;
    public Tile Biome_2;
    public Tilemap map;
    public Object Single_obj;
    public Object Group_obj;
    public int amount;
    public string[] target_biome_array = new string[] { "Main land", "Biome 1", "Biome 2" };
    public int target_biome;
    public int index = 0;

    public float waterPercent = 50f;
    public float beachPercent = 10f;

    Vector2 scrollPos;

    [MenuItem("Window/Frost Editor")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(FrostEditor));
    }

    void convert_target_biome()
    {
        switch (index)
        {
            case 0:
                target_biome = 4;
                break;
            case 1:
                target_biome = 5;
                break;
            case 2:
                target_biome = 6;
                break;
            default:
                Debug.LogError("Unrecognized Option");
                break;
        }
    }

    public bool restraincheck(int pos_x, int x, int pos_y, int y)
    {
        if (pos_x + x > 0 & pos_y + y > 0)
        {
            if (pos_x + x < width & pos_y < height)
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


    void OnGUI()
    {
        EditorGUIUtility.labelWidth = 100;

        var label_style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };


        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Space(5);
        GUILayout.Label("Map settings", label_style);
        GUILayout.Space(3);

        width = EditorGUILayout.IntField("Map width", width);
        height = EditorGUILayout.IntField("Map height", height);
        map = (Tilemap)EditorGUILayout.ObjectField("Tile Map", map, typeof(Tilemap), true);
        modifier = EditorGUILayout.FloatField("Modifier", modifier);
        seed = EditorGUILayout.IntField("Seed", seed);
        waterPercent = EditorGUILayout.FloatField("Water percent", waterPercent);
        beachPercent = EditorGUILayout.FloatField("Beach percent", beachPercent);

        GUILayout.Space(3);
        GUILayout.Label("Tiles", label_style);
        GUILayout.Space(5);

        Coast = (Tile)EditorGUILayout.ObjectField("Coast Tile", Coast, typeof(Tile), true);
        Water = (Tile)EditorGUILayout.ObjectField("Water Tile", Water, typeof(Tile), true);
        Main_land = (Tile)EditorGUILayout.ObjectField("Main biome Tile", Main_land, typeof(Tile), true);
        Biome_1 = (Tile)EditorGUILayout.ObjectField("Biome_1 Tile", Biome_1, typeof(Tile), true);
        Biome_2 = (Tile)EditorGUILayout.ObjectField("Biome_2 Tile", Biome_2, typeof(Tile), true);

        GUILayout.Space(15);

        if (GUILayout.Button("Generate Map"))
        {
            map.ClearAllTiles();
            Map.setTiles(Coast, Water, Main_land, Biome_1, Biome_2, map);
            Generation.world(width, height, seed, modifier, waterPercent, beachPercent);
            seed = Setup.seed;
            
        }

        GUILayout.Space(20);
        GUILayout.Label("Object generation settings", label_style);
        GUILayout.Space(5);

        Single_obj = EditorGUILayout.ObjectField("Single object", Single_obj, typeof(Object), true);
        Group_obj = EditorGUILayout.ObjectField("Grouped objects", Group_obj, typeof(Object), true);
        amount = EditorGUILayout.IntField("Object amount", amount);

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Target biome", label_style);
        GUILayout.FlexibleSpace();
        index = EditorGUILayout.Popup(index, target_biome_array, GUILayout.Width(250));
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(15);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Generate object"))
        {
            convert_target_biome();
            ObjectGeneration.randomObj(Single_obj, target_biome, amount);

        }
        GUILayout.Space(3);
        if (GUILayout.Button("Generate group objects"))
        {
            convert_target_biome();
            ObjectGeneration.randomMultiObj(Group_obj, target_biome, amount);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }
}