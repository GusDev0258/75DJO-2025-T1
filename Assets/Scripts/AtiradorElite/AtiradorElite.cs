using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AtiradorElite : MonoBehaviour
{
    private NavMeshAgent agent;

    private GameObject player;
    private Animator animator;
    public FieldOfView fov;

    public int life = 200;

    public float attackDistance = 5f;

    public LineRenderer laser;
    private float laserDistance = 50f;

    private float tempoDeMira;

    private float acuracia = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();
        laser.enabled = false;
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        print("inicializada");
    }

    // Update is called once per frame
    void Update()
    {
        if (fov.canSeePlayer)
        {
            animator.SetBool("estahMirando", true);
            animator.SetBool("canWalk", false);
            animator.SetTrigger("mirarPlayer");
            laser.enabled = true;
            agent.isStopped = true;
        }
        else
        {
            animator.SetBool("estahMirando", false);
            animator.SetBool("canWalk", true);
        }
    }

    private void Mirar()
    {
        agent.isStopped = true;
        animator.SetBool("mirarPlayer", true);
        laser.enabled = true;
        tempoDeMira = 1f;
    }

    private void UpdateLaser()
    {
        tempoDeMira -= Time.deltaTime;

        Vector3 origem = transform.position + Vector3.up * 1.5f;
        Vector3 dir = (player.transform.position - origem).normalized;

        laser.SetPosition(0, origem);

        RaycastHit hit;
        if (Physics.Raycast(origem, dir, out hit, laserDistance))
        {
            laser.SetPosition(1, hit.point);
        }
        else
        {
            laser.SetPosition(1, origem + dir * laserDistance);
        }

        if (tempoDeMira <= 0f)
            Atirar();
    }

    private void Atirar()
    {
        laser.enabled = false;
        animator.SetBool("mirarPlayer", false);
        animator.SetTrigger("atirar");
        tempoDeMira = 0f;
        StartCoroutine(DepoisDeAtirar());
    }
    
    private IEnumerator DepoisDeAtirar()
    {
        yield return new WaitForSeconds(0.5f);

        if (Random.value <= acuracia)
        {
            player.GetComponent<MovimentarPersonagem>().UpdateLife(-80);
        }
    }
}
