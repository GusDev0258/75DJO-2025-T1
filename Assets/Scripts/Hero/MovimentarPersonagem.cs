using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovimentarPersonagem : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 4f;

    public float jumpHeight = 3f;

    public float gravity = -20f;

    public Transform groundCheck;

    public float sphereRadius = 0.4f;
    public LayerMask groundMask;

    public bool isOnTheGround;

    private Vector3 fallingSpeed;

    private Transform cameraTransform;

    private bool isCrouched;
    private bool blockedStand;
    public float standHeight, crouchHeight, standedCameraPosition, crouchedCameraPosition;

    public AudioClip jumpSound;
    public AudioClip walkingSound;
    private AudioSource audioSource;

    private int life = 100;
    public Slider lifeSlider;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        isOnTheGround = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * (speed * Time.deltaTime));
        if (isOnTheGround)
        {
            audioSource.clip = walkingSound;
            if (move.magnitude > 0)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.Stop();
            }
        }

        CheckIfIsCrouchBlocked();
        
        if (isOnTheGround && Input.GetButtonDown("Jump") && !isCrouched)
        {
            fallingSpeed.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            audioSource.clip = jumpSound;
            audioSource.Play();
        }

        if (!isOnTheGround )
        {   
            fallingSpeed.y += gravity  * 2 * Time.deltaTime;
        }

        fallingSpeed.y += gravity * Time.deltaTime;

        controller.Move(fallingSpeed * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.LeftControl) && isOnTheGround)
        {
            CrouchStandup();
        }

    }

    public void UpdateLife(int newLife)
    {
        life = Mathf.CeilToInt(Mathf.Clamp(life + newLife, 0, 100));
        lifeSlider.value = life;
    }

    void OnGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, sphereRadius);
    }

    private void CrouchStandup()
    {
        isCrouched = !isCrouched;
        if (blockedStand)
        {
            return;
        }
        if (isCrouched)
        {
            controller.height = crouchHeight;
            cameraTransform.localPosition = new Vector3(0, crouchedCameraPosition, 0);
        }
        else
        {
            controller.height = standHeight;
            cameraTransform.localPosition = new Vector3(0, standedCameraPosition, 0);
        }
    }

    private void CheckIfIsCrouchBlocked()
    {
        RaycastHit hit;
        blockedStand = Physics.Raycast(transform.position, Vector3.up, out hit, crouchHeight);
    }
}
