using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemigoEspiritu : MonoBehaviour
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
    int vidaMaxima = 45;
    public Slider barraVida;


    public bool banderaMuerto = false;
    private bool puedeRecibirDano = true;

    public AudioSource audioSourceEspiritu;
    public AudioSource audioSourceSwingEspirtu;

    public AudioClip Agro;
    public AudioClip[] sonidosDeDano;
    public AudioClip[] sonidosDeMuerte;
    public AudioClip[] sonidosDeSwing;

    private bool teSigue;


    private void Awake()
    {
        vidaActual = vidaMaxima;
    }
    // Start is called before the first frame update
    void Start()
    {
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
            teSigue = false;
            ani.SetBool("run", false);

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
                        agent.isStopped = true;
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


            if (Vector3.Distance(transform.position, target.transform.position) > 2 && !atacando)
            {
                if (!teSigue)
                {
                    audioSourceEspiritu.clip = Agro;
                    audioSourceEspiritu.Play();
                    teSigue = true;
                }
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);

                ani.SetBool("walk", false);
                ani.SetBool("run", true);

                ani.SetBool("attack", false);

                agent.isStopped = false;
                agent.speed = 3;
                agent.SetDestination(target.transform.position);
            }
            else
            {
                ani.SetBool("walk", false);
                ani.SetBool("run", false);
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

    //******************* DETECCION DE COLLISION PARA DANO *****************

    private void OnTriggerEnter(Collider other)
    {
        PersonajeAnimaciones personajeAnimaciones = other.GetComponentInParent<PersonajeAnimaciones>();

        if (other.CompareTag("Espada") && puedeRecibirDano)
        {
            vidaActual -= 10;
            barraVida.value = vidaActual;
            puedeRecibirDano = false;

            personajeAnimaciones.DanoEspada();
            StartCoroutine(ResetearInvulnerabilidad());
        }

        if (other.CompareTag("Pico") && puedeRecibirDano)
        {
            vidaActual -= 3;
            barraVida.value = vidaActual;
            puedeRecibirDano = false;
            personajeAnimaciones.DanoPico();

            StartCoroutine(ResetearInvulnerabilidad());
        }

        if (other.CompareTag("Pala") && puedeRecibirDano)
        {
            vidaActual -= 2;
            barraVida.value = vidaActual;
            puedeRecibirDano = false;
            personajeAnimaciones.DanoPala();

            StartCoroutine(ResetearInvulnerabilidad());
        }

        if (other.CompareTag("Hacha") && puedeRecibirDano)
        {
            vidaActual -= 5;
            barraVida.value = vidaActual;
            puedeRecibirDano = false;
            personajeAnimaciones.DanoHacha();

            StartCoroutine(ResetearInvulnerabilidad());
        }
    }


    IEnumerator ResetearInvulnerabilidad()
    {
        if (vidaActual <= 0)
        {
            banderaMuerto = true;
            MuerteAnim();
        }
        PlayAudioRecibeEspirtu();

        yield return new WaitForSeconds(0.5f); // Tiempo de invulnerabilidad

        puedeRecibirDano = true;
    }

    public void PlayAudioRecibeEspirtu()
    {
        if (sonidosDeDano.Length > 0)
        {
            int indice = Random.Range(0, sonidosDeDano.Length);
            audioSourceEspiritu.clip = sonidosDeDano[indice];
        }
        audioSourceEspiritu.pitch = Random.Range(0.9f, 1.1f);
        audioSourceEspiritu.Play();
    }
    void MuerteAnim()
    {
        if (sonidosDeMuerte.Length > 0)
        {
            int indice = Random.Range(0, sonidosDeMuerte.Length);
            audioSourceEspiritu.clip = sonidosDeMuerte[indice];
        }
        audioSourceEspiritu.pitch = Random.Range(0.9f, 1.1f);
        audioSourceEspiritu.Play();
        ani.Play("Muelte");
    }

    public void AtaqueSonidoEspiritu()
    {
        Debug.Log("Sonido ataque");
        if (sonidosDeSwing.Length > 0)
        {
            int indice = Random.Range(0, sonidosDeSwing.Length);
            audioSourceSwingEspirtu.clip = sonidosDeSwing[indice];
        }
        audioSourceSwingEspirtu.pitch = Random.Range(0.9f, 1.1f);
        audioSourceSwingEspirtu.Play();
    }

    public void MuerteDestroy()
    {
        Destroy(gameObject);
    }
}
