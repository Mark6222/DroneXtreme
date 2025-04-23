using UnityEngine;
using Unity.Netcode;

public class SpawnSinglePlayer : NetworkBehaviour
{
    bool isMultiplayer = false;
    public GameObject PlayerPrefab;
    public GameObject PlayerSpawn;

    void Start()
    {
        isMultiplayer = NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient;
        if (!isMultiplayer)
        {
            GameObject player = Instantiate(PlayerPrefab, PlayerSpawn.transform.position, Quaternion.identity) as GameObject;
            player.GetComponent<PlayerManeger>().OnlineDrone = false;
        }
    }

    void Update()
    {

    }
}
