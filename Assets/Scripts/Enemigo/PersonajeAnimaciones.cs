using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeAnimaciones : MonoBehaviour
{


    public Animator animator;

    private bool alternarAtaque = false;

    public float cooldownAtaque = 1.0f;
    private float lastAttackTime = -Mathf.Infinity;


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
    }


    public void TerminarAtaque()
    {
        animator.SetBool("isAttacking", false);
    }

    public void CambiarBandera()
    {
        animator.SetBool("bandera", true);

    }

}
