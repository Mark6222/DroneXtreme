using UnityEngine;

public class terrainPoint : MonoBehaviour
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
            GameObject o = Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(o, 3f);
            enabled = false;
        }
    }
}
