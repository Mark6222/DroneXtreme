using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            PlayerPrefs.SetInt("IsMultiplayer", 1);
            NetworkManager.Singleton.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        else
        {
            PlayerPrefs.SetInt("IsMultiplayer", 0);
            SceneManager.LoadScene(sceneName);
        }
    }
}
