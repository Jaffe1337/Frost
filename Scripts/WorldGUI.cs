using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(World))]

public class WorldGUI : Editor
{
	public override void OnInspectorGUI()
	{
		World worldGen = (World)target;

		DrawDefaultInspector();


		if (GUILayout.Button("Generate"))
		{
			worldGen.button();
		}

	}
}
