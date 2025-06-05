using UnityEngine;

public class SeguimientoCabeza : MonoBehaviour
{
    public Transform headBone;      // el hueso de la cabeza
    public Transform player;        // tu personaje
    public float lookDistance = 5f; // distancia para que te mire
    public float rotationSpeed = 5f;

    void Update()   //Hace que el npc te siga con la cabeza cuando estas cerca de el
    {
        if (headBone == null || player == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= lookDistance)
        {
            Vector3 direction = (player.position - headBone.position).normalized;

            // mirar hacia el jugador
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // CORRECCIÓN para modelos con cabeza mirando Y+
            Quaternion correction = Quaternion.Euler(90f, 0f, 0f);
            targetRotation *= correction;

            // suavizado
            headBone.rotation = Quaternion.Slerp(headBone.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}