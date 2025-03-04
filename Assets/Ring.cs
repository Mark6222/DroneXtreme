using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private ParticleSystem checkpointParticles;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Drone"))
        {
            checkpointParticles.Play();
        }
    }
}
