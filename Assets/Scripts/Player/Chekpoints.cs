using System.Collections;
using System.Collections.Generic;
using SmartPoint;
using UnityEngine;

namespace SmartPoint
{
        public class Chekpoints : MonoBehaviour
    {
        [SerializeField]
        private CheckPointController CP_Controller;
        [SerializeField]
        private TeleportMode teleportMode = TeleportMode.HighestIndex;
        [SerializeField]
        private bool resetVelocity = true;

        private void Start()
        {
            if (CP_Controller == null)
            {
                CP_Controller = FindObjectOfType<CheckPointController>();
            }

            // Subscribirse al evento de muerte de cada entidad
            foreach (GameObject entity in CP_Controller.GetEntities())
            {
                if (entity.TryGetComponent<Personaje>(out var vida))
                {
                    vida.OnDeath += HandleDeath;
                }
                else
                {
                    Debug.LogWarning($"✘ {entity.name} no tiene componente Personaje");
                }
            }
        }

        private void HandleDeath(GameObject entity) //Evento que teleporta al jugador cuando reaparece al chekpoint anterior
        {

            if (resetVelocity)
            {
                if (entity.TryGetComponent<FirstPersonController>(out var cc))
                {
                    cc.enabled = false;
                    TeleportEntity(entity);
                    cc.enabled = true;
                }
                else if (entity.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.velocity = rb.angularVelocity = Vector3.zero;
                    TeleportEntity(entity);
                }
                else
                {
                    TeleportEntity(entity);
                }
            }
            else
            {
                TeleportEntity(entity);
            }
            if (entity.TryGetComponent<Personaje>(out var personaje))
            {
                personaje.vida = 1f;
                personaje.barraVida.fillAmount = 1f;
            }
        }

        private void TeleportEntity(GameObject go)  //Funcion de teleport
        {
            Debug.Log("Se activa tp");

            switch (teleportMode)
            {
                case TeleportMode.Nearest:
                    CP_Controller.TeleportToNearest(go);
                    break;
                case TeleportMode.HighestIndex:
                    CP_Controller.TeleportToLatest(go);
                    break;
                case TeleportMode.MostRecentlyActivated:
                    CP_Controller.TeleportToRecentlyActivated(go);
                    break;
                case TeleportMode.Random:
                    CP_Controller.TeleportToRandom(go);
                    break;
            }
        }
    }
}

