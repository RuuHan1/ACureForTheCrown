using Unity.VisualScripting;
using UnityEngine;

// Singleton Pattern: Oyun boyunca sadece bir tane AudioManager olmasini garantileriz.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource punishmentSource;

    [Header("Background Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Game Sound Effects (SFX)")]
    public AudioClip swipeSound; // Artik tek bir kaydirma sesimiz var

    [Header("Punishemnt Sound Effects")]
    public AudioClip imprisonSound; // Hapse atma sesi
    private void Awake()
    {
        // Singleton Kurulumu
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Sahne degisse bile muzik kesilmez
        }
        else
        {
            Destroy(gameObject);
            return; // Eger bu kopya yok edilecekse, asagidaki kodlarin calismasini engelle
        }
    }
    private void OnEnable()
    {
        GameEvents.GameStarted += OnGameStarted;
    }
    private void OnDisable()
    {
        GameEvents.GameStarted -= OnGameStarted;
    }
    private void OnGameStarted()
    {
        if (backgroundMusic != null)
        {
            PlayMusic(backgroundMusic);
        }
    }

    private void Start()
    {
        
    }

    /// <summary>
    /// Arka plan muzigini calar ve donguye (loop) alir.
    /// </summary>
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return; // Ayni muzik zaten caliyorsa basa sarma

        musicSource.clip = clip;
        musicSource.loop = true; // Muzigin surekli tekrarlamasini saglar
        musicSource.Play();
    }

    /// <summary>
    /// Tek seferlik ses efektlerini calar (Kart kaydirma vb.)
    /// Sesi tekrara dusmekten kurtarmak icin hafif pitch degisikligi secenegi eklendi.
    /// </summary>
    public void PlaySFX(AudioClip clip, bool randomizePitch = false)
    {
        if (clip == null) return;

        // Eger randomizePitch true gelirse, sesin inceligini/kalinligini hafifce degistirir
        // Bu sayede ayni kart sesi defalarca caldiginda kulagi yormaz ve robotik hissettirmez.
        if (randomizePitch)
        {
            sfxSource.pitch = Random.Range(0.9f, 1.1f);
        }
        else
        {
            sfxSource.pitch = 1f; // Varsayilan deger
        }

        // PlayOneShot, ayni anda gelen seslerin birbirini kesmeden ust uste calmasini saglar.
        sfxSource.PlayOneShot(clip);
    }

    public void PlayPunishmentSFX(AudioClip clip)
    {
        if (clip == null) return;
        punishmentSource.PlayOneShot(clip);
    }
}