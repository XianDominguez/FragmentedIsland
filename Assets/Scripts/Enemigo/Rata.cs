using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rata : MonoBehaviour
{
    public AudioClip[] sonidosRata;       // Clips cargados desde Resources
    private AudioSource audioSourceRata;


    void Start()
    {
        audioSourceRata = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomSound());
    }

    IEnumerator PlayRandomSound()
    {
        while (true)
        {
            float waitTime = Random.Range(3f, 5f);
            yield return new WaitForSeconds(waitTime);

            if (sonidosRata.Length > 0)
            {
                AudioClip clip = sonidosRata[Random.Range(0, sonidosRata.Length)];
                audioSourceRata.PlayOneShot(clip);
            }
        }
    }
}
