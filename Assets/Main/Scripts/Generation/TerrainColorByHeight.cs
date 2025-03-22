using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
public class TerrainColorByHeight : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
    [SerializeField] private TerrainLayer[] terrainLayers;

    [Range(0, 1)]
    [SerializeField] private float[] Heights;

    void Start()
    {
        HeightColoring();
    }

    public void HeightColoring()
    {
        TerrainData terrainData = terrain.terrainData;
        terrainData.terrainLayers = terrainLayers;
        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;
        int layers = terrainData.alphamapLayers;

        float[,,] alphaMaps = terrainData.GetAlphamaps(0, 0, width, height);
        float[] heights = new float[width * height];
        float max = 0;
        float min = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float num = terrainData.GetHeight(y, x);
                if (num > max)
                {
                    max = num;
                }
                else if (num < min)
                {
                    min = num;
                }
            }
        }
        // Debug.Log("Max: " + max + " Min: " + min);
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                float normalizedHeight = terrainData.GetHeight(y, x);
                // Debug.Log("Normalized size: " + normalizedHeight);
                for (int i = 0; i < layers; i++)
                {
                    alphaMaps[x, y, i] = 0;
                }

                for (int h = 0; h < layers; h++)
                {
                    if (normalizedHeight > Heights[h] * max)
                    {
                        alphaMaps[x, y, h] = 1;
                        break;
                    }

                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMaps);
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(TerrainColorByHeight))]
public class MyScriptEditor2 : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainColorByHeight myScript = (TerrainColorByHeight)target;

        if (GUILayout.Button("Generate"))
        {
            myScript.HeightColoring();
        }
    }
}
#endif