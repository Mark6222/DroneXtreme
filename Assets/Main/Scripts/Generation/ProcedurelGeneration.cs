using System;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Splines;
using System.Reflection;
using Unity.Entities;
using Unity.Collections;
using Unity.VisualScripting;
using Mono.Cecil;
public class ProcedurelGeneration : MonoBehaviour
{
    [SerializeField] private GameObject splineObject;
    [SerializeField] private Transform Spawn;
    SplineContainer splineContainer;
    [SerializeField] private SplineContainer splineContainerTerrain;
    public int RaceSize = 100;
    public int length = 100;
    public int amplitude = 20;
    public int randomNum = 30;
    float prevRandomX, prevRandomY, prevRandomZ;

    GameObject[] points;
    public Vector3[] pointTransforms;
    void Start()
    {
        SpawnPoints();
        splineContainer = splineObject.GetComponent<SplineContainer>();
        prevRandomX = UnityEngine.Random.Range(-randomNum, randomNum);
        prevRandomY = UnityEngine.Random.Range(-randomNum, randomNum);
        prevRandomZ = 0;
    }
    public void SpawnPoints()
    {
        pointTransforms = new Vector3[RaceSize];
        points = new GameObject[RaceSize];
        float x, y, z;
        x = Spawn.position.x;
        y = Spawn.position.y;
        z = Spawn.position.z;
        {
            splineContainer = splineObject.GetComponent<SplineContainer>();
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
                float moveY = UnityEngine.Random.Range(-prevRandomY + 20, prevRandomY + 20);
                prevRandomX = moveX;
                prevRandomY = moveY;
                Vector3 offset = (right * moveX) + (Vector3.up * moveY);

                pointTransforms[i] = basePosition + offset;
                splineContainer.Spline.Add(new BezierKnot(pointTransforms[i]), TangentMode.AutoSmooth);
                splineContainerTerrain.Spline.Add(new BezierKnot(pointTransforms[i]), TangentMode.AutoSmooth);
                x = x + circleX;
                z = z + circleZ;
            }
            // foreach(BezierKnot knot in splineContainer.Splin){

            // }
            splineContainer.Spline.Closed = true;
        }
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