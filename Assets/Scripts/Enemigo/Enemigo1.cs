using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo1 : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public Quaternion angulo;
    public float grado;

    public GameObject target;
    public bool atacando;

    private string attackAnimationName = "attack";

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {     
        ComportamientoEnemigo();
    }

    public void ComportamientoEnemigo()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 8)
        {
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
                    }
                    break;
                case 1:
                    {
                        grado = Random.Range(0, 360);
                        angulo = Quaternion.Euler(0, grado, 0);
                        rutina++;
                    }
                    break;
                case 2:
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                        transform.Translate(Vector3.forward * 1 * Time.deltaTime);
                        ani.SetBool("walk", true);
                    }
                    break;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, target.transform.position) > 1.3 && !atacando)
            {
                var lookPOs = target.transform.position - transform.position;
                lookPOs.y = 0;
                var rotation = Quaternion.LookRotation(lookPOs);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);

                ani.SetBool("walk", false);

                ani.SetBool("run", true);
                transform.Translate(Vector3.forward * 4 * Time.deltaTime);

                ani.SetBool("attack", false);
            }
            else 
            {
                ani.SetBool("walk", false);
                ani.SetBool("run", false);

                ani.SetBool("attack", true);
                atacando = true;
            }
          

        }
    }

    public void FinalAni()
    {
        ani.SetBool("attack", false);
        atacando = false;
    }
}
