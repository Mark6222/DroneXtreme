using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPropellers : MonoBehaviour
{
    public float spinSpeed = 100f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }
}
