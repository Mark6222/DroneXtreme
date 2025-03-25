using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class Spawn : NetworkBehaviour
{
    GameObject[] Drones;
    GameObject[] points;
    GameObject[] RingSpawns;
    public ParticleSystem partival;

    public GameObject Prefab;

    public void SpawnPlayer()
    {
        points = gameObject.GetComponent<PointsManager>().points;
        RingSpawns = GameObject.FindGameObjectsWithTag("RingSpawnPoint");
        Transform spawnPoint = points[0].transform;
        Vector3 spawnPosition = spawnPoint.position - spawnPoint.forward * 2f;

        if (NetworkManager.Singleton.IsServer)
        {
            Drones = GameObject.FindGameObjectsWithTag("Drone");
            foreach (GameObject drone in Drones)
            {
                drone.transform.position = RingSpawns[1].transform.position;
                drone.transform.LookAt(points[1].transform);
                drone.transform.rotation = Quaternion.Euler(drone.transform.rotation.eulerAngles.x, drone.transform.rotation.eulerAngles.y + 90, drone.transform.rotation.eulerAngles.z + 50);
            }
        }
        else
        {
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
