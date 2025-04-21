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

    public float attackDistance = 6f;

    public LineRenderer laser;
    private float laserDistance = 40f;

    private float tempoDeMira;

    private float acuracia = 0.2f;
    private float chanceAtirar = 0.2f;
    private Coroutine rotinaDeAtaque;
    private Coroutine rotinaObservacao;

    public AudioClip somTiro;
    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
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
            if (rotinaObservacao == null)
            {
                rotinaObservacao = StartCoroutine(ObservarESortearAtaque());
            }

            if (!animator.GetBool("estahEscondido"))
            {
                Esconder();
            }
            else
            {
                AtualizaPosicaoLaser();
            }
        }
        else
        {
            if (rotinaObservacao != null)
            {
                StopCoroutine(rotinaObservacao);
                rotinaObservacao = null;
            }

            agent.isStopped = false;
            animator.SetBool("estahEscondido", false);
            animator.SetBool("patrulhando", true);
            animator.ResetTrigger("atirar");
            laser.enabled = false;
            animator.SetTrigger("patrulhar");
        }
    }

    private IEnumerator ObservarESortearAtaque()
    {
        while (fov.canSeePlayer)
        {
            yield return new WaitForSeconds(1f);

            if (animator.GetBool("estahEscondido") && rotinaDeAtaque == null)
            {
                if (Random.value <= chanceAtirar)
                {
                    animator.SetBool("estahEscondido", false);
                    animator.SetTrigger("atirar");
                    animator.SetBool("patrulhando", false);
                    LevantaAtira();
                }
            }
        }

        rotinaObservacao = null;
    }

    public void PlaySomTiro()
    {
        audioSource.PlayOneShot(somTiro);
    }

    private void Esconder()
    {
        agent.isStopped = true;
        animator.SetBool("estahEscondido", true);
        animator.SetBool("patrulhando", false);
        animator.SetTrigger("esconder");
    }

    private void AtualizaPosicaoLaser()
    {
        if (!laser.enabled)
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
        animator.SetTrigger("atirar");
        yield return new WaitForSeconds(0.5f);

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