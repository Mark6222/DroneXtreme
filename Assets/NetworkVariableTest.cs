using UnityEngine;
using Unity.Netcode;
using TMPro;
using System.Collections.Generic;

public class NetworkVariableTest : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private NetworkList<Vector3> syncedPositions;

    public List<Transform> sourceTransforms;

    private void Awake()
    {
        syncedPositions = new NetworkList<Vector3>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            syncedPositions.Clear();
            foreach (var t in sourceTransforms)
                syncedPositions.Add(t.position);
        }

        syncedPositions.OnListChanged += _ => UpdateUI();
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = $"Transforms: {syncedPositions.Count}";
    }

    private void OnDestroy()
    {
        syncedPositions.OnListChanged -= _ => UpdateUI();
    }
}