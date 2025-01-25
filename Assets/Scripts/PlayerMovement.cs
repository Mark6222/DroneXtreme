using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
public class PlayerMovement : NetworkBehaviour
{
    private Rigidbody rig;
    public float rotationSpeed = 40;
    public float movementSpeed = 10f;
    public float maxMovementSpeed = 10f;
    public float speedMultiplier = 5f;
    public float Speed = 10f;
    float leftHorizontal = 0;
    public float leftVertical = 0;
    float leftMagnitude = 0;
    float rightHorizontal = 0;
    float rightVertical = 0;
    float rightMagnitude = 0;
    public bool Test = true;
    private AudioSource audioSource;
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -100f, 0);
        audioSource = GetComponent<AudioSource>();
    }
    public void OnLeftStick(InputValue inputValue)
    {
        Vector2 leftStickInput = inputValue.Get<Vector2>();
        leftHorizontal = leftStickInput.x;
        leftVertical = leftStickInput.y;
        leftMagnitude = leftStickInput.magnitude;
        audioSource.volume = leftVertical  + 0.1f;

    }
    public void OnRightStick(InputValue inputValue)
    {
        Vector2 rightStickInput = inputValue.Get<Vector2>();
        rightHorizontal = rightStickInput.x;
        rightVertical = rightStickInput.y;
        rightMagnitude = rightStickInput.magnitude;
        movementSpeed = rightMagnitude * maxMovementSpeed;
    }
    void Update()
    {
        if(!IsOwner && !Test) return;
        float speed = leftVertical;
        Vector3 rotationVelocity = new Vector3(rightHorizontal * rotationSpeed * rightMagnitude,
        leftHorizontal * rotationSpeed * leftMagnitude, rightVertical * rotationSpeed * rightMagnitude);
        rig.angularVelocity = transform.TransformDirection(rotationVelocity);

        movementSpeed = leftMagnitude * maxMovementSpeed * Speed;
        Vector3 newVelocity = new Vector3(0, leftVertical * movementSpeed, leftVertical * 0.1f);
        if (leftVertical > 0)
        {
            Vector3 worldVelocity = transform.TransformDirection(newVelocity);
            worldVelocity *= speedMultiplier;
            rig.AddForce(worldVelocity, ForceMode.VelocityChange);
        }
    }
}
