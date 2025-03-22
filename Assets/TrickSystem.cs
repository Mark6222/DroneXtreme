using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using NUnit.Framework;

public class TrickSystem : NetworkBehaviour
{
    GameObject Drone;
    public TextMeshProUGUI mutiplyerText, addedScoreText, playerScoreText, actionsTextOne, actionsTextTwo, actionsTextThree;
    public GameObject actionTexts, scoringUI, scores;
    bool Flip, BarrelRoll, DrifTurn, Inverted, DiveBomb;
    [SerializeField] bool TornadoFlip, DiveFlipRoll, DriftTurnDive;
    bool WallRide, GroundKiss, ThreadTheNeedle;
    private Rigidbody rb;
    private PlayerScore playerScore;
    [SerializeField] private int scoreAdded = 10;
    [SerializeField] private int Multiplyer = 1;
    [SerializeField] private LayerMask detectionLayerMask;
    [SerializeField] private LayerMask detectionTerrainLayerMask;

    [Header("Test Detection Toggles")]
    [SerializeField] private bool testFlip = false;
    [SerializeField] private bool testBarrelRoll = false;
    [SerializeField] private bool testDrifTurn = false;
    [SerializeField] private bool testInverted = false;
    [SerializeField] private bool testDiveBomb = false;
    [SerializeField] private bool testTornadoFlip = false;
    [SerializeField] private bool testDiveFlipRoll = false;
    [SerializeField] private bool testDriftTurnDive = false;
    [SerializeField] private bool testWallRide = false;
    [SerializeField] private bool testGroundKiss = false;
    [SerializeField] private bool testThreadTheNeedle = false;
    private Dictionary<string, Animator> UiAnimators = new();
    public bool Offline = true;
    public GameObject Canvas;
    void Start()
    {
        Canvas.SetActive(false);
        playerScore = new PlayerScore();
        Offline = !IsClient && !IsOwner;

        if (!Offline) SceneManager.sceneLoaded += OnSceneLoaded;
        else init();
        if(Offline || IsOwner) init();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        Offline = !IsClient && !IsOwner;
        if (scene.name == "StuntMode")
        {
            enabled = true;
            if (Offline) init();
            else if (IsOwner) init();
        }
    }
    void init()
    {
        if (SceneManager.GetActiveScene().name == "StuntMode")
        {
            if(Offline) Canvas.SetActive(true);
            else if(IsOwner) Canvas.SetActive(true);
            else Canvas.SetActive(false);
        }
        Debug.Log("Trick System Initialized");
        Drone = gameObject;
        rb = GetComponent<Rigidbody>();

        // mutiplyerText = GameObject.Find("MultiplyerText").GetComponent<TextMeshProUGUI>();
        // addedScoreText = GameObject.Find("ScoreToAddText").GetComponent<TextMeshProUGUI>();
        // actionsTextOne = GameObject.Find("ActionsTextOne").GetComponent<TextMeshProUGUI>();
        // actionsTextTwo = GameObject.Find("ActionsTextTwo").GetComponent<TextMeshProUGUI>();
        // actionsTextThree = GameObject.Find("ActionsTextThree").GetComponent<TextMeshProUGUI>();
        // scores = GameObject.Find("Scores");
        // scoringUI = GameObject.Find("ScoringUI");
        // actionTexts = GameObject.Find("ActionTexts");

        UiAnimators.Add("MultiplyerText", mutiplyerText.GetComponent<Animator>());
        UiAnimators.Add("ScoreToAddText", addedScoreText.GetComponent<Animator>());
        UiAnimators.Add("PlayerScoreText", playerScoreText.GetComponent<Animator>());
        UiAnimators.Add("ActionTexts", actionTexts.GetComponent<Animator>());
        UiAnimators.Add("Scores", scores.GetComponent<Animator>());
        UiAnimators.Add("ScoringUI", scoringUI.GetComponent<Animator>());
        actions.Add("  ");
    }
    float addScoreTimer = 0.5f;
    void Update()
    {
        if (Offline || IsOwner)
        {
            Run();
        }
    }
    void Run()
    {
        // Detect if the drone is performing any of these actions and set the booleans accordingly
        if (testFlip) Flip = DetectFlip();
        if (testBarrelRoll) BarrelRoll = DetectBarrelRoll();
        if (testDrifTurn) DrifTurn = DetectDrifTurn();
        if (testInverted) Inverted = DetectInverted();
        if (testDiveBomb) DiveBomb = DetectDiveBomb();
        if (testTornadoFlip) TornadoFlip = DetectTornadoFlip();
        if (testWallRide) WallRide = DetectWallRide();
        if (testGroundKiss) GroundKiss = DetectGroundKiss();
        if (testThreadTheNeedle) ThreadTheNeedle = DetectThreadTheNeedle();
        if (testDiveFlipRoll) DiveFlipRoll = DetectDiveFlipRoll();
        if (testDriftTurnDive) DriftTurnDive = DetectDriftTurnDive();

        MultiplyerScore();
        // if drone is performing any of these actions then add points to score
        if (Flip) Debug.Log("Action: " + "Flip");
        if (BarrelRoll) Debug.Log("Action: " + "BarrelRoll");
        if (DrifTurn) Debug.Log("Action: " + "DrifTurn");
        if (Inverted) Debug.Log("Action: " + "Inverted");
        if (DiveBomb) Debug.Log("Action: " + "DiveBomb");

        if (TornadoFlip) Debug.Log("Action: " + "TornadoFlip");
        if (DiveFlipRoll) Debug.Log("Action: " + "DiveFlipRoll");
        if (DriftTurnDive) Debug.Log("Action: " + "DriftTurnDive");

        if (WallRide) Debug.Log("Action: " + "WallRide");
        if (GroundKiss) Debug.Log("Action: " + "GroundKiss");
        if (ThreadTheNeedle) Debug.Log("Action: " + "ThreadTheNeedle");

        AddScore("Flip", Flip, scoreAdded, 0);
        AddScore("BarrelRoll", BarrelRoll, scoreAdded, 0);
        AddScore("DriftTurn", DrifTurn, scoreAdded, 0);
        AddScore("Inverted", Inverted, 2, 0);
        AddScore("DiveBomb", DiveBomb, 1, 0);

        AddScore("WallRide", WallRide, scoreAdded, 15);
        AddScore("GroundKiss", GroundKiss, scoreAdded, 20);
        AddScore("ThreadTheNeedle", ThreadTheNeedle, scoreAdded, 30);

        if (Flip || BarrelRoll || DrifTurn || Inverted || DiveBomb || TornadoFlip || DiveFlipRoll || DriftTurnDive || WallRide || GroundKiss || ThreadTheNeedle)
        {
            addScoreTimer = 0.5f;
        }
        addScoreTimer -= Time.deltaTime;
        if (addScoreTimer <= 0f && !Flip && !BarrelRoll && !DrifTurn && !Inverted && !DiveBomb && !TornadoFlip && !DiveFlipRoll && !DriftTurnDive && !WallRide && !GroundKiss && !ThreadTheNeedle)
        {
            playerScore.AddPoints();
            if (playerScore.GetScoreToAdd() > 0) UiAnimators["Scores"].SetTrigger("scoreBash");
            playerScore.ResetScoreToAdd();
            UpdatePlayerUI();
        }
        UpdateActionsText();
    }
    void UpdatePlayerUI()
    {
        playerScoreText.text = "" + playerScore.GetScore();
        addedScoreText.text = "" + playerScore.GetScoreToAdd();
        mutiplyerText.text = "X" + Multiplyer;
    }
    List<string> actions = new();
    void UpdateActionsText()
    {
        if (actions.Count == 0)
        {
            actions.Add("  ");
        }
        if (Flip) actions.Add("Flip");
        if (BarrelRoll) actions.Add("BarrelRoll");
        if (DrifTurn) actions.Add("DrifTurn");
        if (Inverted) actions.Add("Inverted");
        if (DiveBomb) actions.Add("DiveBomb");
        if (TornadoFlip) actions.Add("TornadoFlip");
        if (DiveFlipRoll) actions.Add("DiveFlipRoll");
        if (DriftTurnDive) actions.Add("DriftTurnDive");
        if (WallRide) actions.Add("WallRide");
        if (GroundKiss) actions.Add("GroundKiss");
        if (ThreadTheNeedle) actions.Add("ThreadTheNeedle");

        ShowActionsText();
    }
    float actionsCooldown = 0f;
    void ShowActionsText()
    {
        actionsCooldown += Time.deltaTime;
        if (actionsCooldown > 1f)
        {
            if (actions.Count > 0) actions.RemoveAt(0);
            actionsCooldown = 0f;
            UiAnimators["ActionTexts"].SetTrigger("ActionsTriggered");
        }
        actionsTextOne.text = actions.Count > 1 ? actions[1] : "";
        actionsTextTwo.text = actions.Count > 2 ? actions[2] : "";
        actionsTextThree.text = actions.Count > 3 ? actions[3] : "";
    }
    private Dictionary<string, float> actionTimers = new();
    private Dictionary<string, float> coolDownTimers = new();

    void AddScore(string actionName, bool actionActive, int score, int bonus)
    {
        if (!actionActive)
        {
            if (coolDownTimers.TryGetValue(actionName, out float cooldown) && cooldown > 0)
            {
                coolDownTimers[actionName] -= Time.deltaTime;
                return;
            }
        }

        if (!actionActive && coolDownTimers.ContainsKey(actionName) && coolDownTimers[actionName] <= 0)
        {
            actionTimers.Remove(actionName);
            coolDownTimers.Remove(actionName);
            return;
        }
        if (!actionTimers.ContainsKey(actionName))
        {
            actionTimers[actionName] = 0f;
            coolDownTimers[actionName] = 2f;
        }
        actionTimers[actionName] += Time.deltaTime;
        if (actionActive)
        {
            int scoreToAdd = (score + bonus) * Multiplyer;
            if (actionName != "ThreadTheNeedle") playerScore.AddToScoreToAdd(scoreToAdd);
            else { playerScore.AddToScoreToAdd(ThreadTheNeedleScoreToAdd); ThreadTheNeedleScoreToAdd = 0; }
            UiAnimators["ScoreToAddText"].SetTrigger("Trigger");
            UpdatePlayerUI();
        }
    }
    private float TornadoFlipTimer, DiveFlipRollTimer, DriftTurnDiveTimer = 0f;
    private bool TornadoFlipStarted, DiveFlipRollStarted, DriftTurnDiveStarted = false;
    void MultiplyerScore()
    {

        Multiplyer = 1;
        if (TornadoFlip && !TornadoFlipStarted)
        {
            Multiplyer = Multiplyer + 1;
            TornadoFlipStarted = true;
            UiAnimators["MultiplyerText"].SetTrigger("MultTrigger");
        }
        else if (TornadoFlipStarted)
        {
            Multiplyer = Multiplyer + 1;
            TornadoFlipTimer += Time.deltaTime;
            if (TornadoFlipTimer >= 5f)
            {
                TornadoFlipStarted = false;
                TornadoFlipTimer = 0f;
            }
        }

        if (DiveFlipRoll && !DiveFlipRollStarted)
        {
            Multiplyer = Multiplyer + 1;
            UiAnimators["MultiplyerText"].SetTrigger("MultTrigger");
            DiveFlipRollStarted = true;
        }
        else if (DiveFlipRollStarted)
        {
            Multiplyer = Multiplyer + 1;
            DiveFlipRollTimer += Time.deltaTime;
            if (DiveFlipRollTimer >= 5f)
            {
                DiveFlipRollStarted = false;
                DiveFlipRollTimer = 0f;
            }
        }

        if (DriftTurnDive && !DriftTurnDiveStarted)
        {
            Multiplyer = Multiplyer + 1;
            UiAnimators["MultiplyerText"].SetTrigger("MultTrigger");
            DriftTurnDiveStarted = true;
        }
        else if (DriftTurnDiveStarted)
        {
            Multiplyer = Multiplyer + 1;
            DriftTurnDiveTimer += Time.deltaTime;
            if (DriftTurnDiveTimer >= 5f)
            {
                DriftTurnDiveStarted = false;
                DriftTurnDiveTimer = 0f;
            }
        }
    }


    [Header("basic movemnet tricks")]
    [SerializeField] private float flipThreshold = 2.5f;
    [SerializeField] private float notFlipThreshold = 3f;
    [SerializeField] private float speedThreshold = 5f;
    [SerializeField] private float rotationThreshold = 250f;

    // basic movemnet tricks
    private float flipRotation = 0f;
    bool DetectFlip() // front And Back Flip
    {
        Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity);
        flipRotation += Mathf.Abs(localAngularVelocity.z) * Time.deltaTime * Mathf.Rad2Deg;
        if (flipRotation >= rotationThreshold && rb.linearVelocity.magnitude > speedThreshold)
        {
            flipRotation = 0f;
            return true;
        }
        return false;
    }

    private float barrelRollRotation = 0f;
    bool DetectBarrelRoll() // Left And Right Roll
    {
        Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity);
        barrelRollRotation += Mathf.Abs(localAngularVelocity.x) * Time.deltaTime * Mathf.Rad2Deg;
        if (barrelRollRotation >= rotationThreshold && rb.linearVelocity.magnitude > speedThreshold)
        {
            barrelRollRotation = 0f;
            return true;
        }
        return false;
    }

    private float driftTurnRotation = 0f;
    [SerializeField] private float driftTurnThreshold = 45f;
    bool DetectDrifTurn() // Maintain Sideways Angle While Turning
    {
        Vector3 localEulerAngles = transform.localEulerAngles;
        driftTurnRotation += Mathf.Abs(localEulerAngles.z) * Time.deltaTime;
        if (driftTurnRotation >= rotationThreshold && rb.linearVelocity.magnitude > speedThreshold)
        {
            driftTurnRotation = 0f;
            return true;
        }
        return false;
    }
    [SerializeField] private float invertedThreshold = 0.9f;
    float invertedTimer = 0f;
    bool StartedInvertedTimer = false;
    private bool DetectInverted()
    {
        if (Vector3.Dot(transform.up, Vector3.down) > invertedThreshold)
        {
            if (!StartedInvertedTimer)
            {
                StartedInvertedTimer = true;
            }
            invertedTimer += Time.deltaTime;
            if (invertedTimer >= 1f)
            {
                StartedInvertedTimer = false;
                invertedTimer = 0f;
                return true;
            }
        }
        else
        {
            invertedTimer = 0f;
            StartedInvertedTimer = false;
        }
        return false;
    }
    [SerializeField] private float diveSpeedThreshold = 5f;
    float diveBombTimer = 0f;
    bool StartedDiveBombTimer = false;
    bool DetectDiveBomb() // Dive at high speed (not fall)
    {
        if (rb.linearVelocity.y < -diveSpeedThreshold)
        {
            if (!StartedDiveBombTimer)
            {
                StartedDiveBombTimer = true;
            }
            diveBombTimer += Time.deltaTime;
            if (diveBombTimer >= 1f)
            {
                StartedDiveBombTimer = false;
                diveBombTimer = 0f;
                return true;
            }
        }
        else
        {
            diveBombTimer = 0f;
            StartedDiveBombTimer = false;
        }
        return false;
    }

    [SerializeField] private float checkRadius = 1f;
    [SerializeField] private float checkDistance = 3f;

    // Envoirmental Tricks
    float wallRideTimer = 0f;
    bool StartedWallRideTimer = false;
    private bool DetectWallRide()
    {
        if (Physics.CheckSphere(transform.position + transform.forward * checkDistance, checkRadius, detectionLayerMask) && rb.linearVelocity.magnitude > speedThreshold)
        {
            if (!StartedWallRideTimer)
            {
                StartedWallRideTimer = true;
            }
            wallRideTimer += Time.deltaTime;
            if (wallRideTimer >= 2f)
            {
                StartedWallRideTimer = false;
                wallRideTimer = 0f;
                return true;
            }
        }
        else
        {
            wallRideTimer = 0f;
            StartedWallRideTimer = false;
        }
        return false;
    }
    [SerializeField] private float groundDistance = 2f;
    float groundKissTimer = 0f;
    bool StartedGroundKissTimer = false;
    private bool DetectGroundKiss() // Fly to close to the ground
    {
        Debug.DrawRay(transform.position, Vector3.down * groundDistance, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, groundDistance, detectionTerrainLayerMask) && rb.linearVelocity.magnitude > speedThreshold)
        {
            if (!StartedGroundKissTimer)
            {
                StartedGroundKissTimer = true;
            }
            groundKissTimer += Time.deltaTime;
            if (groundKissTimer >= 0.5f)
            {
                StartedGroundKissTimer = false;
                groundKissTimer = 0f;
                return true;
            }
        }
        else
        {
            groundKissTimer = 0f;
            StartedGroundKissTimer = false;
        }
        return false;
    }
    [SerializeField] private float detectionDistance = 2f;
    bool StartedThreadTheNeedle = false;
    int ThreadTheNeedleScoreToAdd = 0;
    private bool DetectThreadTheNeedle() // Fly through tight space
    {
        Vector3[] directions = { transform.right, -transform.right, transform.up, -transform.up, transform.forward, -transform.forward };
        int tight = 0;
        foreach (Vector3 dir in directions)
        {
            Debug.DrawRay(transform.position, dir * detectionDistance, Color.red);
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, detectionDistance, detectionLayerMask))
            {
                tight++;
            }
        }
        if (tight == 3 && rb.linearVelocity.magnitude > speedThreshold)
        {
            ThreadTheNeedleScoreToAdd += 1;
            StartedThreadTheNeedle = true;
        }
        else
        {
            if (StartedThreadTheNeedle)
            {
                StartedThreadTheNeedle = false;
                return true;
            }
        }
        return false;
    }
    // Combo Tricks
    bool DetectTornadoFlip() // front flip with roll
    {
        if (actions.Count > 1)
        {
            if ((actions[actions.Count - 1] == "Flip" && actions[actions.Count - 2] == "BarrelRoll") ||
            (actions[actions.Count - 2] == "Flip" && actions[actions.Count - 1] == "BarrelRoll"))
            {
                return true;
            }
            return false;
        }
        return false;
    }

    bool DetectDiveFlipRoll() // While Diving you do a flip or roll
    {
        if (actions.Count > 1)
        {
            if (((actions[actions.Count - 1] == "Flip" && actions[actions.Count - 2] == "DiveBomb") || (actions[actions.Count - 2] == "Flip" && actions[actions.Count - 1] == "DiveBomb")) ||
            ((actions[actions.Count - 1] == "DiveBomb" && actions[actions.Count - 2] == "BarrelRoll") || (actions[actions.Count - 2] == "DiveBomb" && actions[actions.Count - 1] == "BarrelRoll")))
            {
                return true;
            }
        }
        return false;

    }
    bool DetectDriftTurnDive() // While Drifting you Dive
    {
        if (actions.Count > 1)
        {
            if ((actions[actions.Count - 1] == "DriftTurn" && actions[actions.Count - 2] == "DiveBomb") ||
            (actions[actions.Count - 2] == "DriftTurn" && actions[actions.Count - 1] == "DiveBomb"))
            {
                return true;
            }
        }
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        playerScore.ResetScoreToAdd();
        actions.Clear();
        actions.Add("!COLLIDED!");
        UiAnimators["ScoringUI"].SetTrigger("collided");
    }
}
