using UnityEngine;
using UnityEditor;
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

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float terrainHeight = terrainData.GetHeight(x, y) / terrainData.size.y;

                for (int i = 0; i < layers; i++)
                {
                    alphaMaps[x, y, i] = 0;
                }

                for (int h = 0; h < Heights.Length; h++)
                {
                    if (terrainHeight > Heights[h])
                    {
                        alphaMaps[x, y, h] = 1;
                        break;
                    }
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMaps);
    }

    void Update()
    {

    }
}
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