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
    [SerializeField] private GameObject prefab;

    [SerializeField] private Transform Spawn;
    SplineContainer splineContainer;
    SplineInstantiate splineInstantiate;
    public int RaceSize = 100;
    public int length = 100;
    public int amplitude = 20;
    public int randomNum = 50;
    public int rotateX, rotateY, rotateZ;
    int rotatePrev = 0;
    GameObject[] points;
    void Start()
    {
        SpawnPoints();
        splineContainer = splineObject.GetComponent<SplineContainer>();
        splineInstantiate = splineObject.GetComponent<SplineInstantiate>();
    }
    public void AddGameObject(GameObject newObj)
    {
        Array.Resize(ref points, points.Length + 1);
        points[points.Length - 1] = newObj;
    }
    public void SpawnPoints()
    {
        points = new GameObject[RaceSize];
        float x, y, z;
        x = Spawn.position.x;
        y = Spawn.position.y;
        z = Spawn.position.z;
        {
            splineContainer = splineObject.GetComponent<SplineContainer>();
            splineInstantiate = splineObject.GetComponent<SplineInstantiate>();
            splineContainer.Spline.Clear();
            for (int i = 0; i < RaceSize; i++)
            {
                float angle = (i / (float)RaceSize) * Mathf.PI * 2f;

                float radiusVariation = UnityEngine.Random.Range(-randomNum, randomNum);
                float finalRadius = amplitude + radiusVariation;

                float circleX = Mathf.Cos(angle) * finalRadius;
                float circleY = Mathf.Sin(angle) * finalRadius;
                float randomX = UnityEngine.Random.Range(-randomNum, randomNum);
                float randomY = UnityEngine.Random.Range(-randomNum, randomNum);
                float randomZ = UnityEngine.Random.Range(-randomNum, randomNum);


                splineContainer.Spline.Add(new BezierKnot(new Vector3(x + circleX + randomX, y + circleY + randomY, z + randomZ)), TangentMode.AutoSmooth);
                x = x + circleX;
                y = y + circleY;
            }

            splineContainer.Spline.Closed = true;
        }
    }
    int num = 0;
    bool Once = true;
    public void Test()
    {
        if(Once) points = GameObject.FindGameObjectsWithTag("Point");
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