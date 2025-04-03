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
    public bool spawnPlayer = false;
    void Update()
    {
        if (managePoints)
        {
            if (!hasTriggered)
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    points = GameObject.FindGameObjectsWithTag("Point");
                    spawnPlayer = true;
                    if (points.Length > 0)
                    {
                        GameObject[] ringSpawnPoints = GameObject.FindGameObjectsWithTag("RingSpawnPoint");
                        RingSpawns = new Transform[ringSpawnPoints.Length];
                        points[1].GetComponent<Ring>().SetStartPanel();
                        points[points.Length - 1].GetComponent<Ring>().SetEndPanel();
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
                }
            }
            if (currentPoint < points.Length)
            {
                if (points[currentPoint].GetComponent<Ring>().isReached || firstTime)
                {
                    firstTime = false;
                    points[currentPoint].SetActive(false);
                    currentPoint++;
                    if (currentPoint < points.Length)
                    {
                        points[currentPoint].SetActive(true);
                        points[currentPoint].GetComponent<Ring>().SetCurrntRing();
                        if (currentPoint + 1 < points.Length)
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