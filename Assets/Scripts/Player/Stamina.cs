using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private FirstPersonController playerController; // Asignar manualmente
    [SerializeField] private Image staminaFillBar; // Asignar tu barra de UI


    [Header("Opciones")]
    [SerializeField] private bool hideWhenFull = true;

    private void Update() //Controla la barra de stamina cuando el jugador corre
    {
        if (playerController == null || staminaFillBar == null) return;

        if (playerController.unlimitedSprint)
        {
            staminaFillBar.fillAmount = 1f;
            if (hideWhenFull) staminaFillBar.gameObject.SetActive(false);
            return;
        }

        float fillPercent = playerController.SprintRemaining / playerController.SprintDuration;
        staminaFillBar.fillAmount = fillPercent;

        if (hideWhenFull)
        {
            staminaFillBar.gameObject.SetActive(fillPercent < 1f);
        }
    }
}
