using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource ambienceSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length < 2)
        {
            Debug.LogError("AudioManager necesita 2 AudioSources (música y ambiente).");
            return;
        }

        musicSource = sources[0];    // Para la música principal
        ambienceSource = sources[1]; // Para ambiente
    }

    public void ChangeMusic(AudioClip newClip)
    {
        if (musicSource.clip == newClip) return;

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();
    }

    public void ChangeAmbience(AudioClip newClip)
    {
        if (ambienceSource.clip == newClip) return;

        ambienceSource.Stop();
        ambienceSource.clip = newClip;
        ambienceSource.Play();
    }
}
