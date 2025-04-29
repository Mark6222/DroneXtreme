using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManeger : NetworkBehaviour
{
    public List<float> RaceTimes = new List<float>();
    public List<float> StuntScores = new List<float>();
    public bool OnlineDrone = true;
    [SerializeField] private float rotationYOffset = 90f;
    [SerializeField] private float rotationZOffset = 50f;
    [SerializeField] private GameObject ScoringUI;
    [SerializeField] private GameObject RacingUI;
    private bool isDeactivated = false;
    public GameObject playerCamera;
    public GameObject EndScreen;
    public GameObject EndScreenContent;
    public GameObject RestartButton;
    public GameObject MainMenuButton;
    public GameObject LeaveButton;
    public GameObject Target;

    [SerializeField] bool isMultiplayer = false;
    bool stuntMode = false;
    public GameObject SplashScreen;
    public Animator SettingScreenAnimator;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}, Mode: {mode}");
        if (NetworkManager != null) isMultiplayer = NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient;
        if (isMultiplayer && !IsHost)
        {
            ResetEndScreenAndPlayer();
        }
        if (NetworkManager != null)
        {
            if ((NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient) && !OnlineDrone)
            {
                gameObject.SetActive(false);
                return;
            }
        }
        HandleSceneSpecificSetup();
    }

    void Start()
    {
        SplashScreen.SetActive(false);
        if (NetworkManager != null) isMultiplayer = NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient;
        if (IsServer && !OnlineDrone)
        {
            gameObject.SetActive(false);
            return;
        }
        HandleSceneSpecificSetup();

    }

    void OnEnable()
    {
        if (isDeactivated && SceneManager.GetActiveScene().name != "SplashScreen")
        {
            ActivatePlayer();
        }
    }

    private void HandleSceneSpecificSetup()
    {
        gameObject.GetComponent<RaceManager>().ResetEndScreen();
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Setting up player for scene: " + sceneName);
        ResetState();

        if (sceneName == "SplashScreen")
        {
            DeactivatePlayer();
        }
        else if (sceneName == "ProceduralGeneration")
        {
            ActivatePlayer(true);
            RaceMode();
        }
        else if (sceneName == "TargetTracking")
        {
            ActivatePlayer(true);
            TargetTrackingMode();
        }
        else if (sceneName == "StuntMode")
        {
            ActivatePlayer(true);
            StuntMode();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((isMultiplayer && IsOwner) || !isMultiplayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "SplashScreen")
            {
                if (SplashScreen.activeSelf)
                {
                    SplashScreen.SetActive(false);
                    gameObject.GetComponent<PlayerMovement>().UnFreeze();
                }
                else
                {
                    SplashScreen.SetActive(true);
                    gameObject.GetComponent<PlayerMovement>().Freeze();
                    SettingScreenAnimator.SetTrigger("Hide");
                }
            }
        }
    }

    public void DisplayOptions()
    {
        if ((isMultiplayer && IsOwner) || !isMultiplayer)
        {
            if (SplashScreen.activeSelf)
            {
                SplashScreen.SetActive(false);
                gameObject.GetComponent<PlayerMovement>().UnFreeze();
            }
            else
            {
                SplashScreen.SetActive(true);
                gameObject.GetComponent<PlayerMovement>().Freeze();
                SettingScreenAnimator.SetTrigger("Hide");
            }
        }
    }

    public void DeactivatePlayer()
    {
        isDeactivated = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != null && child.gameObject != null)
            {
                child.gameObject.SetActive(false);
            }
        }

        Component[] components = gameObject.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component is Behaviour && !(component is PlayerManeger))
            {
                ((Behaviour)component).enabled = false;
            }
        }
    }

    public void ActivatePlayer(bool force = false)
    {
        isDeactivated = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != null && child.gameObject != null)
            {
                if (force || !child.gameObject.activeSelf)
                {
                    Debug.Log("IsOwner: " + IsOwner + ", Child: " + child.gameObject.name);
                    child.gameObject.SetActive(true);
                    if (isMultiplayer && !IsOwner && (child.gameObject.name == "Canvas" || child.gameObject.name == "PlayerCamera"))
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
        }
        Component[] components = gameObject.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component is Behaviour && !(component is PlayerManeger))
            {
                ((Behaviour)component).enabled = true;
            }
        }
    }

    private void ResetState()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != null && child.gameObject != null)
            {
                child.gameObject.SetActive(true);
            }
        }
        Component[] components = gameObject.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component is Behaviour && !(component is PlayerManeger))
            {
                ((Behaviour)component).enabled = true;
            }
        }
        isDeactivated = false;
    }

    void RaceMode()
    {
        stuntMode = false;
        GameObject trollyCam = GameObject.FindGameObjectWithTag("TrollyCam");
        if (trollyCam != null)
        {
            trollyCam.SetActive(false);
        }
        ActivatePlayer(true);
        if (gameObject.GetComponent<TrickSystem>() != null)
            gameObject.GetComponent<TrickSystem>().enabled = false;

        if (gameObject.GetComponent<RaceManager>() != null)
            gameObject.GetComponent<RaceManager>().enabled = true;

        if (gameObject.GetComponent<StuntManager>() != null)
            gameObject.GetComponent<StuntManager>().enabled = false;

        if (gameObject.GetComponent<TargetTracking>() != null)
            gameObject.GetComponent<TargetTracking>().enabled = false;

        if (ScoringUI != null)
            ScoringUI.SetActive(false);

        if (RacingUI != null)
            RacingUI.SetActive(true);

        StartCoroutine(WaitAndSpawnPlayer());
        EndScreenHost();
    }
    void TargetTrackingMode()
    {
        stuntMode = false;
        GameObject trollyCam = GameObject.FindGameObjectWithTag("TrollyCam");
        if (trollyCam != null)
        {
            trollyCam.SetActive(false);
        }
        ActivatePlayer(true);
        if (gameObject.GetComponent<TrickSystem>() != null)
            gameObject.GetComponent<TrickSystem>().enabled = false;

        if (gameObject.GetComponent<RaceManager>() != null)
            gameObject.GetComponent<RaceManager>().enabled = false;

        if (gameObject.GetComponent<StuntManager>() != null)
            gameObject.GetComponent<StuntManager>().enabled = false;

        if (gameObject.GetComponent<TargetTracking>() != null)
            gameObject.GetComponent<TargetTracking>().enabled = true;

        if (ScoringUI != null)
            ScoringUI.SetActive(false);

        if (RacingUI != null)
            RacingUI.SetActive(true);

        StartCoroutine(SpawnPlayerInTargetTracking());
        EndScreenHost();
    }
    private IEnumerator SpawnPlayerInTargetTracking()
    {
        var pointsManager = GameObject.Find("ProceduralGeneration").GetComponent<PointsManager>();

        yield return new WaitUntil(() => pointsManager.spawnPlayer);

        GameObject SpawnPoint = GameObject.Find("SpawnPoint");
        if (SpawnPoint != null)
        {
            transform.position = SpawnPoint.transform.position;
            transform.rotation = SpawnPoint.transform.rotation;
        }
        gameObject.GetComponent<TargetTracking>().ShowUI();
        Target = GameObject.FindGameObjectWithTag("Target");
    }
    void StuntMode()
    {
        stuntMode = true;
        ActivatePlayer(true);

        if (gameObject.GetComponent<RaceManager>() != null)
            gameObject.GetComponent<RaceManager>().enabled = false;

        if (gameObject.GetComponent<StuntManager>() != null)
            gameObject.GetComponent<StuntManager>().enabled = true;

        if (gameObject.GetComponent<TrickSystem>() != null)
            gameObject.GetComponent<TrickSystem>().enabled = true;

        if (gameObject.GetComponent<TargetTracking>() != null)
            gameObject.GetComponent<TargetTracking>().enabled = false;

        if (ScoringUI != null)
            ScoringUI.SetActive(true);

        if (RacingUI != null)
            RacingUI.SetActive(false);
        GameObject SpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (SpawnPoint != null)
        {
            transform.position = SpawnPoint.transform.position + Vector3.up;
        }
        gameObject.GetComponent<StuntManager>().ShowUI();
        EndScreenHost();
    }

    private IEnumerator WaitAndSpawnPlayer()
    {
        var pointsManager = GameObject.Find("ProceduralGeneration").GetComponent<PointsManager>();

        yield return new WaitUntil(() => pointsManager.spawnPlayer);

        var points = pointsManager.points;
        var ringSpawns = GameObject.FindGameObjectsWithTag("RingSpawnPoint");

        transform.position = ringSpawns[0].transform.position + Vector3.up;
        transform.LookAt(points[1].transform);

        var rotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y + rotationYOffset, rotation.z + rotationZOffset);

        gameObject.GetComponent<RaceManager>().ShowUI();
    }
    public void ResetEndScreenAndPlayer()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        // gameObject.GetComponent<TrickSystem>().playerScore.ResetScore();

        if (stuntMode)
        {
            gameObject.GetComponent<StuntManager>().ResetEndScreen();
        }
        else
        {
            gameObject.GetComponent<RaceManager>().ResetEndScreen();
        }
        RestartRace();
    }

    public void CompleteReset()
    {
        Debug.Log("Performing complete drone reset");

        RaceTimes.Clear();

        if (SceneManager.GetActiveScene().name == "ProceduralGeneration")
        {
            var pointsManager = GameObject.Find("ProceduralGeneration")?.GetComponent<PointsManager>();
            var ringSpawns = GameObject.FindGameObjectsWithTag("RingSpawnPoint");

            if (pointsManager != null && ringSpawns.Length > 0)
            {
                transform.position = ringSpawns[1].transform.position + Vector3.up;

                var points = pointsManager.points;
                if (points.Length > 1)
                {
                    transform.LookAt(points[1].transform);
                    var rotation = transform.rotation.eulerAngles;
                    transform.rotation = Quaternion.Euler(rotation.x, rotation.y + rotationYOffset, rotation.z + rotationZOffset);
                }
            }
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.leftStickInput = Vector2.zero;
            movement.rightStickInput = Vector2.zero;
        }

        RaceManager raceManager = GetComponent<RaceManager>();
        if (raceManager != null)
        {
            raceManager.PlayersList.Clear();
            if (raceManager.Countdown != null) raceManager.Countdown.text = "";
            if (raceManager.RaceTime != null) raceManager.RaceTime.text = "";
            if (raceManager.EndScreen != null) raceManager.EndScreen.SetActive(false);
        }

        if (isDeactivated)
        {
            ActivatePlayer(true);
        }

        Debug.Log("Drone reset complete");
    }

    public void RestartRace()
    {
        // CompleteReset();

        if (SceneManager.GetActiveScene().name == "ProceduralGeneration")
        {
            RaceMode();
        }
        else if (SceneManager.GetActiveScene().name == "StuntMode")
        {
            StuntMode();
        }
    }

    public void EndScreenHost()
    {
        if (isMultiplayer && !IsHost)
        {
            RestartButton.SetActive(false);
            MainMenuButton.SetActive(false);
            LeaveButton.SetActive(true);
        }
        else if (isMultiplayer && IsHost)
        {
            RestartButton.SetActive(true);
            MainMenuButton.SetActive(true);
            LeaveButton.SetActive(false);
        }
        else
        {
            RestartButton.SetActive(true);
            MainMenuButton.SetActive(true);
            LeaveButton.SetActive(false);
        }
    }
    public void LeaveGame()
    {
        if (isMultiplayer)
        {
            NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId);
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("SplashScreen");
        }
        else
        {
            SceneManager.LoadScene("SplashScreen");
        }
    }
    public void ResumeGame()
    {
        SplashScreen.SetActive(false);
        gameObject.GetComponent<PlayerMovement>().UnFreeze();
    }
}
