# Frost

Frost is a library that uses SimplexNoise to generate worlds for game development. It is aimed towards the Unity game engine and comes with pre-written scripts for quick implementation.


# How to import Frost

- First download the git repository.
- Then open up your Unity project and copy both the "Tiles" folder and the "Scripts" folder under Assets.
- Then go to "Frost/bin/Release/netstandard2.0/" and find both the "Frost.dll" and "SimplexNoise.dll" --> Put these under "Assets/Scripts/"
- You should now be able to open the Frost editor window under Window --> Frost editor. (Window is on the top hotbar)
- There will now appear a window where you can enter settings for your generation.


# Generate the map

Here is a short guide on how to generate a map using the Editor window.

- First you choose a map size. For example width = 250, height = 250.
- The seed can be left open because if it is not given a value, there will be generated a random value for us.
- Then we will need a tilemap. To get that right-click in the hierarchy tree on the left side. Then go 2D Object --> Tilemap --> Rectangular.
- Drag the newly created tilemap object over to the Tile Map value.
- After that you can optionally change the beach and/or water percent. 
- Lastly you will need to add the tiles to each biome value. You can add your own tiles or use the default ones under Assets/Tiles. Drag each tile to the biome you want it in. (Do not use the tile with the "play" button on it) 
- Then press Generate Map.



# Generate objects

There is an option between randomly generating singular objects across the map or group of objects. 

- First create a 2D Gameobject by either generating a default gameobject (GameObject/2D Object/Sprites/Some desired object) or importing a custom sprite by first dragging the picture from the “File explorer” into the Assets directory in Unity and then into the Unity Hierarchy.
- If you wish, add the components to the newly created object (the objects that are genearated are clones of the origianl object)
- In the FrostEditor drag the Gameobject to either Single_obj field for singular object scatter or Group_obj for grouped scatter.
- Choose the amount of objects/groups you want to have, in the amount field.
- Then choose which biome the objects will spawn in by clicking the dropdown menu and choosing the biome.
- Click "Generate object" for singular object scatter or Generate group objects for grouped scatter.

# Known working Unity versions

- Unity 2020.3


