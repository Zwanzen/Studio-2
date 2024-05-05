using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpreadPrefabsAcrossTerrain))]
public class SpreadEditor : Editor
{
    float SminVal = 1;
    float SminLimit = 0.1f;
    float SmaxVal = 1;
    float SmaxLimit = 10;

    float WminVal = 1;
    float WminLimit = 0.1f;
    float WmaxVal = 1;
    float WmaxLimit = 10;
    public override void OnInspectorGUI()
    {
        SpreadPrefabsAcrossTerrain spread = (SpreadPrefabsAcrossTerrain)target;
        spread.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", spread.prefab, typeof(GameObject), false);
        spread.numClones = EditorGUILayout.IntField("Number of Clones", spread.numClones);
        spread.terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", spread.terrain, typeof(Terrain), true);
        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField("Group Amount:", spread.Groups.Count.ToString());
        if(spread.Groups.Count == 0)
        {
            EditorGUILayout.LabelField("No Groups");
        }
        else
        {
            spread.selectedGroup = EditorGUILayout.IntSlider("Selected Group", spread.selectedGroup, 1, spread.Groups.Count);
        }
        if (GUILayout.Button("Create Group"))
        {
            spread.CreateGroup();
        }
        if (GUILayout.Button("Remove Group"))
        {
            spread.RemoveGroup();
        }

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Fix List"))
        {
            spread.FixList();
        }

        EditorGUILayout.Space(20);
        spread.randomXZRotation = EditorGUILayout.Toggle("Random XZ Rotation", spread.randomXZRotation);
        EditorGUILayout.LabelField("Min:", SminVal.ToString());
        EditorGUILayout.LabelField("Max:", SmaxVal.ToString());
        EditorGUILayout.MinMaxSlider("Scale Multiplier",ref SminVal, ref SmaxVal, SminLimit, SmaxLimit);
        spread.randomScaleMultiplierMin = SminVal;
        spread.randomScaleMultiplierMax = SmaxVal;
        spread.randomWidth = EditorGUILayout.Toggle("Random Width", spread.randomWidth);
        if (spread.randomWidth)
        {
            EditorGUILayout.LabelField("Min:", WminVal.ToString());
            EditorGUILayout.LabelField("Max:", WmaxVal.ToString());
            EditorGUILayout.MinMaxSlider("Scale Multiplier",ref WminVal, ref WmaxVal, WminLimit, WmaxLimit);
            spread.randomWidthMultiplierMin = WminVal;
            spread.randomWidthMultiplierMax = WmaxVal;
        }

        if (GUILayout.Button("Spread Prefabs"))
        {
            if(spread.Groups.Count != 0)
                spread.CleanChildren();
            spread.PlaceCopies();
        }

        if (GUILayout.Button("Clear Prefabs"))
        {
            spread.CleanChildren();
        }
    }
}
