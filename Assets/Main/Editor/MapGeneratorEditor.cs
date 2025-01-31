using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor (typeof (MapGenerater))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI(){
        MapGenerater mapGen = (MapGenerater)target;
        if(DrawDefaultInspector()){
            if(mapGen.autoUpdate){
                mapGen.GenerateMap();
            }
        }
        if(GUILayout.Button ("Generate")){
            mapGen.GenerateMap();
        }
    }
}
