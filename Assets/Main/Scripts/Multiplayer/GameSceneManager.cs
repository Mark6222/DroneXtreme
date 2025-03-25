using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
