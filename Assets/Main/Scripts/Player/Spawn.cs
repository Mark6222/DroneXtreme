using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    GameObject[] Drones;
    GameObject[] points;
    public ParticleSystem partival;

    public GameObject Prefab;

    void Start()
    {
        StartCoroutine(RunForTenSeconds());
    }
    IEnumerator RunForTenSeconds()
    {
        float duration = 5;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        points = GameObject.FindGameObjectsWithTag("Point");

        Transform spawnPoint = points[0].transform;
        Vector3 spawnPosition = spawnPoint.position - spawnPoint.forward * 2f;

        if (PlayerPrefs.GetInt("IsMultiplayer") == 1)
        {
            Drones = GameObject.FindGameObjectsWithTag("Drone");
            foreach (GameObject drone in Drones)
            {
                drone.transform.position = spawnPosition;
                drone.transform.rotation = spawnPoint.rotation;
                drone.transform.LookAt(spawnPoint);
            }
        }
        else
        {
            Instantiate(Prefab, spawnPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        partival.Play();
    }
}
