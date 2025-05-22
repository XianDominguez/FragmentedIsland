using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntradaCueva : MonoBehaviour
{
    public Transform puntoDestino; // Donde se teletransporta el jugador
    public CanvasGroup fadeCanvas; // UI para fade (CanvasGroup con imagen negra)
    public AudioSource sonidoEntrada; // Sonido ambiental al entrar
    public float fadeDuration = 1f;
    public KeyCode teclaInteractuar = KeyCode.E;

    private bool enZona = false;
    private GameObject jugador;

    void Update()
    {
        if (enZona && Input.GetKeyDown(teclaInteractuar))
        {
            StartCoroutine(EntrarACueva());
        }

    }

    private IEnumerator EntrarACueva()
    {
        // Fade In
        yield return StartCoroutine(Fade(0, 1, fadeDuration));

        // Sonido
        if (sonidoEntrada != null)
            sonidoEntrada.Play();

        // Teleportar jugador
        jugador.transform.position = puntoDestino.position;

        // Esperar un segundo antes de Fade Out
        yield return new WaitForSeconds(1f);

        // Fade Out
        yield return StartCoroutine(Fade(1, 0, fadeDuration));
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        fadeCanvas.alpha = to;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enZona = true;
            jugador = other.gameObject;
            // Mostrar UI: "Presiona E para entrar"
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enZona = false;
            jugador = null;
            // Ocultar UI
        }
    }
}
