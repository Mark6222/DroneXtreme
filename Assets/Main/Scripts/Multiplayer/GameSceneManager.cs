using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        if (sceneName == "Restart")
        {
            string scene = SceneManager.GetActiveScene().name;
            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log("Switching scene on server: " + scene);
                NetworkManager.Singleton.SceneManager.LoadScene(scene, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene(scene);
            }
        }
        else
        {
            // check if there 
            if (NetworkManager.Singleton.IsServer)
            {
                Debug.Log("Switching scene on server: " + sceneName);
                NetworkManager.Singleton.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
