using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zorro : MonoBehaviour
{
    public AudioClip[] sonidosZorro;       // Clips cargados desde Resources
    private AudioSource audioSourceZorro;


    void Start()
    {
        audioSourceZorro = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomSound());
    }

    IEnumerator PlayRandomSound()
    {
        while (true)
        {
            float waitTime = Random.Range(3f, 5f);
            yield return new WaitForSeconds(waitTime);

            if (sonidosZorro.Length > 0)
            {
                AudioClip clip = sonidosZorro[Random.Range(0, sonidosZorro.Length)];
                audioSourceZorro.PlayOneShot(clip);
            }
        }
    }
}
