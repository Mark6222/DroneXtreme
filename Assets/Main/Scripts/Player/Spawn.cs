using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    GameObject[] Drones;
    GameObject[] points;
    GameObject[] RingSpawns;
    public ParticleSystem partival;

    public GameObject Prefab;

    public void SpawnPlayer()
    {
        Debug.Log("SpawnPlayer called");
        points = gameObject.GetComponent<PointsManager>().points;
        RingSpawns = GameObject.FindGameObjectsWithTag("RingSpawnPoint");
        if (RingSpawns.Length < 2)
        {
            Debug.LogError("Not enough RingSpawnPoints found");
            return;
        }
        Debug.Log("RingSpawns found: " + RingSpawns.Length);
        Transform spawnPoint = points[0].transform; 
        Vector3 spawnPosition = spawnPoint.position - spawnPoint.forward * 2f;

        if (PlayerPrefs.GetInt("IsMultiplayer") == 1)
        {
            Debug.Log("Multiplayer mode");
            Drones = GameObject.FindGameObjectsWithTag("Drone");
            foreach (GameObject drone in Drones)
            {
                Debug.Log("Setting drone position and rotation");
                drone.transform.position = RingSpawns[1].transform.position;
                drone.transform.LookAt(points[1].transform);
                drone.transform.rotation = Quaternion.Euler(drone.transform.rotation.eulerAngles.x, drone.transform.rotation.eulerAngles.y + 90, drone.transform.rotation.eulerAngles.z + 50);
            }
        }
        else
        {
            Debug.Log("Single player mode");
            GameObject drone = Instantiate(Prefab, RingSpawns[1].transform.position += Vector3.up, Quaternion.identity);
            drone.transform.position = RingSpawns[1].transform.position;
            drone.transform.LookAt(points[1].transform);
            drone.transform.rotation = Quaternion.Euler(drone.transform.rotation.eulerAngles.x, drone.transform.rotation.eulerAngles.y + 90, drone.transform.rotation.eulerAngles.z + 50);
        }
    }
    // Update is called once per frame
    void Update()
    {
        // partival.Play();
    }
}
