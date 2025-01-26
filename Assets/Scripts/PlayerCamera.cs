using UnityEngine;
using Unity.Netcode;

public class PlayerCamera : NetworkBehaviour
{
    private void Start()
    {
        if (IsOwner)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
