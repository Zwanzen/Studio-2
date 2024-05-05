using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpreadPrefabsAcrossTerrain))]
public class SpreadEditor : Editor
{

    public override void OnInspectorGUI()
    {
        SpreadPrefabsAcrossTerrain spread = (SpreadPrefabsAcrossTerrain)target;
        spread.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", spread.prefab, typeof(GameObject), false);
        spread.numClones = EditorGUILayout.IntField("Number of Clones", spread.numClones);
        spread.terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", spread.terrain, typeof(Terrain), true);

        if (GUILayout.Button("Spread Prefabs"))
        {
            spread.CleanChildren();
            spread.PlaceCopies();
        }

        if (GUILayout.Button("Clear Prefabs"))
        {
            spread.CleanChildren();
        }
    }
}
