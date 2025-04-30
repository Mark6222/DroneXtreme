using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;

public class ProcedurelGeneration : MonoBehaviour
{
    [SerializeField] private Transform Spawn;
    [SerializeField] private GameObject Spline;

    [SerializeField] private GameObject Spline2;
    [SerializeField] private GameObject Spline3;

    SplineContainer splineContainer;
    SplineContainer splineContainerTerrain;
    SplineContainer splineContainerTrolly;

    public int RaceSize = 100;
    public int amplitude = 20;
    public int randomNum = 30;
    float prevRandomX, prevRandomY, prevRandomZ;
    GameObject[] points;
    public Vector3[] pointTransforms;
    public bool targetTracking = false;

    void Start()
    {

        Debug.Log("ProcedurelGeneration Start");
        SpawnPoints();
        splineContainer = Spline.GetComponent<SplineContainer>();
        prevRandomX = UnityEngine.Random.Range(-randomNum, randomNum);
        prevRandomY = UnityEngine.Random.Range(-randomNum, randomNum);
        prevRandomZ = 0;
    }
    public void SpawnPoints()
    {
        Debug.Log("SpawnPoints called");
        pointTransforms = new Vector3[RaceSize];
        points = new GameObject[RaceSize];
        float x, y, z;
        x = Spawn.position.x;
        y = Spawn.position.y;
        z = Spawn.position.z;
        {
            splineContainer = Spline.GetComponent<SplineContainer>();
            splineContainerTerrain = Spline2.GetComponent<SplineContainer>();
            splineContainerTrolly = Spline3.GetComponent<SplineContainer>();
            splineContainer.Spline.Clear();
            splineContainerTerrain.Spline.Clear();
            for (int i = 0; i < RaceSize; i++)
            {
                float angle = (i / (float)RaceSize) * Mathf.PI * 2f;

                float circleX = Mathf.Cos(angle) * amplitude;
                float circleZ = Mathf.Sin(angle) * amplitude;

                Vector3 basePosition = new Vector3(x + circleX, y, z + circleZ);

                Vector3 forward = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

                Vector3 right = new Vector3(-forward.z, 0, forward.x);

                float moveX = UnityEngine.Random.Range(-prevRandomX + 20, prevRandomX + 20);
                float moveY = 0f;
                if (!targetTracking) moveY = UnityEngine.Random.Range(-prevRandomY + 10, prevRandomY + 10);
                prevRandomX = moveX;
                if (!targetTracking) prevRandomY = moveY;
                Vector3 offset = (right * moveX) + (Vector3.up * moveY);

                pointTransforms[i] = basePosition + offset;
                splineContainer.Spline.Add(new BezierKnot(pointTransforms[i]), TangentMode.AutoSmooth);
                splineContainerTerrain.Spline.Add(new BezierKnot(pointTransforms[i]), TangentMode.AutoSmooth);
                splineContainerTrolly.Spline.Add(new BezierKnot(pointTransforms[i]), TangentMode.AutoSmooth);
                x = x + circleX;
                z = z + circleZ;
                Debug.Log($"Point {i}: {pointTransforms[i]}");
            }
            splineContainer.Spline.Closed = true;
            splineContainerTerrain.Spline.Closed = true;
            splineContainerTrolly.Spline.Closed = true;
        }
        if (SceneManager.GetActiveScene().name != "TargetTracking")
        {
            Spline.GetComponent<SplineInstantiate>().enabled = true;
            gameObject.GetComponent<PointsManager>().managePoints = true;
        }
        Spline2.GetComponent<SplineInstantiate>().enabled = true;
        gameObject.GetComponent<TerrainGenerater>().Generate();

    }
    int num = 0;
    bool Once = true;
    public void Test()
    {
        if (Once) points = GameObject.FindGameObjectsWithTag("Point");
        Once = false;
        points[num].SetActive(false);
        num = num + 1;
    }
    void Update()
    {

    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(ProcedurelGeneration))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ProcedurelGeneration myScript = (ProcedurelGeneration)target;

        if (GUILayout.Button("Generate"))
        {
            myScript.SpawnPoints();
        }
        if (GUILayout.Button("Test"))
        {
            myScript.Test();
        }
    }
}
#endif
