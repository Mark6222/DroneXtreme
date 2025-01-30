using UnityEngine;

public class Point : MonoBehaviour
{
    public GameObject prefab;
    void Start()
    {
        Debug.Log("hello");
    }

    private float timer = 0f;
    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered)
        {
            timer += Time.deltaTime;

            if (timer >= 10f)
            {
                DoSomething();
                hasTriggered = true;
            }
        }
    }

    void DoSomething()
    {
        Instantiate(prefab, gameObject.transform.position, gameObject.transform.localRotation);
    }
}
