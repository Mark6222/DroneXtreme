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
        if (PlayerPrefs.GetInt("IsMultiplayer") == 1)
        {
            Drones = GameObject.FindGameObjectsWithTag("Drone");
            foreach (GameObject drone in Drones)
            {
                drone.transform.position = points[0].transform.position;
            }
        }
        else
        {
            Instantiate(Prefab, points[0].transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // partival.Play();
    }
}
