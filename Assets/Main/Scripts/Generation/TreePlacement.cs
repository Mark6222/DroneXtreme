using System.Collections.Generic;
using UnityEngine;

public class TreePlacement : MonoBehaviour
{
    void Start()
    {
        // TreeInstance tree = new TreeInstance
        // {
        //     position = new Vector3(0.5f, 0, 0.5f),
        //     widthScale = 1f,
        //     heightScale = 1f,
        //     color = Color.white,
        //     lightmapColor = Color.white,
        //     prototypeIndex = 0
        // };

        // Terrain terrain = GetComponent<Terrain>();
        // TerrainData terrainData = terrain.terrainData;
        // List<TreeInstance> trees = new List<TreeInstance>(terrainData.treeInstances);
        // trees.Add(tree);
        // terrainData.treeInstances = trees.ToArray();
    }
    void RemoveTreesAlongPath()
    {
        Vector3 removePosition = new Vector3(0.5f, 0, 0.5f);
        float removeRadius = 10f;

        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;
        List<TreeInstance> trees = new List<TreeInstance>(terrainData.treeInstances);

        trees.RemoveAll(t => Vector3.Distance(t.position, removePosition) < removeRadius);

        terrainData.treeInstances = trees.ToArray();
    }
    void ColourPath()
    {

    }
}
