using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;
using NUnit.Framework;

public class RaceManager : NetworkBehaviour
{
    public TextMeshProUGUI RaceTime, Countdown;
    private float raceStartTime;
    private bool raceStarted;
    public GameObject EndScreen;
    public GameObject PlayerItem;
    public GameObject Content;
    public GameObject RacingUI;

    bool offline = true;


    void Start()
    {
        gameObject.GetComponent<PlayerCamera>().cameraActivated = true;
        RaceTime.enabled = false;
        Countdown.enabled = false;
        raceStarted = false;
        SceneManager.sceneLoaded += OnSceneLoaded;
        offline = !NetworkManager.Singleton.IsServer;
        if (offline)
        {
            RacingUI.SetActive(true);
            RaceTime.enabled = true;
            Countdown.enabled = true;
            StartCoroutine(StartCountdown());
        }
        else if (SceneManager.GetActiveScene().name == "ProceduralGeneration" && IsOwner)
        {
            RacingUI.SetActive(true);
            RaceTime.enabled = true;
            Countdown.enabled = true;
            StartCoroutine(StartCountdown());
        }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("Scene Loaded: " + arg0.name + " isServer: " + NetworkManager.Singleton.IsServer);
        if (arg0.name == "ProceduralGeneration" && IsOwner)
        {
            RaceTime.enabled = true;
            Countdown.enabled = true;
            StartCoroutine(StartCountdown());
        }
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
        raceStartTime = Time.time;
        raceStarted = true;
    }

    public void EndRace()
    {
        RaceTime.enabled = false;
        Countdown.enabled = false;
        raceStarted = false;
        EndScreen.SetActive(true);
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Drone");
        foreach (GameObject player in Players)
        {
            player.GetComponent<PlayerCamera>().cameraActivated = false;
            GameObject item = Instantiate(PlayerItem, Content.transform);
            item.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = player.name;
            item.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = Time.time - player.GetComponent<RaceManager>().raceStartTime + "s";
        }
    }
    void Update()
    {
        offline = !NetworkManager.Singleton.IsServer;
        if (raceStarted)
        {
            float raceTime = Time.time - raceStartTime;
            RaceTime.text = raceTime.ToString("F2") + "s";
        }
        else
        {
            RaceTime.text = "";
            Countdown.text = "";
        }
    }
}