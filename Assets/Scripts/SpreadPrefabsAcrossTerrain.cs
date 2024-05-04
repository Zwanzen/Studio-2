using UnityEngine;

public class SpreadPrefabsAcrossTerrain : MonoBehaviour
{
    // The prefab to be cloned
    public GameObject prefab;

    // The number of clones to spread
    public int numClones;

    // The terrain component
    public Terrain terrain;

    void Start()
    {
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
                // Calculate the position of the clone
                float x = i * spacing - terrainSize.x / 2;
                float z = j * spacing - terrainSize.z / 2;

                // Instantiate the prefab at the calculated position
                GameObject clone = Instantiate(prefab, new Vector3(x, 0, z), Quaternion.identity);

                // Set the parent of the clone to the terrain
                clone.transform.parent = terrain.transform;
            }
        }
    }
}