using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;
using NUnit.Framework;

public class RaceManager : MonoBehaviour
{
    public TextMeshProUGUI RaceTime, Countdown;
    private float raceStartTime;
    private bool raceStarted;
    public GameObject EndScreen;
    public GameObject PlayerItem;
    public GameObject Content;
    public Dictionary<GameObject, GameObject> EndSceenPlayers = new Dictionary<GameObject, GameObject>();
    GameObject[] Players;
    void Start()
    {
        EndScreen.SetActive(false);
        RaceTime.text = "";
        Countdown.text = "";
        raceStarted = false;
    }
    public void ShowUI()
    {
        StartCoroutine(StartCountdown());
    }
    IEnumerator StartCountdown()
    {
        raceStarted = true;
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
        yield return new WaitForSeconds(1);
        if (this == null) yield break;
        Countdown.text = "";
        StartRace();
    }

    void StartRace()
    {
        Players = GameObject.FindGameObjectsWithTag("Drone");

        EndSceenPlayers.Clear();
        raceStartTime = 0f;
        raceStartTime = Time.time;
        raceStarted = true;
    }

    public void EndRace()
    {
        gameObject.GetComponent<PlayerManeger>().playerCamera.SetActive(false);
        GameObject trollyCam = GameObject.FindGameObjectWithTag("TrollyCam");
        if (trollyCam != null)
        {
            trollyCam.SetActive(false);
        }
        RaceTime.enabled = false;
        Countdown.enabled = false;
        raceStarted = false;
        EndScreen.SetActive(true);
        gameObject.GetComponent<PlayerManeger>().RaceTimes.Add(Time.time - raceStartTime);
        Players = GameObject.FindGameObjectsWithTag("Drone");
        foreach (GameObject player in Players)
        {
            GameObject item = Instantiate(PlayerItem, Content.transform);
            EndSceenPlayers.Add(item, player);
        }
    }

    public void ResetEndScreen()
    {
        foreach (KeyValuePair<GameObject, GameObject> kvp in EndSceenPlayers)
        {
            Destroy(kvp.Key);
        }
        EndScreen.SetActive(false);
    }
    void Update()
    {
        if (raceStarted)
        {
            float raceTime = Time.time - raceStartTime;
            RaceTime.text = raceTime.ToString("F2") + "s";
        }
        else
        {
            RaceTime.text = "";
            Countdown.text = "";
            int index = 0;
            foreach (KeyValuePair<GameObject, GameObject> kvp in EndSceenPlayers)
            {
                GameObject player = kvp.Value;
                GameObject item = kvp.Key;
                if (player != null && item != null)
                {
                    TextMeshProUGUI textComponent = item.GetComponent<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        RaceManager playerRaceManager = player.GetComponent<RaceManager>();
                        if (playerRaceManager != null)
                        {
                            textComponent.text = (Time.time - playerRaceManager.raceStartTime).ToString("F2") + "s";
                        }
                        else
                        {
                            textComponent.text = player.name;
                        }
                    }
                    index++;
                }
            }
        }
    }
}