using UnityEngine;

public class terrainPoint : MonoBehaviour
{
    public GameObject prefab;
    void Start()
    {
    }

    private float timer = 0f;
    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                DoSomething();
                hasTriggered = true;
            }
        }
    }

    void DoSomething()
    {
        Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation);
    }
}
