using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AtiradorElite : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agent;

    private GameObject player;
    private Animator animator;
    private FieldOfView fov;

    public int life = 200;

    public float attackDistance = 5f;

    public LineRenderer laser;
    private float laserDistance = 30f;

    private float tempoDeMira;

    private float acuracia = 0.3f;
    private Coroutine rotinaDeAtaque;

    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<LineRenderer>();
        fov = GetComponent<FieldOfView>();
        if (fov == null)
        {
            fov = gameObject.AddComponent<FieldOfView>();
        }

        if (laser == null)
        {
            laser = gameObject.AddComponent<LineRenderer>();
        }

        agent = GetComponent<NavMeshAgent>();
        laser.enabled = false;
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fov.canSeePlayer)
        {
            if (!animator.GetBool("estahEscondido"))
            {
                Esconder();
            }

            if (Random.value <= 0.2f && rotinaDeAtaque == null)
            {
                LevantaAtira();
            }
        }
        else
        {
            agent.isStopped = false;
            animator.SetBool("estahEscondido", false);
            animator.SetBool("patrulhando", true);
            animator.ResetTrigger("atirar");
            laser.enabled = false;
            animator.SetTrigger("patrulhar");
        }
    }

    private void Esconder()
    {
        agent.isStopped = true;
        animator.SetBool("estahEscondido", true);
        animator.SetBool("patrulhando", false);
        animator.SetTrigger("esconder");

        laser.enabled = true;
        Vector3 dir = (player.transform.position - transform.position).normalized;

        laser.SetPosition(0, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, laserDistance))
        {
            laser.SetPosition(1, hit.point);
        }
        else
        {
            laser.SetPosition(1, transform.position + dir * laserDistance);
        }
    }

    private void LevantaAtira()
    {
        animator.SetBool("estahEscondido", false);
        if (Random.value <= acuracia)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dir, out hit, laserDistance))
            {
                laser.enabled = true;
                laser.SetPosition(0, transform.position);
                laser.SetPosition(1, hit.point);
            }

            rotinaDeAtaque = StartCoroutine(RealizarAtaque(hit.point));
        }
        else
        {
            StartCoroutine(RetornarEsconderijo());
        }
    }

    private IEnumerator RealizarAtaque(Vector3 pontoImpacto)
    {
        yield return new WaitForSeconds(2f); 

        animator.SetTrigger("atirar");

        if (Vector3.Distance(player.transform.position, pontoImpacto) <= 2f)
        {
            player.GetComponent<MovimentarPersonagem>().UpdateLife(-80);
            animator.ResetTrigger("atirar");
        }

        laser.enabled = false;
        rotinaDeAtaque = null;

        StartCoroutine(RetornarEsconderijo());
    }

    private IEnumerator RetornarEsconderijo()
    {
        yield return new WaitForSeconds(1.5f);
        animator.ResetTrigger("esconder");
        animator.ResetTrigger("atirar");
        Esconder();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        agent.isStopped = true;
        animator.SetTrigger("tomouTiro");
        StartCoroutine(VoltarPatrulha());
    }

    private IEnumerator VoltarPatrulha()
    {
        yield return new WaitForSeconds(0.6f);
        if (life > 0)
        {
            animator.ResetTrigger("tomouTiro");
            animator.SetBool("patrulhando", true);
            animator.SetTrigger("patrulhar");
            agent.isStopped = false;
        }
        else
        {
           animator.SetTrigger("morreu");
           agent.isStopped = true;
           this.enabled = false;
        }
    }
}