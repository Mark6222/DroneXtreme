using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public GameObject[] points;
    Transform[] RingSpawns;
    int currentPoint = 0;
    private float timer = 0f;
    private bool hasTriggered = false;
    bool firstTime = true;
    public bool managePoints = false;

    void Update()
    {
        if (managePoints)
        {
            Debug.Log("managePoints is true");
            if (!hasTriggered)
            {
                timer += Time.deltaTime;
                Debug.Log("Timer: " + timer);

                if (timer >= 1f)
                {
                    points = GameObject.FindGameObjectsWithTag("Point");
                    Debug.Log("Points found: " + points.Length);
                    gameObject.GetComponent<Spawn>().SpawnPlayer();
                    if (points.Length > 0) // Ensure points array is not empty
                    {
                        GameObject[] ringSpawnPoints = GameObject.FindGameObjectsWithTag("RingSpawnPoint");
                        RingSpawns = new Transform[ringSpawnPoints.Length];
                        for (int i = 0; i < ringSpawnPoints.Length; i++)
                        {
                            RingSpawns[i] = ringSpawnPoints[i].transform;
                        }
                        foreach (GameObject point in points)
                        {
                            point.SetActive(false);
                        }
                        points[currentPoint].SetActive(true);
                        hasTriggered = true;
                    }
                    else
                    {
                        Debug.LogError("No points found with tag 'Point'");
                    }
                }
            }
            if (currentPoint < points.Length)
            {
                Debug.Log("Current point: " + currentPoint);
                // or firstTime == true
                if (points[currentPoint].GetComponent<Ring>().isReached || firstTime)
                {
                    firstTime = false;
                    points[currentPoint].SetActive(false);
                    currentPoint++;
                    if (currentPoint < points.Length)
                    {
                        points[currentPoint].SetActive(true);
                        points[currentPoint].GetComponent<Ring>().SetCurrntRing();
                        if (currentPoint + 1 < points.Length) // Ensure the next point exists
                        {
                            points[currentPoint + 1].SetActive(true);
                            points[currentPoint + 1].GetComponent<Ring>().SetNextRing();
                        }
                    }
                }
            }
        }
    }
}