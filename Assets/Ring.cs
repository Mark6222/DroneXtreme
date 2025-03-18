using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private ParticleSystem checkpointParticles;
    private AudioSource audioSource;
    public bool isReached = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drone"))
        {
            checkpointParticles.Play();
            audioSource.Play();
            isReached = true;
            Destroy(gameObject, 0.5f);
        }
    }
}
