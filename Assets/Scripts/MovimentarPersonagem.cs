using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentarPersonagem : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;

    public float jumpHeight = 8f;

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



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        isOnTheGround = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * (speed * Time.deltaTime));

        CheckIfIsCrouchBlocked();
        
        if (isOnTheGround && Input.GetButtonDown("Jump"))
        {
            fallingSpeed.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (!isOnTheGround )
        {   
            fallingSpeed.y += gravity  * 2 * Time.deltaTime;
        }

        fallingSpeed.y += gravity * Time.deltaTime;

        controller.Move(fallingSpeed * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            CrouchStandup();
        }

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
