using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Proximity : MonoBehaviour
{
    public string targetTag = "Wall";
    public Text multiplyerText;
    public Text SpeedText;
    public float maxRange = 5f;

    private GameObject[] wallObjects;
    public GameObject[] proxyMeterUI;
    private Rigidbody rig;
    private float Speed = 0;
    float proximityValue = 0;
    float score = 0;
    float multiplyer = 0;
    bool scoreCountStarted = false;
    float addScore = 100;
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        wallObjects = GameObject.FindGameObjectsWithTag(targetTag);
    }

    void Update()
    {
        float closestDistance = Mathf.Infinity;
        Speed = rig.linearVelocity.magnitude;
        foreach (GameObject wall in wallObjects)
        {
            Vector3 pointOnWall = wall.GetComponent<Collider>().ClosestPoint(transform.position);
            float distance = Vector3.Distance(transform.position, pointOnWall);
            if (distance < closestDistance)
            {
                closestDistance = distance;
            }
        }
        if (closestDistance <= maxRange)
        {
            proximityValue = Mathf.Lerp(100, 0, closestDistance / maxRange);
        }
        multiplyerText.text = "x" + multiplyer;
        if (Speed > 15)
        {
            if (proximityValue > 30)
            {
                multiplyer = 1.0f;
                if (Speed > 40) multiplyer = multiplyer + 0.5f;
            }
            else if (proximityValue > 60)
            {
                multiplyer = 2.0f;
                if (Speed > 60) multiplyer = multiplyer + 0.5f;
            }
            else if (proximityValue > 90)
            {
                multiplyer = 3.0f;
            }
            else
            {
                multiplyer = 0.0f;
            }
            if (proximityValue > 15)
            {
                if (!scoreCountStarted)
                {
                    scoreCountStarted = true;
                    score = 0;
                }
                SpeedText.text = "" + score;
                if (!scoreCountStarted) score = 0;
                score = score + (addScore * multiplyer);
            }
            else
            {
                scoreCountStarted = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        addScore = 0;
        StartCoroutine(WaitAndDoSomething());
    }

    IEnumerator WaitAndDoSomething()
    {
        yield return new WaitForSeconds(2f);
        addScore = 100;
    }
}
