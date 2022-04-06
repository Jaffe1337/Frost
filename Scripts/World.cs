using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;


public class World : MonoBehaviour
{
    public int width;
    public int height;


    // public int width, height;
    public float modifier = 0.01f; // find new name
    public int seed = 0;


    public Tile Tile2;
    public Tile Tile3;
    public Tile Tile4;
    public Tile Tile5;
    public Tile Tile6;

    public Tile Border;

    public Tilemap map;

    public float waterPercent = 50f;
    public float beachPercent = 10f;


    public void button()
    {
        // Tiles and map:
        Frost.Map.Tile2 = Tile2;
        Frost.Map.Tile3 = Tile3;
        Frost.Map.Tile4 = Tile4;
        Frost.Map.Tile5 = Tile5;
        Frost.Map.Tile6 = Tile6;

        Frost.Map.Border = Border;
        Frost.Map.map = map;

        // Values:
        Frost.Setup.width = width;
        Frost.Setup.height = height;
        Frost.Setup.noiseModifier = modifier;
        Frost.Setup.seed = seed;

        // Generation:

        map.ClearAllTiles();
        map = Frost.Generation.world();

        seed = Frost.Setup.seed;

    }


}



