using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;
using Unity.Transforms;

public class PlayerCamera : NetworkBehaviour
{
    public bool Offline = true;
    public GameObject camera;
    private bool cameraActivated = false;

    void Update()
    {
        if (!cameraActivated)
        {
            NetworkManager networkManager = GameObject.FindGameObjectWithTag("NetworkManager")?.GetComponent<NetworkManager>();
            Offline = networkManager == null || !networkManager.IsHost || !networkManager.IsConnectedClient;
            
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

            cameraActivated = true;
        }
    }
}
