using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance;

    [Header("Music Settings")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField, Range(0f, 1f)] private float volume = 0.4f;
    [SerializeField] private bool loopMusic = true;

    private AudioSource audioSource;

    private void Awake()
    {
        // Eğer daha önce bir müzik yöneticisi oluştuysa yenisini yok et.
        // Böylece sahne değişince aynı müzik iki kez üst üste çalmaz.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Bu obje sahne değişince yok olmasın.
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = backgroundMusic;
        audioSource.volume = volume;
        audioSource.loop = loopMusic;
        audioSource.playOnAwake = false;

        // 2D oyun için müzik mekânsal değil, direkt genel arka plan sesi olsun.
        audioSource.spatialBlend = 0f;
    }

    private void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (audioSource == null)
        {
            return;
        }

        if (backgroundMusic == null)
        {
            Debug.LogWarning("Background music atanmadı.");
            return;
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);

        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}