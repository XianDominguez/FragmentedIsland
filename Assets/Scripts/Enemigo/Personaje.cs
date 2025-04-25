using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class Personaje : MonoBehaviour
{
    public float Vida;
    public Image barraVida;

    public AudioSource audioSource;
    Camera cam;

    [Header("Stamina")]
    [Space]
    public float staminaMax = 100f;
    public float staminaActual;
    public float regeneracion = 10f;
    public float gastoPorCorrer = 20f;

    public Image barraStamina;

    private bool corriendo;
    [Header("Ataque")]
    [Space]
    public float distanciaAtaque = 5000f;
    public float delayAtaque = 0.4f;
    public float velocidadAtaque = 1f;
    public int danoAtaque = 1;
    public LayerMask attackLayer;

    public GameObject hitEffect;
    public AudioClip swingEspada;
    public AudioClip sonidoGolpe;

    bool atacando = false;
    bool listoParaAtacar = true;
    int cuentaAtaque;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Vida = 1;
        barraVida.fillAmount = 1;
        staminaActual = staminaMax;
        ActualizarBarra();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Botón izquierdo del mouse
        {
            Ataque();
        }

        corriendo = Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0;

        if (corriendo && staminaActual > 0)
        {
            staminaActual -= gastoPorCorrer * Time.deltaTime;
            if (staminaActual < 0) staminaActual = 0;
        }
        else if (staminaActual < staminaMax)
        {
            staminaActual += regeneracion * Time.deltaTime;
            if (staminaActual > staminaMax) staminaActual = staminaMax;
        }

        ActualizarBarra();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("arma"))
        {
            Debug.Log("Dano");
            Vida = Vida - 0.1f;
            barraVida.fillAmount = Vida;
        }
    }

    void ActualizarBarra()
    {
        if (barraStamina != null)
        {
            barraStamina.fillAmount = staminaActual / staminaMax;
        }
    }

    void Ataque()
    {
        if (!listoParaAtacar || atacando) return;
        
        listoParaAtacar = false;
        atacando = true;

        Invoke(nameof(ResetAttack),velocidadAtaque);
        Invoke(nameof(AttackRaycast),delayAtaque);

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swingEspada);

        print("Ataca");
        
    }

    void ResetAttack()
    {
        atacando = false;
        listoParaAtacar = true;
    }

    void AttackRaycast()
    {
        print("Se ejecuta atakraycast");

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, distanciaAtaque, attackLayer))
        {
            print("El raycast detecta al enemigo");
            Debug.DrawLine(cam.transform.position, hit.point, Color.red, 1000f);
            HitTarget(hit.point);

            if (hit.transform.TryGetComponent<Enemigo1>(out Enemigo1 enemigo1))
            {
                enemigo1.RecibirDano(danoAtaque);
                print("Golpea al enemigo");
            }
        }
    }

    void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(sonidoGolpe);

        GameObject GO = Instantiate(hitEffect, pos , Quaternion.identity);
        Destroy(GO,20);
    }
}
