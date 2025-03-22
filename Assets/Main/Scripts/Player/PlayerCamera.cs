using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;
using Unity.Transforms;

public class PlayerCamera : NetworkBehaviour
{
    public bool Offline = true;
    public GameObject camera;

    void Update()
    {
        Offline = !GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().IsHost || !GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().IsConnectedClient;
        if (SceneManager.GetActiveScene().name != "SplashScreen")
        {
            if (Offline) camera.SetActive(true);
            else if (IsOwner) camera.SetActive(true);
            else camera.SetActive(false);
        }
        else
        {
            camera.SetActive(false);
        }
    }
}
