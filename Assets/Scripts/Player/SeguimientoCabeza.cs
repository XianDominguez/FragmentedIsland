using UnityEngine;

public class SeguimientoCabeza : MonoBehaviour
{
    public Transform headBone;      // hueso de la cabeza
    public Transform player;        // tu personaje
    public float lookDistance = 100f; // distancia para empezar a mirar
    public float rotationSpeed = 5f;
   
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= lookDistance)
        {
            Vector3 direction = (player.position - headBone.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            headBone.rotation = Quaternion.Slerp(headBone.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
