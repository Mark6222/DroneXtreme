using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerCamera : NetworkBehaviour
{
    private void Start()
    {
        if (IsOwner)
        {
            if(SceneManager.GetActiveScene().name == "SplashScreen"){
                gameObject.SetActive(false);
            }
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
