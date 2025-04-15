using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Glock : MonoBehaviour
{
    private Animator animator;

    private bool isShooting;

    private RaycastHit hit;

    public GameObject shootEffect;

    public GameObject postEffectShoot;

    public GameObject spark;

    private AudioSource shootingAudio;

    private int magazine = 3;
    private int bullets = 17;
    public AudioClip[] clips;

    public Text bulletsText;

    public GameObject imgCursor;
    // Start is called before the first frame update
    void Start()
    {
        isShooting = false;
        animator = GetComponent<Animator>();
        shootingAudio = GetComponent<AudioSource>();
        UpdateBulletsText();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("runningAction"))
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (!isShooting && bullets > 0)
            {
                shootingAudio.clip = clips[0];
                bullets--;
                isShooting = true;
                StartCoroutine(Shooting());
            }
            else
            {
                if (!isShooting && magazine > 0 && bullets == 0)
                {
                    ReloadGun();
                }
                else if(!isShooting && !shootingAudio.isPlaying)
                {
                    shootingAudio.clip = clips[2];
                    shootingAudio.time = 0;
                    shootingAudio.Play();
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("ReloadGun"))
            {
                if (magazine > 0 && bullets < 17)
                {
                    ReloadGun();
                }
                else if(!isShooting && !shootingAudio.isPlaying)
                {
                    shootingAudio.clip = clips[2];
                    shootingAudio.time = 0;
                    shootingAudio.Play();
                }
            }
        }
        UpdateBulletsText();
        if (Input.GetButton("Fire2"))
        {
            animator.SetBool("toAim", true);
            imgCursor.SetActive(false);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 45, Time.deltaTime * 10);
        }
        else
        {
            animator.SetBool("toAim", false);
            imgCursor.SetActive(true);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * 10);
        }
    }

    private void ReloadGun()
    {
        shootingAudio.clip = clips[1];
        shootingAudio.time = 1.05f;
        shootingAudio.Play();
        animator.Play("GlockReload");
        bullets = 17;
        magazine--;
    }

    IEnumerator Shooting()
    {
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY, 0));
        animator.Play("GlockFire");
        shootingAudio.time = 0;
        shootingAudio.Play();

        GameObject shootingEffectObject = Instantiate(shootEffect, postEffectShoot.transform.position,
            postEffectShoot.transform.rotation);

        GameObject sparkObject = null;
        if (Physics.SphereCast(ray, 0.1f, out hit))
        {
            sparkObject = Instantiate(spark, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            if (hit.transform.tag == "Drag")
            {
                Vector3 bulletDirection = ray.direction;
                hit.rigidbody.AddForceAtPosition(bulletDirection * 500, hit.point);
            }
            else
            {
                if (hit.transform.tag == "TakeDamage")
                {
                    ILevarDano takeDamage = hit.transform.GetComponent<ILevarDano>();
                    takeDamage.TakeDamage(5);
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(shootingEffectObject);
        Destroy(sparkObject);
        isShooting = false;
    }

    private void UpdateBulletsText()
    {
        bulletsText.text = bullets.ToString() + "/" + magazine.ToString();
    }

    public void addMagazine()
    {
        magazine++;
        UpdateBulletsText();
    }
}