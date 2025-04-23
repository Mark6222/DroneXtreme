using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;
using NUnit.Framework;
public class StuntManager : MonoBehaviour
{
    public TextMeshProUGUI RaceTime, Countdown;
    private float timeRemaining;
    private bool stuntStarted;
    public GameObject EndScreen;
    public GameObject PlayerItem;
    public GameObject Content;
    public TextMeshProUGUI ScoreTitle;
    public GameObject ScoringUI;
    public List<GameObject> PlayersList = new List<GameObject>();
    GameObject[] Players;
    void Start()
    {
        gameObject.GetComponent<PlayerMovement>().Freeze();
        EndScreen.SetActive(false);
        RaceTime.text = "";
        Countdown.text = "";
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
        ScoringUI.SetActive(true);
        gameObject.GetComponent<PlayerMovement>().UnFreeze();
        yield return new WaitForSeconds(1);
        if (this == null) yield break;
        Countdown.text = "";
        StartStunt();
    }

    void StartStunt()
    {
        Players = GameObject.FindGameObjectsWithTag("Drone");

        PlayersList.Clear();
        timeRemaining = 300f;
        stuntStarted = true;
    }

    public void EndStunt()
    {
        Debug.Log("EndStunt");
        ScoringUI.SetActive(false);
        gameObject.GetComponent<PlayerManeger>().playerCamera.SetActive(false);
        GameObject trollyCam = GameObject.FindGameObjectWithTag("TrollyCam");
        if (trollyCam != null)
        {
            trollyCam.SetActive(false);
        }
        stuntStarted = false;
        EndScreen.SetActive(true);
        gameObject.GetComponent<PlayerManeger>().StuntScores.Add(gameObject.GetComponent<TrickSystem>().playerScore.GetScore());
        Players = GameObject.FindGameObjectsWithTag("Drone");
        ScoreTitle.text = "Score(s)";
        foreach (GameObject player in Players)
        {
            GameObject item = Instantiate(PlayerItem, Content.transform);
            PlayersList.Add(item);
            TextMeshProUGUI textComponent = item.transform.Find("Time").GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                List<float> playerRaceTimes = player.GetComponent<PlayerManeger>().RaceTimes;
                if (playerRaceTimes != null)
                {
                    textComponent.text = (player.GetComponent<TrickSystem>().playerScore.GetScore()).ToString("F2") + "";
                }
                else
                {
                    textComponent.text = player.name;
                }
            }
        }
        gameObject.GetComponent<TrickSystem>().playerScore.ResetScore();
    }

    public void ResetEndScreen()
    {
        foreach (GameObject item in PlayersList)
        {
            Destroy(item);
        }
        EndScreen.SetActive(false);
    }
    void Update()
    {
        if (stuntStarted)
        {
            timeRemaining -= Time.deltaTime;
            RaceTime.text = timeRemaining.ToString("F2") + "s";
            if (timeRemaining > 0.01f)
            {
                timeRemaining -= Time.deltaTime;
                RaceTime.text = timeRemaining.ToString("F2") + "s";
            }
            else if (timeRemaining > 0f && timeRemaining < 0.01f)
            {
                timeRemaining -= Time.deltaTime;
                RaceTime.text = timeRemaining.ToString("F2") + "s";
            }
            else
            {
                timeRemaining -= Time.deltaTime;
                RaceTime.text = "";
                EndStunt();
            }
        }
        else
        {
            RaceTime.text = "";
        }
    }

}
