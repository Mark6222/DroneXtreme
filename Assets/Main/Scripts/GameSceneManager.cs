using UnityEngine;
using Unity.Netcode;

public class GameSceneManager : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
