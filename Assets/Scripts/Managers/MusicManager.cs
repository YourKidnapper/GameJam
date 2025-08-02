using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip shopMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        PlayShopMusic();
    }

    public void PlayShopMusic()
    {
        if (musicSource == null || shopMusic == null) return;

        musicSource.clip = shopMusic;
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource?.Stop();
    }
}