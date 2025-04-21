using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CommonEnemy : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agent;

    private GameObject player;

    private Animator animator;

    public float attackDistance = 2.0f;

    public int life = 50;

    private AudioSource audioSource;

    public AudioClip deadSound;

    public AudioClip stepSound;

    private FieldOfView fov;
    private Patrulhar patrulhar;
    private bool estahPatrulhando;

    private PontuacaoJogador pontuacaoManager;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        pontuacaoManager = FindObjectOfType<PontuacaoJogador>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.reverbZoneMix = 1.1f;
        audioSource.spatialBlend = 1.0f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 20f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        fov = GetComponent<FieldOfView>();
        patrulhar = GetComponent<Patrulhar>();
        estahPatrulhando = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            ToDie();
            return;
        }

        if (fov.canSeePlayer)
        {
            if (estahPatrulhando)
            {
                estahPatrulhando = false;
                patrulhar.enabled = false;
            }

            LookToPlayer();
            HuntPlayer();
        }
        else
        {
            patrulhar.enabled = true;
            estahPatrulhando = true;
            agent.isStopped = false;
            patrulhar.Andar();
            animator.SetBool("canWalk", true);
            animator.SetBool("stopAttack", true);
            // animator.ResetTrigger("tookShot");
            FixRigidExit();
        }
    }

    private void HuntPlayer()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceFromPlayer < attackDistance)
        {
            agent.isStopped = true;
            agent.SetDestination(player.transform.position);
            animator.SetTrigger("attack");
            animator.SetBool("canWalk", false);
            animator.SetBool("stopAttack", false);
            FixRigidEnter();
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
            animator.SetBool("canWalk", true);
            animator.SetBool("stopAttack", true);
            FixRigidExit();
        }
    }

    private void LookToPlayer()
    {
        Vector3 lookingDirection = player.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookingDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 300);
    }

    private void FixRigidEnter()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void FixRigidExit()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void TakeDamage(int damage)
    {
        if (life > 0 && !animator.GetBool("morreu"))
        {
            life -= damage;
            agent.isStopped = true;
            animator.SetTrigger("tookShot");
            animator.SetBool("canWalk", false);
            animator.SetBool("stopAttack", true);
            StartCoroutine(VoltarAndarDepoisDeTomarDano());
        }
    }

    private IEnumerator VoltarAndarDepoisDeTomarDano()
    {
        yield return new WaitForSeconds(0.3f);
        if (life > 0)
        {
            agent.isStopped = false;
            patrulhar.enabled = true;
            animator.SetBool("canWalk", true);
            animator.SetBool("stopAttack", false);
        }
    }

    public void DoDamage()
    {
        if (patrulhar.enabled)
        {
            estahPatrulhando = false;
            patrulhar.enabled = false;
        }

        player.GetComponent<MovimentarPersonagem>().UpdateLife(-10);
        patrulhar.enabled = true;
    }

    private void ToDie()
    {
        audioSource.clip = deadSound;
        audioSource.Play();

        pontuacaoManager.AdicionarInimigosMortos();
        pontuacaoManager.AdicionarPontuacao(10);
        agent.isStopped = true;
        animator.SetBool("canWalk", false);
        animator.SetBool("stopAttack", true);
        animator.SetBool("dead", true);
        animator.SetTrigger("morreu");

        this.enabled = false;
    }

    public void Step()
    {
        audioSource.PlayOneShot(stepSound, 0.3f);
    }
}