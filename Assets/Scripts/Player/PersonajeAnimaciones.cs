using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeAnimaciones : MonoBehaviour
{


    public Animator animator;

    private bool alternarAtaque = false;

    public float cooldownAtaque = 1.0f;
    private float lastAttackTime = -Mathf.Infinity;

    public bool animacionEjecutandose;

    public AudioSource audioSourceSonidoEspada;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento
        if (!animator.GetBool("isAttacking")) // SOLO si NO está atacando
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= lastAttackTime + cooldownAtaque)
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

            lastAttackTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time >= lastAttackTime + cooldownAtaque)
        {
            animator.SetBool("isSecondary", true);
            animator.SetTrigger("AccionSecundaria");
            animator.SetBool("bandera", false);
            lastAttackTime = Time.time;
        }

        
    }

    public void EstadoAnimaciones()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Ataque 1") || stateInfo.IsName("Ataque 2") || stateInfo.IsName("PalaExcavar"))
        {
            animacionEjecutandose = true;
        }
        else
        {
            animacionEjecutandose = false;
        }
    }

    public void PlayAudioEspada()
    {
        audioSourceSonidoEspada.pitch = Random.Range(0.9f, 1.1f);
        audioSourceSonidoEspada.Play();
    }

    public void TerminarAtaque()
    {
        animator.SetBool("isAttacking", false);
    }

    public void TerminarAccSec()
    {
        animator.SetBool("isSecondary", false);
    }

    public void CambiarBandera()
    {
        animator.SetBool("bandera", true);

    }

}
