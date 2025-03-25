using UnityEngine;

public class terrainPoint : MonoBehaviour
{
    public GameObject prefab;
    void Start()
    {
        Debug.Log("terrainPoint Start");
        Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation);
        enabled = false;
    }
}
