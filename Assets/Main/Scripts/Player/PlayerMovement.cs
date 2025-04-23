using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using Unity.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
public class PlayerMovement : NetworkBehaviour
{
    public bool isClient, isOwner = false;
    private Rigidbody rig;
    [Header("Player Settings")]
    public float rotationSpeed = 40;
    private float movementSpeed = 10f;
    public float maxMovementSpeed = 10f;
    public float speedMultiplier = 5f;
    public float Speed = 10f;
    public bool Offline = true;
    private AudioSource audioSource;
    public float Drag = 2f;
    public bool Fixed = true;

    [ReadOnly] public Vector2 rightStickInput;
    [ReadOnly] public Vector2 leftStickInput;
    bool unFreeze = false;
    public GameObject PlayerCamera;

    void Start()
    {
        // OR onwer
        rig = GetComponent<Rigidbody>();
        // Physics.gravity = new Vector3(0, -100f, 0);
        audioSource = GetComponent<AudioSource>();
        rig.isKinematic = false;
    }
    public void Freeze()
    {
        rig = GetComponent<Rigidbody>();
        rig.useGravity = false;
        rig.isKinematic = true;
    }
    public void UnFreeze()
    {
        rig = GetComponent<Rigidbody>();
        unFreeze = true;
        rig.isKinematic = false;
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner || Offline)
        {
            Debug.Log("Player spawned: " + gameObject.name);
        }
    }
    public void OnLeftStick(InputValue inputValue)
    {
        if (IsOwner || Offline)
        {
            leftStickInput = inputValue.Get<Vector2>();
        }
    }
    public void OnRightStick(InputValue inputValue)
    {
        if (IsOwner || Offline)
        {
            rightStickInput = inputValue.Get<Vector2>();
        }
    }
    void Update()
    {
        if (!Fixed)
        {
            isOwner = IsOwner;
            isClient = IsClient;
            // Or Oflfine 
            if (IsOwner || Offline) ControlDrone();
        }
    }
    void FixedUpdate()
    {
        if (Fixed)
        {
            isOwner = IsOwner;
            isClient = IsClient;
            // Or Oflfine 
            if (IsOwner || Offline) ControlDrone();
        }
    }
    void ControlDrone()
    {
        rotationSpeed = PlayerPrefs.GetFloat("Sensitivity");
        Speed = PlayerPrefs.GetFloat("Speed");
        Drag = PlayerPrefs.GetFloat("Drag");
        PlayerCamera.transform.localRotation = Quaternion.Euler(PlayerPrefs.GetFloat("CameraTilt"), -90, 0);

        if (NetworkManager != null) Offline = !NetworkManager.Singleton.IsServer;
        Vector3 rotationVelocity = new Vector3(rightStickInput.x * rotationSpeed * rightStickInput.magnitude, leftStickInput.x * rotationSpeed * leftStickInput.magnitude, rightStickInput.y * rotationSpeed * rightStickInput.magnitude);
        rig.angularVelocity = transform.TransformDirection(rotationVelocity);
        if (leftStickInput.y > 0)
        {
            if (unFreeze)
            {
                rig = GetComponent<Rigidbody>();
                rig.useGravity = true;
                unFreeze = false;
                rig.isKinematic = false;
            }
            movementSpeed = leftStickInput.magnitude * maxMovementSpeed * Speed;
            Vector3 newVelocity = new Vector3(0, leftStickInput.y * movementSpeed, leftStickInput.y * 0.1f);
            Vector3 worldVelocity = transform.TransformDirection(newVelocity);

            worldVelocity *= speedMultiplier;
            rig.AddForce(worldVelocity, ForceMode.VelocityChange);
        }
    }
}
