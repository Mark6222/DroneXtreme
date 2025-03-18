using UnityEngine;

public class PointsManager : MonoBehaviour
{
    GameObject[] points;
    int currentPoint = 0;
    private float timer = 0f;
    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered)
        {
            timer += Time.deltaTime;

            if (timer >= 1.2f)
            {
                points = GameObject.FindGameObjectsWithTag("Point");
                foreach (GameObject point in points)
                {
                    point.SetActive(false);
                }
                points[currentPoint].SetActive(true);
                hasTriggered = true;
            }
        }
        if (currentPoint < points.Length)
        {
            if (points[currentPoint].GetComponent<Ring>().isReached)
            {
                points[currentPoint].SetActive(false);
                currentPoint++;
                if (currentPoint < points.Length)
                {
                    points[currentPoint].SetActive(true);
                    points[currentPoint].SetActive(true);
                }
            }
        }
    }
}