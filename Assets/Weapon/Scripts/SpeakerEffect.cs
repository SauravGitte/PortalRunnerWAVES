using UnityEngine;

public class SpeakerEffect : MonoBehaviour
{
    [Header("Pulse Scaling Settings")]
    public float minScale = 1.0f;  // Minimum scale size
    public float maxScale = 1.2f;  // Maximum scale size
    public float pulseSpeed = 2.0f; // Speed of scaling up & down

    [Header("Swaying Settings")]
    public float swayAmount = 0.1f; // Max sway distance in Y direction
    public float swaySpeed = 2.0f;  // Speed of swaying movement

    [Header("Audio Sync (Optional)")]
    public bool useAudio = false;    // Enable real-time music sync
    public AudioSource audioSource;  // Assign an audio source
    public float audioSensitivity = 10.0f; // Adjust reaction strength

    private Vector3 originalScale;
    private Vector3 originalPosition;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    void Update()
    {
        // **Pulsing Effect** (Scaling)
        float scaleMultiplier = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * pulseSpeed) + 1) / 2);

        // **Swaying Effect** (Moving up & down randomly)
        float yOffset = Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        // **Optional: Audio Sync**
        if (useAudio && audioSource != null)
        {
            float audioLevel = GetAudioLevel() * audioSensitivity;
            scaleMultiplier += audioLevel * 0.1f;  // Increase size slightly with audio
            yOffset += audioLevel * 0.05f; // Sway slightly with audio
        }

        // Apply transformations
        transform.localScale = originalScale * scaleMultiplier;
        // transform.position = new Vector3(originalPosition.x, originalPosition.y + yOffset, originalPosition.z);
    }

    // **Audio Analysis (Gets the current audio loudness)**
    float GetAudioLevel()
    {
        float[] samples = new float[256];
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        float sum = 0f;
        foreach (float sample in samples) sum += sample;
        return sum / samples.Length; // Average volume
    }
}
