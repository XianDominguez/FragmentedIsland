using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public FirstPersonController firstPersonController;

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

    public Animator animator;
    private bool alternarAtaque = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Vida = 1;
        barraVida.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento
        if (!animator.GetBool("isAttacking")) // SOLO si NO está atacando
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetBool("Correr", true);
                    animator.SetBool("Andar", false);
                }
                else
                {
                    animator.SetBool("Andar", true);
                    animator.SetBool("Correr", false);
                }
            }
            else
            {
                animator.SetBool("Andar", false);
                animator.SetBool("Correr", false);
            }
        }

        // Ataque
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetBool("isAttacking", true); // empieza ataque
            animator.SetBool("bandera", false);

            if (alternarAtaque)
            {
                animator.SetTrigger("Ataque1");
            }
            else
            {
                animator.SetTrigger("Ataque2");
            }

            alternarAtaque = !alternarAtaque;
 
        }
    }

    public void TerminarAtaque()
    {
        animator.SetBool("isAttacking", false); 
    }

    public void CambiarBandera()
    {
        animator.SetBool("bandera", true);

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
