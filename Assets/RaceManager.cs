using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.Netcode;

public class RaceManager : NetworkBehaviour
{
    public TextMeshProUGUI RaceTime, Countdown;
    private float raceStartTime;
    private bool raceStarted;
    public GameObject EndScreen;
    public GameObject PlayerItem;
    public GameObject Content;
    public TextMeshProUGUI Name, EndRaceTime;


    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        int countdownTime = 3;
        while (countdownTime > 0)
        {
            Countdown.text = countdownTime.ToString();
            yield return new WaitForSeconds(1);
            countdownTime--;
        }
        Countdown.text = "Go!";
        yield return new WaitForSeconds(1);
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
        raceStarted = false;
        EndScreen.SetActive(true);
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Drone");
        foreach (GameObject player in Players)
        {
            player.transform.Find("Virtual Camera").gameObject.SetActive(false);
            GameObject item = Instantiate(PlayerItem, Content.transform);
            item.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = player.name;
            item.transform.Find("Time").GetComponent<TextMeshProUGUI>().text = Time.time - player.GetComponent<RaceManager>().raceStartTime + "s";
        }
    }
    void Update()
    {
        if (raceStarted)
        {
            float raceTime = Time.time - raceStartTime;
            RaceTime.text = raceTime.ToString("F2") + "s";
        }
    }
}