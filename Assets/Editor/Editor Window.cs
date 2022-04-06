using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.Tilemaps;

class FrostEditor : EditorWindow
{
    public int width, height;
    public float modifier;
    public float skip = 0.01f; // find new name
    public Tile Coast;
    public Tile Water;
    public Tile Main_land;
    public Tile Biome_1;
    public Tile Biome_2;
    public Tilemap map;
    public Object Single_obj;
    public Object Group_obj;
    public int amount;
    public string[] target_biome_array = new string[] {"Coast","Water","Main land","Biome 1", "Biome 2"};
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
                target_biome = 2;
                break;
            case 1:
                target_biome = 3;
                break;
            case 2:
                target_biome = 4;
                break;
            case 3:
                target_biome = 5;
                break;
            case 4:
                target_biome = 6;
                break;
            default:
                Debug.LogError("Unrecognized Option");
                break;
        }
    }

    void OnGUI()
    {
        
        EditorGUIUtility.labelWidth = 100;
        //EditorGUIUtility.fieldWidth = 20;
        var label_style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };


        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Space(5);
        GUILayout.Label("Map settings", label_style);
        GUILayout.Space(3);

        width = EditorGUILayout.IntField("Map width", width);      
        height = EditorGUILayout.IntField("Map height", height);       
        map = (Tilemap)EditorGUILayout.ObjectField("Tile Map", map, typeof(Tilemap), true);      
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
            Debug.Log(width);
        }

        GUILayout.Space(20);
        GUILayout.Label("Object generation settings",label_style);
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
            Debug.Log(target_biome);

        }
        GUILayout.Space(3);
        if (GUILayout.Button("Generate group objects"))
        {
           
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();

       
    }
}