using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    private AudioSource audioSource;

    [Range(0.0f, 1.0f)]
    public float amplitude = 0.2f;

    [Range(100.0f, 8000.0f)]
    public float frequency = 440.0f; // A4

    public bool isPlayingTone = false;

    private const int SAMPLE_RATE = 44100;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; // force 2D sound
    }

    public void PlayNote(float hertz, float volume = 0.2f)
    {
        frequency = hertz;
        amplitude = Mathf.Clamp01(volume);
        isPlayingTone = true;

        // Length in seconds (adjust as needed)
        float duration = 0.3f;

        // Generate and play clip
        AudioClip clip = CreateSineWaveClip(frequency, amplitude, duration);
        audioSource.clip = clip;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    public void StopPlaying()
    {
        isPlayingTone = false;
        if (audioSource.isPlaying)
            audioSource.Stop();
    }

    private AudioClip CreateSineWaveClip(float frequency, float amplitude, float duration)
    {
        int sampleCount = Mathf.CeilToInt(SAMPLE_RATE * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / SAMPLE_RATE) * amplitude;
        }

        AudioClip clip = AudioClip.Create("Sine_" + frequency, sampleCount, 1, SAMPLE_RATE, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
