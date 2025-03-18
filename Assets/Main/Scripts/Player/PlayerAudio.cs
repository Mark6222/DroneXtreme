using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudio : MonoBehaviour
{
    private PlayerMovement player;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip droneLanding;
    [SerializeField] private AudioClip droneFlying;
    float basePitch = 0.7f;
    float peakPitch = 1.5f;

    private bool isLanding = false;

    void Start()
    {
        player = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();

        if (droneFlying != null)
        {
            audioSource.clip = droneFlying;
            audioSource.loop = true;
            audioSource.volume = 0.5f; 
            audioSource.pitch = basePitch; // Start at max speed
            audioSource.Play();
        }
    }

    void Update()
    {
        float inputY = player.leftStickInput.y;
        // Debug.Log(inputY);

        if (inputY > 0)
        {
            if (audioSource.clip != droneFlying)
            {
                audioSource.clip = droneFlying;
                audioSource.loop = true;
                audioSource.Play();
            }

            // Reduce pitch & volume based on inputY
            audioSource.volume = Mathf.Lerp(0.3f, 1f, inputY);
            float pitchDiff = peakPitch - basePitch;
            pitchDiff = pitchDiff * inputY;
            audioSource.pitch = basePitch + pitchDiff;

            isLanding = false;
        }
        else
        {
            if (!isLanding)
            {
                audioSource.Stop();
                audioSource.clip = droneLanding;
                audioSource.loop = false;
                audioSource.Play();
                isLanding = true;
            }
        }
    }
}