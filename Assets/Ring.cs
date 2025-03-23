using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private ParticleSystem checkpointParticles;
    private AudioSource audioSource;
    public bool isReached = false;
    public bool isNextRing = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drone") && !isNextRing)
        {
            checkpointParticles.Play();
            audioSource.Play();
            isReached = true;
            Destroy(gameObject, 0.5f);
        }
    }

    public void SetNextRing()
    {
        isNextRing = true;
        gameObject.GetComponent<Renderer>().material.color = Color.grey;
    }

    public void SetCurrntRing()
    {
        isNextRing = false;
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }
}
