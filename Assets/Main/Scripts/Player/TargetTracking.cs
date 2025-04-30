using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class TargetTracking : MonoBehaviour
{
    public TextMeshProUGUI TrackingScore, RaceTime, Countdown;
    private bool trackingStarted;
    public GameObject EndScreen;
    public GameObject PlayerItem;
    public GameObject Content;
    public TextMeshProUGUI TimeTitle;
    private float timeRemaining;
    public float timeTaken = 300f;

    public List<GameObject> PlayersList = new List<GameObject>();
    GameObject[] Players;
    PlayerScore playerScore;
    public CinemachineSplineCart cinemachineSplineCart;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target");

        playerScore = new PlayerScore();
        gameObject.GetComponent<PlayerMovement>().Freeze();
        EndScreen.SetActive(false);
        RaceTime.text = "";
        TrackingScore.text = "";
        Countdown.text = "";
        trackingStarted = false;
    }
    public void ShowUI()
    {
        StartCoroutine(StartCountdown());
    }
    IEnumerator StartCountdown()
    {
        int countdownTime = 3;
        while (countdownTime > 0)
        {
            if (this == null) yield break;
            Countdown.text = countdownTime.ToString();
            yield return new WaitForSeconds(1);
            countdownTime--;
        }
        if (this == null) yield break;
        Countdown.text = "Go!";
        gameObject.GetComponent<PlayerMovement>().UnFreeze();
        cinemachineSplineCart.AutomaticDolly.Enabled = true;
        StartTargetTracking();
        yield return new WaitForSeconds(1);
        if (this == null) yield break;
        Countdown.text = "";
    }

    private void StartTargetTracking()
    {
        Players = GameObject.FindGameObjectsWithTag("Drone");
        PlayersList.Clear();
        playerScore.ResetScoreToAdd();
        playerScore.ResetScore();
        trackingStarted = true;
        timeRemaining = timeTaken;
    }
    public int targetTrackingThreshold = 10;
    public int targetTrackingIterations = 10;
    public GameObject target;
    public Slider slider;
    public GameObject distanceMeter;
    private float scoreUpdateTimer = 0f;
    public float scoreUpdateInterval = 0.05f;
    void Update()
    {
        if (target != null && trackingStarted)
        {
            timeRemaining -= Time.deltaTime;
            RaceTime.text = timeRemaining.ToString("F2") + "s";
            if (timeRemaining > 0.01f)
            {
                timeRemaining -= Time.deltaTime;
                RaceTime.text = timeRemaining.ToString("F2") + "s";
            }
            else
            {
                timeRemaining -= Time.deltaTime;
                RaceTime.text = "";
                EndTracking();
            }


            float distance = Vector3.Distance(transform.position, target.transform.position);
            scoreUpdateTimer += Time.deltaTime;
            if (scoreUpdateTimer >= scoreUpdateInterval)
            {
                AddScoreForDistance((int)distance);
                scoreUpdateTimer = 0f;
            }
            if (distance < 40)
            {
                slider.value = 40 - distance;
            }
            else if (distance > 40 && distance < 60)
            {
                int t = (int)(distance - 40f);
                slider.value = 0 - t;
            }
            else
            {
                slider.value = -20;
            }
            TrackingScore.text = "Score" + playerScore.GetScore().ToString("F2");
        }
        else
        {
            RaceTime.text = "";
            TrackingScore.text = "";
        }
    }
    public int scoreIterations = 15;
    public int scoreAdd = 4;
    int score = 0;
    public void AddScoreForDistance(int distance)
    {
        int MatchDistance = 0;
        for (int i = 0; i < scoreIterations; i++)
        {
            int previousDistance = MatchDistance;
            MatchDistance += scoreAdd;
            if (distance < MatchDistance && distance > previousDistance)
            {
                int scoreToAdd = 10 - i;
                Debug.Log("Distance: " + distance + " MatchDistance: " + MatchDistance + " i: " + i + " scoreToAdd: " + scoreToAdd);
                playerScore.AddPointsForTracking(scoreToAdd);
                break;
            }
            else if(distance > 60)
            {
                playerScore.AddPointsForTracking(-5);
            }
        }
    }
    public void EndTracking()
    {
        gameObject.GetComponent<PlayerManeger>().playerCamera.SetActive(false);
        GameObject trollyCam = GameObject.FindGameObjectWithTag("TrollyCam");
        if (trollyCam != null)
        {
            trollyCam.SetActive(false);
        }
        RaceTime.enabled = false;
        TrackingScore.enabled = false;
        distanceMeter.SetActive(false);
        Countdown.enabled = false;
        trackingStarted = false;
        EndScreen.SetActive(true);
        Players = GameObject.FindGameObjectsWithTag("Drone");
        TimeTitle.text = "Score";
        foreach (GameObject player in Players)
        {
            GameObject item = Instantiate(PlayerItem, Content.transform);
            TextMeshProUGUI textComponent = item.transform.Find("Time").GetComponent<TextMeshProUGUI>();
            PlayersList.Add(item);
            if (textComponent != null)
            {
                textComponent.text = (playerScore.GetScore()).ToString("F2");
            }
        }
    }

    public void ResetEndScreen()
    {
        foreach (GameObject item in PlayersList)
        {
            Destroy(item);
        }
        EndScreen.SetActive(false);
    }

}
