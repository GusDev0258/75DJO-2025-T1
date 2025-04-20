using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewDistance = 10f;
    [Range(0, 360)] public float viewAngle = 35f;
    public bool canSeePlayer;
    
    private GameObject player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        SearchVisiblePlayer();
    }

    private void LookToPlayer()
    {
        Vector3 direcaoOlhar = player.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direcaoOlhar);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 300);
    }

    private void SearchVisiblePlayer()
    {
        Collider[] alvosDentroRaio = Physics.OverlapSphere(transform.position, viewDistance);
        foreach (Collider alvo in alvosDentroRaio)
        {
            if (alvo.gameObject == player)
            {
                Vector3 dirToAlvo = (alvo.transform.position - transform.position).normalized;
                dirToAlvo.y = 0;
                if (Vector3.Angle(transform.forward, dirToAlvo) < viewAngle / 2)
                {
                    float disToAlvo = Vector3.Distance(transform.position, alvo.transform.position);
                    if (!Physics.Raycast(transform.position, dirToAlvo, disToAlvo))
                    {
                        canSeePlayer = true;
                        LookToPlayer();
                        return;
                    }
                }
            }
        }

        canSeePlayer = false;
    }
    
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, viewDistance);
    //     
    //     Vector3 viewAngleA = DirectionFromAngle(-viewAngle / 2);
    //     Vector3 viewAngleB = DirectionFromAngle(viewAngle / 2);
    //     
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewDistance);
    //     Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewDistance);
    // }
    
    // private Vector3 DirectionFromAngle(float eulerY, float anguloEmGraus)
    // {
    //     anguloEmGraus += eulerY;
    //     return new Vector3(Mathf.Sin(anguloEmGraus * Mathf.Deg2Rad), 0, Mathf.Cos(anguloEmGraus * Mathf.Deg2Rad));
    // }
}