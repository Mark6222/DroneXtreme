using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System;
public class MapGenerater : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap };
    public DrawMode drawMode;
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public bool autoUpdate;
    public int ocatves;
    public float lacunarity;
    [Range(0, 1)]
    public float persistance;
    public int seed;
    public Vector2 offset;
    public TerrainType[] regions;
    void Awake(){
        GenerateMap();
    }
    public void GenerateMap()
    {
        float[,] noiseMap = GenerateNoiseMap.GenerateMap(mapWidth, mapHeight, seed, noiseScale, ocatves, persistance, lacunarity, offset);
        Color[] colourMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapHeight; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindAnyObjectByType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerater.TextureFromHeightMap(noiseMap));
        }else if(drawMode == DrawMode.ColourMap){
            display.DrawTexture(TextureGenerater.TextureFromColourMap(colourMap, mapWidth, mapHeight));
        }
    }
    void OnValidate()
    {
        if (mapWidth < 1) mapWidth = 1;
        if (mapHeight < 1) mapHeight = 1;
        if (lacunarity < 1) lacunarity = 1;
        if (ocatves < 0) ocatves = 1;
    }
}
[System.Serializable]
public struct TerrainType
{
    public float height;
    public Color colour;
    public string name;

}