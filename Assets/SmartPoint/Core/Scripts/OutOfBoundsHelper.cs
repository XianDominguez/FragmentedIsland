using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SmartPoint
{
    public class OutOfBoundsHelper : MonoBehaviour
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
            }
        }

        private void HandleDeath(GameObject entity)
        {
            if (resetVelocity)
            {
                if (entity.TryGetComponent<CharacterController>(out var cc))
                {
                    cc.SimpleMove(Vector3.zero);
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
        }

        private void TeleportEntity(GameObject go)
        {

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
