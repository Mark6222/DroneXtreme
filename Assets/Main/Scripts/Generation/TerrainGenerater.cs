using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

public class TerrainGenerater : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
    [SerializeField] private float heightMultiplier = 10f;
    [SerializeField] private int treeCount = 10000;
    [SerializeField] private float minHeight = 0.2f;
    [SerializeField] private float maxHeight = 0.9f;
    public Texture2D texture;
    public int textureSize = 1000;
    Transform[] pointTransforms;
    public float baseRadius = 45f;
    public float slopeRadius = 10f;
    public int smoothingIterations = 5;
    public float removeRadius = 130f;


    void Start()
    {
        StartCoroutine(RunForTenSeconds());
    }

    IEnumerator RunForTenSeconds()
    {
        float duration = 2;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (terrain == null) terrain = Terrain.activeTerrain;

        MapDisplay display = FindAnyObjectByType<MapDisplay>();
        texture = display.texture2D;

        GameObject[] terrainHeights = GameObject.FindGameObjectsWithTag("TerrainHeight");
        int i = 0;
        pointTransforms = new Transform[terrainHeights.Length];
        foreach (GameObject point in terrainHeights)
        {
            pointTransforms[i] = point.transform;
            Destroy(point);
            i++;
        }

        ApplyHeightMap();
        SetTerrainHeights();
        SmoothTerrain();
        GenerateTrees();
    }

    void SetTerrainHeights()
    {
        TerrainData terrainData = terrain.terrainData;
        int heightmapResolution = terrainData.heightmapResolution;
        float[,] heights = terrainData.GetHeights(0, 0, heightmapResolution, heightmapResolution);

        Vector3 terrainSize = terrainData.size;
        Vector3 terrainPosition = terrain.transform.position;

        foreach (Transform point in pointTransforms)
        {
            float targetHeight = (point.position.y - 20 - terrainPosition.y) / terrainSize.y;

            int centerX = Mathf.RoundToInt((point.position.x - terrainPosition.x) / terrainSize.x * (heightmapResolution - 1));
            int centerZ = Mathf.RoundToInt((point.position.z - terrainPosition.z) / terrainSize.z * (heightmapResolution - 1));

            int baseRadiusPixels = Mathf.RoundToInt((baseRadius / terrainSize.x) * heightmapResolution);
            int slopeRadiusPixels = Mathf.RoundToInt((slopeRadius / terrainSize.x) * heightmapResolution);
            int totalRadiusPixels = baseRadiusPixels + slopeRadiusPixels;

            for (int x = -totalRadiusPixels; x <= totalRadiusPixels; x++)
            {
                for (int z = -totalRadiusPixels; z <= totalRadiusPixels; z++)
                {
                    int terrainX = Mathf.Clamp(centerX + x, 0, heightmapResolution - 1);
                    int terrainZ = Mathf.Clamp(centerZ + z, 0, heightmapResolution - 1);

                    float distance = Mathf.Sqrt(x * x + z * z);
                    if (distance > totalRadiusPixels) continue;

                    float heightFactor = 1f;

                    float newHeight = targetHeight * heightFactor;
                    heights[terrainZ, terrainX] = newHeight;
                }
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }
    public void GenerateTrees()
    {
        TerrainData terrainData = terrain.terrainData;
        List<TreeInstance> newTrees = new List<TreeInstance>();

        for (int i = 0; i < treeCount; i++)
        {
            float randomX = UnityEngine.Random.Range(0f, 1f);
            float randomZ = UnityEngine.Random.Range(0f, 1f);

            float worldX = terrain.transform.position.x + randomX * terrainData.size.x;
            float worldZ = terrain.transform.position.z + randomZ * terrainData.size.z;
            float worldY = terrain.SampleHeight(new Vector3(worldX, 0, worldZ)) + terrain.transform.position.y;

            if (worldY < minHeight || worldY > maxHeight)
                continue;

            TreeInstance tree = new TreeInstance
            {
                position = new Vector3(randomX, (worldY - terrain.transform.position.y) / terrainData.size.y, randomZ),
                prototypeIndex = 0,
                widthScale = 1f,
                heightScale = 1f,
                color = Color.white,
                lightmapColor = Color.white
            };
            Vector3 worldPosition = new Vector3(
            tree.position.x * terrain.terrainData.size.x + terrain.transform.position.x,
            tree.position.y * terrain.terrainData.size.y + terrain.transform.position.y,
            tree.position.z * terrain.terrainData.size.z + terrain.transform.position.z
            );
            bool remove = false;
            foreach (Transform point in pointTransforms)
            {
                if (Vector3.Distance(worldPosition, point.position) <= removeRadius)
                {
                    remove = true;
                    break;
                }
            }
            if (!remove) newTrees.Add(tree); ;
        }

        terrainData.treeInstances = newTrees.ToArray();
    }

    void SmoothTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int iteration = 0; iteration < smoothingIterations; iteration++)
        {
            heightMap = ApplySmoothing(heightMap, terrainData.heightmapResolution);
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    float[,] ApplySmoothing(float[,] heightMap, int resolution)
    {
        float[,] newHeightMap = (float[,])heightMap.Clone();

        for (int x = 1; x < resolution - 1; x++)
        {
            for (int z = 1; z < resolution - 1; z++)
            {
                float sum = 0f;
                int count = 0;

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dz = -1; dz <= 1; dz++)
                    {
                        sum += heightMap[x + dx, z + dz];
                        count++;
                    }
                }
                newHeightMap[x, z] = sum / count;
            }
        }

        return newHeightMap;
    }

    void ApplyHeightMap()
    {
        TerrainData terrainData = terrain.terrainData;
        int width = terrainData.heightmapResolution;
        int height = terrainData.heightmapResolution;

        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float normalizedX = (float)x / width;
                float normalizedY = (float)y / height;
                float pixelHeight = texture.GetPixelBilinear(normalizedX, normalizedY).grayscale;

                heights[y, x] = pixelHeight * heightMultiplier / terrainData.size.y;
            }
        }

        terrainData.SetHeights(0, 0, heights);
        Debug.Log("Heightmap applied to terrain!");
    }
}
[CustomEditor(typeof(TerrainGenerater))]
public class MyScriptEditor3 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainGenerater myScript = (TerrainGenerater)target;

        if (GUILayout.Button("Generate"))
        {
            myScript.GenerateTrees();
        }
    }
}