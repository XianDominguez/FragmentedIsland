using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnZombie : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public Quaternion angulo;
    public float grado;

    public GameObject target;
    public bool atacando;

    public NavMeshAgent agent;
    private Vector3 destinoAleatorio;

    [Header("Vida")]
    public int vidaActual;
    int vidaMaxima = 40;
    public Slider barraVida;


    public bool banderaMuerto = false;
    private bool puedeRecibirDano = true;

    // Start is called before the first frame update
    void Start()
    {
        vidaActual = vidaMaxima;
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (banderaMuerto == false)
        {
            ComportamientoEnemigo();
        }
    }

    public void ComportamientoEnemigo()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 8)
        {
            cronometro += 1 * Time.deltaTime;
            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    {
                        ani.SetBool("walk", false);
                        if (agent.isOnNavMesh)
                        {
                            agent.isStopped = true; // o agent.Stop() si usás una versión más vieja
                        }
                        else
                        {
                            Debug.LogWarning("El agente no está sobre el NavMesh.");
                        }
                    }
                    break;
                case 1:
                    {
                        Vector3 puntoRandom = transform.position + Random.insideUnitSphere * 10;
                        NavMeshHit hit;

                        if (NavMesh.SamplePosition(puntoRandom, out hit, 10.0f, NavMesh.AllAreas))
                        {
                            destinoAleatorio = hit.position;
                            agent.SetDestination(destinoAleatorio);
                            agent.speed = 1.5f;
                            agent.isStopped = false;
                            ani.SetBool("walk", true);
                            rutina++;
                        }
                    }
                    break;
                case 2:
                    {

                        if (!agent.pathPending && agent.remainingDistance < 0.5f)
                        {
                            ani.SetBool("walk", false);
                            rutina = 0;
                            cronometro = 0;
                            agent.isStopped = true;
                        }
                    }
                    break;
            }
        }
        else
        {
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);


            if (Vector3.Distance(transform.position, target.transform.position) > 2.4 && !atacando)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);

                ani.SetBool("walk", true);

                ani.SetBool("attack", false);

                agent.isStopped = false;
                agent.speed = 3;
                agent.SetDestination(target.transform.position);
            }
            else
            {
                ani.SetBool("walk", false);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);

                ani.SetBool("attack", true);

                agent.isStopped = true;
                atacando = true;
            }




        }
    }

    public void FinalAni()
    {
        ani.SetBool("attack", false);
        atacando = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Espada") && puedeRecibirDano)
        {
            Debug.Log("Dano Zombie");
            vidaActual -= 10;
            barraVida.value = vidaActual;
            puedeRecibirDano = false;
            StartCoroutine(ResetearInvulnerabilidad());
            if (vidaActual <= 0)
            {
                banderaMuerto = true;
                MuerteAnim();
            }
        }
    }
    IEnumerator ResetearInvulnerabilidad()
    {
        yield return new WaitForSeconds(0.5f); // Tiempo de invulnerabilidad
        puedeRecibirDano = true;
    }
    void MuerteAnim()
    {
        ani.Play("MuerteZombie");
    }

    public void MuerteDestroy()
    {
        Destroy(gameObject);
    }
}
