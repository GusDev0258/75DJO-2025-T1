using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentarPersonagem : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;

    public float jumpHeight = 6f;

    public float gravity = -20f;

    public Transform groundCheck;

    public float sphereRadius = 0.4f;
    public LayerMask groundMask;

    public bool isOnTheGround;

    private Vector3 fallingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isOnTheGround = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (isOnTheGround && Input.GetButtonDown("Jump"))
        {
            fallingSpeed.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        fallingSpeed.y += gravity * Time.deltaTime;

        controller.Move(fallingSpeed * Time.deltaTime);

    }

    void OnGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, sphereRadius);
    }
}
