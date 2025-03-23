using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Point : MonoBehaviour
{
    public GameObject prefab;
    ProcedurelGeneration pg;

    void Start()
    {
        pg = FindFirstObjectByType<ProcedurelGeneration>();
    }

    void Update()
    {
        if (pg.complete)
        {
            Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation);
            enabled = false;
        }
    }
}
