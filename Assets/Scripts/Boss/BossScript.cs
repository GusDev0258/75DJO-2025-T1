using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour, ILevarDano
{
    public AudioClip screamSound;
    private bool viuPlayerPrimeiraVez;
    private NavMeshAgent agent;

    private GameObject player;

    private Animator animator;

    public float atacarDistance = 2.0f;

    public int life = 50;

    private AudioSource audioSource;

    public AudioClip morreuSound;

    public AudioClip stepSound;

    private FieldOfView fov;

    private PatrulharLimitado patrulharLimitado;
    private PontuacaoJogador pontuacaoManager;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        pontuacaoManager = FindObjectOfType<PontuacaoJogador>();
        viuPlayerPrimeiraVez = false;
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
        patrulharLimitado = GetComponent<PatrulharLimitado>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            pontuacaoManager.DestravarPortaFora();
            ToDie();
        }

        if (fov.canSeePlayer)
        {
            print("to vendo ele");
            LookToPlayer();
            GrunirToPlayer();
            HuntPlayer();
            if (!audioSource.isPlaying)
            {
                audioSource.clip = screamSound;
                audioSource.Play();
            }
        }
    }

    private void HuntPlayer()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceFromPlayer < atacarDistance)
        {
            print("to caçando");
            agent.SetDestination(player.transform.position);
            patrulharLimitado.enabled = false;
            agent.isStopped = true;
            animator.SetTrigger("atacar");
            animator.SetBool("podeAndar", false);
            animator.SetBool("pararAtaque", false);
        }
        else
        {
            animator.SetBool("podeAndar", true);
            animator.SetBool("pararAtaque", true);
            agent.SetDestination(player.transform.position);
            agent.isStopped = false;
        }

        if (distanceFromPlayer >= atacarDistance + 1)
        {
            animator.SetBool("pararAtaque", true);
            patrulharLimitado.enabled = true;
        }
    }

    public void DoDamage()
    {
        player.GetComponent<MovimentarPersonagem>().UpdateLife(-20);
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
        agent.isStopped = true;
        animator.SetBool("podeAndar", false);
        animator.SetTrigger("tookDamage");
        StartCoroutine(VoltarAndarDepoisDeTomarDano());
    }

    private IEnumerator VoltarAndarDepoisDeTomarDano()
    {
        yield return new WaitForSeconds(0.5f);
        if (life > 0)
        {
            animator.SetBool("podeAndar", true);
            agent.isStopped = false;
        }
    }

    private void GrunirToPlayer()
    {
        if (!viuPlayerPrimeiraVez)
        {
            agent.isStopped = true;
            animator.SetBool("gruhnir", true);
            audioSource.spatialBlend = 0f;
            audioSource.volume = 1f;
            audioSource.PlayOneShot(screamSound);
            StartCoroutine(VoltarDepoisDeGrunir());
            viuPlayerPrimeiraVez = true;
        }
    }

    private IEnumerator VoltarDepoisDeGrunir()
    {
        yield return new WaitForSeconds(0.5f);
        agent.isStopped = false;
        animator.SetBool("gruhnir", false);
        audioSource.spatialBlend = 1f;
        animator.ResetTrigger("tookDamage");
    }

    private void LookToPlayer()
    {
        Vector3 lookingDirection = player.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookingDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 300);
    }

    private void ToDie()
    {
        audioSource.clip = morreuSound;
        audioSource.Play();
        agent.isStopped = true;
        animator.ResetTrigger("tookDamage");
        animator.ResetTrigger("atacar");

        animator.SetBool("podeAndar", false);
        animator.SetBool("pararAtaque", true);

        animator.SetBool("morreu", true);

        StartCoroutine(DisableAfterDeath());
    }

    private IEnumerator DisableAfterDeath()
    {
        yield return new WaitForSeconds(0.2f);
        this.enabled = false;
    }

    public void AdicionarPontuacaoEMorrer()
    {
        pontuacaoManager.AdicionarInimigosMortos();
        pontuacaoManager.AdicionarPontuacao(30);
    }

    public void Step()
    {
        audioSource.PlayOneShot(stepSound, 0.8f);
    }
}