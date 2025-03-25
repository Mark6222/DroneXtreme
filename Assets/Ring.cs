using System;
using NUnit.Framework;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private ParticleSystem checkpointParticles;
    private AudioSource audioSource;
    public bool isReached = false;
    public bool isNextRing = false;
    public GameObject startPanel;
    public GameObject endPanel;
    bool lastRing = false;
    private bool raceEnded = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drone") && !isNextRing && !isReached)
        {
            isReached = true;
            if (lastRing && !raceEnded)
            {
                raceEnded = true;
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Drone"))
                {
                    NetworkManager networkManager = GameObject.FindGameObjectWithTag("NetworkManager")?.GetComponent<NetworkManager>();
                    bool offline = networkManager == null || !networkManager.IsHost || !networkManager.IsConnectedClient;
                    if (g.GetComponent<RaceManager>().IsOwner || offline)
                    {
                        g.GetComponent<RaceManager>().EndRace();
                    }
                }
            }
            checkpointParticles.Play();
            audioSource.Play();
            Destroy(gameObject, 0.5f);
        }
    }

    public void SetNextRing()
    {
        isNextRing = true;
        gameObject.GetComponent<Renderer>().material.color = Color.grey;
    }

    public void SetCurrntRing()
    {
        isNextRing = false;
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }
    public void SetStartPanel()
    {
        startPanel.SetActive(true);
    }
    public void SetEndPanel()
    {
        lastRing = true;
        endPanel.SetActive(true);
    }
}
