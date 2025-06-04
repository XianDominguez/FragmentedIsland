using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioMusicaZona : MonoBehaviour
{
    public AudioClip zoneMusic;
    public AudioClip ambienceClip; // Nuevo campo para ambiente

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (zoneMusic != null)
                AudioManager.Instance.ChangeMusic(zoneMusic);

            if (ambienceClip != null)
                AudioManager.Instance.ChangeAmbience(ambienceClip);
        }
    }
}
