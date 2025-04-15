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
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0.5f;
        audioSource.reverbZoneMix = 1.1f;
    }

    // Update is called once per frame
    void Update()
    {
       HuntPlayer(); 
       LookToPlayer();
       if (life <= 0)
       {
           this.ToDie();
       }
    }

    private void HuntPlayer()
    {
       float distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
       if (distanceFromPlayer < attackDistance)
       {
           agent.isStopped = true;
           print("Ataquei");
           animator.SetTrigger("attack");
           animator.SetBool("canWalk", false);
           animator.SetBool("stopAttack", false);
           FixRigidEnter();
       }

       if (distanceFromPlayer >= attackDistance + 1)
       {
           animator.SetBool("stopAttack", true);
           FixRigidExit();
       }

       if (animator.GetBool("canWalk"))
       {
           agent.isStopped = false;
           agent.SetDestination(player.transform.position);
           animator.ResetTrigger("attack");
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
        life -= damage;
        agent.isStopped = true;
        animator.SetTrigger("tookShot");
        animator.SetBool("canWalk", false);
    }

    public void DoDamage()
    {
        player.GetComponent<MovimentarPersonagem>().UpdateLife(-10);
    }

    private void ToDie()
    {
        audioSource.clip = deadSound;
        audioSource.Play();

        agent.isStopped = true;
        animator.SetBool("canWalk", false);
        animator.SetBool("stopAttack", true);
        
        animator.SetBool("dead", true);

        this.enabled = false;
    }

    public void Step()
    {
        audioSource.PlayOneShot(stepSound, 0.3f);
    }
    
}
