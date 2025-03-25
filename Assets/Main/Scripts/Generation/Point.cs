using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Point : MonoBehaviour
{
    public GameObject prefab;
    void Start()
    {
        Debug.Log("Point Start");
        Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation);
        enabled = false;
    }
}
