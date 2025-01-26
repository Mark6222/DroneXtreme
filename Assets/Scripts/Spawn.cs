using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Transform spawn;
    GameObject [] Drones;
    void Start()
    {
        Drones = GameObject.FindGameObjectsWithTag("Drone");
        foreach(GameObject drone in Drones){
            drone.transform.position = spawn.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
