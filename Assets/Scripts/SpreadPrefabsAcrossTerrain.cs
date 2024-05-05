using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SpreadPrefabsAcrossTerrain : MonoBehaviour
{
    public GameObject prefab;
    public int numClones;
    public Terrain terrain;

    public int selectedGroup = 0;
    public List<Transform> Groups = new List<Transform>();

    public bool randomXZRotation = false;
    [HideInInspector]
    public float randomScaleMultiplierMin = 1.0f;
    [HideInInspector]
    public float randomScaleMultiplierMax = 1.0f;
    public bool randomWidth = false;
    [HideInInspector]
    public float randomWidthMultiplierMin = 1.0f;
    [HideInInspector]
    public float randomWidthMultiplierMax = 1.0f;

    public void CleanChildren()
    {
        int nbChildren = Groups[selectedGroup - 1].transform.childCount;

        for (int i = nbChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(Groups[selectedGroup - 1].GetChild(i).gameObject);
        }
    }

    public void CreateGroup()
    {
        GameObject group = new GameObject("Group " + (Groups.Count + 1));
        group.transform.parent = transform;
        Groups.Add(group.transform);
        selectedGroup++;
    }

    public void RemoveGroup()
    {
        // If there are no groups, return
        if (Groups.Count == 0)
            return;
        // If the selected group has transform, destroy it
        if (Groups[selectedGroup - 1].transform != null)
            DestroyImmediate(Groups[selectedGroup-1].gameObject);


        Groups.RemoveAt(selectedGroup-1);
        ReorderGroupNames();
    }

    public void FixList()
    {
        Groups.RemoveAt(selectedGroup - 1);
    }

    private void ReorderGroupNames()
    {
        for (int i = 0; i < Groups.Count; i++)
        {
            Groups[i].name = "Group " + (i + 1);
        }
    }

    public void PlaceCopies()
    {
        if(Groups.Count == 0)
        {
            CreateGroup();
        }

        // Calculate the terrain size
        Vector3 terrainSize = terrain.terrainData.size;

        // Calculate the number of clones per row and column
        int clonesPerRow = (int)Mathf.Sqrt(numClones);
        int clonesPerCol = clonesPerRow;

        // Calculate the spacing between clones
        float spacing = Mathf.Min(terrainSize.x / clonesPerRow, terrainSize.z / clonesPerCol);

        // Spread the clones across the terrain
        for (int i = 0; i < clonesPerRow; i++)
        {
            for (int j = 0; j < clonesPerCol; j++)
            {
                /*
                // Calculate the position of the clone
                float x = i * spacing + terrain.transform.position.x;
                float z = j * spacing + terrain.transform.position.z;
                */
                float tx = terrain.transform.position.x;
                float tz = terrain.transform.position.z;


                Vector3 randPos = new Vector3(Random.Range(tx, terrainSize.x + tx), 0, Random.Range(tz, terrainSize.z + tz));

                // Adding height to the vector
                randPos = new(randPos.x, terrain.SampleHeight(randPos), randPos.z);

                // Rotate to align with terrain normal
                Vector3 normal = terrain.terrainData.GetInterpolatedNormal((randPos.x - tx) / terrainSize.x, (randPos.z - tz) / terrainSize.z);
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);

                // Add a random rotation around local Y
                rotation *= Quaternion.Euler(0, Random.Range(0, 360), 0);
                if (randomXZRotation)
                    rotation *= Quaternion.Euler(Random.Range(0, 360), 0, Random.Range(0, 360));

                // Instantiate the prefab at the calculated position
                GameObject clone = Instantiate(prefab, randPos, Quaternion.identity);

                // Apply rotation
                clone.transform.rotation = rotation;

                // Apply random scale
                if (!randomWidth)
                {
                    clone.transform.localScale = Vector3.one * Random.Range(randomScaleMultiplierMin, randomScaleMultiplierMax);
                }
                else
                {
                    float xScale = Random.Range(randomWidthMultiplierMin, randomWidthMultiplierMax);
                    float yScale = Random.Range(randomScaleMultiplierMin, randomScaleMultiplierMax);
                    float zScale = Random.Range(randomWidthMultiplierMin, randomWidthMultiplierMax);
                    clone.transform.localScale = new Vector3(xScale, yScale, zScale);
                }

                // Set the parent of the clone to the terrain
                clone.transform.parent = Groups[selectedGroup - 1].transform;
            }
        }
    }
}