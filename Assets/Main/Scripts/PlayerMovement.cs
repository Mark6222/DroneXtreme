using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using Unity.Collections;
public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody rig;
    [Header("Player Settings")]
    public float rotationSpeed = 40;
    private float movementSpeed = 10f;
    public float maxMovementSpeed = 10f;
    public float speedMultiplier = 5f;
    public float Speed = 10f;
    public bool Offline = true;
    private AudioSource audioSource;
    

    [ReadOnly] public Vector2 rightStickInput;
    [ReadOnly] public Vector2 leftStickInput;
    void Start()
    {
        GameObject net = GameObject.FindGameObjectWithTag("NetworkManager");
        if (net == null)
        {
            Offline = true;
        }
        else
        {
            Offline = false;
        }
        rig = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -100f, 0);
        audioSource = GetComponent<AudioSource>();
        if (!IsOwner && !Offline)
        {
            enabled = false;
        }
        rig.isKinematic = false;
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Debug.Log("Player spawned: " + gameObject.name);
        }
    }
    public void OnLeftStick(InputValue inputValue)
    {
        leftStickInput = inputValue.Get<Vector2>();
    }
    public void OnRightStick(InputValue inputValue)
    {
        rightStickInput = inputValue.Get<Vector2>();
    }
    void Update()
    {
        // if (!IsOwner || Offline) return;
        audioSource.volume = leftStickInput.y + 0.1f;
        Vector3 rotationVelocity = new Vector3(rightStickInput.x * rotationSpeed * rightStickInput.magnitude, leftStickInput.x * rotationSpeed * leftStickInput.magnitude, rightStickInput.y * rotationSpeed * rightStickInput.magnitude);
        rig.angularVelocity = transform.TransformDirection(rotationVelocity);
        if (leftStickInput.y > 0)
        {
            movementSpeed = leftStickInput.magnitude * maxMovementSpeed * Speed;
            Vector3 newVelocity = new Vector3(0, leftStickInput.y * movementSpeed, leftStickInput.y * 0.1f);
            Vector3 worldVelocity = transform.TransformDirection(newVelocity);
            
            worldVelocity *= speedMultiplier;
            rig.AddForce(worldVelocity, ForceMode.VelocityChange);
        }
    }
}
