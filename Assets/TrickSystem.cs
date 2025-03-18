using System.Collections.Generic;
using UnityEngine;

public class TrickSystem : MonoBehaviour
{
    GameObject Drone;

    bool Flip, BarrelRoll, DrifTurn, Inverted, DiveBomb;
    bool TornadoFlip, DiveFlipRoll, DriftTurnDive;
    bool WallRide, GroundKiss, ThreadTheNeedle;
    private Rigidbody rb;
    [SerializeField] private int scoreAdded = 10;
    [SerializeField] private int Multiplyer = 0;
    [SerializeField] private LayerMask detectionLayerMask;
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

    void Start()
    {
        Drone = gameObject;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
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

        // if drone is performing any of these actions then add points to score
        if (Flip || BarrelRoll || DrifTurn || Inverted || DiveBomb || TornadoFlip || DiveFlipRoll || DriftTurnDive || WallRide || GroundKiss || ThreadTheNeedle)
        {
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
        }
    }

    private bool isFlipping;
    [Header("basic movemnet tricks")]
    [SerializeField] private float flipThreshold = 2.5f;
    [SerializeField] private float notFlipThreshold = 3f;
    [SerializeField] private float speedThreshold = 5f;

    // basic movemnet tricks
    bool DetectFlip() // front And Back Flip
    {
        Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity);
        if ((Mathf.Abs(localAngularVelocity.z) > flipThreshold) && rb.linearVelocity.magnitude > speedThreshold)
        {
            return true;
        }
        return false;
    }

    bool DetectBarrelRoll() // Left And Right Roll
    {
        Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity);
        if ((Mathf.Abs(localAngularVelocity.x) > flipThreshold) && rb.linearVelocity.magnitude > speedThreshold)
        {
            return true;
        }

        return false;
    }
    [SerializeField] private float driftTurnThreshold = 45f;

    bool DetectDrifTurn() // Maintain Sideways Angle While Turning
    {
        Vector3 localEulerAngles = transform.localEulerAngles;
        if ((Mathf.Abs(localEulerAngles.x) > driftTurnThreshold && Mathf.Abs(localEulerAngles.x) < (360 - driftTurnThreshold))
        && ((Mathf.Abs(localEulerAngles.z) > flipThreshold) && rb.linearVelocity.magnitude > speedThreshold))
        {
            return true;
        }
        return false;
    }
    [SerializeField] private float invertedThreshold = 0.9f;
    private bool DetectInverted()
    {
        return Vector3.Dot(transform.up, Vector3.down) > invertedThreshold;
    }
    [SerializeField] private float diveSpeedThreshold = 5f;
    bool DetectDiveBomb() // Dive at high speed (not fall)
    {
        if (rb.linearVelocity.y < -diveSpeedThreshold)
        {
            return true;
        }
        return false;
    }

    [SerializeField] private float checkRadius = 1f;
    [SerializeField] private float checkDistance = 3f;

    // Envoirmental Tricks
    private bool DetectWallRide()
    {
        return Physics.CheckSphere(transform.position + transform.forward * checkDistance, checkRadius, detectionLayerMask) && rb.linearVelocity.magnitude > speedThreshold;
    }
    [SerializeField] private float groundDistance = 1.5f;
    private bool DetectGroundKiss() // Fly to close to the ground
    {
        Debug.DrawRay(transform.position, Vector3.down * groundDistance, Color.red);
        return Physics.Raycast(transform.position, Vector3.down, groundDistance) && rb.linearVelocity.magnitude > speedThreshold;
    }
    [SerializeField] private float detectionDistance = 2f;
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
            return true;
        }
        return false;
    }
    // Combo Tricks
    bool DetectTornadoFlip() // front flip with roll
    {
        if (Flip && BarrelRoll && rb.linearVelocity.magnitude > speedThreshold)
        {
            return true;
        }
        return false;
    }

    bool DetectDiveFlipRoll() // While Diving you do a flip or roll
    {

        if ((Flip && DiveBomb) || (BarrelRoll && DiveBomb))
        {
            return true;
        }
        return false;
    }
    bool DetectDriftTurnDive() // While Drifting you Dive
    {
        if (DrifTurn && DiveBomb)
        {
            return true;
        }
        return false;
    }
}
